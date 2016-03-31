using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GridCity.Pathfinding {
    struct PathInfo {
        public NodeInfo.AllowedType Type { get; set; }
        public Utility.Units.Speed Speed { get; set; }
        public bool Hidden { get; set; }
    }
    class Path {
        public List<Node> Nodes { get; private set; }
        public List<PathInfo> Infos { get; private set; }
        public Path(List<Node> nodes, List<PathInfo> infos) {
            Contract.Requires(nodes != null && infos != null && nodes.Count == infos.Count + 1);
            Nodes = nodes;
            Infos = infos;
        }
        public Utility.Units.Distance getLength() {
            Utility.Units.Distance len = new Utility.Units.Distance(0);
            for (int i = 0; i < Nodes.Count - 1; ++i) {
                len += Nodes[i].getDistanceTo(Nodes[i + 1]);
            }
            return len;
        }
    }
}
