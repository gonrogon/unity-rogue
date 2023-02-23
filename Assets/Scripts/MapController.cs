using UnityEngine;
using Rogue.Map;
using GG.Mathe;

namespace Rogue {

public class MapController : MonoBehaviour
{
    [SerializeField]
    private MapView m_view;

    [SerializeField]
    private Map.Data.DataBiome[] m_biomes;

    private GameMap m_map;

    public GameMap TheMap => m_map;

    public PathManager m_pathManager = new PathManager();

    public PathFinder m_pathFinder = new PathFinder();

    public int Width => m_map.Width;

    public int Height => m_map.Height;

    public void Setup(int width, int height)
    {
        CreateMap(width, height);
        CreateTerrain();

        m_pathManager.Setup(m_map);
        m_pathFinder .Setup(m_map);

        Context.Provide(m_map);
        Context.Provide(m_pathManager);
    }

    public void Step()
    {
        m_pathManager.Process(m_pathFinder);
    }

    public void Sync()
    {
        m_view.Sync(m_map);
    }

    public void GenerateMap()
    {
        GameMapGenerator.Generate(null, m_map);
    }

    private void CreateTerrain()
    {
        foreach (var biome in m_biomes)
        {
            m_map.LoadTerrainBiome(biome);
        }
    }

    private void CreateMap(int width, int height)
    {
        m_map = new GameMap(width, height);
    }

    public void FindPath()
    {
        //m_debug.ClearAllTiles();

        m_pathManager.Enqueue(new PathRequest(new Vec2i(1, 0), new Vec2i(0, 1), (success, path) =>
        {
            Debug.Log($"Response from map: {success.ToString().ToLower()}");

            foreach (Vec2i coord in path)
            {
                m_view.SetDebugPath(coord);
            }
        }));

        //m_map.Step();
    }
}

}