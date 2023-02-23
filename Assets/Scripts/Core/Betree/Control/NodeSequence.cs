using System.Collections.Generic;

namespace Rogue.Core.Betree
{
    public class NodeSequence : NodeComposite
    {
        //private List<Node>.Enumerator m_cursor = default;

        private int m_cur = 0;

        public NodeSequence() : this(null) {}

        public NodeSequence(params Node[] children) : this ((IEnumerable<Node>)children) {}

        public NodeSequence(IEnumerable<Node> children) : base(children) {}

        public override void OnInit()
        {
            //m_cursor = m_children.GetEnumerator();
            m_cur = 0;
        }

        public override NodeState OnUpdate()
        {
            //while (true)
            while (m_cur < m_children.Count)
            {
                //NodeState state = m_cursor.Current.Tick();
                NodeState state = m_children[m_cur].Tick();

                switch (state)
                {
                    case NodeState.Failure: return NodeState.Failure;
                    case NodeState.Running: return NodeState.Running;
                    case NodeState.Success:
                    {
                        ++m_cur;
                        /*
                        if (!m_cursor.MoveNext())
                        {
                            return NodeState.Success;
                        }
                        */
                    }
                    break;
                    // Unexpected state returned by child node.
                    default:
                    {
                        return NodeState.Invalid;
                    }
                }
            }
            // The end of the list has been reached without finding a failed node.
            return NodeState.Success;
        }
    }
}
