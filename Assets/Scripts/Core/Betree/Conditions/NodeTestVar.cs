namespace Rogue.Core.Betree
{
    /// <summary>
    /// Node to check if the value of a variable is equal to a specified one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NodeTestVar<T> : NodeCondition
    {
        /// <summary>
        /// Name of the variable to set.
        /// </summary>
        private readonly string m_name;

        /// <summary>
        /// Value to check.
        /// </summary>
        private readonly T m_value;

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
