using GridCity.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GridCity.Fields {
    abstract class ConnectableField : Field {
        public enum Orientation_CW { ZERO, NINETY, ONEEIGHTY, TWOSEVENTY };
        private BaseNodeLayout BaseLayout { get; }
        public List<Node> Nodes { get; set; } = new List<Node>();
        protected ConnectableField(Utility.GlobalCoordinate pos, BaseNodeLayout layout, Orientation_CW orientation) : base(pos.X, pos.Y) {
            IsEmpty = false;
            BaseLayout = layout;
            initNodes(orientation);
        }
        private void initNodes(Orientation_CW orientation) {
            for (int i = 0; i < BaseLayout.NodeCoordinates.Count; ++i) {
                var coord = BaseLayout.NodeCoordinates[i];
                var wc = OrientCoordinate(coord, orientation).toWorldCoordinate(Position, k_scale);
                var info = BaseLayout.NodeInfos[i];
                Nodes.Add(new Node(wc, info));
            }
            for (int i = 0; i < BaseLayout.NodeConnections.Count; ++i) {
                var connection = BaseLayout.NodeConnections[i];
                var pathInfo = BaseLayout.PathInfos[i];
                Nodes[connection.Item1].addNextNode(Nodes[connection.Item2], pathInfo);
                Nodes[connection.Item2].addPreviousNode(Nodes[connection.Item1]);
            }
        }
        private Utility.LocalCoordinate OrientCoordinate(Utility.LocalCoordinate coord, Orientation_CW orientation) {
            switch (orientation) {
                case Orientation_CW.ZERO:
                    return coord;
                case Orientation_CW.NINETY:
                    return new Utility.LocalCoordinate(coord.Y, 1f - coord.X);
                case Orientation_CW.ONEEIGHTY:
                    return new Utility.LocalCoordinate(1f - coord.X, 1f - coord.Y);
                case Orientation_CW.TWOSEVENTY:
                    return new Utility.LocalCoordinate(1f - coord.Y, coord.X);
                default:
                    throw new ArgumentOutOfRangeException("orientation", "enum is not in use");
            }
        }
        private void replaceNode(Node node) {
            for (int i = 0; i < Nodes.Count; ++i) {
                if (Nodes[i].samePos(node)) {
                    if (!Nodes[i].Removed) {
                        throw new Exception("Cannot replace nodes that are not ready to be removed");
                    }
                    Nodes[i] = node;
                    break;
                }
            }
        }
        public List<Node> getBorderNodes(Border border) {
            List<Node> nodes = new List<Node>();
            var offset = new Utility.Coordinate(Position.X * k_scale, Position.Y * k_scale);
            switch (border) {
                case Border.LEFT:
                    foreach (Node node in Nodes) {
                        if (node.WorldPosition.X == offset.X) {
                            nodes.Add(node);
                        }
                    }
                    break;
                case Border.TOP:
                    foreach (Node node in Nodes) {
                        if (node.WorldPosition.Y == offset.Y + k_scale) {
                            nodes.Add(node);
                        }
                    }
                    break;
                case Border.RIGHT:
                    foreach (Node node in Nodes) {
                        if (node.WorldPosition.X == offset.X + k_scale) {
                            nodes.Add(node);
                        }
                    }
                    break;
                case Border.BOTTOM:
                    foreach (Node node in Nodes) {
                        if (node.WorldPosition.Y == offset.Y) {
                            nodes.Add(node);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("border", "There should not be that kind of border type!");
            }
            return nodes;
        }
        public bool connectTo(ConnectableField other) {
            if (!isNeighborOf(other)) {
                return false;
            }
            Border border = getConnectingBorder(other);
            Border otherBorder = (other).getConnectingBorder(this);
            List<Node> nodes = getBorderNodes(border);
            List<Node> otherNodes = other.getBorderNodes(otherBorder);
            if (nodes.Count != otherNodes.Count) {
                return false;
            }
            for (int i = 0; i < nodes.Count; ++i) {
                if (!nodes[i].samePos(otherNodes[nodes.Count - 1 - i])) {
                    return false;
                }
            }
            for (int i = 0; i < nodes.Count; ++i) {
                var thisNode = nodes[nodes.Count - 1 - i];
                var otherNode = otherNodes[i];
                foreach (var nextNodePair in otherNode.NextNodes) {
                    var nextNodeOfOtherNode = nextNodePair.Key;
                    foreach (var pathInfo in nextNodePair.Value) {
                        if (!thisNode.addNextNode(nextNodeOfOtherNode, pathInfo)) {
                            throw new Exception("Cannot add a node that's already there");
                        }
                    }
                    if (!nextNodeOfOtherNode.removePreviousNode(otherNode)) {
                        throw new Exception("Cannot remove a node that isn't there");
                    }
                    if (!nextNodeOfOtherNode.addPreviousNode(thisNode)) {
                        throw new Exception("Cannot add a node that's already there");
                    }
                }
                foreach (Node prevNode in otherNode.PreviousNodes) {
                    if (!thisNode.addPreviousNode(prevNode)) {
                        throw new Exception("Cannot add a node that's already there");
                    }
                    var pathInfos = prevNode.NextNodes[otherNode];
                    foreach (var pathInfo in pathInfos) {
                        if (!prevNode.addNextNode(thisNode, pathInfo)) {
                            throw new Exception("Cannot add a node that's already there");
                        }
                    }
                    if (!prevNode.removeNextNode(otherNode)) {
                        throw new Exception("Cannot remove a node that isn't there");
                    }
                }
                otherNode.Removed = true;
                thisNode.Info.Public = thisNode.Info.Public && otherNode.Info.Public;
                other.replaceNode(thisNode);
            }
            return true;
        }
        static public bool connect(List<ConnectableField> list) {
            if (list.Count <= 1) {
                return list.Count == 1;
            }
            for (int i = 0; i < list.Count - 1; ++i) {
                if (!list[i].connectTo(list[i + 1])) {
                    throw new ArgumentException("Not all Connectables were connectable");
                }
            }
            return true;
        }
    }
}
