using System;

namespace Rogue.Game
{
    public class SchedulerHandler
    {
        /// <summary>
        /// Schedulable handled by this handler.
        /// </summary>
        private ISchedulable m_schedulable = null;

        /// <summary>
        /// Time elapsed, in game units.
        /// </summary>
        private int m_elapsed = 0;

        /// <summary>
        /// Flag indicating whether the scheduling is done or not.
        /// </summary>
        public bool IsDone => m_schedulable == null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="schedulable">Schedulable to handler.</param>
        public SchedulerHandler(ISchedulable schedulable) : this(schedulable, 0) {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="schedulable">Schedulable to handler.</param>
        /// <param name="initial">Initial wait.</param>
        public SchedulerHandler(ISchedulable schedulable, int initial)
        {
            m_schedulable = schedulable;
            m_elapsed     = Math.Max(0, initial);
        }

        /// <summary>
        /// Start the handler.
        /// </summary>
        public void Start()
        {
            m_schedulable.OnScheduleStart(this);
        }

        /// <summary>
        /// Finish the handler.
        /// </summary>
        public void Finish()
        {
            m_schedulable.OnScheduleFinish();
        }

        /// <summary>
        /// Uodate the handler.
        /// </summary>
        /// <param name="elapsed">Time elapsed since the last update.</param>
        /// <returns>Time to the next trigger.</returns>
        public int Update(int elapsed)
        {
            // If the elapsed time is less than zero, the object is waiting to pay the cost of the last action.
            if (m_elapsed <  0)
            {
                m_elapsed += elapsed;
            }
            // Trigger a new action.
            if (m_elapsed >= 0)
            {
                m_elapsed -= m_schedulable.OnScheduleTrigger();
            }
            // Return the time to the next trigger.
            return m_elapsed >= 0 ? 0 : -m_elapsed;
        }

        /// <summary>
        /// Kill the handler.
        /// </summary>
        public void Kill()
        {
            m_schedulable = null;
            m_elapsed     = 0;
        }
    }
}
