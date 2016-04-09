public class MainClass {

    //-------------------------------------------------------------------------
    // Methods
    //-------------------------------------------------------------------------
    public static void Main(string[] args) {
        GridCity.Game game = new GridCity.Game(20, 20, 1600, 1200);
        game.Loop();
    }
}