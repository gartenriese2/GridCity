namespace GridCity.GUI {

    using System.Windows;

    public partial class GridCitySplashScreen : Window {

        //-------------------------------------------------------------------------
        // Constructors
        //-------------------------------------------------------------------------
        public GridCitySplashScreen() {
            InitializeComponent();
            DataContext = this;
        }

        //-------------------------------------------------------------------------
        // Properties
        //-------------------------------------------------------------------------
        public string Version { get; } = "v0.1.7";
    }
}
