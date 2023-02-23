using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeWander : NodeAgentAction
    {
        private static Vec2i[] Dirs = {new Vec2i(-1, 0), new Vec2i(0, 1), new Vec2i(1, 0), new Vec2i(0, -1) };

        private static int MaxDirs = 4;

        private Ident m_entity;

        public NodeWander(Ident entity)
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

            int start = Random.Range(0, MaxDirs);
            
            for (int i = 0; i < MaxDirs; i++)
            {
                Vec2i coord = entityPos.value + Dirs[(start + i) % MaxDirs];

                if (Query.MapIsPassable(coord, m_entity))
                {
                    SetVar("moveDir", coord - entityPos.value);

                    return NodeState.Success;
                }
            }

            return NodeState.Failure;
        }
    }
}
