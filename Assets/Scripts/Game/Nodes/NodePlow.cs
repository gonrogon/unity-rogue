using Rogue.Core;
using Rogue.Core.BHT;
using Rogue.Game;
using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodePlow : NodeAgentAction
    {
        private Ident m_entity;

        public NodePlow(Ident entity)
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

            var job = (Jobs.JobPlow)Context.Jobs.At((int)objJob);

            var msg = new Msg.ActionPlow(job.cropId, job.Location);

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
