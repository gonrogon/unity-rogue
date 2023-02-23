
namespace Rogue.Core.BHT
{
    public class NodeFailure : NodeAction
    {
        public override NodeState Evaluate() => NodeState.Failure;
    }
}
