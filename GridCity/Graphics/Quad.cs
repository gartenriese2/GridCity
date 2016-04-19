namespace GridCity.Graphics {

    using Pencil.Gaming.Graphics;

    internal class Quad {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Quad() {
            Init();
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        private int VAO { get; } = GL.GenVertexArray();

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public void Bind() {
            GL.BindVertexArray(VAO);
        }

        public void Unbind() {
            GL.BindVertexArray(0);
        }

        public void Draw() {
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        private void Init() {
            Gl.VertexBuffer vbo = new Gl.VertexBuffer(0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0);
            Gl.VertexBuffer uv_vbo = new Gl.VertexBuffer(0, 0, 1, 0, 1, 1, 0, 1);
            Gl.IndexBuffer ibo = new Gl.IndexBuffer(0, 1, 2, 0, 2, 3);

            Bind();
            ibo.Bind();
            vbo.Bind();
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0, 0);
            uv_vbo.Bind();
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, true, 0, 0);
            Unbind();
            uv_vbo.Unbind();
        }
    }
}
