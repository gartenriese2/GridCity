namespace GridCity.Graphics {

    using Pencil.Gaming.MathUtils;

    internal class OrthographicCamera : Camera {

        public OrthographicCamera(Vector3 pos, float width, float height, float near, float far) : base(pos) {
            Width = width;
            Height = height;
            ProjMat = Matrix.CreateOrthographic(width, height, near, far);
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public float Width { get; }

        public float Height { get; }

        // TODO: zoom methods
    }
}
