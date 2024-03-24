using Rogue.Core.Betree;

namespace Rogue.Game.Betree
{
    public class NodeActionBase : NodeAction
    {
        public AgentBase.State AgentState => Blackboard.Get<AgentBase.State>("state");

        public void AddActionCost(int cost)
        {
            AgentState.cost += cost;
        }
    }
}
