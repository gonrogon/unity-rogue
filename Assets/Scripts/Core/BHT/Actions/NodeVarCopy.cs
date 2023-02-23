namespace Rogue.Core.BHT
{
    /// <summary>
    /// Defines an action to copy a variable.
    /// </summary>
    public class NodeVarCopy : NodeAction
    {
        /// <summary>
        /// Variable to copy.
        /// </summary>
        private readonly string m_source;

        /// <summary>
        /// Variable to write.
        /// </summary>
        private readonly string m_target;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Variable to copy.</param>
        /// <param name="target">Variable to write.</param>
        public NodeVarCopy(string source, string target)
        {
            m_source = source;
            m_target = target;
        }

        public override NodeState Evaluate()
        {
            if (TryFindVar(m_source, out object var))
            {
                SetVar(m_target, var);
            }

            return NodeState.Success;
        }
    }
}
