namespace GridCity.Graphics.Gl {

    using System;
    using Pencil.Gaming.Graphics;

    internal class Buffer {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        protected Buffer(BufferTarget target) {
            Handle = GL.GenBuffer();
            Target = target;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public int Handle { get; }

        protected BufferTarget Target { get; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public void Bind() {
            GL.BindBuffer(Target, Handle);
        }

        public void Unbind() {
            GL.BindBuffer(Target, 0);
        }

        public int GetSize() {
            int[] size = new int[1];
            GL.BindBuffer(Target, Handle);
            GL.GetBufferParameter(Target, BufferParameterName.BufferSize, size);
            GL.BindBuffer(Target, 0);
            return size[0];
        }
    }
}
