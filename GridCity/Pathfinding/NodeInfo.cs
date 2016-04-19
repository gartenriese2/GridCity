namespace GridCity.Pathfinding {

    using System;
    using System.Collections.Generic;

    internal class NodeInfo {

        //---------------------------------------------------------------------
        // Enumerations
        //---------------------------------------------------------------------
        public enum AllowedType {
            PEDSTRIAN, CAR
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public List<AllowedType> AllowedTypes { get; set; } = new List<AllowedType>();

        public bool Public { get; set; }

        public Dictionary<AllowedType, Utility.Units.Time> TimePenalties { get; set; } = new Dictionary<AllowedType, Utility.Units.Time>();

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public static AllowedType StringToAllowedType(string str) {
            if (str == "PEDESTRIAN") {
                return AllowedType.PEDSTRIAN;
            }

            if (str == "CAR") {
                return AllowedType.CAR;
            }

            throw new ArgumentException("There is no enum for that string");
        }

        public static Utility.Units.Distance MinDist(AllowedType type) {
            switch (type) {
                case AllowedType.PEDSTRIAN:
                    return new Utility.Units.Distance(1f);
                case AllowedType.CAR:
                    return new Utility.Units.Distance(4f);
                default:
                    throw new ArgumentOutOfRangeException("type", "enum is not implemented");
            }
        }
    }
}
