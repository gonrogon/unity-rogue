using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeJobTrade : NodeJob<Jobs.JobTrade>
    {
        public NodeJobTrade(Node run, Node cancel) : base(run, cancel) {}

        public override void OnJobStart()
        {
            Blackboard.Set("itemId",         m_job.item);
            Blackboard.Set("itemLocation",   m_job.itemLocation);
            Blackboard.Set("marketId",       m_job.market);
            Blackboard.Set("marketLocation", m_job.marketLocation);
            ReserveJob();
        }

        public override void OnJobSuccess()
        {
            CompleteJob();
        }
    }
}
