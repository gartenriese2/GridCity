namespace GridCity.Utility {

    using System;

    internal class LocalCoordinate {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public LocalCoordinate(float x, float y) {
            if (x < 0f || y < 0f || x > 1f || y > 1f) {
                throw new ArgumentOutOfRangeException("LocalCoordinates must be in [0;1]");
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
        public Coordinate ToWorldCoordinate(GlobalCoordinate globalOffset, float scale) {
            return new Coordinate((globalOffset.X + X) * scale, (globalOffset.Y + Y) * scale);
        }
    }
}
