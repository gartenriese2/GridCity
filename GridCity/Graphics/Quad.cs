using Pencil.Gaming.Graphics;

namespace GridCity.Graphics {
    class Quad {
        private int VAO { get; }
        public Quad() {
            VAO = GL.GenVertexArray();
            
            Gl.VBO vbo = new Gl.VBO(0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0);
            Gl.VBO uv_vbo = new Gl.VBO(0, 0, 1, 0, 1, 1, 0, 1);
            Gl.IBO ibo = new Gl.IBO(0, 1, 2, 0, 2, 3);

            bind();
            ibo.bind();
            vbo.bind();
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0, 0);
            uv_vbo.bind();
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, true, 0, 0);
            unbind();
            uv_vbo.unbind();
        }
        public void bind() {
            GL.BindVertexArray(VAO);
        }
        public void unbind() {
            GL.BindVertexArray(0);
        }
        public void draw() {
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
    }
}
