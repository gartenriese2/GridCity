namespace GridCity {

    using System;
    using System.Diagnostics;
    using Utility;
    using Utility.Units;

    internal class Date : ITickable {

        public readonly uint SecondsADay = 60 * 60 * 24;

        public Date(Weekday day, Clock time) {
            CurrentClock = time;
            CurrentDay = day;
        }

        public enum Weekday {
            MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY, SUNDAY
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public Clock CurrentClock { get; set; } = new Clock(new Time(0));

        public Weekday CurrentDay { get; set; } = Weekday.MONDAY;

        public uint SpeedFactor { get; set; } = 1;

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            return WeekdayToString(CurrentDay) + ", " + CurrentClock;
        }

        public bool Tick(Time elapsed) {
            Debug.Assert(elapsed.Seconds < SecondsADay, "elapsed seconds should not be more than a whole day");
            var oldHour = CurrentClock.Hour;
            CurrentClock += new Time(SpeedFactor * elapsed.Seconds);
            if (oldHour > CurrentClock.Hour) {
                CurrentDay = GetNextDay(CurrentDay);
            }

            return true;
        }

        private string WeekdayToString(Weekday day) {
            switch (day) {
                case Weekday.MONDAY:
                    return "Monday";
                case Weekday.TUESDAY:
                    return "Tuesday";
                case Weekday.WEDNESDAY:
                    return "Wednesday";
                case Weekday.THURSDAY:
                    return "Thursday";
                case Weekday.FRIDAY:
                    return "Friday";
                case Weekday.SATURDAY:
                    return "Saturday";
                case Weekday.SUNDAY:
                    return "Sunday";
                default:
                    throw new ArgumentOutOfRangeException("No such weekday");
            }
        }
        
        private Weekday GetNextDay(Weekday day) {
            switch (day) {
                case Weekday.MONDAY:
                    return Weekday.TUESDAY;
                case Weekday.TUESDAY:
                    return Weekday.WEDNESDAY;
                case Weekday.WEDNESDAY:
                    return Weekday.THURSDAY;
                case Weekday.THURSDAY:
                    return Weekday.FRIDAY;
                case Weekday.FRIDAY:
                    return Weekday.SATURDAY;
                case Weekday.SATURDAY:
                    return Weekday.SUNDAY;
                case Weekday.SUNDAY:
                    return Weekday.MONDAY;
                default:
                    throw new ArgumentOutOfRangeException("No such weekday");
            }
        }
    }
}
