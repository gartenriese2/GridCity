using System;
using System.Collections.Generic;

namespace GridCity.Pathfinding {
    class NodeInfo {
        public enum AllowedType { PEDSTRIAN, CAR }
        public static AllowedType stringToAllowedType(string str) {
            if (str == "PEDESTRIAN")
                return AllowedType.PEDSTRIAN;
            if (str == "CAR")
                return AllowedType.CAR;
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
        public List<AllowedType> AllowedTypes { get; set; } = new List<AllowedType>();
        public bool Public { get; set; }
        public Dictionary<AllowedType, Utility.Units.Time> TimePenalties { get; set; } = new Dictionary<AllowedType, Utility.Units.Time>();
    }
    class Node {
        public Utility.Coordinate WorldPosition { get; }
        public NodeInfo Info { get; }
        public Dictionary<Node, List<PathInfo>> NextNodes { get; } = new Dictionary<Node, List<PathInfo>>();
        public List<Node> PreviousNodes { get; } = new List<Node>();
        public bool Removed { get; set; } = false;
        public Node(Utility.Coordinate pos, NodeInfo info) {
            WorldPosition = pos;
            Info = info;
        }
        public bool samePos(Node other) {
            return WorldPosition.X == other.WorldPosition.X && WorldPosition.Y == other.WorldPosition.Y;
        }
        public bool addNextNode(Node node, PathInfo info) {
            if (!NextNodes.ContainsKey(node)) {
                NextNodes.Add(node, new List<PathInfo> { info });
                return true;
            } else if (!NextNodes[node].Contains(info)) {
                NextNodes[node].Add(info);
                return true;
            }
            return false;
        }
        public bool addPreviousNode(Node node) {
            if (!PreviousNodes.Contains(node)) {
                PreviousNodes.Add(node);
                return true;
            }
            return false;
        }
        public bool removeNextNode(Node node) {
            return NextNodes.Remove(node);
        }
        public bool removePreviousNode(Node node) {
            return PreviousNodes.Remove(node);
        }
        public override string ToString() {
            return "(" + WorldPosition.X + "|" + WorldPosition.Y + ")";
        }
        public Utility.Units.Distance getDistanceTo(Node other) {
            return new Utility.Units.Distance((float)Math.Sqrt(Math.Pow(WorldPosition.X - other.WorldPosition.X, 2) + Math.Pow(WorldPosition.Y - other.WorldPosition.Y, 2)));
        }
    }
}
