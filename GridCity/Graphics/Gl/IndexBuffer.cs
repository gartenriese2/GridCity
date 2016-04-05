namespace GridCity.Graphics.Gl {

    using System;
    using System.Diagnostics;
    using Pencil.Gaming.Graphics;
    
    internal class IndexBuffer : Buffer {

        public IndexBuffer(params int[] data) : base(BufferTarget.ElementArrayBuffer) {
            Debug.Assert(data.Length > 2, "There must be at least 3 indices");
            GL.BindBuffer(Target, Handle);
            GL.BufferData(Target, new IntPtr(data.Length * sizeof(int)), data, BufferUsageHint.StaticDraw);
            GL.BindBuffer(Target, 0);
        }
    }
}
