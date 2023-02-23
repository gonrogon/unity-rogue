using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeFollowPath : NodeAgentAction
    {
        private Ident m_entity;

        private int m_index = 1;

        private bool m_reach = true;

        public NodeFollowPath(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            var entityPos = Query.GetPosition(m_entity);
            var objPath   = FindVar("path");

            if (!entityPos || objPath == null)
            {
                return NodeState.Failure;
            }

            Path2i path = (Path2i)objPath;

            int count = m_reach ? path.Count - 1 : path.Count;

            if (m_index < count)
            {
                Vec2i dir = path.At(m_index) - entityPos.value;
                SetVar("moveDir", dir);

                m_index++;

                return NodeState.Success;
            }
            
            m_index = 1;

            return NodeState.Failure;
        }
    }
}
