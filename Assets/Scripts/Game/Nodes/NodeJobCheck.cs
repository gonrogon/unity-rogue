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
    public class NodeJobCheck<T> : NodeDecorator
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="var">Name of the variable.</param>
        /// <param name="check">Type of check.</param>
        public NodeJobCheck() {}

        public override NodeState Evaluate()
        {
            object var = FindVar("targetJob");

            if (var == null)
            {
                return NodeState.Success;
            }
            
            if (Context.Jobs.At((int)var) is not T)
            {
                return NodeState.Success;
            }

            return m_child.Evaluate();
        }
    }
}
