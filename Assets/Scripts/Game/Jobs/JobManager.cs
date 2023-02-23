using System;
using System.Collections.Generic;
using GG.Mathe;

namespace Rogue.Game.Jobs
{
    /// <summary>
    /// Defines a manager for the job system.
    /// </summary>
    public class JobManager
    {
        /// <summary>
        /// Dictionary with the peding jobs.
        /// </summary>
        private readonly Dictionary<int, Job> m_jobs = new Dictionary<int, Job>();

        /// <summary>
        /// Identifier to assign to the next job.
        /// </summary>
        private int m_nextJobId = 0;

        /// <summary>
        /// Adds a new job.
        /// </summary>
        /// <param name="job">Job to add.</param>
        /// <returns>Identifier assigned to the job.</returns>
        public int AddJob(Job job)
        {
            job.Setup(m_nextJobId);
            m_jobs[m_nextJobId] = job;
            
            return m_nextJobId++;
        }

        /// <summary>
        /// Gets the by its identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Reference to the job if it exists; otherwise, null.</returns>
        public Job At(int id)
        {
            if (m_jobs.TryGetValue(id, out Job job))
            {
                return job;
            }

            return null;
        }

        /// <summary>
        /// Finds the nearest job to a location.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <returns>Reference to the job if it exists; otherwise, null.</returns>
        public int FindNearestJob(Vec2i location) => FindNearestJob<Job>(location, null);

        /// <summary>
        /// Finds the nearest job to a location.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <returns>Reference to the job if it exists; otherwise, null.</returns>
        public int FindNearestJob(Vec2i location, Func<Job, bool> pred) => FindNearestJob<Job>(location, pred);

        /// <summary>
        /// Finds the nearest job to a location.
        /// </summary>
        /// <typeparam name="T">Type of job.</typeparam>
        /// <param name="location">Location.</param>
        /// <returns>Reference to the job if it exists; otherwise, null.</returns>
        public int FindNearestJob<T>(Vec2i location) where T : Job => FindNearestJob<T>(location, null);

        /// <summary>
        /// Finds the nearest job to a location that satisfies the predicate.
        /// </summary>
        /// <typeparam name="T">Type of job.</typeparam>
        /// <param name="location">Location.</param>
        /// <param name="pred">Predicate.</param>
        /// <returns>Reference to the job if it is found; otherwise, null.</returns>
        public int FindNearestJob<T>(Vec2i location, Func<T, bool> pred) where T : Job
        {
            int jid = -1;
            int min = int.MaxValue;

            foreach (var pair in m_jobs)
            {
                if (pair.Value is T job && (pred == null || pred(job)))
                {
                    if (pair.Value.Reserved || pair.Value.Done)
                    {
                        continue;
                    }

                    int dist = pair.Value.Location.SqrDistance(location);

                    if (dist < min)
                    {
                        jid = pair.Key;
                        min = dist;
                    }
                }
            }

            return jid;
        }

        /// <summary>
        /// Updates the job manager.
        /// </summary>
        public void Update()
        {
            var done = new Queue<int>();

            foreach (var pair in m_jobs)
            {
                if (pair.Value.Done)
                {
                    done.Enqueue(pair.Key);
                }
            }

            while (done.Count > 0)
            {
                int i = done.Dequeue();
                if (i < 0)
                {
                    continue;
                }

                FinalizeJob(m_jobs[i]);
                m_jobs.Remove(i);
            }
        }

        private void FinalizeJob(Job job)
        {
            if (job.Done)
            {
                job.onCompleted?.Invoke(job);
            }
        }
    }
}
