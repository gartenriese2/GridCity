namespace GridCity.People {

    internal class Occupation {

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public bool Occupied { get; private set; } = false;

        public Resident Occupier { get; private set; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public bool Occupy(Resident resident) {
            if (Occupied) {
                return false;
            }

            Occupied = true;
            Occupier = resident;
            return true;
        }
    }
}
