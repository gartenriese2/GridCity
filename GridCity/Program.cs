class MainClass {
    static void Main(string[] args) {
        GridCity.Game game = new GridCity.Game(20, 20, 1600, 1200);
        game.loop();
    }
}