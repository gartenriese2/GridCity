namespace GridCity.Pathfinding {
    class Connection {
        public Node A { get; private set; }
        public Node B { get; private set; }
        public Connection(Node a, Node b) {
            A = a;
            B = b;
        }

        public Node getOther(Node node) {
            return node == A ? B : node == B ? A : null;
        }
    }
}
