using GridCity.People;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace GridCity.Fields {
    class FieldFactory {
        private XDocument FieldsDoc { get; }
        private XDocument NodesDoc { get; }
        private XDocument PathsDoc { get; }
        public FieldFactory() {
            FieldsDoc = XDocument.Parse(Properties.Resources.Fields);
            NodesDoc = XDocument.Parse(Properties.Resources.Nodes);
            PathsDoc = XDocument.Parse(Properties.Resources.Paths);
        }

        private List<Utility.LocalCoordinate> getCoordsFromElement(XElement el) {
            var nodesType = el.Elements("nodes");
            if (nodesType.Count() == 0) {
                throw new ArgumentException("Element has no nodes type!");
            }
            var nodes = nodesType.First().Elements("node");
            if (nodes.Count() == 0) {
                throw new ArgumentException("Element has no nodes!");
            }
            List<Utility.LocalCoordinate> list = new List<Utility.LocalCoordinate>();
            foreach (var node in nodes) {
                list.Add(new Utility.LocalCoordinate(float.Parse(node.Attribute("x").Value, CultureInfo.InvariantCulture), float.Parse(node.Attribute("y").Value, CultureInfo.InvariantCulture)));
            }
            return list;
        }
        private List<Pathfinding.NodeInfo> getNodeInfosFromElement(XElement el) {
            var nodesType = el.Elements("nodes");
            if (nodesType.Count() == 0) {
                throw new ArgumentException("Element has no nodes type!");
            }
            var nodes = nodesType.First().Elements("node");
            if (nodes.Count() == 0) {
                throw new ArgumentException("Element has no nodes!");
            }
            List<Pathfinding.NodeInfo> infos = new List<Pathfinding.NodeInfo>();
            foreach (var node in nodes) {
                string type = node.Attribute("type").Value;
                var types = from e in NodesDoc.Root.Elements("type") where e.Attribute("name").Value == type select e;
                if (types.Count() == 0) {
                    throw new NotImplementedException(type + " is not implemented in Nodes.xml");
                }
                var nodeElement = types.First();
                List<Pathfinding.NodeInfo.AllowedType> allowed = new List<Pathfinding.NodeInfo.AllowedType>();
                var allowedElements = from e2 in nodeElement.Elements("allowed") select e2.Value;
                foreach (var allowedString in allowedElements) {
                    allowed.Add(Pathfinding.NodeInfo.stringToAllowedType(allowedString));
                }
                var isPublicStr = nodeElement.Element("public").Value;
                bool isPublic = isPublicStr == "YES" ? true : false;
                Dictionary<Pathfinding.NodeInfo.AllowedType, Utility.Units.Time> penalties = new Dictionary<Pathfinding.NodeInfo.AllowedType, Utility.Units.Time>();
                var penaltyElements = from e3 in nodeElement.Elements("penalty") select e3;
                foreach (var penaltyElement in penaltyElements) {
                    var penalty = new Utility.Units.Time(float.Parse(penaltyElement.Value, CultureInfo.InvariantCulture));
                    penalties.Add(Pathfinding.NodeInfo.stringToAllowedType(penaltyElement.Attribute("type").Value), penalty);
                }
                infos.Add(new Pathfinding.NodeInfo { AllowedTypes = allowed, Public = isPublic, TimePenalties = penalties });
            }
            return infos;
        }
        private List<Pathfinding.PathInfo> getPathInfosFromElement(XElement el) {
            var pathsType = el.Elements("paths");
            if (pathsType.Count() == 0) {
                throw new ArgumentException("Element has no paths type!");
            }
            var paths = pathsType.First().Elements("path");
            if (paths.Count() == 0) {
                throw new ArgumentException("Element has no paths!");
            }
            List<Pathfinding.PathInfo> infos = new List<Pathfinding.PathInfo>();
            foreach (var path in paths) {
                string type = path.Attribute("type").Value;
                var types = from e in PathsDoc.Root.Elements("path") where e.Attribute("name").Value == type select e;
                if (types.Count() == 0) {
                    throw new NotImplementedException("This type of path is not implemented in the xml file");
                }
                var pathElement = types.First();
                var allowedType = Pathfinding.NodeInfo.stringToAllowedType(pathElement.Element("type").Value);
                var speedElement = pathElement.Element("speed");
                var speed = new Utility.Units.Speed(float.Parse(speedElement.Value, CultureInfo.InvariantCulture));
                var hiddenAttr = path.Attributes("hidden");
                bool hidden = hiddenAttr.Count() == 0 ? false : hiddenAttr.Single().Value == "yes" ? true : false;
                infos.Add(new Pathfinding.PathInfo { Type = allowedType, Speed = speed, Hidden = hidden });
            }
            return infos;
        }
        private List<Tuple<int, int>> getConnectionsFromElement(XElement el) {
            var pathsType = el.Elements("paths");
            if (pathsType.Count() == 0) {
                throw new ArgumentException("Element has no paths type!");
            }
            var paths = pathsType.First().Elements("path");
            if (paths.Count() == 0) {
                throw new ArgumentException("Element has no paths!");
            }
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            foreach (var path in paths) {
                int a = int.Parse(path.Attribute("from").Value);
                int b = int.Parse(path.Attribute("to").Value);
                list.Add(Tuple.Create(a, b));
            }
            return list;
        }
        public Roads.Road getRoad(string name, ConnectableField.Orientation_CW orientation, Utility.GlobalCoordinate pos) {
            var types = from el in FieldsDoc.Root.Elements("type") where (string)el.Attribute("name") == name select el;
            if (types.Count() == 0) {
                throw new NotImplementedException("This type of config is not implemented in the xml file");
            }
            var type = types.First();
            var coords = getCoordsFromElement(type);
            var nodeInfos = getNodeInfosFromElement(type);
            var connections = getConnectionsFromElement(type);
            var pathInfos = getPathInfosFromElement(type);
            var tex = (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject(name);
            if (tex == null) {
                throw new ArgumentException(name + " does not have a texture");
            }
            return new Roads.Road(pos, new Pathfinding.BaseNodeLayout(coords, nodeInfos, connections, pathInfos), orientation, (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject(name), name);
        }
        public T getOccupationalBuilding<T>(string name, ConnectableField.Orientation_CW orientation, Utility.GlobalCoordinate pos) where T : Buildings.OccupationalBuilding {
            var types = from el in FieldsDoc.Root.Elements("type") where (string)el.Attribute("name") == name select el;
            if (types.Count() == 0) {
                throw new NotImplementedException("This type of config is not implemented in the xml file");
            }
            var type = types.First();
            var coords = getCoordsFromElement(type);
            var nodeInfos = getNodeInfosFromElement(type);
            var connections = getConnectionsFromElement(type);
            var pathInfos = getPathInfosFromElement(type);
            var occupations = getOccupationsFromElement(type);
            var size = getSizeFromElement(type);

            return (T)Activator.CreateInstance(typeof(T), name, pos, new Pathfinding.BaseNodeLayout(coords, nodeInfos, connections, pathInfos), orientation, occupations, size);
        }
        public Buildings.ResidentialBuilding getResidentialBuilding(string name, ConnectableField.Orientation_CW orientation, Utility.GlobalCoordinate pos) {
            var types = from el in FieldsDoc.Root.Elements("type") where (string)el.Attribute("name") == name select el;
            if (types.Count() == 0) {
                throw new NotImplementedException("This type of config is not implemented in the xml file");
            }
            var type = types.First();
            var coords = getCoordsFromElement(type);
            var nodeInfos = getNodeInfosFromElement(type);
            var connections = getConnectionsFromElement(type);
            var pathInfos = getPathInfosFromElement(type);
            var numHouseholds = getHouseholdsFromElement(type);
            var size = getSizeFromElement(type);

            return new Buildings.ResidentialBuilding(name, pos, new Pathfinding.BaseNodeLayout(coords, nodeInfos, connections, pathInfos), orientation, numHouseholds, size);
        }

        private Dictionary<Resident.Type, uint> getOccupationsFromElement(XElement type) {
            var list = type.Elements("occupations");
            Dictionary<Resident.Type, uint> dic = new Dictionary<Resident.Type, uint>();
            foreach (var el in list) {
                var t = el.Attribute("type").Value;
                var min = uint.Parse(el.Attribute("min").Value);
                var max = uint.Parse(el.Attribute("max").Value);
                dic.Add(Resident.stringToType(t), Utility.RandomGenerator.get(min, max));
            }
            return dic;
        }
        private uint getHouseholdsFromElement(XElement type) {
            var el = type.Element("households");
            var min = uint.Parse(el.Attribute("min").Value);
            var max = uint.Parse(el.Attribute("max").Value);
            return Utility.RandomGenerator.get(min, max);
        }

        private Tuple<uint, uint> getSizeFromElement(XElement type) {
            var els = type.Elements("size");
            if (!els.Any()) {
                return Tuple.Create(1u, 1u);
            }
            var el = els.First();
            uint x = uint.Parse(el.Attribute("x").Value);
            uint y = uint.Parse(el.Attribute("y").Value);
            return Tuple.Create(x, y);
        }
    }
}
