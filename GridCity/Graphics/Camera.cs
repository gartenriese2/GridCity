using Pencil.Gaming.MathUtils;

namespace GridCity.Graphics {
    class Camera {
        public Matrix ProjMat { get; protected set; }
        public Matrix ViewMat { get; protected set; }
        private Vector3 Pos { get; set; }
        private Vector3 Dir { get; } = new Vector3(0, 0, -1);
        private Vector3 Up { get; } = new Vector3(0, 1, 0);
        protected Camera(Vector3 pos) {
            Pos = pos;
            createViewMat();
        }
        private void createViewMat() {
            ViewMat = Matrix.LookAt(Pos, Pos + Dir, Up);
        }
        public void move(Vector3 v) {
            Pos += v;
            createViewMat();
        }
    }
    class OrthographicCamera : Camera {
        public OrthographicCamera(Vector3 pos, float width, float height, float near, float far) : base(pos) {
            ProjMat = Matrix.CreateOrthographic(width, height, near, far);
        }
        // TODO: zoom methods
    }
}
