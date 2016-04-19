namespace GridCity.Graphics {

    using Pencil.Gaming.MathUtils;

    internal class Camera {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        protected Camera(Vector3 pos) {
            Pos = pos;
            CreateViewMat();
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public Matrix ProjMat { get; protected set; }

        public Matrix ViewMat { get; protected set; }

        private Vector3 Pos { get; set; }

        private Vector3 Dir { get; } = new Vector3(0, 0, -1);

        private Vector3 Up { get; } = new Vector3(0, 1, 0);

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public void Move(Vector3 v) {
            Pos += v;
            CreateViewMat();
        }

        private void CreateViewMat() {
            ViewMat = Matrix.LookAt(Pos, Pos + Dir, Up);
        }
    }
}
