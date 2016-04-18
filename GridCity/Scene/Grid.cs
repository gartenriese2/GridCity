namespace GridCity.Scene {

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Fields;

    internal class Grid {

        public Grid(uint x, uint y) {
            Size = Tuple.Create(x, y);
            Fields = new List<Field>();
            for (uint i = 0; i < y; ++i) {
                for (uint j = 0; j < x; ++j) {
                    Fields.Add(new EmptyField(new Utility.GlobalCoordinate(j, i)));
                }
            }
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public uint Width => Size.Item1;

        public uint Height => Size.Item2;

        private List<Field> Fields { get; }

        private Tuple<uint, uint> Size { get; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            return "Grid: x=" + Width + ", y=" + Height;
        }

        public List<T> GetFields<T>() {
            List<T> list = new List<T>();
            foreach (var field in Fields) {
                if (field is T) {
                    list.Add((T)(object)field);
                }
            }

            return list;
        }

        public T GetField<T>(Utility.GlobalCoordinate pos) where T : Field {
            Field field = GetField(pos);
            if (!(field is T)) {
                throw new ArgumentException("Field is not of type " + nameof(T));
            }

            return (T)field;
        }

        public T SetField<T>(T field) where T : Field {
            return (T)SetField((Field)field);
        }

        public Field RemoveField(Utility.GlobalCoordinate pos) {
            uint x = pos.X;
            uint y = pos.Y;
            if (x >= Width || y >= Height) {
                throw new ArgumentOutOfRangeException("Position is outside of Grid");
            }

            var idx = CoordsToIndex(x, y);
            if (!Fields[(int)idx].IsEmpty) {
                var field = Fields[(int)idx];
                Fields[(int)idx] = new EmptyField(new Utility.GlobalCoordinate(x, y));
                return field;
            }

            Debug.WriteLine("Trying to remove a field that is empty");
            return null;
        }

        private Field GetField(Utility.GlobalCoordinate pos) {
            if (pos.X >= Width || pos.Y >= Height) {
                throw new ArgumentOutOfRangeException("Position is outside of Grid");
            }

            var idx = CoordsToIndex(pos.X, pos.Y);
            return Fields[(int)idx];
        }

        private Field SetField(Field field) {
            if (field == null) {
                throw new ArgumentNullException("field");
            }

            uint x = field.X;
            uint y = field.Y;
            if (x >= Width) {
                Debug.WriteLine("Trying to access column " + x + " when there are only " + Width + " columns");
                return null;
            }

            if (y >= Height) {
                Debug.WriteLine("Trying to access row " + y + " when there are only " + Height + " rows");
                return null;
            }

            var idx = CoordsToIndex(x, y);
            if (!Fields[(int)idx].IsEmpty) {
                Debug.WriteLine("Trying to set a field that isn't empty");
                return null;
            }

            Fields[(int)idx] = field;
            return field;
        }

        private uint CoordsToIndex(uint x, uint y) {
            return (y * Width) + x;
        }
    }
}
