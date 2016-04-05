namespace GridCity.Graphics {

    using Pencil.Gaming.MathUtils;

    internal class OrthographicCamera : Camera {

        public OrthographicCamera(Vector3 pos, float width, float height, float near, float far) : base(pos) {
            ProjMat = Matrix.CreateOrthographic(width, height, near, far);
        }

        // TODO: zoom methods
    }
}
