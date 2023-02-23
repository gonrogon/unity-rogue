using System.Collections.Generic;

namespace Rogue.Core.Betree
{
    public abstract class NodeComposite : Node
    {
        protected List<Node> m_children = null;

        protected NodeComposite() : this(null) {}

        protected NodeComposite(params Node[] children) : this ((IEnumerable<Node>)children) {}

        protected NodeComposite(IEnumerable<Node> children)
        {
            if (children != null)
            {
                m_children = new (children);
            }
            else
            {
                m_children = new ();
            }

            OnAttached(Tree, Parent);
        }

        public override void OnAttached(Tree tree, Node parent)
        {
            base.OnAttached(tree, parent);
            
            foreach (Node child in m_children)
            {
                child.OnAttached(tree, this);
            }
        }

        public void Add(Node node)
        {
            m_children.Add(node);
            node.OnAttached(Tree, this);
        }

        public override void OnQuit(NodeState state)
        {
            if (state != NodeState.Aborted)
            {
                return;
            }

            foreach (Node child in m_children)
            {
                child.Halt();
            }
        }
    }
}
