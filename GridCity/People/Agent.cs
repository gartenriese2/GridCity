namespace GridCity.People {

    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Graphics;
    using Simulation;
    using Utility;
    using Utility.Units;
    
    internal class Agent : ITickable, IDrawable {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        private Agent() {
            Texture = new Texture("Agent");
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public static List<Agent> Agents { get; } = new List<Agent>();

        public bool IsMoving { get; private set; } = false;

        public bool IsVisible { get; private set; } = false;

        public List<Coordinate> Trace { get; private set; } = new List<Coordinate>();

        public Texture Texture { get; }

        private Pathfinding.Path Path { get; set; }

        private int Idx { get; set; }

        private Pathfinding.Node PreviousNode { get; set; }

        private Pathfinding.Node NextNode { get; set; }

        private Pathfinding.PathInfo Info { get; set; }

        private Time CurrentWait { get; set; } = Time.Zero;

        private Coordinate Position { get; set; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public static Agent Create() {
            Agents.Add(new Agent());
            return Agents.Last();
        }

        public override string ToString() {
            return "Agent at " + Position;
        }

        public bool Tick(Time elapsedTime) {
            if (!IsMoving) {
                return false;
            }

            ////if ((float)elapsedSeconds > 0.5) {
            ////    Console.WriteLine("Warning: elapsedSeconds is pretty big: " + elapsedSeconds);
            ////}

            if (CurrentWait < elapsedTime) {
                CheckOtherAgents();
            }

            if (CurrentWait >= elapsedTime) {
                CurrentWait -= elapsedTime;
                return true;
            }

            Time secondsLeft = new Time(elapsedTime);
            if (CurrentWait.Seconds > 0) {
                secondsLeft -= CurrentWait;
                CurrentWait = Time.Zero;
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
                        CurrentWait = new Time(PreviousNode.Info.TimePenalties[Info.Type]);
                        if (CurrentWait >= secondsLeft) {
                            CurrentWait -= secondsLeft;
                            Trace.Add(new Coordinate(Position.X, Position.Y));
                            return true;
                        } else {
                            secondsLeft -= CurrentWait;
                            CurrentWait = Time.Zero;
                        }
                    }

                    distToNextNode = (new Vec2D(NextNode.WorldPosition) - new Vec2D(Position)).Length;
                    remainingDist = Info.Speed * secondsLeft;
                }
            }

            Vec2D dir = new Vec2D(NextNode.WorldPosition) - new Vec2D(PreviousNode.WorldPosition);
            Vec2D dirCompare = new Vec2D(NextNode.WorldPosition) - new Vec2D(Position);
            dir.Normalize();
            dirCompare.Normalize();
            Debug.Assert(dir.X == dirCompare.X && dir.Y == dirCompare.Y, "Position is not on the vector from PreviousNode to NextNode");
            dir *= (float)remainingDist;
            Position = (new Vec2D(Position) + dir).ToCoordinate();
            Trace.Add(new Coordinate(Position.X, Position.Y));
            return true;
        }

        public void Dispatch(Pathfinding.Path path, Time alreadyElapsedTime) {
            Dispatch(path);
            if (alreadyElapsedTime > Time.Zero) {
                Tick(alreadyElapsedTime);
            }
        }

        public void Dispatch(Pathfinding.Path path) {
            Debug.Assert(path.Nodes.Count > 1, "path must have at least 2 Nodes");
            Path = path;
            Idx = 0;
            PreviousNode = path.Nodes[Idx];
            NextNode = path.Nodes[Idx + 1];
            Info = path.Infos[Idx];
            CurrentWait = PreviousNode.Info.TimePenalties.ContainsKey(Info.Type) ? new Time(PreviousNode.Info.TimePenalties[Info.Type]) : Time.Zero;
            Position = PreviousNode.WorldPosition;
            Trace.Add(Position);
            IsMoving = true;
            IsVisible = !Info.Hidden;
        }

        private bool CheckOtherAgents() {
            foreach (var agent in Agents) {
                if (this == agent || !agent.IsMoving) {
                    continue;
                }

                if (agent.PreviousNode == PreviousNode && agent.NextNode == NextNode) {
                    // on same path
                    Distance dist = (Position.ToVec() - PreviousNode.WorldPosition.ToVec()).Length;
                    Distance otherDist = (agent.Position.ToVec() - PreviousNode.WorldPosition.ToVec()).Length;
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
                    Distance distToNode = (NextNode.WorldPosition.ToVec() - Position.ToVec()).Length;
                    if ((float)distToNode < 1f) {
                        Distance otherDistToNode = (NextNode.WorldPosition.ToVec() - agent.Position.ToVec()).Length;
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
                    Distance distBetweenAgents = (agent.Position.ToVec() - agent.PreviousNode.WorldPosition.ToVec()).Length + (NextNode.WorldPosition.ToVec() - Position.ToVec()).Length;
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
                    Distance distBetweenAgents = (agent.Position.ToVec() - agent.PreviousNode.WorldPosition.ToVec()).Length
                                                + (NextNode.WorldPosition.ToVec() - Position.ToVec()).Length
                                                + (agent.PreviousNode.WorldPosition.ToVec() - NextNode.WorldPosition.ToVec()).Length;
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
    }
}
