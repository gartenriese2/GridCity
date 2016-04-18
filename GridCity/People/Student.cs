namespace GridCity.People {

    using System.Collections.Generic;
    using Fields.Buildings;
    using Simulation.Time;
    using Utility;
    using Utility.Units;

    internal class Student : Occupant {

        public Student(ResidentialBuilding home) : base(home) {
        }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override bool FindOccupation(List<OccupationalBuilding> buildings) {
            var tuple = FindOccupation(buildings, Type.STUDENT);
            if (tuple == null) {
                return false;
            }

            // set activities
            Time timeToOccupation = tuple.Item1.Item2;
            List<Clock> possibleStartTimes = new List<Clock> { new Clock(8), new Clock(8, 15), new Clock(8, 30), new Clock(10), new Clock(10, 15), new Clock(10, 30) };
            List<Clock> possibleEndTimes = new List<Clock> { new Clock(11, 30), new Clock(11, 45), new Clock(12), new Clock(13, 30), new Clock(13, 45), new Clock(14), new Clock(15, 30), new Clock(15, 45), new Clock(16), new Clock(17, 30), new Clock(17, 45), new Clock(18) };

            if (RandomGenerator.Get() < 0.9) {
                var to = RandomGenerator.GetFromList(possibleStartTimes) - timeToOccupation;
                var from = RandomGenerator.GetFromList(possibleEndTimes);
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.MONDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });
            }

            if (RandomGenerator.Get() < 0.9) {
                var to = RandomGenerator.GetFromList(possibleStartTimes) - timeToOccupation;
                var from = RandomGenerator.GetFromList(possibleEndTimes);
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.TUESDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });
            }

            if (RandomGenerator.Get() < 0.9) {
                var to = RandomGenerator.GetFromList(possibleStartTimes) - timeToOccupation;
                var from = RandomGenerator.GetFromList(possibleEndTimes);
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.WEDNESDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });
            }

            if (RandomGenerator.Get() < 0.9) {
                var to = RandomGenerator.GetFromList(possibleStartTimes) - timeToOccupation;
                var from = RandomGenerator.GetFromList(possibleEndTimes);
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.THURSDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });
            }

            if (RandomGenerator.Get() < 0.5) {
                var to = RandomGenerator.GetFromList(possibleStartTimes) - timeToOccupation;
                var from = RandomGenerator.GetFromList(possibleEndTimes);
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.CreateRandomClockBetween(to, to - Time.FromMinutes(10))), Path = PathToOccupation });
                Activities.Enqueue(new Activity { Date = new Date(Date.Weekday.FRIDAY, Clock.CreateRandomClockBetween(from, from + Time.FromMinutes(10))), Path = PathFromOccupation });
            }

            return true;
        }
    }
}
