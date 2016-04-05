namespace GridCity.Fields.Buildings {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using People;

    internal abstract class OccupationalBuilding : Building {

        public OccupationalBuilding(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, Dictionary<Resident.Type, uint> jobs, Tuple<uint, uint> size) : base(pos, layout, orientation, name, size) {
            foreach (var t in jobs) {
                Occupations.Add(t.Key, new List<Occupation>());
                for (uint i = 0; i < t.Value; ++i) {
                    Occupations[t.Key].Add(new Occupation());
                }
            }
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public Dictionary<Resident.Type, List<Occupation>> Occupations { get; private set; } = new Dictionary<Resident.Type, List<Occupation>>();

        public List<Occupation> OpenOccupations(Resident.Type type) => Occupations.FirstOrDefault(x => x.Key == type).Value?.Where(x => !x.Occupied).ToList() ?? new List<Occupation>();

        public List<Occupation> OccupiedOccupations(Resident.Type type) => Occupations.FirstOrDefault(x => x.Key == type).Value?.Where(x => x.Occupied).ToList() ?? new List<Occupation>();

        public bool HasOpenOccupations(Resident.Type type) => OpenOccupations(type).Count > 0;

        public Occupation FirstOpenOccupation(Resident.Type type) => OpenOccupations(type).First();

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            string str = string.Empty;
            foreach (var t in Occupations) {
                str += t.Key.ToString() + ": " + OccupiedOccupations(t.Key).Count + "/" + Occupations[t.Key].Count + ", ";
            }

            return base.ToString() + str;
        }
    }
}
