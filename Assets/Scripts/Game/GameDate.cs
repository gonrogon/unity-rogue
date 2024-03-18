using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Rogue.Game
{
    public struct Date
    {
        public int tick;

        public int hour;

        public int day;

        public int month;

        public int year;

        public void Diff(Date date)
        {
            int years  = Math.Abs(year - date.year);
            int months = Math.Abs(year - date.month);
        }
    }

    public class GameDate
    {
        private int mDaysPerYear = 1;

        private int mHoursPerDay = 1;

        private int mTicksPerHour = 1;

        public int Years { get; private set; } = 0;

        public int Days { get; private set; } = 0;

        public int Hours { get; private set; } = 0;

        public int Ticks { get; private set; } = 0;

        public void Setup(int ticksPerHour, int hoursPerDay, int daysPerYear)
        {
            Years = 0;
            Days  = 0;
            Hours = 0;
            Ticks = 0;
            mTicksPerHour = ticksPerHour;
            mHoursPerDay  = hoursPerDay;
            mDaysPerYear  = daysPerYear;
        }

        /// <summary>
        /// Adds the elapsed ticks.
        /// </summary>
        /// <param name="elapsed">Elapsed time.</param>
        public void Update(int elapsed)
        {
            Ticks = elapsed;

            if (Ticks > mTicksPerHour) { Hours = Ticks / mTicksPerHour; Ticks %= mTicksPerHour; }
            if (Hours > mHoursPerDay)  { Days  = Hours / mHoursPerDay;  Hours %= mHoursPerDay;  }
            if (Days  > mDaysPerYear)  { Years = Days  / mDaysPerYear;  Days  %= mDaysPerYear;  }
        }
    }
}