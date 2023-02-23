using GG.Mathe;

namespace Rogue.Map
{
    public static class GameMapGenerator
    {
        public static void Generate(GameMapGeneratorConfig config, GameMap map)
        {
            float[,] noise = Noise.Generate(0.0f, 0.0f, map.Cols, map.Rows, 0, 1.0f, 4, 0.25f, 0.25f);

            for (int y = 0; y < map.Rows; y++)
            {
                for (int x = 0; x < map.Cols; x++)
                {
                    if (noise[x, y] < 0.5f)
                    {
                        map.SetCell(new Vec2i(x, y), "default", "default", null);
                    }
                    else
                    {
                        map.SetCell(new Vec2i(x, y), "default", "default", "default");
                    }
                }
            }
        }
    }
}
