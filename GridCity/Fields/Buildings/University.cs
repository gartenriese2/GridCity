namespace GridCity.Fields.Buildings {

    using System;
    using System.Collections.Generic;
    using People;

    internal class University : OccupationalBuilding {

        public University(string name, Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, Dictionary<Resident.Type, uint> jobs, Tuple<uint, uint> size) : base(name, pos, layout, orientation, jobs, size) {
        }
    }
}
