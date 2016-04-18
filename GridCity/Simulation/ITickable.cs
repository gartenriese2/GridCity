namespace GridCity.Simulation {

    internal interface ITickable {

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        bool Tick(Utility.Units.Time elapsedTime);
    }
}
