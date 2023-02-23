namespace Rogue.Core.Betree
{
    public class NodeSetVar<T> : NodeAction
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
        /// <param name="value">Value.</param>
        public NodeSetVar(string name, T value)
        {
            m_name  = name;
            m_value = value;
        }

        public override NodeState OnUpdate()
        {
            Blackboard.Set(m_name, m_value);
            return NodeState.Success;
        }
    }
}
