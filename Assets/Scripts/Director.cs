using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Rogue.Map;
using Rogue.Coe;
using GG.Mathe;
using Rogue.Game;

namespace Rogue {

public class Director : MonoBehaviour
{
    public List<TextAsset> m_templatesToLoad;

    public List<TextAsset> m_categoriesToLoad;

    public List<TextAsset> m_itemTypesToLoad;

    public List<TextAsset> m_bodiesToLoad;

    public Input input;
        
    public Grid grid;

    public Views.ViewManager views;

    public Data.ViewTable viewTable;

    public Map.Data.DataFloor m_floor;

    public Map.Data.DataWall m_wall;

    public Tilemap m_tilemap;

    public Tilemap m_debug;

    public MapView m_mapView;

    public Tile m_empty;

    public Tile m_test;

    public MapController m_mapCtrl;

    private GameWorld m_world;

    private float m_elapsed = 0.0f;

    private float m_timestep = 0.033f;

    public Core.Ident eid = Core.Ident.Zero;

    public Game.TimeManager m_timeManager = new ();

    public Game.Scheduler m_scheduler = new();

    public Game.Jobs.JobManager m_jobs = new();

    private Game.BodySystem m_bodies = new();

    private Game.InventorySystem m_inventories = new();

    bool m_pickUpEnabled = false;

    bool m_buildEnabled = false;

    bool m_plowEnabled = false;

    public InventoryUI m_invetoryUI;

    private Game.Builder m_builder;

    private Game.Crops.CropSystem m_crops = new();

    private Game.CategoryStore m_categories = new();

    private Game.ItemTypeStore m_itemtypes = new();

    private Game.Stock.StockSystem m_stocks = new();

    public bool m_paused = false;

    public int money = 1000;

    [SerializeField]
    private Transform m_cameraTarget = null;

