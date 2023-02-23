using System.Collections.Generic;

namespace Rogue.Core.Betree
{
    public class NodeSelector : NodeComposite
    {
        //private List<Node>.Enumerator m_cursor = default;

        private int m_cur = 0;

        public NodeSelector() : this(null) {}

        public NodeSelector(params Node[] children) : this ((IEnumerable<Node>)children) {}

        public NodeSelector(IEnumerable<Node> children) : base(children) {}

        public override void OnInit()
        {
            //m_cursor = m_children.GetEnumerator();
            m_cur = 0;
        }

        public override NodeState OnUpdate()
        {
            //while (m_cursor.MoveNext())
            while (m_cur < m_children.Count)
            {
                //NodeState state = m_cursor.Current.Tick();
                NodeState state = m_children[m_cur].Tick();

                switch (state)
                {
                    case NodeState.Success: return NodeState.Success;
                    case NodeState.Running: return NodeState.Running;
                    case NodeState.Failure: 
                    {
                        ++m_cur;
                    }
                    break;
                    // Unexpected state returned by child node.
                    default:
                    {
                        return NodeState.Invalid;
                    }
                }
            }
            // The end of the list has been reached without finding a successful node.
            return NodeState.Failure;
        }
    }
}
