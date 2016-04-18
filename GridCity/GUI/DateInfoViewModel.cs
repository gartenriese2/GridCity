namespace GridCity.GUI {

    using System.Windows.Input;

    internal class DateInfoViewModel : PropertyChangedBase {

        //---------------------------------------------------------------------
        // Fields
        //---------------------------------------------------------------------
        private ICommand speedSubstractCommand;

        private ICommand speedAddCommand;

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public DateInfoViewModel(DateInfoModel model) {
            Model = model;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public DateInfoModel Model { get; private set; }
        
        public ICommand SpeedSubstractCommand {
            get {
                if (speedSubstractCommand == null) {
                    speedSubstractCommand = new RelayCommand(param => Substract(), param => CanSubstract());
                }

                return speedSubstractCommand;
            }
        }

        public ICommand SpeedAddCommand {
            get {
                if (speedAddCommand == null) {
                    speedAddCommand = new RelayCommand(param => Add(), param => CanAdd());
                }

                return speedAddCommand;
            }
        }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        private bool CanSubstract() {
            return Model.SpeedFactor.CanSubstract();
        }

        private bool CanAdd() {
            return Model.SpeedFactor.CanAdd();
        }

        private void Substract() {
            Model.TrySubtractSpeed();
        }

        private void Add() {
            Model.TryAddSpeed();
        }
    }
}
