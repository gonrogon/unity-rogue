using UnityEngine;
using Rogue.Coe;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game
{
    public class Builder
    {
        //public TemplateDatabase templates;

        public MapView map;

        private Crops.CropSystem m_crops;

        private bool m_weaponStockpile = false;

        public Builder(Crops.CropSystem crops)
        {
            m_crops = crops;
        }

        public void Build()
        {
            Rogue.Gui.ContextSettings settings = new();
            settings.start  = OnSelectionStart;
            settings.end    = OnSelectionFinish;
            settings.notify = OnSelectionNotity;

            Context.Input.Contexts.Push("build", settings);
        }

        public void OnSelectionStart(Rogue.Gui.Context context)
        {
            
        }

        public void OnSelectionFinish(Rogue.Gui.Context context)
        {
            var buildContext = (Gui.ContextBuild)context;
            map.ClearDebug();

            foreach (Vec2i coord in buildContext.Selection)
            {
                Context.Map.SetWall(coord, "default", "default");
            }

            /*
            foreach (Vec2i coord in buildContext.Selection)
            {
                GameEntity entity = Context.World.Create("scaffold_terrain");
                //GameEntity ent = new();
                //ent.Populate(templates.Find("scaffoldTerrain"));
                //ent.Populate(Context.World.FindTemplate("scaffold_terrain"));
                //Ident eid = Context.World.Add(ent);

                
                int jid = Context.Jobs.AddJob(new Jobs.JobBuilding(entity.Id, 100, coord));

                entity.FindFirstComponent<Comp.Location>().position = coord;
                entity.FindFirstComponent<Comp.Terrain>().SetJob(jid);
                Context.World.Start(entity);
                Context.Map.Add(coord, entity.Id);
            }
            */
        }

        public void OnSelectionNotity(Rogue.Gui.Context context)
        {
            var buildContext = (Gui.ContextBuild)context;

            map.ClearDebug();
            map.SetSelection(buildContext.Selection);
        }

        # region @@@ STOCKPILE @@@

        public void BuildStockpile(bool weapon = false)
        {
            m_weaponStockpile = weapon;

            Rogue.Gui.ContextSettings settings = new();
            settings.start  = OnStockpileSelectionStart;
            settings.end    = OnStockpileSelectionFinish;
            settings.notify = OnStockpileSelectionNotify;

            Context.Input.Contexts.Push("build", settings);
        }

        public void OnStockpileSelectionStart(Rogue.Gui.Context context)
        {}

        public void OnStockpileSelectionFinish(Rogue.Gui.Context context)
        {
            var bc = (Gui.ContextBuild)context;
            map.ClearDebug();

            int id;
            if (m_weaponStockpile)
            {
                id = Game.Stock.StockpileFactory.CreateWeaponStockpile(bc.Selection);
            }
            else
            {
                id = Game.Stock.StockpileFactory.CreateGenericStockpile(bc.Selection);//Context.Stock.CreateStockpile(bc.Selection);
            }

            //GameEntity ent = new();
            //ent.Populate(templates.Find("stockpile"));
            //Context.World.Add(ent);
            GameEntity entity = Context.World.Create("stockpile");
            entity.FindFirstComponent<Comp.Stockpile>().id = id;
            Context.World.Start(entity);
        }

        public void OnStockpileSelectionNotify(Rogue.Gui.Context context)
        {
            var bc = (Gui.ContextBuild)context;
            map.ClearDebug();
            map.SetSelection(bc.Selection);
        }

        # endregion

        public void BuildCrop()
        {
            Rogue.Gui.ContextSettings settings = new();
            settings.start  = OnCropSelectionStart;
            settings.end    = OnCropSelectionFinish;
            settings.notify = OnCropSelectionNotify;

            Context.Input.Contexts.Push("build", settings);
        }

        public void OnCropSelectionStart(Rogue.Gui.Context context)
        {
            
        }

        public void OnCropSelectionFinish(Rogue.Gui.Context context)
        {
            var buildContext = (Gui.ContextBuild)context;
            map.ClearDebug();

            int cropId = m_crops.CreateCrop("grass", buildContext.Selection);
            if (cropId < 0)
            {
                return;
            }

            var crop = m_crops.At(cropId);

            GameEntity entity = Context.World.Create("crop");

            entity.FindFirstComponent<Comp.Crop>().cropId = cropId;
            //GameEntity ent = new();
            //entity.AddComponent(new Comp.Name("crop", "a wonderful crop full of plants"));
            //entity.AddComponent(new Comp.Crop(cropId));
            //entity.AddComponent(new Comp.View("ViewCrop", null, null));
            //entity.AddBehaviour(new Behav.Cultivable());
            //entity.SetView("ViewCrop", null);
            Context.World.Start(entity);

            crop.SetEntity(entity.Id);

            Context.Map.AddMulticell(crop.EntityId, crop);
            m_crops.CreateJobs(cropId);
        }

        public void OnCropSelectionNotify(Rogue.Gui.Context context)
        {
            var buildContext = (Gui.ContextBuild)context;

            map.ClearDebug();
            map.SetSelection(buildContext.Selection);
        }

        public void BuildMarket()
        {
            Rogue.Gui.ContextSettings settings = new();
            settings.start  = OnMarketSelectionStart;
            settings.end    = OnMarketSelectionFinish;
            settings.notify = OnMarketSelectionNotify;

            Context.Input.Contexts.Push("build", settings);
        }

        public void OnMarketSelectionStart(Rogue.Gui.Context context)
        {
            
        }

        public void OnMarketSelectionFinish(Rogue.Gui.Context context)
        {
            var buildContext = (Gui.ContextBuild)context;
            map.ClearDebug();

            Rect2i zone = new (buildContext.Selection.Min, new Vec2i(3, 3));


            GameEntity entity = Context.World.Create("market");

            entity.FindFirstComponent<Comp.Inventory>().iid = Context.Inventories.Add();
            entity.FindFirstComponent<Comp.Building>().zone = zone;
            Context.World.Start(entity);
            //Context.Map.AddMulticell(entity.Id, crop);
        }

        public void OnMarketSelectionNotify(Rogue.Gui.Context context)
        {
            var buildContext = (Gui.ContextBuild)context;

            map.ClearDebug();
            map.SetSelection(buildContext.Selection);
        }
    }
}
