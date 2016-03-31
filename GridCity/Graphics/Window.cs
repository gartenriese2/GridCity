using GridCity.Utility.Units;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;
using System.Collections.Generic;

namespace GridCity.Graphics {
    class Window : ITickable {
        private GlfwWindowPtr Ptr { get; }
        public int Width { get; }
        public int Height { get; }
        private HashSet<Key> PressedKeys { get; } = new HashSet<Key>();
        private bool ShouldClose { get; set; } = false;
        public Window(uint width, uint height) {
            Width = (int)width;
            Height = (int)height;
            Glfw.Init();
            Ptr = Glfw.CreateWindow((int)width, (int)height, "GridCity", GlfwMonitorPtr.Null, GlfwWindowPtr.Null);
            Glfw.MakeContextCurrent(Ptr);
            GL.ClearColor(Color4.Black);

            Glfw.SetKeyCallback(Ptr, keyfun);
        }
        private void keyfun(GlfwWindowPtr wnd, Key key, int scanCode, KeyAction action, KeyModifiers mods) {
            if (!PressedKeys.Contains(key) && (action == KeyAction.Press || action == KeyAction.Repeat)) {
                PressedKeys.Add(key);
            }
            if (key == Key.Escape) {
                ShouldClose = true;
            }
        }
        public bool tick(Time elapsedTime) {
            Glfw.PollEvents();
            Glfw.SwapBuffers(Ptr);
            return !(Glfw.WindowShouldClose(Ptr) || ShouldClose);
        }
        public bool queryPressedKey(Key key) {
            if (PressedKeys.Contains(key)) {
                PressedKeys.Remove(key);
                return true;
            }
            return false;
        }
    }
}
