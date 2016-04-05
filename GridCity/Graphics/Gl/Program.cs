namespace GridCity.Graphics.Gl {

    using System;
    using System.Collections.Generic;
    using Pencil.Gaming.Graphics;

    internal class Program {

        public Program() {
            Handle = GL.CreateProgram();
            if (Handle == 0) {
                Console.WriteLine("Error creating program");
            }
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public uint Handle { get; set; }

        private Dictionary<string, int> Uniforms { get; set; } = new Dictionary<string, int>();

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public void AttachShaders(params Shader[] shaders) {
            foreach (var shader in shaders) {
                GL.AttachShader(Handle, shader.Handle);
            }
        }

        public void Use() {
            int[] linkStatus = new int[1];
            GL.GetProgram(Handle, ProgramParameter.LinkStatus, linkStatus);
            if (linkStatus[0] == 0) {
                Link();
            }

            GL.UseProgram(Handle);
        }

        public bool SetUniform1<T>(string name, T value) {
            if (!Uniforms.ContainsKey(name)) {
                return false;
            }

            int loc = Uniforms[name];
            GL.Uniform1(loc, (dynamic)value);
            return true;
        }

        public bool SetUniform2<T>(string name, T value) {
            if (!Uniforms.ContainsKey(name)) {
                return false;
            }

            int loc = Uniforms[name];
            GL.Uniform2(loc, (dynamic)value);
            return true;
        }

        public bool SetUniform3<T>(string name, T value) {
            if (!Uniforms.ContainsKey(name)) {
                return false;
            }

            int loc = Uniforms[name];
            GL.Uniform3(loc, (dynamic)value);
            return true;
        }

        public bool SetUniform4<T>(string name, T value) {
            if (!Uniforms.ContainsKey(name)) {
                return false;
            }

            int loc = Uniforms[name];
            GL.Uniform4(loc, (dynamic)value);
            return true;
        }

        public bool SetUniform(string name, Pencil.Gaming.MathUtils.Matrix mat) {
            if (!Uniforms.ContainsKey(name)) {
                return false;
            }

            int loc = Uniforms[name];
            GL.UniformMatrix4(loc, false, ref mat);
            return true;
        }

        private bool Link() {
            GL.LinkProgram(Handle);
            int[] success = new int[1];
            GL.GetProgram(Handle, ProgramParameter.LinkStatus, success);
            if (success[0] == 0) {
                var log = GL.GetProgramInfoLog((int)Handle);
                Console.WriteLine("Program linking failed with the following error:\n" + log);
                return false;
            }

            Uniforms.Clear();
            int[] numUniforms = new int[1];
            GL.GetProgram(Handle, ProgramParameter.ActiveUniforms, numUniforms);
            for (int i = 0; i < numUniforms[0]; ++i) {
                int size;
                ActiveUniformType type;
                var name = GL.GetActiveUniform((int)Handle, i, out size, out type);
                int loc = GL.GetUniformLocation(Handle, name);
                Uniforms.Add(name, loc);
            }

            return true;
        }
    }
}
