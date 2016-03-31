using GridCity.Graphics;
using GridCity.Utility;
using GridCity.Utility.Units;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GridCity.People {
    class Agent : ITickable, IDrawable {
        public static List<Agent> Agents { get; } = new List<Agent>();
        public static Agent create() {
            Agents.Add(new Agent());
            return Agents.Last();
        }
        private Agent() {
            Texture = new Texture("Agent");
        }
        public bool IsMoving { get; private set; } = false;
        public bool IsVisible { get; private set; } = false;
        private Pathfinding.Path Path { get; set; }
        private int Idx { get; set; }
        private Pathfinding.Node PreviousNode { get; set; }
        private Pathfinding.Node NextNode { get; set; }
        private Pathfinding.PathInfo Info { get; set; }
        private Time CurrentWait { get; set; } = new Time(0);
        private Coordinate Position { get; set; }
        public List<Coordinate> Trace { get; private set; } = new List<Coordinate>();

        public Texture Texture {
            get;
        }

        public bool tick(Time elapsedSeconds) {
            if (!IsMoving)
                return false;

            //if ((float)elapsedSeconds > 0.5) {
            //    Console.WriteLine("Warning: elapsedSeconds is pretty big: " + elapsedSeconds);
            //}

            if (CurrentWait < elapsedSeconds) {
                checkOtherAgents();
            }

            if (CurrentWait >= elapsedSeconds) {
                CurrentWait -= elapsedSeconds;
                return true;
            }

            Time secondsLeft = new Time((float)elapsedSeconds);
            if (CurrentWait.Seconds > 0) {
                secondsLeft -= CurrentWait;
                CurrentWait.Seconds = 0;
            }

            var distToNextNode = (new Vec2D(NextNode.WorldPosition) - new Vec2D(Position)).Length;
            Distance remainingDist = Info.Speed * secondsLeft;
            while (remainingDist >= distToNextNode) {
                if (NextNode == Path.Nodes.Last()) {
                    IsMoving = false;
                    IsVisible = false;
                    Position = NextNode.WorldPosition;
                    Trace.Add(new Coordinate(Position.X, Position.Y));
                    return true;
                } else {
                    secondsLeft -= distToNextNode / Info.Speed;
                    ++Idx;
                    PreviousNode = Path.Nodes[Idx];
                    NextNode = Path.Nodes[Idx + 1];
                    Info = Path.Infos[Idx];
                    IsVisible = !Info.Hidden;
                    Position = PreviousNode.WorldPosition;
                    if (PreviousNode.Info.TimePenalties.ContainsKey(Info.Type)) { // time penalty
                        CurrentWait.Seconds = PreviousNode.Info.TimePenalties[Info.Type].Seconds;
                        if (CurrentWait >= secondsLeft) {
                            CurrentWait -= secondsLeft;
                            Trace.Add(new Coordinate(Position.X, Position.Y));
                            return true;
                        } else {
                            secondsLeft -= CurrentWait;
                            CurrentWait.Seconds = 0;
                        }
                    }
                    distToNextNode = (new Vec2D(NextNode.WorldPosition) - new Vec2D(Position)).Length;
                    remainingDist = Info.Speed * secondsLeft;
                }
            }
            Vec2D dir = new Vec2D(NextNode.WorldPosition) - new Vec2D(PreviousNode.WorldPosition);
            Vec2D dirCompare = new Vec2D(NextNode.WorldPosition) - new Vec2D(Position);
            dir.normalize();
            dirCompare.normalize();
            Contract.Assert(dir.X == dirCompare.X && dir.Y == dirCompare.Y);
            dir *= (float)remainingDist;
            Position = (new Vec2D(Position) + dir).toCoordinate();
            Trace.Add(new Coordinate(Position.X, Position.Y));
            return true;
        }
        public void dispatch(Pathfinding.Path path, Time alreadyElapsedTime) {
            dispatch(path);
            if ((float)alreadyElapsedTime > 0) {
                tick(alreadyElapsedTime);
            }
        }
        public void dispatch(Pathfinding.Path path) {
            Contract.Requires(path != null && path.Nodes.Count > 1);
            Path = path;
            Idx = 0;
            PreviousNode = path.Nodes[Idx];
            NextNode = path.Nodes[Idx + 1];
            Info = path.Infos[Idx];
            CurrentWait.Seconds = PreviousNode.Info.TimePenalties.ContainsKey(Info.Type) ? PreviousNode.Info.TimePenalties[Info.Type].Seconds : 0;
            Position = PreviousNode.WorldPosition;
            Trace.Add(Position);
            IsMoving = true;
            IsVisible = !Info.Hidden;
        }
        private bool checkOtherAgents() {
            foreach (var agent in Agents) {
                if (this == agent || !agent.IsMoving) {
                    continue;
                }
                if (agent.PreviousNode == PreviousNode && agent.NextNode == NextNode) {
                    // on same path
                    Distance dist = (Position.toVec() - PreviousNode.WorldPosition.toVec()).Length;
                    Distance otherDist = (agent.Position.toVec() - PreviousNode.WorldPosition.toVec()).Length;
                    if (otherDist > dist) {
                        // other agent in front of this agent
                        // we don't check for equality, because then one agent will advance while the others checked after that will have to wait
                        Speed currentPossibleSpeed = Info.Speed;
                        Distance minDistBetweenAgents = Pathfinding.NodeInfo.MinDist(Info.Type);
                        Distance distBetweenAgents = otherDist - dist;
                        if (distBetweenAgents < minDistBetweenAgents) {
                            Distance wait = minDistBetweenAgents - distBetweenAgents;
                            Time waitTime = wait / currentPossibleSpeed;
                            if (CurrentWait < waitTime) {
                                CurrentWait = waitTime;
                                return true;
                            }
                        }
                    }
                } else if (agent.NextNode == NextNode) {
                    // going to same node
                    Distance distToNode = (NextNode.WorldPosition.toVec() - Position.toVec()).Length;
                    if ((float)distToNode < 1f) {
                        Distance otherDistToNode = (NextNode.WorldPosition.toVec() - agent.Position.toVec()).Length;
                        if (distToNode > otherDistToNode) {
                            // yield
                            Time yieldTime = otherDistToNode / agent.Info.Speed;
                            if (CurrentWait < yieldTime) {
                                CurrentWait = yieldTime;
                                return true;
                            }
                        }
                    }
                } else if (Path.Nodes.Count > Idx + 2 && agent.PreviousNode == NextNode && agent.NextNode == Path.Nodes[Idx + 2]) {
                    // other agent is in front of path after next node
                    Distance distBetweenAgents = (agent.Position.toVec() - agent.PreviousNode.WorldPosition.toVec()).Length + (NextNode.WorldPosition.toVec() - Position.toVec()).Length;
                    Distance minDistBetweenAgents = Pathfinding.NodeInfo.MinDist(Info.Type);
                    if (distBetweenAgents < minDistBetweenAgents) {
                        Distance wait = minDistBetweenAgents - distBetweenAgents;
                        Time waitTime = wait / Info.Speed;
                        if (CurrentWait < waitTime) {
                            CurrentWait = waitTime;
                            return true;
                        }
                    }
                } else if (Path.Nodes.Count > Idx + 3 && agent.PreviousNode == Path.Nodes[Idx + 2] && agent.NextNode == Path.Nodes[Idx + 3]) {
                    // other agent is in front of path after two next nodes
                    Distance distBetweenAgents = (agent.Position.toVec() - agent.PreviousNode.WorldPosition.toVec()).Length
                                                + (NextNode.WorldPosition.toVec() - Position.toVec()).Length
                                                + (agent.PreviousNode.WorldPosition.toVec() - NextNode.WorldPosition.toVec()).Length;
                    Distance minDistBetweenAgents = Pathfinding.NodeInfo.MinDist(Info.Type);
                    if (distBetweenAgents < minDistBetweenAgents) {
                        Distance wait = minDistBetweenAgents - distBetweenAgents;
                        Time waitTime = wait / Info.Speed;
                        if (CurrentWait < waitTime) {
                            CurrentWait = waitTime;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public override string ToString() {
            return "Agent at " + Position;
        }
    }
}
