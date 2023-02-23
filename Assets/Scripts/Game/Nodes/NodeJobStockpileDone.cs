﻿using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeJobStockpileDone : NodeJob<Jobs.JobStockpile>
    {
        public override NodeState Evaluate()
        {
            if (base.Evaluate() == NodeState.Success)
            {
                return NodeState.Failure;
            }

            m_job.Complete();

            return NodeState.Success;
        }
    }
}
