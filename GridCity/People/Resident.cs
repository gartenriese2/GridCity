using GridCity.Fields.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using GridCity.Utility.Units;

namespace GridCity.People {
    abstract class Resident {
        public ResidentialBuilding Home { get; protected set; }
        public Traveller Traveller { get; } = Traveller.Create();
        public Agent Agent { get; set; } = Agent.create();
        public Queue<Activity> Activities { get; protected set; } = new Queue<Activity>();
        protected Resident(ResidentialBuilding home) {
            Home = home;
            Traveller.Keys.AddRange(Home.Nodes);
        }
        public bool checkTime(Time elapsedSeconds, Date date) {
            if (Agent.IsMoving || Activities.Count == 0) {
                return false;
            }
            var next = Activities.First();
            if (next.Date.CurrentDay == date.CurrentDay && date.CurrentClock.getDifference(next.Date.CurrentClock).Seconds < 5) {
                Agent.dispatch(next.Path);
                Activities.Enqueue(Activities.Dequeue());
            }
            return false;
        }
    }
    abstract class Occupant : Resident {
        public Occupant(ResidentialBuilding home) : base(home) {

        }
        public bool HasOccupation { get; private set; } = false;
        public Occupation Occupation { get; private set; }
        public Pathfinding.Path PathToOccupation { get; protected set; }
        public Pathfinding.Path PathFromOccupation { get; protected set; }
        public bool setOccupation(Occupation occupation) {
            if (HasOccupation) {
                return false;
            }
            HasOccupation = true;
            Occupation = occupation;
            return true;
        }
        abstract public bool findOccupation<U>(List<U> buildings) where U : OccupationalBuilding;
    }
    class Worker : Occupant {
        public Worker(ResidentialBuilding home) : base(home) {
            Traveller.NonReusableTypes.Add(Pathfinding.NodeInfo.AllowedType.CAR);
        }
        public override bool findOccupation<U>(List<U> buildings) {
            var map = new Dictionary<U, Tuple<Tuple<Pathfinding.Path, Time>, Tuple<Pathfinding.Path, Time>>>();
            foreach (var building in buildings.Where(x => x.HasOpenOccupations)) {
                var idx = Traveller.Keys.Count;
                Traveller.Keys.AddRange(building.Nodes);
                var pathTo = Pathfinding.Pathfinder.findQuickestPath(Home, building, Traveller);
                var pathFrom = Pathfinding.Pathfinder.findQuickestPath(building, Home, Traveller);
                Traveller.Keys.RemoveRange(idx, building.Nodes.Count);
                if (pathTo != null && pathFrom != null) {
                    map.Add(building, Tuple.Create(pathTo, pathFrom));
                }
            }
            var orderedMap = map.OrderBy(x => (x.Value.Item1.Item2 + x.Value.Item2.Item2).Seconds);
            for (int i = 0; i < orderedMap.Count(); ++i) {
                if (i == orderedMap.Count() - 1 || Utility.RandomGenerator.get() < 0.5) {
                    var mapEntry = orderedMap.ElementAt(i);
                    var building = mapEntry.Key;
                    Occupation occupation = building.FirstOpenOccupation;
                    occupation.occupy(this);
                    setOccupation(occupation);
                    PathToOccupation = mapEntry.Value.Item1.Item1;
                    PathFromOccupation = mapEntry.Value.Item2.Item1;

                    // set activities
                    Time timeToOccupation = mapEntry.Value.Item1.Item2;
                    Clock to = Clock.createRandomClockBetween(new Clock(5, 30, 0), new Clock(9, 0, 0)) - timeToOccupation;
                    Clock from = to + timeToOccupation + new Time(8 * 3600); // 8 hours working time
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.createRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.createRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.createRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.createRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.createRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.createRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.createRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.createRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.createRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.createRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });

                    return true;
                }
            }
            return false;
        }
    }
    class Pensioner : Resident {
        public Pensioner(ResidentialBuilding home) : base(home) {
        }
    }
    class Unemployed : Resident {
        public Unemployed(ResidentialBuilding home) : base(home) {
        }
    }
    class Infant : Resident {
        public Infant(ResidentialBuilding home) : base(home) {
        }
    }
    class Kid : Resident {
        public Kid(ResidentialBuilding home) : base(home) {
        }
    }
    class Teen : Occupant {
        public Teen(ResidentialBuilding home) : base(home) {
        }
        public override bool findOccupation<U>(List<U> buildings) {
            var map = new Dictionary<U, Tuple<Tuple<Pathfinding.Path, Time>, Tuple<Pathfinding.Path, Time>>>();
            foreach (var building in buildings.Where(x => x.HasOpenOccupations)) {
                var idx = Traveller.Keys.Count;
                Traveller.Keys.AddRange(building.Nodes);
                var pathTo = Pathfinding.Pathfinder.findQuickestPath(Home, building, Traveller);
                var pathFrom = Pathfinding.Pathfinder.findQuickestPath(building, Home, Traveller);
                Traveller.Keys.RemoveRange(idx, building.Nodes.Count);
                if (pathTo != null && pathFrom != null) {
                    map.Add(building, Tuple.Create(pathTo, pathFrom));
                }
            }
            var orderedMap = map.OrderBy(x => (x.Value.Item1.Item2 + x.Value.Item2.Item2).Seconds);
            for (int i = 0; i < orderedMap.Count(); ++i) {
                if (i == orderedMap.Count() - 1 || Utility.RandomGenerator.get() < 0.5) {
                    var mapEntry = orderedMap.ElementAt(i);
                    var building = mapEntry.Key;
                    Occupation occupation = building.FirstOpenOccupation;
                    occupation.occupy(this);
                    setOccupation(occupation);
                    PathToOccupation = mapEntry.Value.Item1.Item1;
                    PathFromOccupation = mapEntry.Value.Item2.Item1;

                    // set activities
                    Time buffer = new Time(300);
                    Time timeToOccupation = mapEntry.Value.Item1.Item2;
                    Clock to = new Clock(8, 0, 0) - timeToOccupation - buffer;
                    Clock from = new Clock(13, 0, 0) + buffer;
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.createRandomClockBetween(to, to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.createRandomClockBetween(from, from + new Time(600))), Path = PathFromOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.createRandomClockBetween(to, to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.createRandomClockBetween(from, from + new Time(600))), Path = PathFromOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.createRandomClockBetween(to, to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.createRandomClockBetween(from, from + new Time(600))), Path = PathFromOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.createRandomClockBetween(to, to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.createRandomClockBetween(from, from + new Time(600))), Path = PathFromOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.createRandomClockBetween(to, to - new Time(600))), Path = PathToOccupation });
                    Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.createRandomClockBetween(from, from + new Time(600))), Path = PathFromOccupation });

                    return true;
                }
            }
            return false;
        }
    }
    class Student : Resident {
        public Student(ResidentialBuilding home) : base(home) {
        }
    }
}
