namespace GridCity.Graphics.Gl {
   
    using System;
    using System.Diagnostics;
    using Pencil.Gaming.Graphics;

    internal class VertexBuffer : Buffer {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public VertexBuffer(params float[] data) : base(BufferTarget.ArrayBuffer) {
            Debug.Assert(data.Length > 0, "Buffer must not be empty");
            GL.BindBuffer(Target, Handle);
            GL.BufferData(Target, new IntPtr(data.Length * sizeof(float)), data, BufferUsageHint.StaticDraw);
            GL.BindBuffer(Target, 0);
        }
    }
}
