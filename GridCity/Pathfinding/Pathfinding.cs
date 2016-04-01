using GridCity.Fields.Buildings;
using GridCity.People;
using GridCity.Utility.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GridCity.Pathfinding {
    static class Pathfinder {
        public static Tuple<Path, Time> findQuickestPath(Building from, Building to, Traveller traveller) {
            return findQuickestPath(from.Nodes[0], to.Nodes[0], traveller);
        }
        private static Tuple<Path, Time> findQuickestPath(Node from, Node to, Traveller traveller) {
            if (!traveller.GetType().IsValueType) {
                throw new ArgumentException("traveller needs to be a copyable struct");
            }
            var allowedNodes = traveller.AllTypes;
            if (!from.Info.AllowedTypes.Intersect(allowedNodes).Any() || !to.Info.AllowedTypes.Intersect(allowedNodes).Any()) {
                Console.WriteLine("Warning: this kind of traveller can't use the start or end node.");
                return null;
            }

            List<Node> closedSet = new List<Node>();
            List<Tuple<Node, Traveller>> openSet = new List<Tuple<Node, Traveller>> { Tuple.Create(from, traveller) };
            Dictionary<Node, Tuple<Node, PathInfo>> cameFrom = new Dictionary<Node, Tuple<Node, PathInfo>>();
            Dictionary<Node, Time> gScore = new Dictionary<Node, Time>();
            gScore.Add(from, new Time(0));
            Dictionary<Node, Time> fScore = new Dictionary<Node, Time>();
            fScore.Add(from, heuristicTimeEstimate(from, to));

            while (openSet.Count != 0) {
                var ordered = fScore.OrderBy(x => x.Value);
                Node currentNode = ordered.First().Key;
                if (currentNode == to) {
                    return Tuple.Create(reconstructPath(cameFrom, to), gScore[currentNode]);
                }

                Time currentGScore = gScore[currentNode];
                var currentTraveller = openSet.Find(x => x.Item1 == currentNode).Item2;
                openSet.RemoveAll(x => x.Item1 == currentNode);
                gScore.Remove(currentNode);
                fScore.Remove(currentNode);
                closedSet.Add(currentNode);
                foreach (var nextNodePair in currentNode.NextNodes) {
                    var nextNode = nextNodePair.Key;
                    if (!nextNode.Info.Public && !currentTraveller.Keys.Contains(nextNode)) {
                        continue;
                    }
                    if (nextNode.Info.AllowedTypes.Intersect(currentTraveller.AllTypes).Count() == 0 && !nextNode.Info.AllowedTypes.Contains(currentTraveller.CurrentType)) {
                        continue;
                    }
                    if (closedSet.Contains(nextNode)) {
                        continue;
                    }
                    foreach (var pathInfo in nextNodePair.Value) {
                        var type = pathInfo.Type;
                        if (!currentTraveller.AllTypes.Contains(type) && currentTraveller.CurrentType != type) {
                            continue;
                        }
                        Traveller nextTraveller = Traveller.Copy(currentTraveller);
                        if (type != nextTraveller.CurrentType) {
                            nextTraveller.CurrentType = type;
                            if (!nextTraveller.ReusableTypes.Contains(type)) {
                                nextTraveller.NonReusableTypes.Remove(type);
                            }
                        }

                        Time tentativeGScore = currentGScore + currentNode.getDistanceTo(nextNode) / pathInfo.Speed;
                        if (currentNode.Info.TimePenalties.ContainsKey(nextTraveller.CurrentType)) {
                            tentativeGScore += currentNode.Info.TimePenalties[nextTraveller.CurrentType];
                        }
                        if (!openSet.Any(x => x.Item1 == nextNode)) {
                            openSet.Add(Tuple.Create(nextNode, nextTraveller));
                        } else if (tentativeGScore >= gScore[nextNode]) {
                            continue;
                        }

                        cameFrom[nextNode] = Tuple.Create(currentNode, pathInfo);
                        gScore[nextNode] = tentativeGScore;
                        fScore[nextNode] = gScore[nextNode] + heuristicTimeEstimate(nextNode, to);
                    }
                }
            }

            return null;
        }

        private static Time heuristicTimeEstimate(Node from, Node to) {
            var dist = from.getDistanceTo(to);
            var speed = new Speed(0);
            speed.KMH = 30f;
            return dist / speed;
        }

        private static Path reconstructPath(Dictionary<Node, Tuple<Node, PathInfo>> cameFrom, Node current) {
            List<Node> nodes = new List<Node> { current };
            List<PathInfo> infos = new List<PathInfo>();
            Node tmp = current;
            while (cameFrom.ContainsKey(tmp)) {
                var tuple = cameFrom[tmp];
                tmp = tuple.Item1;
                nodes.Add(tuple.Item1);
                infos.Add(tuple.Item2);
            }
            nodes.Reverse();
            infos.Reverse();
            return new Path(nodes, infos);
        }

        //private static Path findShortestPath(Node from, Node to, List<NodeInfo.AllowedType> allowedNodes) {
        //    if (!from.Info.AllowedTypes.Intersect(allowedNodes).Any() || !to.Info.AllowedTypes.Intersect(allowedNodes).Any()) {
        //        Console.WriteLine("Warning: this kind of traveller can't use the start or end node.");
        //        return null;
        //    }

        //    List<Node> closedSet = new List<Node>();
        //    List<Node> openSet = new List<Node> { from };
        //    Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        //    Dictionary<Node, float> gScore = new Dictionary<Node, float>();
        //    gScore.Add(from, 0f);
        //    Dictionary<Node, float> fScore = new Dictionary<Node, float>();
        //    fScore.Add(from, heuristicDistanceEstimate(from, to));

        //    while (openSet.Count != 0) {
        //        var ordered = fScore.OrderBy(x => x.Value);
        //        Node current = ordered.First().Key;
        //        if (current == to) {
        //            return reconstructPath(cameFrom, to);
        //        }

        //        var currentGScore = gScore[current];
        //        openSet.Remove(current);
        //        gScore.Remove(current);
        //        fScore.Remove(current);
        //        closedSet.Add(current);
        //        foreach (var nextNodePair in current.NextNodes) {
        //            var nextNode = nextNodePair.Key;
        //            if (!nextNode.Info.AllowedTypes.Intersect(allowedNodes).Any()) {
        //                continue;
        //            }
        //            if (closedSet.Contains(nextNode)) {
        //                continue;
        //            }
        //            var tentativeGScore = currentGScore + current.getDistanceTo(nextNode);
        //            if (!openSet.Contains(nextNode)) {
        //                openSet.Add(nextNode);
        //            } else if (tentativeGScore >= gScore[nextNode]) {
        //                continue;
        //            }

        //            cameFrom[nextNode] = current;
        //            gScore[nextNode] = tentativeGScore;
        //            fScore[nextNode] = gScore[nextNode] + heuristicDistanceEstimate(nextNode, to);
        //        }
        //    }

        //    return null;
        //}
        //private static Path findShortestPath(Node from, Node to) {
        //    List<Node> closedSet = new List<Node>();
        //    List<Node> openSet = new List<Node> { from };
        //    Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        //    Dictionary<Node, float> gScore = new Dictionary<Node, float>();
        //    gScore.Add(from, 0f);
        //    Dictionary<Node, float> fScore = new Dictionary<Node, float>();
        //    fScore.Add(from, heuristicDistanceEstimate(from, to));

        //    while (openSet.Count != 0) {
        //        var ordered = fScore.OrderBy(x => x.Value);
        //        Node current = ordered.First().Key;
        //        if (current == to) {
        //            return reconstructPath(cameFrom, to);
        //        }

        //        var currentGScore = gScore[current];
        //        openSet.Remove(current);
        //        gScore.Remove(current);
        //        fScore.Remove(current);
        //        closedSet.Add(current);
        //        foreach (var nextNodePair in current.NextNodes) {
        //            var nextNode = nextNodePair.Key;
        //            if (closedSet.Contains(nextNode)) {
        //                continue;
        //            }
        //            var tentativeGScore = currentGScore + current.getDistanceTo(nextNode);
        //            if (!openSet.Contains(nextNode)) {
        //                openSet.Add(nextNode);
        //            } else if (tentativeGScore >= gScore[nextNode]) {
        //                continue;
        //            }

        //            cameFrom[nextNode] = current;
        //            gScore[nextNode] = tentativeGScore;
        //            fScore[nextNode] = gScore[nextNode] + heuristicDistanceEstimate(nextNode, to);
        //        }
        //    }

        //    return null;
        //}

        private static Distance heuristicDistanceEstimate(Node a, Node b) {
            return a.getDistanceTo(b);
        }

        private static List<Node> reconstructPath(Dictionary<Node, Node> cameFrom, Node current) {
            List<Node> totalPath = new List<Node> { current };
            Node tmp = current;
            while (cameFrom.ContainsKey(tmp)) {
                tmp = cameFrom[tmp];
                totalPath.Add(tmp);
            }
            totalPath.Reverse();
            return totalPath;
        }

        public static void printPath(List<Node> path) {
            if (path != null) {
                foreach (Node node in path) {
                    Console.WriteLine(node.ToString());
                }
            }
        }
    }
}
