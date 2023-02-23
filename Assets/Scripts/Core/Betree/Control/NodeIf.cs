namespace Rogue.Core.Betree
{
    public class NodeIf : NodeCompositeIf
    {
        protected Node m_condition;

        protected Node m_success;

        protected Node m_failure;

        public NodeIf(Node condition, Node success, Node failure)
        {
            m_condition = condition;
            m_success   = success;
            m_failure   = failure;
        }

        public override void OnAttached(Tree tree, Node parent)
        {
            base.OnAttached(tree, parent);

            m_condition?.OnAttached(tree, this);
            m_success  ?.OnAttached(tree, this);
            m_failure  ?.OnAttached(tree, this);
        }

        public override void OnQuit(NodeState state)
        {
            if (state != NodeState.Aborted)
            {
                return;
            }

            m_condition?.Halt();
            m_success  ?.Halt();
            m_failure  ?.Halt();
        }

        public override NodeState OnCondition() => m_condition.Tick();

        public override NodeState OnSuccess() => m_success != null ? m_success.Tick() : NodeState.Success;

        public override NodeState OnFailure() => m_failure != null ? m_failure.Tick() : NodeState.Success;
    }
}
