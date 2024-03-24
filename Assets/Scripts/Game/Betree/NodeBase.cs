using Rogue.Core.Betree;

namespace Rogue.Game.Betree
{
    public class NodeBase : Node
    {
        public AgentBase.State AgentState => Blackboard.Get<AgentBase.State>("state");

        public void AddActionCost(int cost)
        {
            AgentState.cost += cost;
        }
    }
}
