using System;

namespace Rogue.Core.Betree
{
    public class NodeTestVar<T> : NodeCondition
    {
        /// <summary>
        /// Name of the variable to set.
        /// </summary>
        private readonly string m_name;

        /// <summary>
        /// Value.
        /// </summary>
        private readonly T m_value;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the variable to set.</param>
        /// <param name="value">Comparison value.</param>
        public NodeTestVar(string name, T value)
        {
            m_name  = name;
            m_value = value;
        }

        public override NodeState OnUpdate()
        {
            if (!Blackboard.TryGet<T>(m_name, out T value))
            {
                return NodeState.Failure;
            }

            if (m_value.Equals(value))
            {
                return NodeState.Failure;
            }

            return NodeState.Success;
        }
    }
}
