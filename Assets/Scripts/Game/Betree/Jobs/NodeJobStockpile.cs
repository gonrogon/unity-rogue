using UnityEngine;
using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeJobStockpile : NodeJob<Jobs.JobStockpile>
    {
        public NodeJobStockpile(Node run, Node cancel) : base(run, cancel) {}

        public override void OnJobStart()
        {
            // Parameters.
            Blackboard.Set("itemId",            m_job.item);
            Blackboard.Set("itemLocation",      m_job.itemLocation);
            Blackboard.Set("stockpile",         m_job.stockpile);
            Blackboard.Set("stockpileLocation", m_job.stockpileLocation);
            ReserveJob();
        }

        public override void OnJobSuccess()
        {
            CompleteJob();
        }

        public override void OnJobFailure()
        {
            Ident eid = AgentState.eid;
            // Check if the item is already in the inventory to drop it.
            if (Query.IsInInventory(AgentState.eid, m_job.item))
            {
                var  drop = Context.World.Send(eid, new Msg.Drop(m_job.item, Query.GetPosition(eid).value)).done;
                if (!drop)
                {
                    Debug.Log("Unable to drop a stockpile failed job");
                }

                CompleteJob();
            }
            // The item has not been picked up so the job is just released.
            else
            {
                ReleaseJob();
            }
        }
    }
}
