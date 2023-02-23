using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeActionStore : NodeActionBase
    {
        private readonly string m_varWhat  = "what";

        private readonly string m_varContainer = "container";

        public NodeActionStore() {}

        public NodeActionStore(string varWhat, string varContainer)
        {
            m_varWhat      = varWhat;
            m_varContainer = varContainer;
        }

        public override NodeState OnUpdate()
        {
            if (!Blackboard.TryGet(m_varWhat,      out Ident what))      { return NodeState.Failure;}
            if (!Blackboard.TryGet(m_varContainer, out Ident container)) { return NodeState.Failure;}

            var msg = Context.World.Send(AgentState.eid, new Msg.ActionStore(what, container));
            if (msg.done)
            {
                AddActionCost(msg.cost);
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}
