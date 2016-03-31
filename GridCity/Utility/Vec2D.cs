using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCity.Utility {
    struct Vec2D {
        public float X { get; private set; }
        public float Y { get; private set; }
        public Vec2D(float x, float y) {
            X = x;
            Y = y;
        }
        public Vec2D(Coordinate coord) {
            X = coord.X;
            Y = coord.Y;
        }
        public float LengthSquared => X * X + Y * Y;
        public Units.Distance Length => new Units.Distance((float)Math.Sqrt(LengthSquared));
        public static Vec2D operator +(Vec2D c1, Vec2D c2) {
            return new Vec2D(c1.X + c2.X, c1.Y + c2.Y);
        }
        public static Vec2D operator -(Vec2D c1, Vec2D c2) {
            return new Vec2D(c1.X - c2.X, c1.Y - c2.Y);
        }
        public static Vec2D operator *(Vec2D c1, float val) {
            return new Vec2D(c1.X * val, c1.Y * val);
        }
        public void normalize() {
            var len = Length;
            X /= len.Meters;
            Y /= len.Meters;
        }
        public Coordinate toCoordinate() {
            return new Coordinate(X, Y);
        }
    }
}
