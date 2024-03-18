using System;

namespace Rogue.Game
{
    public class Clock
    {
        private int m_ticksPerHour;

        private int m_hoursPerDay;

        private int m_daysPerMonth;

        private int m_monthsPerYear;

        private int m_elapsed;

        private Date m_date = new();

        public Date Date => m_date;

        public Clock() : this(3600000, 24, 30, 12) {}

        public Clock(int ticksPerHour, int hoursPerDay, int daysPerMonth, int monthsPerYear)
        {
            Reset(ticksPerHour, hoursPerDay , daysPerMonth, monthsPerYear);
        }

        public void Step(int elapsed)
        {
            m_elapsed += elapsed;
            m_date     = TicksToDate(this, m_elapsed);
        }

        public void Reset(int tickPerHour, int hoursPerDay, int daysPerMonth, int monthPerYear)
        {
            m_ticksPerHour  = tickPerHour;
            m_hoursPerDay   = hoursPerDay;
            m_daysPerMonth  = daysPerMonth;
            m_monthsPerYear = monthPerYear;
            m_elapsed       = 0;
        }

        /// <summary>
        /// Calculates the differnce between two dates.
        /// </summary>
        /// <param name="clock">Clock.</param>
        /// <param name="a">First date.</param>
        /// <param name="b">Second date.</param>
        /// <returns>Date with the difference.</returns>
        public static Date Diff(Clock clock, Date a, Date b)
        {
            return TicksToDate(clock, Math.Abs(DateToTicks(clock, a) - DateToTicks(clock, b)));
        }

        /// <summary>
        /// Converts ticks to date.
        /// </summary>
        /// <param name="clock">Clock.</param>
        /// <param name="ticks">Ticks.</param>
        /// <returns>Date.</returns>
        public static Date TicksToDate(Clock clock, int ticks)
        {
            Date date = new () { tick = ticks };

            if (date.tick  > clock.m_ticksPerHour)  { date.hour  = date.tick  / clock.m_ticksPerHour;  date.tick  %= clock.m_ticksPerHour;  }
            if (date.hour  > clock.m_hoursPerDay)   { date.day   = date.hour  / clock.m_hoursPerDay;   date.hour  %= clock.m_hoursPerDay;   }
            if (date.day   > clock.m_daysPerMonth)  { date.month = date.day   / clock.m_daysPerMonth;  date.day   %= clock.m_daysPerMonth;  }
            if (date.month > clock.m_monthsPerYear) { date.year  = date.month / clock.m_monthsPerYear; date.month %= clock.m_monthsPerYear; }

            return date;
        }

        /// <summary>
        /// Converts a date to ticks.
        /// </summary>
        /// <param name="clock">Clock.</param>
        /// <param name="date">Date.</param>
        /// <returns>Ticks.</returns>
        public static int DateToTicks(Clock clock, Date date)
        {
            return 
                date.tick  + 
                date.hour  * clock.m_ticksPerHour  +
                date.day   * clock.m_hoursPerDay   * clock.m_ticksPerHour +
                date.month * clock.m_daysPerMonth  * clock.m_hoursPerDay  * clock.m_ticksPerHour +
                date.year  * clock.m_monthsPerYear * clock.m_daysPerMonth * clock.m_hoursPerDay  * clock.m_ticksPerHour;
        }
    }
}
