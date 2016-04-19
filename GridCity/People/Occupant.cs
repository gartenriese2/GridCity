namespace GridCity.People {
    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fields.Buildings;
    using Utility.Units;

    internal abstract class Occupant : Resident {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Occupant(ResidentialBuilding home) : base(home) {
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public bool HasOccupation { get; private set; } = false;

        public Occupation Occupation { get; private set; }

        protected Pathfinding.Path PathToOccupation { get; set; }

        protected Pathfinding.Path PathFromOccupation { get; set; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public abstract bool FindOccupation(List<OccupationalBuilding> buildings);

        public bool SetOccupation(Occupation occupation) {
            if (HasOccupation) {
                return false;
            }

            HasOccupation = true;
            Occupation = occupation;
            return true;
        }

        protected Tuple<Tuple<Pathfinding.Path, Time>, Tuple<Pathfinding.Path, Time>> FindOccupation(List<OccupationalBuilding> buildings, Type type) {
            var map = new Dictionary<OccupationalBuilding, Tuple<Tuple<Pathfinding.Path, Time>, Tuple<Pathfinding.Path, Time>>>();
            foreach (var building in buildings.Where(x => x.HasOpenOccupations(type))) {
                var idx = Traveller.Keys.Count;
                Traveller.Keys.AddRange(building.Nodes);
                var pathTo = Pathfinding.Pathfinder.FindQuickestPath(Home, building, Traveller);
                var pathFrom = Pathfinding.Pathfinder.FindQuickestPath(building, Home, Traveller);
                Traveller.Keys.RemoveRange(idx, building.Nodes.Count);
                if (pathTo != null && pathFrom != null) {
                    map.Add(building, Tuple.Create(pathTo, pathFrom));
                }
            }

            var orderedMap = map.OrderBy(x => (x.Value.Item1.Item2 + x.Value.Item2.Item2).Seconds);
            bool lookAgain = true;
            while (lookAgain) {
                lookAgain = false;

                for (int i = 0; i < orderedMap.Count(); ++i) {
                    if (i == orderedMap.Count() - 1 || Utility.RandomGenerator.Get() < 0.5) {
                        var mapEntry = orderedMap.ElementAt(i);
                        var building = mapEntry.Key;
                        if (!building.TryOccupy(type, this)) {
                            lookAgain = true;
                            continue;
                        }

                        PathToOccupation = mapEntry.Value.Item1.Item1;
                        PathFromOccupation = mapEntry.Value.Item2.Item1;
                        return mapEntry.Value;
                    }
                }

                if (lookAgain) {
                    orderedMap = orderedMap.Where(x => x.Key.HasOpenOccupations(type)).ToDictionary(x => x.Key, x => x.Value).OrderBy(x => (x.Value.Item1.Item2 + x.Value.Item2.Item2).Seconds);
                }
            }

            return null;
        }
    }
}
