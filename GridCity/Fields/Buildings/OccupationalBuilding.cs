using GridCity.People;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GridCity.Fields.Buildings {
    abstract class OccupationalBuilding : Building {
        public Dictionary<Resident.Type, List<Occupation>> Occupations { get; private set; } = new Dictionary<Resident.Type, List<Occupation>>();
        public List<Occupation> OpenOccupations(Resident.Type type) => Occupations.FirstOrDefault(x => x.Key == type).Value?.Where(x => !x.Occupied).ToList() ?? new List<Occupation>();
        public List<Occupation> OccupiedOccupations(Resident.Type type) => Occupations.FirstOrDefault(x => x.Key == type).Value?.Where(x => x.Occupied).ToList() ?? new List<Occupation>();
        public bool HasOpenOccupations(Resident.Type type) => OpenOccupations(type).Count > 0;
        public Occupation FirstOpenOccupation(Resident.Type type) => OpenOccupations(type).First();
        public  OccupationalBuilding(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, Dictionary<Resident.Type, uint> jobs, Tuple<uint, uint> size) : base(pos, layout, orientation, name, size) {
            foreach (var t in jobs) {
                Occupations.Add(t.Key, new List<Occupation>());
                for (uint i = 0; i < t.Value; ++i) {
                    Occupations[t.Key].Add(new Occupation());
                }
            }
        }
        public override string ToString() {
            string str = "";
            foreach (var t in Occupations) {
                str += t.Key.ToString() + ": " + OccupiedOccupations(t.Key).Count + "/" + Occupations[t.Key].Count + ", ";
            }
            return base.ToString() + str;
        }
    }
    class School : OccupationalBuilding {
        public School(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, Dictionary<Resident.Type, uint> jobs, Tuple<uint, uint> size) : base(name, pos, layout, orientation, jobs, size) {
        }
    }
    class University : OccupationalBuilding {
        public University(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, Dictionary<Resident.Type, uint> jobs, Tuple<uint, uint> size) : base(name, pos, layout, orientation, jobs, size) {
        }
    }
    class WorkBuilding : OccupationalBuilding {
        public WorkBuilding(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, Dictionary<Resident.Type, uint> jobs, Tuple<uint, uint> size) : base(name, pos, layout, orientation, jobs, size) {
        }
    }
}
