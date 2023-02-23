using System.Collections.Generic;

namespace Rogue.Core.Betree
{
    public class NodeParallel : NodeComposite
    {
        public enum Policy
        {
            One,
            All,
        }

        public Policy SuccessPolicy { get; set; } = Policy.One;

        public Policy FailurePolicy { get; set; } = Policy.One;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeParallel() 
            :
            this(Policy.One, Policy.One, null) 
        {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="children">List of child nodes.</param>
        public NodeParallel(params Node[] children) 
            :
            this(Policy.One, Policy.One, (IEnumerable<Node>) children)
        {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="children">List of child nodes.</param>
        public NodeParallel(IEnumerable<Node> children) 
            :
            this(Policy.One, Policy.One, children)
        {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="success">Policy for success.</param>
        /// <param name="failure">Policy for failure.</param>
        /// <param name="children">List of child nodes.</param>
        public NodeParallel(Policy success, Policy failure, params Node[] children) 
            :
            this(success, failure, ((IEnumerable<Node>) children))
        {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="success">Policy for success.</param>
        /// <param name="failure">Policy for failure.</param>
        /// <param name="children">List of child nodes.</param>
        public NodeParallel(Policy success, Policy failure, IEnumerable<Node> children)
            :
            base(children)
        {
            SuccessPolicy = success;
            FailurePolicy = failure;
        }

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
            int successCount = 0;
            int failureCount = 0;

            foreach (Node node in m_children)
            {
                NodeState state = node.IsTerminated ? node.State : node.Tick();

                switch (state)
                {
                    case NodeState.Success:
                    {
                        successCount++;

                        if (SuccessPolicy == Policy.One)
                        {
                            return NodeState.Success;
                        }
                    }
                    break;

                    case NodeState.Failure:
                    {
                        failureCount++;

                        if (FailurePolicy == Policy.One)
                        {
                            return NodeState.Failure;
                        }
                    }
                    break;

                    case NodeState.Running: break;
                }
            }

            if (FailurePolicy == Policy.All && failureCount >= m_children.Count) { return NodeState.Failure; }
            if (SuccessPolicy == Policy.All && successCount >= m_children.Count) { return NodeState.Success; }

            return NodeState.Running;
        }
    }
}
