using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Game.Comp
{
    public class Scaffold : GameComponent<Scaffold>, IConstruction
    {
        /// <summary>
        /// Template to buy.
        /// </summary>
        public string template = string.Empty;

        /// <summary>
        /// Amount of work already done.
        /// </summary>
        public int work = 0;

        public int Job { get; private set; } = -1;
        
        public int TotalWork { get; private set; } = 100;

        public int Progress => work / TotalWork;

        public bool Done => work >= TotalWork;

        public Scaffold() {}

        public Scaffold(int totalWork, int work = 0)
        {
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

        }
    }
}
