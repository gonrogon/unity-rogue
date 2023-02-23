using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeMoveAway : NodeAgentAction
    {
        private Ident m_entity;

        public NodeMoveAway(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            var entityPos = Query.GetPosition(m_entity);
            if (!entityPos)
            {
                return NodeState.Failure;
            }
            
            if (!Query.MapGetRandomPassable(entityPos.value, m_entity, out Vec2i where))
            {
                return NodeState.Failure;
            }

            if (!Move(where - entityPos.value))
            {
                return NodeState.Failure;
            }

            return NodeState.Success;
        }

        public bool Move(Vec2i dir)
        {
            Msg.ActionMove msg = new Msg.ActionMove(dir);

            Context.World.Send(m_entity, msg);
            AddActionCost(msg.cost);

            return msg.state == Msg.ActionState.Good;
        }
    }
}
