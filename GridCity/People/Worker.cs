namespace GridCity.People {
    
    using System.Collections.Generic;
    using Fields.Buildings;
    using Utility;
    using Utility.Units;

    internal class Worker : Occupant {

        public Worker(ResidentialBuilding home) : base(home) {
            Traveller.NonReusableTypes.Add(Pathfinding.NodeInfo.AllowedType.CAR);
        }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override bool FindOccupation(List<OccupationalBuilding> buildings) {
            var tuple = FindOccupation(buildings, Type.WORKER);
            if (tuple == null) {
                return false;
            }

            // set activities
            Time timeToOccupation = tuple.Item1.Item2;
            Clock to = Clock.CreateRandomClockBetween(new Clock(5, 30, 0), new Clock(9, 0, 0)) - timeToOccupation;
            Clock from = to + timeToOccupation + new Time(8 * 3600); // 8 hours working time
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.CreateRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.CreateRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.CreateRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.CreateRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.CreateRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.CreateRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.CreateRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.CreateRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.CreateRandomClockBetween(to + new Time(600), to - new Time(600))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.CreateRandomClockBetween(from + new Time(600), from - new Time(600))), Path = PathFromOccupation });

            return true;
        }
    }
}
