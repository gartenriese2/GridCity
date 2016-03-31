namespace GridCity.People {
    class Occupation {
        public bool Occupied { get; private set; } = false;
        public Resident Occupier { get; private set; }
        public bool occupy(Resident resident) {
            if (Occupied) {
                return false;
            }
            Occupied = true;
            Occupier = resident;
            return true;
        }
    }
}
