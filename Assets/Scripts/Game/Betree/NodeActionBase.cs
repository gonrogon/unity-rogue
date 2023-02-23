using Rogue.Core.Betree;

namespace Rogue.Game.Betree
{
    public class NodeActionBase : NodeAction
    {
        public AgentBetree.State AgentState => Blackboard.Get<AgentBetree.State>("state");

        public void AddActionCost(int cost)
        {
            AgentState.cost += cost;
        }
    }
}
