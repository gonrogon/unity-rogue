namespace Rogue.Core.Betree
{
    /// <summary>
    /// Node to set the value of a variable.
    /// </summary>
    /// <typeparam name="T">Type of the value to set.</typeparam>
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
