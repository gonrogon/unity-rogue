using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.Core.Collections;

namespace Rogue.Game
{
    public class TimeManager
    {
        public delegate void AlarmAction(TimeManager manager, int id);

        private class Alarm
        {
            public enum Mode { Once, Repeat, Done }

            public Mode mode;

            public int start;

            public int wait;

            public int elapsed;

            public AlarmAction action;

            public Alarm(int start, int wait, bool repeat, AlarmAction action)
            {
                this.mode    = repeat ? Mode.Repeat : Mode.Once;
                this.start   = start;
                this.wait    = wait;
                this.elapsed = 0;
                this.action  = action;
            }
        }

        private readonly Clock m_clock = new ();

        private readonly Bag<int, Alarm> m_alarms = new ();

        private int m_nextAlarmId = 0;

        public Date Date => m_clock.Date;

        public void Setup(int ticksPerHour, int hoursPerDay, int daysPerMonth, int monthsPerYear)
        {
            m_clock.Reset(ticksPerHour, hoursPerDay, daysPerMonth, monthsPerYear);
        }

        public int AddAlarm(Date start, Date wait, bool repeat, AlarmAction action)
        {
            int timeToStart = Clock.DateToTicks(m_clock, Clock.Diff(m_clock, m_clock.Date, start));
            int timeToWait  = Clock.DateToTicks(m_clock, wait);

            return AddAlarm(new Alarm(timeToStart, timeToWait, repeat, action));
        }

        public int AddAlarm(int start, int wait, bool repeat, AlarmAction action)
        {
            return AddAlarm(new Alarm(start, wait, repeat, action));
        }

        private int AddAlarm(Alarm alarm)
        {
            m_alarms.Add(m_nextAlarmId, alarm);
            return m_nextAlarmId++;
        }

        public void RemoveAlarm(int id)
        {
            int index = m_alarms.FindFirst(id);
            if (index < 0)
            { 
                return;
            }
            // Mark the alarm as done.
            m_alarms.At(index).Value.mode = Alarm.Mode.Done;
        }

        public void Step(int elapsed)
        {
            m_clock.Step(elapsed);
            // Update the alarms and remove the finished ones.
            m_alarms.RemoveAll(pair => 
            {
                if (UpdateAlarm(pair.Key, pair.Value, elapsed))
                {
                    return true;
                }

                return false;
            });
        }

        private bool UpdateAlarm(int id, Alarm alarm, int elapsed)
        {
            if (alarm.mode == Alarm.Mode.Done) 
            {
                return true;
            }

            alarm.elapsed += elapsed;
            
            if (alarm.elapsed > alarm.wait + alarm.start)
            {
                // The initial wait is done only once.
                if (alarm.start > 0)
                {
                    alarm.elapsed -= alarm.start;
                    alarm.start    = 0;
                }

                alarm.elapsed %= alarm.wait;
                alarm.action(this, id);
                // Consume the alarm.
                if (alarm.mode != Alarm.Mode.Repeat)
                {
                    alarm.mode   = Alarm.Mode.Done;
                    alarm.action = null;
                }
            }

            return false;
        }
    }
}
