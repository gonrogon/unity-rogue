using System.Collections.Generic;
using UnityEngine;
using Rogue.Core;
using Rogue.Core.Betree;
using GG.Mathe;

namespace Rogue.Game.Betree
{
    public class NodeSelectJob : NodeActionBase
    {
        private const float JobMemory = 5.0f;

        private readonly Dictionary<int, float> m_visited = new();

        public override NodeState OnUpdate()
        {
            UpdateVisitedJobs();

            var  position = Query.GetPosition(AgentState.eid);
            if (!position)
            {
                return NodeState.Failure;
            }

            int job = Context.Jobs.FindNearestJob(position.value, job => 
            {
                if (m_visited.ContainsKey(job.Id))
                {
                    return false;
                }

                return true;
            });

            if (job < 0)
            {
                return NodeState.Failure;
            }

            m_visited.Add(job, Time.time);
            Blackboard.Set("job", Context.Jobs.At(job));

            return NodeState.Success;
        }

        private void UpdateVisitedJobs()
        {
            List<int> old = new();

            foreach (var pair in m_visited)
            {
                if (Time.time - pair.Value > JobMemory)
                {
                    old.Add(pair.Key);
                }
            }

            foreach (int job in old)
            {
                m_visited.Remove(job);
            }
        }
    }
}
