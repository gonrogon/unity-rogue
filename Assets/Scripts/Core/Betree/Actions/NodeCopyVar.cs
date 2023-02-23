namespace Rogue.Core.Betree
{
    public class NodeCopyVar : NodeAction
    {
        /// <summary>
        /// Name of the variable to copy.
        /// </summary>
        public string m_source;

        /// <summary>
        /// Name of the variable to set.
        /// </summary>
        public string m_target;

        public NodeCopyVar(string source, string target)
        {
            m_source = source;
            m_target = target;
        }

        public override NodeState OnUpdate()
        {
            if (!Blackboard.TryGet(m_source, out object value))
            {
                return NodeState.Failure;
            }

            Blackboard.Set(m_target, value);
            return NodeState.Success;
        }
    }
}
