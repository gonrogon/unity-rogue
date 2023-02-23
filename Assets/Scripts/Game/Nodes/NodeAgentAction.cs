using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeAgentAction : NodeAction
    {
        public AgentBHT.State GetAgentState() => (AgentBHT.State)GetGlobalVar("state");

        public void AddActionCost(int cost)
        {
            GetAgentState().cost += cost;
        }
    }
}
