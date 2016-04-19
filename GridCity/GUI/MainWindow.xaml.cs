namespace GridCity.GUI {

    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows;

    public partial class MainWindow : Window {

        //-------------------------------------------------------------------------
        // Fields
        //-------------------------------------------------------------------------
        private readonly uint offset = 100u;

        private readonly uint minWidth = 640u;

        private readonly uint minHeight = 480u;

        private bool isLoaded = false;

        //-------------------------------------------------------------------------
        // Constructors
        //-------------------------------------------------------------------------
        public MainWindow() {
            SplashScreen.Show();

            InitializeComponent();
            Hide();

            int width = (int)SystemParameters.PrimaryScreenWidth;
            int height = (int)SystemParameters.PrimaryScreenHeight;
            Debug.Assert(width > (2 * offset) + minWidth, "Screen resolution is not big enough: Width " + width + " < " + ((2 * offset) + minWidth));
            Debug.Assert(height > (2 * offset) + minHeight, "Screen resolution is not big enough: Height " + height + " < " + ((2 * offset) + minHeight));

            CalculatedWidth = (uint)(width - (2 * offset));
            CalculatedHeight = (uint)(height - (2 * offset));

            Width = CalculatedWidth;
            Height = CalculatedHeight;
            Topmost = true;
            Top = offset;
            Left = offset;

            dataInfoView.DataContext = new DateInfoViewModel(Game.DateInfoModel);
        }

        //-------------------------------------------------------------------------
        // Properties
        //-------------------------------------------------------------------------
        private uint CalculatedWidth { get; }

        private uint CalculatedHeight { get; }

        private Game Game { get; set; } = new Game();

        private GridCitySplashScreen SplashScreen { get; } = new GridCitySplashScreen();

        //-------------------------------------------------------------------------
        // Methods
        //-------------------------------------------------------------------------
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            if (!isLoaded) {
                isLoaded = true;
                Task.Run(() => GameTask(CalculatedWidth, CalculatedHeight, offset));
            }
        }

        private void GameTask(uint width, uint height, uint borderSize) {
            Game.InitGraphics(20, 20, width, height, borderSize, borderSize);
            Dispatcher.InvokeAsync(() => SplashScreen.Close());
            Game.InitSimulation(20, 20);
            Dispatcher.InvokeAsync(() => Show());
            Game.Loop();
            Environment.Exit(0);
        }
    }
}
