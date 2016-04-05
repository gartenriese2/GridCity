namespace GridCity.Utility {

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class RandomGenerator {

        private static Random rnd = new Random();

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public static double Get() {
            return rnd.NextDouble();
        }

        public static uint Get(uint minValue, uint maxValue) {
            return (uint)rnd.Next((int)minValue, (int)maxValue + 1);
        }

        public static int Get(int minValue, int maxValue) {
            return rnd.Next(minValue, maxValue + 1);
        }

        public static T GetFromList<T>(List<T> list) {
            Debug.Assert(list != null && list.Count > 0, "There must be at least one element in the list");
            int idx = Get(0, list.Count - 1);
            return list[idx];
        }
    }
}
