using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeBuild : NodeAgentAction
    {
        private Ident m_entity;

        public NodeBuild(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            object objJob = GetGlobalVar("targetJob");
            if (objJob == null)
            {
                return NodeState.Failure;
            }

            var job = (Jobs.JobBuilding)Context.Jobs.At((int)objJob);

            var msg = new Msg.ActionBuild(job.building, 20);

            Context.World.Send(m_entity, msg);
            AddActionCost(msg.cost);

            if (msg.done)
            {
                return NodeState.Failure;
            }

            return NodeState.Success;
        }
    }
}
