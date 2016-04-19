namespace GridCity.Fields.Buildings {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using People;

    internal class ResidentialBuilding : Building {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public ResidentialBuilding(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, uint numHouseholds, Tuple<uint, uint> size) : base(pos, layout, orientation, name, size) {
            for (uint i = 0; i < numHouseholds; ++i) {
                Households.Add(Household.GetRandomHousehold(this));
            }
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public List<Household> Households { get; } = new List<Household>();

        public List<Resident> Residents => Households.SelectMany(h => h.Residents).ToList();

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            string str = base.ToString();
            foreach (var hh in Households) {
                str += hh.ToString();
            }

            return str;
        }
    }
}
