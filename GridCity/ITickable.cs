namespace GridCity {

    using Utility.Units;

    internal interface ITickable {

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        bool Tick(Time elapsedTime);
    }
}
