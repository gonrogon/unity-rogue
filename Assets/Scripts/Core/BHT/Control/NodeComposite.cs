using System;
using System.Collections.Generic;

namespace Rogue.Core.BHT
{
    public abstract class NodeComposite : Node
    {
        /// <summary>
        /// List of child nodes.
        /// </summary>
        protected List<Node> m_children = null;

        /// <summary>
        /// Index of the active child node or less than zero if there is not an active child node.
        /// </summary>
        protected int m_active = -1;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeComposite() : this((IEnumerable<Node>)null) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodes">List of child nodes</param>
        protected NodeComposite(params Node[] nodes) : this((IEnumerable<Node>)nodes) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodes">List of child nodes.</param>
        protected NodeComposite(List<Node> nodes) : this((IEnumerable<Node>)nodes) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodes">List of child nodes.</param>
        protected NodeComposite(IEnumerable<Node> nodes)
        {
            if (nodes == null)
            { 
                m_children = new ();
            }
            else
            {
                m_children = new (nodes);
            }

            foreach (Node child in m_children)
            {
                child.OnAttached(this);
            }
        }

        public override void OnAttached(Node parent)
        {
            base.OnAttached(parent);

            foreach (Node child in m_children)
            {
                child.OnAttached(this);
            }
        }

        /// <summary>
        /// Adds a new child node.
        /// </summary>
        /// <param name="child">Child node to add.</param>
        public virtual void Attach(Node child)
        {
            m_children.Add(child);
            child.OnAttached(this);
        }

        /// <summary>
        /// Moves the active index to the first child node.
        /// </summary>
        protected void MoveToBeg()
        {
            m_active = 0;
        }

        /// <summary>
        /// Moves the active index to the last child node.
        /// </summary>
        protected void MoveToEnd()
        {
            m_active = m_children.Count - 1;
        }

        /// <summary>
        /// Moves the active index to the previous child node.
        /// </summary>
        /// <returns>True if there is a previous child node; otherwise, false.</returns>
        protected bool MoveToPrev()
        {
            if (m_active <= 0)
            {
                m_active = 0;
                return false;
            }

            m_active--;
            return true;
        }

        /// <summary>
        /// Moves the active index to the next child node.
        /// </summary>
        /// <returns>True if there is a next child node; otherwise, false.</returns>
        protected bool MoveToNext()
        {
            if (m_active >= m_children.Count - 1)
            {
                m_active = 0;
                return false;
            }

            m_active++;
            return true;
        }

        /// <summary>
        /// Resets the node to its initial state.
        /// </summary>
        public override void Reset()
        {
            m_active = -1;
        }
    }
}
