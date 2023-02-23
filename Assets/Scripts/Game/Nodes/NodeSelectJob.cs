using System.Collections.Generic;
using UnityEngine;
using Rogue.Core;
using Rogue.Core.BHT;
using GG.Mathe;

namespace Rogue.Game.Nodes
{
    public class NodeSelectJob : NodeAction
    {
        private static readonly float VisitedJobMemory = 5.0f;

        private Dictionary<int, float> m_visitedJobs = new();

        private Ident m_entity;

        public bool skipOverlap = false;

        public NodeSelectJob(Ident entity)
        {
            m_entity = entity;
        }

        public override NodeState Evaluate()
        {
            UpdateVisitedJobs();

            Vec2i position = Query.GetPosition(m_entity).value;
            int   jid      = Context.Jobs.FindNearestJob(position, (Jobs.Job job) => 
            {
                /*
                if (skipOverlap)
                {
                    if (job.Location == position)
                    {
                        return false;
                    }
                }
                */

                if (Query.MapIsStuck(position, m_entity, job.Location))
                {
                    return false;
                }

                if (m_visitedJobs.ContainsKey(job.Id))
                {
                    return false;
                }

                return true;
            });

            if (jid < 0)
            {
                return NodeState.Failure;
            }

            m_visitedJobs.Add(jid, Time.time); 

            var job = Context.Jobs.At(jid);
            job.Reserve();

            if (job.Done)
            {
                return NodeState.Failure;
            }

            //SetVar("targetPos", job.Location);
            //SetGlobalVar("targetJob", jid);

            SetGlobalVar("targetLocation", job.Location);
            SetGlobalVar("targetJob",      jid);

            return NodeState.Success;
        }

        private void UpdateVisitedJobs()
        {
            List<int> old = new();

            foreach (var pair in m_visitedJobs)
            {
                if (Time.time - pair.Value > VisitedJobMemory)
                {
                    old.Add(pair.Key);
                }
            };

            foreach(int jid in old)
            {
                m_visitedJobs.Remove(jid);
            }
        }
    }
}
