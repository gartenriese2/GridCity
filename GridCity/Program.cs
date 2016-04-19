using System;
using System.Windows;

public class MainClass {

    //-------------------------------------------------------------------------
    // Methods
    //-------------------------------------------------------------------------
    [STAThread]
    public static void Main(string[] args) {
        Application app = new Application();
        SplashScreen splashScreen = new SplashScreen("../../Resources/splashscreen.png");
        splashScreen.Show(true, true);
        app.Run(new GridCity.GUI.MainWindow());
    }
}