    // Start is called before the first frame update
    void Start()
    {
        m_timeManager.Setup(10000, 10, 25, 4);

        m_builder = new(m_crops);

        viewTable.grid = grid;
        //GameViewUtil.AddFactory(viewTable);

        //m_templates = new TemplateDatabase();
        
        m_world = new GameWorld();
        //m_map   = new GameMap(32, 32);

        Context.Provide(input);
        Context.Provide(m_world);
        Context.Provide(m_timeManager);
        Context.Provide(m_scheduler);
        Context.Provide(m_categories);
        Context.Provide(m_itemtypes);
        Context.Provide(m_jobs);
        Context.Provide(m_bodies);
        Context.Provide(m_inventories);
        Context.Provide(m_crops);

        m_builder.map = m_mapView;

        
        m_world.AddListener(new InventoryWorldListener(m_inventories));
        m_world.AddListener(new BodyWorldListener(m_bodies));

        // CATEGORIES AND TYPES

        foreach (TextAsset asset in m_categoriesToLoad) { m_categories.LoadFromText(asset.text); }
        foreach (TextAsset asset in m_itemTypesToLoad)  { m_itemtypes .LoadFromText(asset.text); }

        // CROPS
        
        Game.Crops.PlantType grass = new();
        grass.name                 = "grass";
        grass.description          = "Simple grass";
        grass.growthTime           = 10;
        //grass.temperatureRange    = 10;
        //grass.temperatureRequired = 20;
        //grass.waterRequired       = 80;
        //grass.waterRange          = 20;
        grass.AddView(new Game.Crops.PlantView(){ biome = "crops", floor = "grass_level0", growth = 20 });
        grass.AddView(new Game.Crops.PlantView(){ biome = "crops", floor = "grass_level1", growth = 50 });
        grass.AddView(new Game.Crops.PlantView(){ biome = "crops", floor = "grass_level2", growth = 70 });

        m_crops.Plants.Add(grass);

        // MAP
        
        m_mapCtrl.Setup(128, 128);
        m_mapCtrl.GenerateMap();
        /*
        m_mapCtrl.TheMap.SetCell(new Vec2i(0, 0), "default", "default", "default");
        m_mapCtrl.TheMap.SetCell(new Vec2i(1, 1), "default", "default", "default");
        m_mapCtrl.TheMap.SetCell(new Vec2i(2, 2), "default", "default", "default");
        m_mapCtrl.TheMap.SetCell(new Vec2i(3, 3), "default", "default", "default");
        m_mapCtrl.TheMap.SetCell(new Vec2i(3, 1), "default", "default", "default");
        m_mapCtrl.TheMap.SetFloor(new Rect2i(Vec2i.Zero, 128, 128), "default", "default");
        */
        views.Listen(m_world, m_mapCtrl.TheMap);

        input.Contexts.Add("build",     new Game.Gui.ContextBuild(input, m_mapView));
        input.Contexts.Add("selectOne", new Game.Gui.ContextSelectOne(input, m_mapView, m_mapCtrl.TheMap));

        // STOCK

        m_stocks.Setup(m_world, m_mapCtrl.TheMap);
        //m_stocks.AddStockpile();
        Context.Provide(m_stocks);

        // BODIES
        /*
        Body body = new ();
        body.Add(new BodyMember(BodyMember.Type.Head, "head", "H"));
        body.Add(new BodyMember(BodyMember.Type.UpperBody, "chest", "UB"));
        body.Add(new BodyMember(BodyMember.Type.Arm, "left arm", "LA"));
        body.Add(new BodyMember(BodyMember.Type.Arm, "right arm", "RA"));
        body.Add(new BodyMember(BodyMember.Type.Hand, "left hand",  "L"));
        body.Add(new BodyMember(BodyMember.Type.Hand, "right hand", "R"));
        body.Add(new BodyMember(BodyMember.Type.LowerBody, "hip", "LB"));
        body.Add(new BodyMember(BodyMember.Type.Leg, "left leg", "LL"));
        body.Add(new BodyMember(BodyMember.Type.Leg, "right leg", "RL"));
        body.Add(new BodyMember(BodyMember.Type.Foot, "left foot", "LF"));
        body.Add(new BodyMember(BodyMember.Type.Foot, "right foot", "RF"));

        m_bodies.AddTemplate("humanoid", body);

        body = new ();
        body.Add(new BodyMember(BodyMember.Type.Hand, "left hand",  "L"));
        body.Add(new BodyMember(BodyMember.Type.Hand, "right hand", "R"));
        body.Add(new BodyMember(BodyMember.Type.Head, "head", "H"));
        m_bodies.AddTemplate("mob", body);

        m_bodies.SaveTemplatesToFile("Assets/Data/Templates/Bodies/h.txt");
        */

        if (m_bodiesToLoad != null)
        {
            for (int i = 0; i < m_bodiesToLoad.Count; i++)
            {
                Debug.Log($"Loading body templates from: {m_bodiesToLoad[i].name}");
                m_bodies.LoadTemplatesFromText(m_bodiesToLoad[i].text);
            }
        }

        // TEMPLATES

        GameEntity ent;

        // STOCKPILE

        if (m_templatesToLoad != null)
        {
            for (int i = 0; i < m_templatesToLoad.Count; i++)
            {
                
                m_world.LoadTemplatesFromText(m_templatesToLoad[i].text);
            }
        }
        //m_world.SaveTemplates("test.tdb");

        // KNIFE

        ent = m_world.Create("knife");
        ent.FindFirstComponent<Game.Comp.Location>().position = new Vec2i(0, 4);
        Context.Map.Add(new Vec2i(0, 4), ent.Id);
        m_world.Start(ent);

        // PLAYER
        /*
        ent = m_world.Create();

        bid = m_bodies.Add();
        m_bodies.Get(bid).Add(new BodyMember(BodyMember.Type.Hand, "left hand",  "L"));
        m_bodies.Get(bid).Add(new BodyMember(BodyMember.Type.Hand, "right hand", "R"));

        ent.AddComponent(new Game.Comp.Life(100, 100, 100));
        ent.AddComponent(new Game.Comp.Location(new Vec2i(1, 0)));
        ent.AddComponent(new Game.Comp.Name("player", "the player character"));
        ent.AddComponent(new Game.Comp.Tag(TagType.Player));
        ent.AddComponent(new Game.Comp.Inventory());
        ent.AddComponent(new Game.Comp.Body("humanoid", Core.Ident.Zero));
        ent.AddComponent(new Game.Comp.View("ViewMob", null, "player"));
        ent.AddBehaviour(new Game.Behav.Name());
        ent.AddBehaviour(new Game.Behav.Life());
        ent.AddBehaviour(new Game.Behav.Move());
        ent.AddBehaviour(new Game.Behav.Equip());
        //ent.AddBehaviour(new Game.Behav.Picker());
        //ent.AddBehaviour(new Game.Behav.Dropper());
        ent.AddBehaviour(new Game.Behav.Attack());
        ent.AddBehaviour(new Game.Behav.Builder());
        ent.AddBehaviour(new Game.Behav.Farmer());
        //ent.SetView("ViewSimple", "Mob");
        //ent.SetView(view);
        //ident = m_world.Add(ent);
        Context.Map.Add(new Vec2i(1, 0), ent.Id);
        eid = ent.Id;
        m_world.Start(ent);    
        */

        // FARMER 1

        ent = m_world.Create("farmer");

        ent.FindFirstAny<Game.Comp.Name>().name = "farmer 1";
        ent.FindFirstComponent<Game.Comp.Location>().position = new Vec2i(8, 4);
        Context.Map.Add(new Vec2i(8, 4), ent.Id);
        m_world.Start(ent);

        //m_scheduler.Add(new Game.Betree.Agents.AgentBetree(ent.Id));
        m_scheduler.Add(new Game.Betree.Agents.AgentFarmer(ent.Id));
        
        // FARMER 2
        
        ent = m_world.Create("farmer");

        ent.FindFirstAny<Game.Comp.Name>().name = "farmer 2";
        ent.FindFirstComponent<Game.Comp.Location>().position = new Vec2i(2, 5);
        Context.Map.Add(new Vec2i(2, 5), ent.Id);
        m_world.Start(ent);

        m_scheduler.Add(new Game.Betree.Agents.AgentFarmer(ent.Id));
        
        // ENEMY
        
        ent = m_world.Create("small spider");
        
        ent.FindFirstAny<Game.Comp.Name>().name = "enemy spider 1";
        ent.FindFirstComponent<Game.Comp.Location>().position = new Vec2i(8, 12);
        Context.Map.Add(new Vec2i(8, 12), ent.Id);
        m_world.Start(ent);

        m_scheduler.Add(new Game.Betree.Agents.AgentSpider(ent.Id));

        // DOOR

        ent = m_world.Create();

        ent.AddComponent(new Game.Comp.Location(new Vec2i(3, 2)));
        ent.AddComponent(new Game.Comp.Name("door", "an ordinary door"));
        ent.AddComponent(new Game.Comp.Lock(true));
        ent.AddComponent(new Game.Comp.Block(TagType.All ^ TagType.Player, true));
        ent.AddComponent(new Game.Comp.View("ViewSimple", "Default", "door"));
        ent.AddBehaviour(new Game.Behav.Name());
        ent.AddBehaviour(new Game.Behav.Door());
        //ent.SetView("ViewSimple");

        //ident = m_world.Add(ent);
        Context.Map.Add(new Vec2i(3, 2), ent.Id);
        m_world.Start(ent);    

        //m_scheduler.Add(new Game.AgentSimple(enemy, eid));
    }

