﻿namespace GridCity.Utility {

    internal class GlobalCoordinate {

        public GlobalCoordinate(uint x, uint y) {
            X = x;
            Y = y;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public uint X { get; private set; }

        public uint Y { get; private set; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            return "(" + X + "|" + Y + ")";
        }
    }
}