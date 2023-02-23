
namespace Rogue.Core.BHT
{
    public abstract class NodeCondition : Node
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        protected NodeCondition() {}

        public override NodeState Evaluate()
        {
            return DoTest() ? NodeState.Success : NodeState.Failure;
        }

        /// <summary>
        /// Tests the condition.
        /// </summary>
        /// <returns>True if the condition is satisfied; otherwise, false.</returns>
        protected abstract bool DoTest();
    }
}
