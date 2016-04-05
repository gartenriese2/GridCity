namespace GridCity.Pathfinding {

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fields.Buildings;
    using People;
    using Utility.Units;
    
    internal static class Pathfinder {

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public static Tuple<Path, Time> FindQuickestPath(Building from, Building to, Traveller traveller) {
            return FindQuickestPath(from.Nodes[0], to.Nodes[0], traveller);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        private static Tuple<Path, Time> FindQuickestPath(Node from, Node to, Traveller traveller) {
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
            fScore.Add(from, HeuristicTimeEstimate(from, to));

            while (openSet.Count != 0) {
                var ordered = fScore.OrderBy(x => x.Value);
                Node currentNode = ordered.First().Key;
                if (currentNode == to) {
                    return Tuple.Create(ReconstructPath(cameFrom, to), gScore[currentNode]);
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

                        Time tentativeGScore = currentGScore + (currentNode.GetDistanceTo(nextNode) / pathInfo.Speed);
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
                        fScore[nextNode] = gScore[nextNode] + HeuristicTimeEstimate(nextNode, to);
                    }
                }
            }

            return null;
        }

        private static Time HeuristicTimeEstimate(Node from, Node to) {
            var dist = from.GetDistanceTo(to);
            var speed = new Speed(0);
            speed.KMH = 30f;
            return dist / speed;
        }

        private static Path ReconstructPath(Dictionary<Node, Tuple<Node, PathInfo>> cameFrom, Node current) {
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
    }
}
