using Pencil.Gaming.Graphics;
using System;
using System.Linq.Expressions;

namespace GridCity.Graphics.Gl {
    class Shader {
        public uint Handle { get; set; }
        public Shader(ShaderType type, string code) {
            if (!allocate(type)) return;
            if (!compile(code)) return;
        }
        
        private bool allocate(ShaderType type) {
            Handle = GL.CreateShader(type);
            if (Handle == 0) {
                Console.WriteLine("Error allocating shader object");
                return false;
            }
            return true;
        }
        bool compile(string code) {
            GL.ShaderSource(Handle, code);
            GL.CompileShader(Handle);

            int[] success = new int[1];
            GL.GetShader(Handle, ShaderParameter.CompileStatus, success);
            
            if (success[0] == 0) {
                var log = GL.GetShaderInfoLog((int)Handle);
                Console.WriteLine("Shader compiling failed with the following error:\n" + log);
                return false;
            }
	        return true;
        }
    }
}
