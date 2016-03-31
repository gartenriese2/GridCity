using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCity.Fields {
    abstract class Field {
        public enum Border { LEFT, TOP, RIGHT, BOTTOM }
        public static readonly uint k_scale = 8;
        public bool IsEmpty { get; protected set; } = true;
        protected Utility.GlobalCoordinate Position { get; set; }
        public Field() { }
        public Field(uint x, uint y) {
            IsEmpty = true;
            Position = new Utility.GlobalCoordinate(x, y);
        }
        public uint X => Position.X;
        public uint Y => Position.Y;
        public override string ToString() {
            return "(" + X + "|" + Y + ")";
        }
        public Graphics.Texture Texture { get; set; }
        public bool isNeighborOf(Field other) {
            if (other.X == X) {
                return other.Y == Y + 1 || other.Y == Y - 1;
            }
            if (other.Y == Y) {
                return other.X == X + 1 || other.X == X - 1;
            }
            return false;
        }
        public Border getConnectingBorder(Field other) {
            if (!isNeighborOf(other)) {
                throw new ArgumentException("Cannot get a connecting border between non-neighboring fields!");
            }
            if (X == other.X) {
                return Y == other.Y + 1 ? Border.BOTTOM : Border.TOP;
            } else {
                return X == other.X + 1 ? Border.LEFT : Border.RIGHT;
            }
        }
    }

    class EmptyField : Field {
        public EmptyField(Utility.GlobalCoordinate pos) : base(pos.X, pos.Y) {
            Texture = new Graphics.Texture("EmptyField");
        }
        public override string ToString() {
            return "Empty Field " + Position;
        }
    }
}
