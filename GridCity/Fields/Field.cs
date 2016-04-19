namespace GridCity.Fields {

    using System;

    internal abstract class Field {

        public static readonly uint Scale = 8;

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Field(uint x, uint y) {
            IsEmpty = true;
            Position = new Utility.GlobalCoordinate(x, y);
        }

        //---------------------------------------------------------------------
        // Enumerations
        //---------------------------------------------------------------------
        public enum Border {
            LEFT, TOP, RIGHT, BOTTOM
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public bool IsEmpty { get; protected set; } = true;
        
        public uint X => Position.X;

        public uint Y => Position.Y;

        public Graphics.Texture Texture { get; set; }

        protected Utility.GlobalCoordinate Position { get; set; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            return "(" + X + "|" + Y + ")";
        }

        public bool IsNeighborOf(Field other) {
            if (other.X == X) {
                return other.Y == Y + 1 || other.Y == Y - 1;
            }

            if (other.Y == Y) {
                return other.X == X + 1 || other.X == X - 1;
            }

            return false;
        }

        public Border GetConnectingBorder(Field other) {
            if (!IsNeighborOf(other)) {
                throw new ArgumentException("Cannot get a connecting border between non-neighboring fields!");
            }

            if (X == other.X) {
                return Y == other.Y + 1 ? Border.BOTTOM : Border.TOP;
            } else {
                return X == other.X + 1 ? Border.LEFT : Border.RIGHT;
            }
        }
    }
}
