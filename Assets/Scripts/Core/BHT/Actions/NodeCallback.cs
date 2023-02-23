namespace Rogue.Core.BHT
{
    public class NodeCallback : NodeAction
    {
        public System.Action<Node> callback;

        public override NodeState Evaluate()
        {
            callback?.Invoke(this);

            return NodeState.Success;
        }
    }
}
