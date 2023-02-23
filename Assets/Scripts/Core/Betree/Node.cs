namespace Rogue.Core.Betree
{
    /// <summary>
    /// Defines a node of the behaviour tree.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Tree.
        /// </summary>
        protected Tree Tree { get; private set; } = null;

        /// <summary>
        /// Parent node.
        /// </summary>
        protected Node Parent { get; private set; } = null;

        /// <summary>
        /// Gets the blackboard.
        /// </summary>
        public Blackboard Blackboard => Tree.Blackboard;

        /// <summary>
        /// Gets the root node.
        /// </summary>
        public Node Root => Tree.Root;

        /// <summary>
        /// Current state.
        /// </summary>
        public NodeState State { get; private set; } = NodeState.Invalid;

        /// <summary>
        /// Flag indicating whether or not the node is terminated.
        /// </summary>
        public bool IsTerminated => State == NodeState.Success || State == NodeState.Failure;

        /// <summary>
        /// Flag indicating whether or not the node is running.
        /// </summary>
        public bool IsRunning => State == NodeState.Running;

        /// <summary>
        /// Notifies the note that it has been attached to a parent node.
        /// </summary>
        /// <param name="parent">Parent node.</param>
        public virtual void OnAttached(Tree tree, Node parent)
        {
            Tree   = tree;
            Parent = parent;
        }

        /// <summary>
        /// Initiate the node.
        /// 
        /// This method is called once immediately before the first call to the update method.
        /// </summary>
        public virtual void OnInit() {}

        /// <summary>
        /// Finalize the node.
        /// 
        /// This method is called once immediately after the update method signals it is no longer running.
        /// </summary>
        /// <param name="state">State that triggered the finalization of the node. Usually the state of the node,
        /// but it may differ in some cases (abort). </param>
        public virtual void OnQuit(NodeState state) {}

        /// <summary>
        /// Updates the node.
        /// 
        /// This method is called exactly once each time the behaviour tree is updated until it signals
        /// it has terminated.
        /// </summary>
        /// <returns>State.</returns>
        public virtual NodeState OnUpdate() => NodeState.Success;

        /// <summary>
        /// Executes a step.
        /// </summary>
        /// <returns>State.</returns>
        public NodeState Tick()
        {
            if (State != NodeState.Running)
            {
                OnInit();
            }

            State = OnUpdate();

            if (State != NodeState.Running)
            {
                OnQuit(State);
            }

            return State;
        }

        /// <summary>
        /// Resets the node.
        /// </summary>
        public void Reset()
        {
            State = NodeState.Invalid;
        }

        /// <summary>
        /// Aborts the node.
        /// </summary>
        public void Abort()
        {
            OnQuit (NodeState.Aborted);
            State = NodeState.Aborted;
        }

        public void Halt()
        {
            if (IsRunning)
            {
                Abort();
            }
        }
    }
}
