namespace GridCity.Pathfinding {

    using System;
    using System.Collections.Generic;

    internal class BaseNodeLayout {

        public BaseNodeLayout(List<Utility.LocalCoordinate> coords, List<NodeInfo> nodeInfos, List<Tuple<int, int>> connections, List<PathInfo> pathInfos) {
            if (coords.Count != nodeInfos.Count) {
                throw new ArgumentException("There are not the same amount of coordinates as there are node infos");
            }

            if (connections.Count != pathInfos.Count) {
                throw new ArgumentException("There are not the same amount of connections as there are path infos");
            }

            NodeCoordinates = coords;
            NodeInfos = nodeInfos;
            foreach (var t in connections) {
                if (t.Item1 < 0 || t.Item1 >= NodeCoordinates.Count || t.Item2 < 0 || t.Item2 >= NodeCoordinates.Count) {
                    throw new ArgumentOutOfRangeException("connections", "The list of tuples uses and index out of range.");
                }
            }

            NodeConnections = connections;
            PathInfos = pathInfos;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public List<Utility.LocalCoordinate> NodeCoordinates { get; }

        public List<NodeInfo> NodeInfos { get; }

        public List<Tuple<int, int>> NodeConnections { get; }

        public List<PathInfo> PathInfos { get; }
    }
}
