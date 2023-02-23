using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeJobStockpile : NodeJob<Jobs.JobStockpile>
    {
        public override NodeState Evaluate()
        {
            if (base.Evaluate() == NodeState.Success)
            {
                return NodeState.Failure;
            }

            SetVar("targetLocation",    m_job.itemLocation);
            SetVar("targetId",          m_job.item);
            SetVar("stockpileLocation", m_job.stockpileLocation);

            return NodeState.Success;
        }
    }
}
