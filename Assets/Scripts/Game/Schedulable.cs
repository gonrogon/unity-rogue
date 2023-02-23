using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Game
{
    public interface ISchedulable
    {
        /// <summary>
        /// Notifies that the object has been scheduled.
        /// </summary>
        void OnScheduleStart(SchedulerHandler handler);

        /// <summary>
        /// Notifies that the object has been finished.
        /// </summary>
        void OnScheduleFinish();

        /// <summary>
        /// Triggers the execution of a new action.
        /// </summary>
        /// <returns>Cost, in game units.</returns>
        int OnScheduleTrigger();
    }
}
