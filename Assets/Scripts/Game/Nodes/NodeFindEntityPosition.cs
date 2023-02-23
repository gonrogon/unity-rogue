using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeFindEntityPosition : NodeAgentAction
    {
        private Ident m_target;

        public NodeFindEntityPosition()
        {}

        public NodeFindEntityPosition(Ident target)
        {
            m_target = target;
        }

        public override NodeState Evaluate()
        {
            Ident target = m_target;
            // Try to get a valid target.
            if (target.IsZero)
            {
                var objTargetEntity  = GetVar("targetEntity");
                if (objTargetEntity != null)
                {
                    target = (Ident)objTargetEntity;
                }

                if (target.IsZero)
                {
                    return NodeState.Failure;
                }
            }
            // Try to get the position.
            var  entityPos = Query.GetPosition(target);
            if (!entityPos)
            {
                return NodeState.Failure;
            }

            SetVar("targetPos", entityPos.value);

            return NodeState.Success;
        }
    }
}
