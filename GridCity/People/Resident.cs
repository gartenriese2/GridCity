namespace GridCity.People {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fields.Buildings;
    using Simulation.Time;
    using Utility.Units;
    
    internal abstract class Resident {

        protected Resident(ResidentialBuilding home) {
            Home = home;
            Traveller.Keys.AddRange(Home.Nodes);
        }

        public enum Type {
            WORKER, TEEN, STUDENT
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public ResidentialBuilding Home { get; protected set; }

        public Traveller Traveller { get; } = Traveller.Create();

        public Agent Agent { get; set; } = Agent.Create();

        public Queue<Activity> Activities { get; protected set; } = new Queue<Activity>();

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public static Type StringToType(string str) {
            if (str == "WORKER") {
                return Type.WORKER;
            }

            if (str == "TEEN") {
                return Type.TEEN;
            }

            if (str == "STUDENT") {
                return Type.STUDENT;
            }

            throw new ArgumentOutOfRangeException("str", "enum not implemented");
        }

        public bool CheckTime(Time elapsedSeconds, Date date) {
            if (Agent.IsMoving || Activities.Count == 0) {
                return false;
            }

            var next = Activities.First();
            if (next.Date.CurrentDay == date.CurrentDay && Math.Abs((float)date.CurrentClock.GetDifferenceInTicks(next.Date.CurrentClock) / TimeSpan.TicksPerSecond) < 5f) {
                Agent.Dispatch(next.Path);
                Activities.Enqueue(Activities.Dequeue());
            }

            return false;
        }
    }
}
