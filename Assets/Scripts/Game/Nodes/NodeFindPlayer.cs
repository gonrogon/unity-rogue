using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;

namespace Rogue.Game.Nodes
{
    public class NodeFindPlayer : NodeAction
    {
        private Ident m_entity;

        private Ident m_player;

        public NodeFindPlayer(Ident entity, Ident player)
        {

        }

        public override NodeState Evaluate()
        {
            var entityPos = Query.GetPosition(m_entity);
            var playerPos = Query.GetPosition(m_player);

            if (entityPos && playerPos)
            {
                int dx = Mathf.Abs(playerPos.value.x - entityPos.value.x);
                int dy = Mathf.Abs(playerPos.value.y - entityPos.value.y);

                if ((dx <= 0 && dy <= 0) || Query.MapIsVisible(entityPos.value, playerPos.value, m_entity))
                {
                    SetVar("targetPos", playerPos.value);

                    return NodeState.Success;
                }
            }

            return NodeState.Failure;
        }
    }
}
