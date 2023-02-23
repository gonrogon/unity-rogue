using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeSeed : NodeAgentAction
    {
        private Ident m_entity;

        public NodeSeed(Ident entity)
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

            var job = (Jobs.JobSeed)Context.Jobs.At((int)objJob);

            var msg = new Msg.ActionSeed(job.cropId, job.Location);
            msg.job = (int)objJob;

            Context.World.Send(m_entity, msg);
            AddActionCost(msg.cost);
            // TODO: If the action fails it should be canceled after some retries.
            if (msg.done)
            {
                return NodeState.Failure;
            }

            return NodeState.Success;
        }
    }
}
