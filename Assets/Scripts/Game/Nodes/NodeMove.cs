using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeMove : NodeAgentAction
    {
        private Ident m_entity;

        public NodeMove(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            object objPos = GetVar("moveDir");

            if (objPos == null)
            {
                return NodeState.Failure;
            }

            if (!Move((Vec2i)objPos))
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