    private void Update()
    {
        if (m_cameraTarget != null)
        {
            m_cameraTarget.localPosition += (Vector3)input.Camera;
        }

        m_elapsed += Time.deltaTime;

        if (m_elapsed > m_timestep)
        {
            /*
            Vec2i coord = m_mapView.GetMouseCoord();

            if (m_mapCtrl.TheMap.HasCoord(coord))
            {
                string msg = string.Empty;

                msg += " - COORD: " + coord.x + ", " + coord.y;

                if (m_mapCtrl.TheMap.HasWall(coord))
                {
                    msg += " - WALL: " + m_mapCtrl.TheMap.GetWallData(coord).title;
                }

                if (m_mapCtrl.TheMap.HasFloor(coord))
                {
                    msg += " - FLOOR: " + m_mapCtrl.TheMap.GetFloorData(coord).title;
                }

                Debug.Log(msg);
            }
            */
            SyncTilemap();

            m_elapsed = 0;


            if (!m_paused)
            {
                UpdateWorld();
            }
        }
    }

    public void UpdateWorld()
    {
        // Elapsed time in milliseconds.
        int millis = (int)(m_timestep * 1000.0f);
        
        m_timeManager.Step(millis);

        m_stocks  .Update();
        m_crops   .Update(millis);
        m_jobs    .Update();

        m_scheduler.Step(millis);
        m_mapCtrl  .Step();
        m_world    .Step(m_timestep);
    }

