using Rogue.Coe;

namespace Rogue.Game.Msg
{
    public class Build : GameMessage<Damage>
    {
        public int work = 0;

        public bool done = false;

        public Build(int work)
        {
            this.work = work;
        }

        public Build() {}
    }
}