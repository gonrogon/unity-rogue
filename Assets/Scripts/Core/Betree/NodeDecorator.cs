namespace Rogue.Core.Betree
{
    public abstract class NodeDecorator : Node
    {
        protected Node m_child = null;

        public NodeDecorator() {}

        public NodeDecorator(Node child)
        {
            m_child = child;
            OnAttached(Tree, Parent);
        }

        public override void OnAttached(Tree tree, Node parent)
        {
            base.   OnAttached(tree, parent);
            m_child.OnAttached(tree, this);
        }

        public override void OnQuit(NodeState state)
        {
            if (state != NodeState.Aborted)
            {
                return;
            }

            m_child.Halt();
        }
    }
}
