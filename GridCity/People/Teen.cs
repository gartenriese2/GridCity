namespace GridCity.People {

    using System.Collections.Generic;
    using Fields.Buildings;
    using Simulation.Time;
    using Utility.Units;

    internal class Teen : Occupant {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Teen(ResidentialBuilding home) : base(home) {
        }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override bool FindOccupation(List<OccupationalBuilding> buildings) {
            var tuple = FindOccupation(buildings, Type.TEEN);
            if (tuple == null) {
                return false;
            }

            // set activities
            Time buffer = Time.FromMinutes(5);
            Time timeToOccupation = tuple.Item1.Item2;
            Clock to = new Clock(8, 0, 0) - timeToOccupation - buffer;
            Clock from = new Clock(13, 0, 0) + buffer;
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
            Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });

            return true;
        }
    }
}
