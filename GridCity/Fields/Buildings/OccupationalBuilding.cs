using GridCity.People;
using System.Collections.Generic;
using System.Linq;

namespace GridCity.Fields.Buildings {
    abstract class OccupationalBuilding : Building {
        public List<Occupation> Occupations { get; private set; } = new List<Occupation>();
        public List<Occupation> OpenOccupations => Occupations.Where(X => !X.Occupied).ToList();
        public List<Occupation> OccupiedOccupations => Occupations.Where(X => X.Occupied).ToList();
        public bool HasOpenOccupations => OpenOccupations.Count > 0;
        public Occupation FirstOpenOccupation => OpenOccupations.First();
        public OccupationalBuilding() : base() { }
        public  OccupationalBuilding(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, uint numJobs) : base(pos, layout, orientation, name) {
            for (uint i = 0; i < numJobs; ++i) {
                Occupations.Add(new Occupation());
            }
        }
        public override string ToString() {
            return base.ToString() + ": filled occupations: " + OccupiedOccupations.Count + ", open occupations: " + OpenOccupations.Count;
        }
    }
    class School : OccupationalBuilding {
        public School() : base() { }
        public School(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, uint numJobs) : base(name, pos, layout, orientation, numJobs) {
        }
    }
    class WorkBuilding : OccupationalBuilding {
        public WorkBuilding() : base() { }
        public WorkBuilding(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, uint numJobs) : base(name, pos, layout, orientation, numJobs) {
        }
    }
}
