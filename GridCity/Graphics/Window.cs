namespace GridCity.Graphics {

    using System.Collections.Generic;
    using Pencil.Gaming;
    using Pencil.Gaming.Graphics;
    using Simulation;
    using Utility.Units;

    internal class Window : ITickable {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Window(uint width, uint height, Utility.Coordinate position) {
            Width = (int)width;
            Height = (int)height;
            Position = position;
            Glfw.Init();
            Ptr = Glfw.CreateWindow((int)width, (int)height, "GridCity", GlfwMonitorPtr.Null, GlfwWindowPtr.Null);
            MakeCurrent();
            Glfw.SetWindowPos(Ptr, (int)Position.X, (int)Position.Y);

            GL.ClearColor(Color4.Black);

            Glfw.SetKeyCallback(Ptr, KeyFun);
            Glfw.SetWindowPosCallback(Ptr, WindowPosFun);
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public int Width { get; }

        public int Height { get; }

        private Utility.Coordinate Position { get; }

        private GlfwWindowPtr Ptr { get; }
        
        private HashSet<Key> PressedKeys { get; } = new HashSet<Key>();

        private bool ShouldClose { get; set; } = false;

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public void MakeCurrent() {
            Glfw.MakeContextCurrent(Ptr);
        }

        public bool Tick(Time elapsedTime) {
            Glfw.PollEvents();
            Glfw.SwapBuffers(Ptr);
            return !(Glfw.WindowShouldClose(Ptr) || ShouldClose);
        }

        public bool QueryPressedKey(Key key) {
            if (PressedKeys.Contains(key)) {
                PressedKeys.Remove(key);
                return true;
            }

            return false;
        }

        private void KeyFun(GlfwWindowPtr wnd, Key key, int scanCode, KeyAction action, KeyModifiers mods) {
            if (!PressedKeys.Contains(key) && (action == KeyAction.Press || action == KeyAction.Repeat)) {
                PressedKeys.Add(key);
            }

            if (key == Key.Escape) {
                ShouldClose = true;
            }
        }

        private void WindowPosFun(GlfwWindowPtr wnd, int x, int y) {
            Glfw.SetWindowPos(Ptr, (int)Position.X, (int)Position.Y);
        }
    }
}
