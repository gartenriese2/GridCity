namespace GridCity.Utility {

    using System;

    internal class Coordinate {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Coordinate(float x, float y) {
            if (x < 0f || y < 0f) {
                throw new ArgumentOutOfRangeException("Coordinates cannot be negative");
            }

            X = x;
            Y = y;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public float X { get; private set; }

        public float Y { get; private set; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            return "(" + X + "|" + Y + ")";
        }

        public Vec2D ToVec() {
            return new Vec2D(this);
        }
    }
}
