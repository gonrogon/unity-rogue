                                                                                                                                                                                                                                 using System;
using System.Collections.Generic;

namespace Rogue.Core.BHT
{
    /// <summary>
    /// Defines a node that evaluates its child nodes sequentially until one succeeds.
    /// 
    /// Returns success if at least one node succeeds; otherwise, failure.
    /// </summary>
    public class NodeSelect : NodeComposite
    {
        public NodeSelect() {}

        public NodeSelect(params Node[] nodes) : base(nodes) {}

        public NodeSelect(List<Node> nodes) : base(nodes) {}

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
                        if (!MoveToNext())
                        {
                            done = true;    
                        }
                    }
                    break;

                    case NodeState.Success: 
                    {
                        done = true;
                        Reset();
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