    public void Pause()
    {
        m_paused = true;
    }

    public void Resume()
    {
        m_paused = false;
    }

    public void Move(string dir)
    {
        Vec2i v = Vec2i.Zero;

        switch (dir)
        {
            case "L": v = Vec2i.Left;  break;
            case "R": v = Vec2i.Right; break;
            case "U": v = Vec2i.Up;    break;
            case "D": v = Vec2i.Down;  break;
        }

        if (m_pickUpEnabled)
        {
            Vec2i pos    = m_world.Find(eid).FindFirstComponent<Game.Comp.Location>().position;
            Vec2i target = pos + v;

            if (Game.Query.MapGetFirstPickable(target, eid, out Core.Ident what))
            {
                m_world.Send(eid, new Game.Msg.PickUp(what));
            }
        }
        else if (m_buildEnabled)
        {
            Vec2i pos    = Game.Query.GetPosition(eid).value;
            Vec2i target = pos + v;

            if (Game.Query.MapGetFirstBuildable(target, eid, out Core.Ident what))
            {
                m_world.Send(eid, new Game.Msg.ActionBuild(what, 20));
            }
        }
        else if (m_plowEnabled)
        {
            Vec2i pos    = Game.Query.GetPosition(eid).value;
            Vec2i target = pos + v;

            if (Game.Query.MapGetFirstCultivable(target, eid, out Core.Ident what))
            {
                m_world.Send(eid, new Game.Msg.ActionPlow(what, target));
            }
        }
        else
        {
            m_world.Send(eid, new Game.Msg.ActionMove(v));
        }
    }

    public void BuildStockpile() => m_builder.BuildStockpile();

    public void BuildWeaponsStockpile() => m_builder.BuildStockpile(true);

    public void BuildCrop() { m_builder.BuildCrop(); }

    public void BuildMarket() { m_builder.BuildMarket(); }

    public void Build()
    {
        m_builder.Build();
    }

    public void TooglePlow()
    {
        m_plowEnabled = !m_plowEnabled;

        if (m_plowEnabled)
        {
            m_buildEnabled = false;
            m_pickUpEnabled =false;
        }
    }

    public void TogglePickUp()
    {
        m_pickUpEnabled = !m_pickUpEnabled;

        if (m_pickUpEnabled)
        {
            m_plowEnabled = false;
            m_buildEnabled = false;
        }
    }

    public void ToggleBuild()
    {
        m_buildEnabled = !m_buildEnabled;

        if (m_buildEnabled)
        {
            m_plowEnabled = false;
            m_pickUpEnabled = false;
        }
    }

    public void FindPath()
    {
        m_mapCtrl.FindPath();
        //m_debug.ClearAllTiles();
        /*
        m_map.RequestPath(new Vec2i(1, 0), new Vec2i(0, 1), (success, path) =>
        {
            Debug.Log($"Response from map: {success.ToString().ToLower()}");

            foreach (Vec2i coord in path)
            {
                m_debug.SetTile(new Vector3Int(coord.x, coord.y), m_test);
            }
        });
        */
        //m_map.Step();
    }

    public void SyncTilemap()
    {
        m_mapCtrl.Sync();
        /*
        Cell cell;

        foreach (Vec2i coord in new Rect2i(Vec2i.Zero, m_map.Width - 1, m_map.Height - 1))
        {
            cell = m_map.GetCell(coord);
            // Show the wall.
            if (cell.wall != null)
            {
                m_tilemap.SetTile(new Vector3Int(coord.x, coord.y, 0), cell.wall.tile);
                continue;
            }
            // No wall, show the floor.
            if (cell.floor != null)
            {
                m_tilemap.SetTile(new Vector3Int(coord.x, coord.y, 0), cell.floor.GetRandomTile());
                continue;
            }

            m_tilemap.SetTile(new Vector3Int(coord.x, coord.y, 0), m_empty);
        }
        */
    }

    public void SyncInventoryUI()
    {
        var iid = m_world.Find(eid).FindFirstComponent<Game.Comp.Inventory>().iid;

        m_invetoryUI.Sync(eid, iid);
    }
}

}