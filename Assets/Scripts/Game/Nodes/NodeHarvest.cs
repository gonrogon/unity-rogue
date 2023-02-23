using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeHarvest : NodeAgentAction
    {
        private Ident m_entity;

        public NodeHarvest(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            object objJob = FindVar("targetJob");
            if (objJob == null)
            {
                return NodeState.Failure;
            }

            var job = (Jobs.JobHarvest)Context.Jobs.At((int)objJob);

            var msg = new Msg.ActionHarvest(job.cropId, job.Location);
            //msg.job = (int)objJob;

            Context.World.Send(m_entity, msg);
            AddActionCost(msg.cost);

            if (msg.Success)
            {
                return NodeState.Failure;
            }

            return NodeState.Success;
        }
    }
}
