namespace Rogue.Core.Betree
{
    public abstract class NodeCompositeIf : Node
    {
        private enum IfState
        {
            Testing,
            Success,
            Failure
        }

        private IfState m_if = IfState.Testing;

        public abstract NodeState OnCondition();

        public abstract NodeState OnSuccess();

        public abstract NodeState OnFailure();

        public override void OnInit()
        {
            m_if = IfState.Testing;
        }

        public override NodeState OnUpdate() => Step();

        public NodeState Step()
        {
            switch (m_if)
            {
                // Execute the success or failure branches.
                case IfState.Success: { return OnSuccess(); }
                case IfState.Failure: { return OnFailure(); }
                // Test the condition.
                default:
                {
                    NodeState test = OnCondition();

                    switch (test)
                    {
                        // Success and failure branches are evaluated in the same tick in which the condition finishes.
                        case NodeState.Success: m_if = IfState.Success; return Step();
                        case NodeState.Failure: m_if = IfState.Failure; return Step();
                        // Conditions can last for more than one tick.
                        case NodeState.Running: return NodeState.Running;
                        // Unexpected value.
                        default:
                        {
                            return test;
                        }
                    }
                }
            }
        }
    }
}
