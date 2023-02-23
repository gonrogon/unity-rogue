using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeActionDrop : NodeActionBase
    {
        private readonly string m_varWhat = "what";

        private readonly string m_varWhere = "where";

        public NodeActionDrop() {}

        public NodeActionDrop(string varTargetId, string varTargetLocation)
        {
            m_varWhat       = varTargetId;
            m_varWhere = varTargetLocation;
        }

        public override NodeState OnUpdate()
        {
            if (!Blackboard.TryGet(m_varWhat,  out Ident what))  { return NodeState.Failure; }
            if (!Blackboard.TryGet(m_varWhere, out Vec2i where)) { return NodeState.Failure; }

            var msg = Context.World.Send(AgentState.eid, new Msg.Drop(what, where));
            if (msg.done)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}
