
namespace Rogue.Core.BHT
{
    public class NodeInvert : NodeDecorator
    {
        public override NodeState Evaluate()
        {
            if (m_activeIndex < 0)
            {
                m_activeIndex = 0;
            }

            switch (m_child.Evaluate())
            {
                case NodeState.Failure:
                {
                    m_activeIndex = -1;
                    return NodeState.Success;
                }

                case NodeState.Success:
                {
                    m_activeIndex = -1;
                    return NodeState.Failure;
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
