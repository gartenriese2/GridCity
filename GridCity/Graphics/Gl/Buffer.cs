using Pencil.Gaming.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCity.Graphics.Gl {
    class Buffer {
        public int Handle { get; }
        protected BufferTarget Target { get; }
        protected Buffer(BufferTarget target) {
            Handle = GL.GenBuffer();
            Target = target;
        }
        public void bind() {
            GL.BindBuffer(Target, Handle);
        }
        public void unbind() {
            GL.BindBuffer(Target, 0);
        }
        public int getSize() {
            int[] size = new int[1];
            GL.BindBuffer(Target, Handle);
            GL.GetBufferParameter(Target, BufferParameterName.BufferSize, size);
            GL.BindBuffer(Target, 0);
            return size[0];
        }
    }
    class VBO : Buffer {
        public VBO(params float[] data) : base(BufferTarget.ArrayBuffer) {
            Contract.Assert(data.Length > 0);
            GL.BindBuffer(Target, Handle);
            GL.BufferData(Target, new IntPtr(data.Length * sizeof(float)), data, BufferUsageHint.StaticDraw);
            GL.BindBuffer(Target, 0);
        }
    }
    class IBO : Buffer {
        public IBO(params int[] data) : base(BufferTarget.ElementArrayBuffer) {
            Contract.Assert(data.Length > 2);
            GL.BindBuffer(Target, Handle);
            GL.BufferData(Target, new IntPtr(data.Length * sizeof(int)), data, BufferUsageHint.StaticDraw);
            GL.BindBuffer(Target, 0);
        }
    }
}
