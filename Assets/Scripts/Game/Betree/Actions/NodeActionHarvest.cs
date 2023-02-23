using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeActionHarvest : NodeActionBase
    {
        private readonly string m_varCrop = "crop";

        private readonly string m_varWhere = "cropLocation";

        public NodeActionHarvest() {}

        public NodeActionHarvest(string varCrop, string varWhere)
        {
            m_varCrop  = varCrop;
            m_varWhere = varWhere;
        }

        public override NodeState OnUpdate()
        {
            if (!Blackboard.TryGet(m_varCrop,  out Ident crop))  { return NodeState.Failure; }
            if (!Blackboard.TryGet(m_varWhere, out Vec2i where)) { return NodeState.Failure; }

            var msg = Context.World.Send(AgentState.eid, new Msg.ActionHarvest() { target = crop, where = where });
            AddActionCost(msg.cost);

            return msg.Success ? NodeState.Success : NodeState.Failure;
        }
    }
}
