using Rogue.Coe;

namespace Rogue.Game.Comp
{
    public class Lock : GameComponent<Lock>
    {
        /// <summary>
        /// Flag indicating whether the lock is closed or not.
        /// </summary>
        public bool closed;

        public Lock() {}

        public Lock(bool closed)
        {
            this.closed = closed;
        }

        public void Open()
        {
            closed = false;
        }

        public void Close()
        {
            closed = true;
        }

        public void Toggle()
        {
            closed = !closed;
        }
    }
}
