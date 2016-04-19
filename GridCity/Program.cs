using System;
using System.Windows;

public class MainClass {

    //-------------------------------------------------------------------------
    // Methods
    //-------------------------------------------------------------------------
    [STAThread]
    public static void Main(string[] args) {
        Application app = new Application();        
        app.Run(new GridCity.GUI.MainWindow());
    }
}