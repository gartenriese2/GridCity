namespace GridCity.Utility.Units {

    using System;

    internal class Speed : IComparable {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Speed(float ms) {
            MS = ms;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public float MS { get; set; } = 0f;

        public float KMH {
            get { return MS * 3.6f; }
            set { MS = value / 3.6f; }
        }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
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
            return new Distance((float)t.Seconds * v.MS);
        }

        public override string ToString() {
            return MS.ToString() + " meters per second";
        }

        public int CompareTo(object obj) {
            if (obj == null) {
                return 1;
            }

            Speed otherSpeed = obj as Speed;
            if (otherSpeed != null) {
                return MS.CompareTo(otherSpeed.MS);
            } else {
                throw new ArgumentException("Object is not a Speed");
            }
        }
    }
}
