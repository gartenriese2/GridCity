namespace GridCity.Graphics.Gl {

    using System;
    using Pencil.Gaming.Graphics;

    internal class Shader {

        public Shader(ShaderType type, string code) {
            if (!Allocate(type)) {
                return;
            }

            if (!Compile(code)) {
                return;
            }
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public uint Handle { get; set; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        private bool Allocate(ShaderType type) {
            Handle = GL.CreateShader(type);
            if (Handle == 0) {
                Console.WriteLine("Error allocating shader object");
                return false;
            }

            return true;
        }

        private bool Compile(string code) {
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
