namespace GridCity.GUI {

    using System.Windows;

    public partial class GridCitySplashScreen : Window {

        //-------------------------------------------------------------------------
        // Constructors
        //-------------------------------------------------------------------------
        public GridCitySplashScreen(bool show = false) {
            InitializeComponent();
            DataContext = this;
            if (show) {
                Show();
            }
        }

        //-------------------------------------------------------------------------
        // Properties
        //-------------------------------------------------------------------------
        public string Version { get; } = "v0.1.7a";
    }
}
