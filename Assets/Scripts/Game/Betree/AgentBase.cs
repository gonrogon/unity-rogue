using Rogue.Core;
using Rogue.Core.Betree;

namespace Rogue.Game.Betree
{
    public abstract class AgentBase : IAgent
    {
        public class State
        {
            public Ident eid;

            public int cost;
        }

        protected SchedulerHandler m_handler = null;

        protected State m_state = null;

        protected Tree m_tree = null;

        public AgentBase() : this (Ident.Zero) {}

        public AgentBase(Ident eid)
        {
            m_state = new () { eid = eid, cost = 0 };
            m_tree  = new Tree();
            m_tree.Blackboard.Set("state", m_state);
        }

        public virtual void OnScheduleStart(SchedulerHandler handler)
        {
            m_handler = handler;
        }

        public virtual void OnScheduleFinish() {}

        public virtual int OnScheduleTrigger()
        {
            m_tree.Tick();

            int lastCost = m_state.cost;
            m_state.cost = 0;

            return lastCost;
        }

        #region @@@ SUBTREES @@@

        protected Node CreatePickUpTree(string what)
        {
            return new NodeSequence(
                CreateGoTree(null, what),
                new NodeActionPickUp(what)
            );
        }

        protected Node CreateDropTree(string what, string where)
        {
            return new NodeSequence(
                CreateGoTree(null, where),
                new NodeActionDrop(what, where)
            );
        }

        protected Node CreateStoreTree(string what, string container, string where)
        {
            return new NodeSequence(
                CreateGoTree(null, where),
                new NodeActionStore(what, container)
            );
        }

        protected Node CreateGoTree(string origin, string target)
        {
            return new NodeSequence(
                new NodeFindPath(origin, target),
                new NodeFollowPath()
            );
        }

        #endregion
    }
}
