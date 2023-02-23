using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeMove2 : NodeAgentAction
    {
        private Ident m_entity;

        public NodeMove2(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            if (FindVar("pathWaypoint") is not Vec2i waypoint)
            {
                return NodeState.Failure;
            }

            var  query = Query.GetPosition(m_entity);
            if (!query)
            {
                return NodeState.Failure;
            }

            if (!Move(waypoint - query.value))
            {
                return NodeState.Failure;
            }

            var  result = Query.GetPosition(m_entity);
            if (!result)
            {
                return NodeState.Failure;
            }

            if (result.value != waypoint)
            {
                return NodeState.Running;
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
