namespace Rogue.Core.BHT
{
    public class NodeRepeat : NodeDecorator
    {
        public NodeRepeat() : base() {}

        public NodeRepeat(Node child) : base(child) {}

        public override NodeState Evaluate()
        {
            if (m_child == null)
            {
                return NodeState.Success;
            }

            if (m_child.Evaluate() == NodeState.Failure)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }
    }
}
