using UnityEngine;
using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeActionPlow : NodeActionBase
    {
        private readonly string m_varCrop = "crop";

        private readonly string m_varWhere = "cropLocation";

        public NodeActionPlow() {}

        public NodeActionPlow(string varCrop, string varWhere)
        {
            m_varCrop  = varCrop;
            m_varWhere = varWhere;
        }

        public override NodeState OnUpdate()
        {
            if (!Blackboard.TryGet(m_varCrop,  out Ident crop))  { return NodeState.Failure; }
            if (!Blackboard.TryGet(m_varWhere, out Vec2i where)) { return NodeState.Failure; }

            var msg = Context.World.Send(AgentState.eid, new Msg.ActionPlow() { target = crop, where  = where });
            AddActionCost(msg.cost);

            if (msg.Success)
            {
                
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}
