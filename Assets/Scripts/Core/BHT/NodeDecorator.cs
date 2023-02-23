namespace Rogue.Core.BHT
{
    public abstract class NodeDecorator : Node
    {
        protected Node m_child = null;

        protected int m_activeIndex = -1;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeDecorator() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="child">Child node to set.</param>
        public NodeDecorator(Node child)
        {
            Attach(child);
        }

        public override void OnAttached(Node parent)
        {
            base    .OnAttached(parent);
            m_child?.OnAttached(this);
        }

        /// <summary>
        /// Sets the child node.
        /// </summary>
        /// <param name="child">Child node to set.</param>
        public virtual void Attach(Node child)
        {
            m_child = child;
            child?.OnAttached(this);
        }
    }
}
