using System;
using System.Collections.Generic;

namespace Rogue.Game
{
    public class Scheduler
    {
        /// <summary>
        /// List of handlers.
        /// </summary>
        private List<SchedulerHandler> m_handlers = new List<SchedulerHandler>();

        /// <summary>
        /// Add a new schedulable.
        /// </summary>
        /// <param name="schedulable">Schedulable to add.</param>
        public void Add(ISchedulable schedulable) => Add(schedulable, 0);

        /// <summary>
        /// Add a new schedulable.
        /// </summary>
        /// <param name="schedulable">Schedulable to add.</param>
        /// <param name="initial">Initial wait.</param>
        public void Add(ISchedulable schedulable, int initial)
        {
            if (schedulable == null)
            {
                throw new ArgumentNullException(nameof(schedulable));
            }

            var handler =  new SchedulerHandler(schedulable, initial);
            handler.Start();
            
            m_handlers.Add(handler);
        }

        /// <summary>
        /// Executes a step.
        /// </summary>
        /// <param name="elapsed">Time elapsed in game units.</param>
        public void Step(int elapsed)
        {
            for (int i = 0; i < m_handlers.Count;)
            {
                var handler = m_handlers[i];
                // If the handler is done, finish it and remove it from the list.
                if (handler.IsDone)
                {
                    handler.Finish();
                    Core.ArrayUtil.RemoveAndSwap(m_handlers, i);

                    continue;
                }
                
                handler.Update(elapsed);
                i++;
            }
        }
    }
}
