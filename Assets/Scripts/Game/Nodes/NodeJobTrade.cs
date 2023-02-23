using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeJobTrade : NodeJob<Jobs.JobTrade>
    {
        public override NodeState Evaluate()
        {
            if (base.Evaluate() == NodeState.Success)
            {
                return NodeState.Failure;
            }

            SetVar("targetLocation", m_job.itemLocation);
            SetVar("targetId",       m_job.item);
            SetVar("marketId",       m_job.market);
            SetVar("marketLocation", m_job.marketLocation);

            return NodeState.Success;
        }
    }
}
