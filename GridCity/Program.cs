using System;
using System.Diagnostics;
using System.Windows;

public class MainClass {

    //-------------------------------------------------------------------------
    // Fields
    //-------------------------------------------------------------------------
    private static readonly long MaxCreationTime = 2000;

    //-------------------------------------------------------------------------
    // Methods
    //-------------------------------------------------------------------------
    [STAThread]
    public static void Main(string[] args) {
        Stopwatch sw = new Stopwatch();

        sw.Start();
        Application app = new Application();
        Console.WriteLine("Application creation took " + sw.ElapsedMilliseconds + "ms");

        sw.Restart();
        var win = new GridCity.GUI.MainWindow();
        var elapsedMS = sw.ElapsedMilliseconds;
        Console.WriteLine("MainWindow creation took " + elapsedMS + "ms");
        if (elapsedMS > MaxCreationTime) {
            Console.WriteLine("================================================================================\n" +
                              "NOTE: GUI initialization took a very long time (" + (elapsedMS / 1000f) + " seconds).\n" +
                              "If you don't have a slow GPU this could be because NVIDIA 3D Vision is installed.\n" +
                              "You can try uninstalling NVIDIA 3D Vision and see if it helps.\n" +
                              "================================================================================");
        }

        app.Run(win);
    }
}