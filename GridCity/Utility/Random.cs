using System;

namespace GridCity.Utility {
    class RandomGenerator {
        private static Random rnd = new Random();
        public static double get() {
            return rnd.NextDouble();
        }
        public static uint get(uint minValue, uint maxValue) {
            return (uint)rnd.Next((int)minValue, (int)maxValue + 1);
        }
    }
}
