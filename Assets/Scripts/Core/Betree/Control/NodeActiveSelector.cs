using System.Collections.Generic;

namespace Rogue.Core.Betree
{
    public class NodeActiveSelector : NodeComposite
    {
        public NodeActiveSelector() : this(null) {}

        public NodeActiveSelector(params Node[] children) : this ((IEnumerable<Node>)children) {}

        public NodeActiveSelector(IEnumerable<Node> children) : base(children) {}

        public override void OnInit() {}

        public override void OnQuit(NodeState state)
        {
            foreach (Node node in m_children)
            { 
                node.Halt();
            }
        }

        public override NodeState OnUpdate()
        {
            var cursor = m_children.GetEnumerator();

            while (cursor.MoveNext())
            {
                NodeState state = cursor.Current.Tick();

                switch (state)
                {
                    case NodeState.Success: return NodeState.Success;
                    case NodeState.Failure: break;
                    case NodeState.Running:
                    {
                        while (cursor.MoveNext())
                        {
                            cursor.Current.Halt();
                        }

                        return NodeState.Running;
                    }
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
