namespace Rogue.Core.BHT
{
    public class NodeConditional : Node
    {
        /// <summary>
        /// Define an enumeration with the states of the node.
        /// </summary>
        protected enum State
        {
            Condition, // Node is evaluating the condition.
            Success,   // Node is evaluating the success.
            Failure,   // Node is evaluating the failure.
        }

        /// <summary>
        /// State.
        /// </summary>
        protected State m_state = State.Condition;

        /// <summary>
        /// Node for the condition.
        /// </summary>
        protected Node m_condition = null;

        /// <summary>
        /// Node to evaluate if the condition succeeds.
        /// </summary>
        protected Node m_success = null;

        /// <summary>
        /// Node to evaluate if the condition fails.
        /// </summary>
        protected Node m_failure = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeConditional() : base() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="condition">Node for the condition.</param>
        /// <param name="success">Node to evaluate if the condition succeeds.</param>
        /// <param name="failure">Node to evaluate if the condition fails.</param>
        public NodeConditional(Node condition, Node success, Node failure) : base()
        {
            AttachCondition(condition);
            AttachSuccess  (success);
            AttachFailure  (failure);
        }

        public override void OnAttached(Node parent)
        {
            base        .OnAttached(parent);
            m_condition?.OnAttached(this);
            m_success  ?.OnAttached(this);
            m_failure  ?.OnAttached(this);
        }

        /// <summary>
        /// Sets the condition node.
        /// </summary>
        /// <param name="condition">Node.</param>
        public virtual void AttachCondition(Node condition)
        {
            m_condition = condition;
            condition?.OnAttached(this);
        }

        /// <summary>
        /// Sets the success.
        /// </summary>
        /// <param name="success">Node to evaluate if the condition succeeds.</param>
        public virtual void AttachSuccess(Node success)
        {
            m_success = success;
            success?.OnAttached(this);
        }

        /// <summary>
        /// Sets the failure.
        /// </summary>
        /// <param name="failure">Node to evaluate if the condition fails.</param>
        public virtual void AttachFailure(Node failure)
        {
            m_failure = failure;
            failure?.OnAttached(this);
        }

        public override NodeState Evaluate()
        {
            switch (m_state)
            {
                case State.Condition:
                {
                    switch (m_condition.Evaluate())
                    {
                        case NodeState.Success: return EvaluateBranch(State.Success, m_success);
                        case NodeState.Failure: return EvaluateBranch(State.Failure, m_failure);
                        // If the evaluation of the condition does not finish, the evaluation is paused until the
                        // next iteration.
                        default:
                        {
                            m_state = State.Condition;
                            return NodeState.Running;
                        }
                    }
                }
                // Condition is already evaluated but the success node or the failure node is still running so its
                // evaluation have to continue.
                case State.Success: return EvaluateBranch(State.Success, m_success);
                case State.Failure: return EvaluateBranch(State.Failure, m_failure);
            }

            return NodeState.Success;
        }

        /// <summary>
        /// Evaluates a branch.
        /// </summary>
        /// <param name="branch">Branch.</param>
        /// <param name="node">Node to evaluate.</param>
        /// <returns>Result of the evaluation.</returns>
        private NodeState EvaluateBranch(State branch, Node node)
        {
            NodeState result = node != null ? node.Evaluate() : NodeState.Success;
            // The node is returned to its initial state if the node evaluation is finished.
            if (result == NodeState.Success || result == NodeState.Failure)
            {
                m_state = State.Condition;
            }
            else
            {
                m_state = branch;
            }

            return result;
        }
    }
}
