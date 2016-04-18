namespace GridCity.Simulation.Time {

    internal class SpeedFactor {

        //---------------------------------------------------------------------
        // Fields
        //---------------------------------------------------------------------
        private static readonly uint MinValue = 1;

        private static readonly uint MaxValue = 2048; // TODO: make dynamic?

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public uint Value { get; private set; } = MinValue;

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            string str = string.Empty;
            for (int i = 1; i <= Value; i *= 2) {
                str += ">";
            }

            return str;
        }

        public bool CanAdd() {
            return Value < MaxValue;
        }

        public bool CanSubstract() {
            return Value > MinValue;
        }

        public bool TryAdd() {
            if (CanAdd()) {
                Value *= 2;
                return true;
            }

            return false;
        }

        public bool TrySubstract() {
            if (CanSubstract()) {
                Value /= 2;
                return true;
            }

            return false;
        }
    }
}
