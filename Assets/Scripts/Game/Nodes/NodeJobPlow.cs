﻿using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeJobPlow : NodeJob<Jobs.JobPlow>
    {
        public override NodeState Evaluate()
        {
            if (base.Evaluate() == NodeState.Success)
            {
                return NodeState.Failure;
            }

            SetVar("targetLocation",    m_job.Location);
            SetVar("targetId",          m_job.cropId);

            return NodeState.Success;
        }
    }
}
