using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Game.Comp
{
    public class Terrain : GameComponent<Terrain>, IConstruction
    {
        /// <summary>
        /// Biome.
        /// </summary>
        public string biome = string.Empty;

        /// <summary>
        /// Wall to build.
        /// </summary>
        public string wall = string.Empty;

        /// <summary>
        /// Floor to build.
        /// </summary>
        public string floor = string.Empty;

        /// <summary>
        /// Amount of work already done.
        /// </summary>
        public int work = 0;

        public int Job { get; private set; } = -1;
        
        public int TotalWork { get; private set; } = 100;

        public int Progress => work / TotalWork;

        public bool Done => work >= TotalWork;

        public Terrain() {}

        public Terrain(int totalWork, int work = 0)
        {
            this.TotalWork = totalWork;
            this.work      = work;
        }

        public Terrain(string biome, string wall, string floor, int totalWork, int work = 0)
        {
            this.biome     = biome;
            this.wall      = wall;
            this.floor     = floor;
            this.TotalWork = totalWork;
            this.work      = work;
        }

        public void SetJob(int jid)
        {
            Job = jid;
        }

        public bool Advance(int amount)
        {
            work += amount;
            if (work >= TotalWork)
            {
                work  = TotalWork;
            }

            return Done;
        }

        public void Construct(Vec2i coord)
        {
            if (!string.IsNullOrEmpty(floor) && !string.IsNullOrEmpty(wall))
            {
                Context.Map.SetCell(coord, biome, floor, wall);
                return;
            }
            
            if (!string.IsNullOrEmpty(floor))
            {
                Context.Map.SetFloor(coord, biome, floor);
                return;
            }

            if (!string.IsNullOrEmpty(wall))
            {
                Context.Map.SetWall(coord, biome, wall);
                return;
            }
        }
    }
}
