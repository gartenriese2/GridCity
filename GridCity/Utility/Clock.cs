namespace GridCity.Utility {

    using System;
    using System.Diagnostics;
    using Units;

    internal class Clock : ITickable {

        private readonly float secondsADay = 60 * 60 * 24;

        public Clock(uint hour, uint minute = 0, uint second = 0) {
            Debug.Assert(minute < 60 && second < 60, "minute and second must be smaller than 60");
            Time.Seconds = second + (60 * minute) + (3600 * hour);
            if (Time.Seconds > secondsADay) {
                Time.Seconds %= secondsADay;
            }
        }

        public Clock(Time seconds) {
            Time = seconds;
            if (Time.Seconds > secondsADay) {
                Time.Seconds %= secondsADay;
            }
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public int Hour => ((int)Math.Floor((float)Time) / 3600) % 24;

        public int Minute => ((int)Math.Floor((float)Time) % 3600) / 60;

        public int Second => (int)Math.Floor((float)Time) % 60;

        private Time Time { get; set; } = new Time(0);

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public static Clock CreateRandomClockBetween(Clock c1, Clock c2) {
            float diff = Math.Abs(c1.Time.Seconds - c2.Time.Seconds);
            double random = Utility.RandomGenerator.Get();
            float offset = diff * (float)random;
            return new Clock(new Time(Math.Min(c1.Time.Seconds, c2.Time.Seconds) + offset));
        }

        public static Clock operator +(Clock c, Time t) {
            return new Clock(c.Time + t);
        }

        public static Clock operator -(Clock c, Time t) {
            Debug.Assert(c.Time >= t, "Can't subtract a time from a time that is smaller");
            return new Clock(c.Time - t);
        }

        public override string ToString() {
            var hours = Hour;
            var minutes = Minute;
            var seconds = Second;
            return (hours < 10 ? "0" : string.Empty) + hours + ":" + (minutes < 10 ? "0" : string.Empty) + minutes + ":" + (seconds < 10 ? "0" : string.Empty) + seconds;
        }

        public Time GetDifference(Clock other) {
            if (other.Time <= Time) {
                return Time - other.Time;
            } else {
                return new Time(secondsADay - (other.Time - Time).Seconds);
            }
        }
        
        public bool Tick(Time elapsed) {
            Time += elapsed;
            if (Time.Seconds > secondsADay) {
                Time.Seconds %= secondsADay;
            }

            return true;
        }
    }
}
