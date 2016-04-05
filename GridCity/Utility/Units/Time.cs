namespace GridCity.Utility.Units {

    using System;

    internal class Time : IComparable {

        public Time(float seconds) {
            Seconds = seconds;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public float Seconds { get; set; } = 0f;

        public float Minutes {
            get { return Seconds / 60f; }
            set { Seconds = value * 60f; }
        }

        public float Hours {
            get { return Seconds / 3600f; }
            set { Seconds = value * 3600f; }
        }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public static explicit operator float(Time t) {
            return t.Seconds;
        }

        public static bool operator >=(Time t1, Time t2) {
            return (float)t1 >= (float)t2;
        }

        public static bool operator <=(Time t1, Time t2) {
            return (float)t1 <= (float)t2;
        }

        public static bool operator >(Time t1, Time t2) {
            return (float)t1 > (float)t2;
        }

        public static bool operator <(Time t1, Time t2) {
            return (float)t1 < (float)t2;
        }

        public static Time operator +(Time t1, Time t2) {
            return new Time(t1.Seconds + t2.Seconds);
        }

        public static Time operator -(Time t1, Time t2) {
            return new Time(t1.Seconds - t2.Seconds);
        }

        public static Time operator *(Time t1, float t2) {
            return new Time(t1.Seconds * t2);
        }

        public static Distance operator *(Time t, Speed v) {
            return new Distance(t.Seconds * v.MS);
        }

        public override string ToString() {
            return Seconds.ToString() + " seconds";
        }

        public int CompareTo(object obj) {
            if (obj == null) {
                return 1;
            }
            
            Time otherTime = obj as Time;
            if (otherTime != null) {
                return Seconds.CompareTo(otherTime.Seconds);
            } else {
                throw new ArgumentException("Object is not a Time");
            }
        }
    }
}
