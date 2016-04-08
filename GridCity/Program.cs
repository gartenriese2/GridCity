public class MainClass {

    //-------------------------------------------------------------------------
    // Methods
    //-------------------------------------------------------------------------
    public static void Main(string[] args) {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        GridCity.Game game = new GridCity.Game(20, 20, 1600, 1200);
        sw.Stop();
        System.Console.WriteLine("Init took " + sw.ElapsedMilliseconds + " ms");
        game.Loop();
    }
}