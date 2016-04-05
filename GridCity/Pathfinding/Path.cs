namespace GridCity.Pathfinding {

    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    internal struct PathInfo {
        public NodeInfo.AllowedType Type { get; set; }

        public Utility.Units.Speed Speed { get; set; }

        public bool Hidden { get; set; }
    }

    internal class Path {

        public Path(List<Node> nodes, List<PathInfo> infos) {
            Contract.Requires(nodes != null && infos != null && nodes.Count == infos.Count + 1);
            Nodes = nodes;
            Infos = infos;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public List<Node> Nodes { get; private set; }

        public List<PathInfo> Infos { get; private set; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public Utility.Units.Distance GetLength() {
            Utility.Units.Distance len = new Utility.Units.Distance(0);
            for (int i = 0; i < Nodes.Count - 1; ++i) {
                len += Nodes[i].GetDistanceTo(Nodes[i + 1]);
            }

            return len;
        }
    }
}
