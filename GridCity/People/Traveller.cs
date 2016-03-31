using System.Collections.Generic;
using System.Linq;

namespace GridCity.People {
    struct Traveller {
        public List<Pathfinding.NodeInfo.AllowedType> ReusableTypes { get; private set; }
        public List<Pathfinding.NodeInfo.AllowedType> NonReusableTypes { get; private set; }
        public List<Pathfinding.NodeInfo.AllowedType> AllTypes => ReusableTypes.Concat(NonReusableTypes).ToList();
        public Pathfinding.NodeInfo.AllowedType CurrentType { get; set; }
        public List<Pathfinding.Node> Keys { get; private set; }
        public static Traveller Create() {
            return new Traveller {
                ReusableTypes = new List<Pathfinding.NodeInfo.AllowedType> { Pathfinding.NodeInfo.AllowedType.PEDSTRIAN },
                NonReusableTypes = new List<Pathfinding.NodeInfo.AllowedType>(),
                CurrentType = Pathfinding.NodeInfo.AllowedType.PEDSTRIAN,
                Keys = new List<Pathfinding.Node>()
            };
        }
        public static Traveller Copy(Traveller other) {
            Traveller newTraveller = Create();
            newTraveller.ReusableTypes.Clear();
            foreach (var reusableType in other.ReusableTypes) {
                newTraveller.ReusableTypes.Add(reusableType);
            }
            foreach (var nonReusableType in other.NonReusableTypes) {
                newTraveller.NonReusableTypes.Add(nonReusableType);
            }
            newTraveller.CurrentType = other.CurrentType;
            foreach (var key in other.Keys) {
                newTraveller.Keys.Add(key);
            }
            return newTraveller;
        }
    }
}
