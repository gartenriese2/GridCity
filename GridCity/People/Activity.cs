namespace GridCity.People {

    internal struct Activity {

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public Date Date { get; set; }

        public Pathfinding.Path Path { get; set; }
    }
}
