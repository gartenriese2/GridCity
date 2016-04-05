namespace GridCity.Pathfinding {

    using System;
    using System.Collections.Generic;

    internal class Node {

        public Node(Utility.Coordinate pos, NodeInfo info) {
            WorldPosition = pos;
            Info = info;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public Utility.Coordinate WorldPosition { get; }

        public NodeInfo Info { get; }

        public Dictionary<Node, List<PathInfo>> NextNodes { get; } = new Dictionary<Node, List<PathInfo>>();

        public List<Node> PreviousNodes { get; } = new List<Node>();

        public bool Removed { get; set; } = false;

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public override string ToString() {
            return "(" + WorldPosition.X + "|" + WorldPosition.Y + ")";
        }

        public bool SamePos(Node other) {
            return WorldPosition.X == other.WorldPosition.X && WorldPosition.Y == other.WorldPosition.Y;
        }

        public bool AddNextNode(Node node, PathInfo info) {
            if (!NextNodes.ContainsKey(node)) {
                NextNodes.Add(node, new List<PathInfo> { info });
                return true;
            } else if (!NextNodes[node].Contains(info)) {
                NextNodes[node].Add(info);
                return true;
            }

            return false;
        }

        public bool AddPreviousNode(Node node) {
            if (!PreviousNodes.Contains(node)) {
                PreviousNodes.Add(node);
                return true;
            }

            return false;
        }

        public bool RemoveNextNode(Node node) {
            return NextNodes.Remove(node);
        }

        public bool RemovePreviousNode(Node node) {
            return PreviousNodes.Remove(node);
        }
        
        public Utility.Units.Distance GetDistanceTo(Node other) {
            return new Utility.Units.Distance((float)Math.Sqrt(Math.Pow(WorldPosition.X - other.WorldPosition.X, 2) + Math.Pow(WorldPosition.Y - other.WorldPosition.Y, 2)));
        }
    }
}
