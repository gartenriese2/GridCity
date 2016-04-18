namespace GridCity.Simulation.Time {

    using System;
    using System.Diagnostics;
    using Utility.Units;

    internal class Date : ITickable {

        //---------------------------------------------------------------------
        // Fields
        //---------------------------------------------------------------------
        public readonly uint SecondsADay = 60 * 60 * 24;

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Date(Weekday day, Clock time) {
            CurrentClock = time;
            CurrentDay = day;
        }

        //---------------------------------------------------------------------
        // Enumerations
        //---------------------------------------------------------------------
        public enum Weekday {
            MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY, SUNDAY
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public Clock CurrentClock { get; set; } = Clock.Zero;

        public Weekday CurrentDay { get; set; } = Weekday.MONDAY;

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            return WeekdayToString(CurrentDay) + ", " + CurrentClock;
        }

        public bool Tick(Time elapsedTime) {
            Debug.Assert(elapsedTime.Seconds < SecondsADay, "elapsed seconds should not be more than a whole day");
            var oldHour = CurrentClock.Hours;
            CurrentClock += elapsedTime;
            if (oldHour > CurrentClock.Hours) {
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
