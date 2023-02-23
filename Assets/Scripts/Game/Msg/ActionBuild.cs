using Rogue.Core;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to perform an action.
    /// </summary>
    public class ActionBuild : Action<ActionBuild>
    {
        public Ident target;

        public int work = 0;

        public bool done = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ActionBuild(Ident target, int work)
        {
            this.target = target;
            this.work   = work;
        }
    }
}