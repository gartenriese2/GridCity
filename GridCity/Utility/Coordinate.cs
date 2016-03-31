using System;

namespace GridCity.Utility {
    class Coordinate {
        public float X { get; private set; }
        public float Y { get; private set; }
        public Coordinate(float x, float y) {
            if (x < 0f || y < 0f) {
                throw new ArgumentOutOfRangeException("Coordinates cannot be negative");
            }
            X = x;
            Y = y;
        }
        public override string ToString() {
            return "(" + X + "|" + Y + ")";
        }
        public Vec2D toVec() {
            return new Vec2D(this);
        }
    }

    class LocalCoordinate {
        public float X { get; private set; }
        public float Y { get; private set; }
        public LocalCoordinate(float x, float y) {
            if (x < 0f || y < 0f || x > 1f || y > 1f) {
                throw new ArgumentOutOfRangeException("LocalCoordinates must be in [0;1]");
            }
            X = x;
            Y = y;
        }
        public Coordinate toWorldCoordinate(GlobalCoordinate globalOffset, float scale) {
            return new Coordinate((globalOffset.X + X) * scale, (globalOffset.Y + Y) * scale);
        }
    }

    class GlobalCoordinate {
        public uint X { get; private set; }
        public uint Y { get; private set; }
        public GlobalCoordinate(uint x, uint y) {
            X = x;
            Y = y;
        }
    }
}
