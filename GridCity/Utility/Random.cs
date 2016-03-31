using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GridCity.Utility {
    class RandomGenerator {
        private static Random rnd = new Random();
        public static double get() {
            return rnd.NextDouble();
        }
        public static uint get(uint minValue, uint maxValue) {
            return (uint)rnd.Next((int)minValue, (int)maxValue + 1);
        }
        public static int get(int minValue, int maxValue) {
            return rnd.Next(minValue, maxValue + 1);
        }
        public static T getFromList<T>(List<T> list) {
            Debug.Assert(list != null && list.Count > 0);
            int idx = get(0, list.Count - 1);
            return list[idx];
        }
    }
}
