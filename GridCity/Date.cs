using GridCity.Utility.Units;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace GridCity {
    class Clock : ITickable {
        public static Clock createRandomClockBetween(Clock c1, Clock c2) {
            float diff = Math.Abs(c1.Time.Seconds - c2.Time.Seconds);
            double random = Utility.RandomGenerator.get();
            float offset = diff * (float)random;
            return new Clock(new Time(Math.Min(c1.Time.Seconds, c2.Time.Seconds) + offset));
        }
        private Time Time { get; set; } = new Time(0);
        public int Hour => ((int)Math.Floor((float)Time) / 3600) % 24;
        public int Minute => ((int)Math.Floor((float)Time) % 3600) / 60;
        public int Second => (int)Math.Floor((float)Time) % 60;
        readonly float k_secondsADay = 60 * 60 * 24;
        public Clock(uint hour, uint minute, uint second) {
            Contract.Requires(minute < 60 && second < 60);
            Time.Seconds = second + 60 * minute + 3600 * hour;
            if (Time.Seconds > k_secondsADay) {
                Time.Seconds %= k_secondsADay;
            }
        }
        public Clock(Time seconds) {
            Time = seconds;
            if (Time.Seconds > k_secondsADay) {
                Time.Seconds %= k_secondsADay;
            }
        }
        public Time getDifference(Clock other) {
            if (other.Time <= Time) {
                return Time - other.Time;
            } else {
                return new Time(k_secondsADay - (other.Time - Time).Seconds);
            }
        }
        public override string ToString() {
            var hours = Hour;
            var minutes = Minute;
            var seconds = Second;
            return (hours < 10 ? "0" : "") + hours + ":" + (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
        }
        public bool tick(Time elapsed) {
            Time += elapsed;
            if (Time.Seconds > k_secondsADay) {
                Time.Seconds %= k_secondsADay;
            }
            return true;
        }
        public static Clock operator +(Clock c, Time t) {
            return new Clock(c.Time + t);
        }
        public static Clock operator -(Clock c, Time t) {
            Contract.Requires(c.Time >= t);
            return new Clock(c.Time - t);
        }
    }

    class Date : ITickable {
        public enum Weekday { MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY, SUNDAY }
        private string weekdayToString(Weekday day) {
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
        public Clock CurrentClock { get; set; } = new Clock(new Time(0));
        public Weekday CurrentDay { get; set; } = Weekday.MONDAY;
        public uint SpeedFactor { get; set; } = 1;
        public Date() {

        }
        public Date(Weekday day, Clock time) {
            CurrentClock = time;
            CurrentDay = day;
        }
        public readonly uint k_secondsADay = 60 * 60 * 24;
        public bool tick(Time elapsed) {
            Debug.Assert(elapsed.Seconds < k_secondsADay);
            var oldHour = CurrentClock.Hour;
            CurrentClock += new Time(SpeedFactor * elapsed.Seconds);
            if (oldHour > CurrentClock.Hour) {
                CurrentDay = getNextDay(CurrentDay);
            }
            return true;
        }
        private Weekday getNextDay(Weekday day) {
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
        public override string ToString() {
            return weekdayToString(CurrentDay) + ", " + CurrentClock;
        }
    }
}
