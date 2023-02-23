using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    /// <summary>
    /// Defines a decorator that checks whether a variable is defined or not ir order to procced with the evaluation
    /// of the child node.
    /// </summary>
    public class NodeTargetOverlapCheck : NodeDecorator
    {
        private Ident m_entity;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="var">Name of the variable.</param>
        /// <param name="check">Type of check.</param>
        public NodeTargetOverlapCheck(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            object var = FindVar("targetPos");
            if (var == null)
            {
                return NodeState.Success;
            }
            
            var  entityPos = Query.GetPosition(m_entity);
            if (!entityPos)
            {
                return NodeState.Success;
            }

            if ((Vec2i)var != entityPos.value)
            {
                return NodeState.Success;
            }

            return m_child.Evaluate();
        }
    }
}
