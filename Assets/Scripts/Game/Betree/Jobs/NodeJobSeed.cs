using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeJobSeed : NodeJob<Jobs.JobSeed>
    {
        public NodeJobSeed(Node run, Node cancel) : base(run, cancel) {}

        public override void OnJobStart()
        {
            Blackboard.Set("crop",         m_job.cropId);
            Blackboard.Set("cropLocation", m_job.Location);
            ReserveJob();
        }

        public override void OnJobSuccess()
        {
            CompleteJob();
        }
    }
}
