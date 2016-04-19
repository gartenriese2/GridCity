namespace GridCity.Simulation.Time {

    using System;
    using System.Diagnostics;
    using Utility;
    using Utility.Units;

    internal class Clock : ITickable {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Clock(uint hours, uint minutes = 0, uint seconds = 0) {
            if (hours > 23) {
                Console.WriteLine("Warning: Constructing a clock from more than 23 hours");
            }

            if (minutes > 59) {
                Console.WriteLine("Warning: Constructing a clock from more than 59 minutes");
            }

            if (seconds > 59) {
                Console.WriteLine("Warning: Constructing a clock from more than 59 seconds");
            }

            TimeSpan = new TimeSpan((int)hours, (int)minutes, (int)seconds);
        }

        private Clock(long ticks) {
            TimeSpan = new TimeSpan(ticks);
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public static Clock Zero => new Clock(0);

        public int Hours => TimeSpan.Hours;

        public int Minutes => TimeSpan.Minutes;

        public int Seconds => TimeSpan.Seconds;

        private TimeSpan TimeSpan { get; set; } = TimeSpan.Zero;

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public static Clock CreateRandomClockBetween(Clock c1, Clock c2) {
            long diffTicks = Math.Abs(c1.TimeSpan.Ticks - c2.TimeSpan.Ticks);
            long offsetTicks = (long)(diffTicks * (float)RandomGenerator.Get());
            return new Clock(Math.Min(c1.TimeSpan.Ticks, c2.TimeSpan.Ticks) + offsetTicks);
        }

        public static Clock operator +(Clock c, Time t) {
            return new Clock(c.TimeSpan.Ticks + t.Ticks);
        }

        public static Clock operator -(Clock c, Time t) {
            Debug.Assert(c.TimeSpan.Ticks >= t.Ticks, "Can't subtract a time from a time that is smaller");
            return new Clock(c.TimeSpan.Ticks - t.Ticks);
        }

        public override string ToString() {
            return TimeSpan.ToString(@"hh\:mm\:ss");
        }

        public long GetDifferenceInTicks(Clock other) {
            return TimeSpan.Ticks - other.TimeSpan.Ticks;
        }
        
        public bool Tick(Time elapsedTime) {
            TimeSpan = new TimeSpan(TimeSpan.Ticks + elapsedTime.Ticks);
            return true;
        }
    }
}
