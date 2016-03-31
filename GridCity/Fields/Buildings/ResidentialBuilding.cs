using GridCity.People;
using System.Collections.Generic;
using System.Linq;

namespace GridCity.Fields.Buildings {
    class ResidentialBuilding : Building {
        public List<Household> Households { get; } = new List<Household>();
        public List<Resident> Residents => Households.SelectMany(h => h.Residents).ToList();
        public ResidentialBuilding() : base() { }
        public ResidentialBuilding(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, uint numHouseholds) : base(pos, layout, orientation, name) {
            for (uint i = 0; i < numHouseholds; ++i) {
                Households.Add(Household.getRandomHousehold(this));
            }
        }
        public override string ToString() {
            string str = base.ToString();
            foreach (var hh in Households) {
                str += hh.ToString();
            }
            return str;
        }
    }
}
