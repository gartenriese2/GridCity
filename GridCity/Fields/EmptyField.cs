namespace GridCity.Fields {

    internal class EmptyField : Field {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public EmptyField(Utility.GlobalCoordinate pos) : base(pos.X, pos.Y) {
            Texture = new Graphics.Texture("EmptyField");
        }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            return "Empty Field " + Position;
        }
    }
}
