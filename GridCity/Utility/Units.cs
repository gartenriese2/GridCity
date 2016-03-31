using System;

namespace GridCity.Utility.Units {
    class Speed : IComparable {
        public Speed(float ms) {
            MS = ms;
        }
        public float MS { get; set; } = 0f;
        public float KMH {
            get { return MS * 3.6f; }
            set { MS = value / 3.6f; }
        }
        public static explicit operator float(Speed v) {
            return v.MS;
        }
        public static bool operator >=(Speed v1, Speed v2) {
            return (float)v1 >= (float)v2;
        }
        public static bool operator <=(Speed v1, Speed v2) {
            return (float)v1 <= (float)v2;
        }
        public static bool operator >(Speed v1, Speed v2) {
            return (float)v1 > (float)v2;
        }
        public static bool operator <(Speed v1, Speed v2) {
            return (float)v1 < (float)v2;
        }
        public static Speed operator +(Speed v1, Speed v2) {
            return new Speed(v1.MS + v2.MS);
        }
        public static Speed operator -(Speed v1, Speed v2) {
            return new Speed(v1.MS - v2.MS);
        }
        public static Distance operator *(Speed v, Time t) {
            return new Distance(t.Seconds * v.MS);
        }

        public int CompareTo(object obj) {
            if (obj == null) return 1;

            Speed otherSpeed = obj as Speed;
            if (otherSpeed != null)
                return MS.CompareTo(otherSpeed.MS);
            else
                throw new ArgumentException("Object is not a Speed");
        }
        public override string ToString() {
            return MS.ToString() + " meters per second";
        }
    }
    class Time : IComparable {
        public Time(float seconds) {
            Seconds = seconds;
        }
        public float Seconds { get; set; } = 0f;
        public float Minutes {
            get { return Seconds / 60f; }
            set { Seconds = value * 60f; }
        }
        public float Hours {
            get { return Seconds / 3600f; }
            set { Seconds = value * 3600f; }
        }
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

        public int CompareTo(object obj) {
            if (obj == null) return 1;

            Time otherTime = obj as Time;
            if (otherTime != null)
                return Seconds.CompareTo(otherTime.Seconds);
            else
                throw new ArgumentException("Object is not a Time");
        }
        public override string ToString() {
            return Seconds.ToString() + " seconds";
        }
    }
    class Distance : IComparable {
        public Distance(float meters) {
            Meters = meters;
        }
        public float Meters { get; set; } = 0f;
        public float KiloMeters {
            get { return Meters / 1000f; }
            set { Meters = value * 1000f; }
        }
        public static explicit operator float(Distance d) {
            return d.Meters;
        }
        public static bool operator >=(Distance d1, Distance d2) {
            return (float)d1 >= (float)d2;
        }
        public static bool operator <=(Distance d1, Distance d2) {
            return (float)d1 <= (float)d2;
        }
        public static bool operator >(Distance d1, Distance d2) {
            return (float)d1 > (float)d2;
        }
        public static bool operator <(Distance d1, Distance d2) {
            return (float)d1 < (float)d2;
        }
        public static Distance operator +(Distance d1, Distance d2) {
            return new Distance(d1.Meters + d2.Meters);
        }
        public static Distance operator -(Distance d1, Distance d2) {
            return new Distance(d1.Meters - d2.Meters);
        }
        public static Speed operator /(Distance d, Time t) {
            return new Speed(d.Meters / t.Seconds);
        }
        public static Time operator /(Distance d, Speed v) {
            return new Time(d.Meters / v.MS);
        }

        public int CompareTo(object obj) {
            if (obj == null) return 1;

            Distance otherDistance = obj as Distance;
            if (otherDistance != null)
                return Meters.CompareTo(otherDistance.Meters);
            else
                throw new ArgumentException("Object is not a Distance");
        }
        public override string ToString() {
            return Meters.ToString() + " meters";
        }
    }
}
