namespace GridCity.GUI {

    using Simulation.Time;

    internal class DateInfoModel : PropertyChangedBase {

        //---------------------------------------------------------------------
        // Fields
        //---------------------------------------------------------------------
        private string date = string.Empty;

        private string speed = string.Empty;

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public DateInfoModel() {
            Speed = SpeedFactor.ToString();
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public SpeedFactor SpeedFactor { get; private set; } = new SpeedFactor();

        public string Date {
            get {
                return date;
            }

            set {
                if (date != value) {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        public string Speed {
            get {
                return speed;
            }

            set {
                if (speed != value) {
                    speed = value;
                    OnPropertyChanged(nameof(Speed));
                }
            }
        }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public bool TryAddSpeed() {
            bool success = SpeedFactor.TryAdd();
            if (success) {
                Speed = SpeedFactor.ToString();
            }

            return success;
        }

        public bool TrySubtractSpeed() {
            bool success = SpeedFactor.TrySubstract();
            if (success) {
                Speed = SpeedFactor.ToString();
            }

            return success;
        }
    }
}
