using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Game
{
    public interface  IAgent : ISchedulable
    {}

    public abstract class AgentBase : IAgent
    {
        protected SchedulerHandler m_handler = null;

        public virtual void OnScheduleStart(SchedulerHandler handler)
        {
            m_handler = handler;
        }

        public virtual void OnScheduleFinish() {}

        public virtual int OnScheduleTrigger() { return 0; }
    }
}
