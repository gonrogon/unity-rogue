
namespace Rogue.Core.BHT
{
    public class NodeIgnore : NodeDecorator
    {
        private readonly bool m_success = true;

        public NodeIgnore() : this(true, null) {}

        public NodeIgnore(bool success, Node child) : base(child) {}

        public override NodeState Evaluate()
        {
            if (m_activeIndex < 0)
            {
                m_activeIndex = 0;
            }

            switch (m_child.Evaluate())
            {
                case NodeState.Success:
                case NodeState.Failure:
                {
                    m_activeIndex = -1;
                    return m_success ? NodeState.Success : NodeState.Failure;
                }

                default:
                {
                    m_activeIndex =  0;
                    return NodeState.Running;
                }
            }
        }
    }
}
