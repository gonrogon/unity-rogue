using System;
using System.Collections.Generic;

namespace Rogue.Core.BHT
{
    /// <summary>
    /// Defines a node that evaluates its child nodes sequentially until one fails.
    /// 
    /// Returns success if all the nodes succeed; otherwise, failure.
    /// </summary>
    public class NodeSequence : NodeComposite
    {
        public NodeSequence() : base() {}

        public NodeSequence(params Node[] nodes) : base(nodes) {}

        public NodeSequence(List<Node> nodes) : base(nodes) {}

        public override NodeState Evaluate()
        {
            if (m_active < 0)
            {
                MoveToBeg();
            }
                        
            var  result = NodeState.Success;
            bool done   = false;

            for (int i = m_active; i < m_children.Count && !done; i++)
            {
                result = m_children[i].Evaluate();

                switch (result)
                {
                    case NodeState.Failure:
                    {
                        done = true;
                        Reset();    
                    }
                    break;

                    case NodeState.Success:
                    {
                        if (!MoveToNext())
                        {
                            done = true;
                        }
                    }
                    break;

                    case NodeState.Running:
                    {
                        done = true;
                    }
                    break;
                }
            }

            return result;
        }
    }
}
