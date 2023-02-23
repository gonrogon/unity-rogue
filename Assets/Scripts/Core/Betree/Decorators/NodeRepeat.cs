namespace Rogue.Core.Betree
{
    public class NodeRepeat : NodeDecorator
    {
        private int m_iterations = 0;

        private int m_counter = 0;

        private bool IsDone => m_iterations >= 0 && m_counter >= m_iterations;

        public NodeRepeat() : this(1, null) {}

        public NodeRepeat(int iterations, Node child) : base(child)
        {
            m_iterations = iterations;
        }

        public override void OnInit()
        {
            m_counter = 0;
        }

        public override NodeState OnUpdate()
        {
            bool done = false;

            while (!done)
            {
                NodeState state = m_child.Tick();

                switch (state)
                {
                    case NodeState.Running: return NodeState.Running;
                    case NodeState.Failure: return NodeState.Failure;
                    case NodeState.Success:
                    {
                        if (m_iterations >= 0 && ++m_counter >= m_iterations)
                        {
                            done = true;
                        }

                        m_child.Reset();
                    }
                    break;
                    // Unexpected state returned by child node.
                    default:
                    {
                        return NodeState.Invalid;
                    }
                }
            }

            return NodeState.Success;
        }
    }
}
