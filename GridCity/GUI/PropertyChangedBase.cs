namespace GridCity.GUI {

    using System.ComponentModel;

    public abstract class PropertyChangedBase : INotifyPropertyChanged {

        //---------------------------------------------------------------------
        // Events
        //---------------------------------------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        protected internal void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
