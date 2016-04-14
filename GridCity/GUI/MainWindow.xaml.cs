namespace GridCity.GUI {

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows;

    public partial class MainWindow : Window {

        private readonly uint offset = 100u;

        private readonly uint minWidth = 640u;

        private readonly uint minHeight = 480u;

        private bool isLoaded = false;

        public MainWindow() {
            InitializeComponent();
            DataContext = Game;

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
        }

        //-------------------------------------------------------------------------
        // Properties
        //-------------------------------------------------------------------------
        private uint CalculatedWidth { get; }

        private uint CalculatedHeight { get; }

        private Game Game { get; set; } = new Game();

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
            Game.Init(20, 20, width, height, borderSize, borderSize);
            Game.Loop();
            Environment.Exit(0);
        }

        private void ButtonMinus_Click(object sender, RoutedEventArgs e) {
            Game.TrySubtractSpeed();
        }

        private void ButtonPlus_Click(object sender, RoutedEventArgs e) {
            Game.TryAddSpeed();
        }
    }
}
