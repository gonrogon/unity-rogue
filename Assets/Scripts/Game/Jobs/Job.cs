using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Jobs
{
    /// <summary>
    /// Defines a job.
    /// </summary>
    [System.Serializable]
    public abstract class Job
    {
        /// <summary>
        /// Defines a delegate to notify when the job advance.
        /// </summary>
        /// <param name="job">Job.</param>
        /// <param name="progress">Progress.</param>
        public delegate void OnAdvanced(Job job, int progress);

        /// <summary>
        /// Defines a delegate to notify when the job is completed.
        /// </summary>
        /// <param name="job">Job.</param>
        public delegate void OnCompleted(Job job);

        /// <summary>
        /// Callback to notify the advance of the job.
        /// </summary>
        public OnAdvanced onAdvance;

        /// <summary>
        /// Callback to notify the completion of the job.
        /// </summary>
        public OnCompleted onCompleted;

        /// <summary>
        /// Job identifier.
        /// </summary>
        private int m_id = 0;

        /// <summary>
        /// Number of entities trying to accomplish the job.
        /// </summary>
        private int m_workers = 0;

        /// <summary>
        /// Total amount of work to complete the job.
        /// </summary>
        private int m_totalWork = 0;

        /// <summary>
        /// Amount of work already done.
        /// </summary>
        private int m_work = 0;

        /// <summary>
        /// Location of the job.
        /// </summary>
        private Vec2i m_location = Vec2i.Zero;

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public int Id => m_id;

        /// <summary>
        /// Percentage of the job already completed.
        /// </summary>
        public int Progress => m_work / m_totalWork;

        /// <summary>
        /// Flag indicating whether the job is completed or not.
        /// </summary>
        public bool Done => m_work >= m_totalWork;

        /// <summary>
        /// Flag indicating whether the job is reseverd by an worker or not.
        /// </summary>
        public bool Reserved => m_workers > 0;

        /// <summary>
        /// Location of the job.
        /// </summary>
        public Vec2i Location => m_location;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Job() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="total">Total work needed to complete the job.</param>
        /// <param name="location">Location.</param>
        public Job(int total, Vec2i location)
        {
            m_work      = 0;
            m_totalWork = total;
            m_location  = location;
        }

        /// <summary>
        /// Sets up the job.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Setup(int id)
        {
            m_id = id;
        }

        /// <summary>
        /// Reserves the job.
        /// </summary>
        public void Reserve()
        {
            m_workers++;
        }

        /// <summary>
        /// Releases a job.
        /// </summary>
        public void Release()
        {
            Debug.Assert(m_workers > 0, "A job without workers was relased");
            m_workers--;
        }

        /// <summary>
        /// Advance the job by an amount of work.
        /// </summary>
        /// <param name="amount">Amount to advance.</param>
        /// <returns>True if the job is completed; otherwise, false.</returns>
        public bool Advance(int amount)
        {
            m_work += amount;
            if (m_work >= m_totalWork)
            {
                m_work  = m_totalWork;
            }

            onAdvance?.Invoke(this, Progress);
            return Done;
        }

        /// <summary>
        /// Completes the job.
        /// </summary>
        public void Complete() => m_work = m_totalWork;
    }
}
