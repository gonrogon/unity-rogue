using System.Collections.Generic;

namespace Rogue.Core.Betree
{
    public class NodeMonitor : NodeParallel
    {
        public new Policy SuccessPolicy 
        {
            get         => base.SuccessPolicy;
            private set => base.SuccessPolicy = value; 
        }

        public new Policy FailurePolicy
        {
            get         => base.FailurePolicy;
            private set => base.FailurePolicy = value;
        }

        public NodeMonitor() : this(null) {}

        public NodeMonitor(params Node[] children) : this((IEnumerable<Node>)children) {}

        public NodeMonitor(IEnumerable<Node> children) : base(Policy.One, Policy.One, children) {}

        public void AddCondition(Node condition)
        {
            m_children.Insert(0, condition);
        }

        public void AddAction(Node action)
        {
            m_children.Add(action);
        }
    }
}
