namespace GridCity {

    using System;
    using System.Diagnostics;
    using System.Linq;
    using Fields;
    using Graphics;
    using Graphics.Gl;
    using Pencil.Gaming.Graphics;
    using Pencil.Gaming.MathUtils;
    using Properties;
    using Utility;
    using Utility.Units;

    internal class Game {

        public Game(uint gridWidth, uint gridHeight, uint windowWidth, uint windowHeight) {
            Window = new Window(windowWidth, windowHeight);
            Prog = new Program();
            Prog.AttachShaders(new Shader(ShaderType.VertexShader, Resources.test_vert), new Shader(ShaderType.FragmentShader, Resources.test_frag));
            Quad = new Quad();
            float div = windowWidth / gridWidth;
            Cam = new OrthographicCamera(new Vector3(gridWidth / 2, gridHeight / 2, 1), windowWidth / div, windowHeight / div, 0.1f, 1000f);

            Grid = new Grid(gridWidth, gridHeight);
            var scene = new Scene(Grid);
            scene.PrintResidents();
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        private Grid Grid { get; }

        private Date Date { get; } = new Date(Date.Weekday.MONDAY, new Clock(5));

        private Window Window { get; }

        private Program Prog { get; }

        private Quad Quad { get; }

        private Camera Cam { get; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public void Loop() {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Time accumulatedTime = new Time(0);
            while (true) {
                int ms = (int)stopwatch.ElapsedMilliseconds;
                if (ms < 1) {
                    continue;
                }

                stopwatch.Restart();
                Time elapsedTime = new Time(ms / 1000f);

                /*
                 *  Time
                 */
                Date.Tick(elapsedTime);
                accumulatedTime += elapsedTime;
                if (accumulatedTime.Seconds > 1f) {
                    accumulatedTime.Seconds %= 1f;
                    Console.WriteLine(Date);
                }

                /*
                 *  Simulation
                 */
                var simulatedSeconds = elapsedTime * Date.SpeedFactor;
                var rbs = Grid.GetFields<Fields.Buildings.ResidentialBuilding>();
                foreach (var rb in rbs) {
                    foreach (var hh in rb.Households) {
                        foreach (var res in hh.Residents) {
                            res.CheckTime(simulatedSeconds, Date);
                        }
                    }
                }

                foreach (var agent in People.Agent.Agents) {
                    agent.Tick(simulatedSeconds);
                }

                /*
                 *  Input
                 */
                if (Window.QueryPressedKey(Pencil.Gaming.Key.PageUp)) {
                    if (Date.SpeedFactor < 2048) {
                        Date.SpeedFactor *= 2;
                    }
                }

                if (Window.QueryPressedKey(Pencil.Gaming.Key.PageDown)) {
                    if (Date.SpeedFactor > 1) {
                        Date.SpeedFactor /= 2;
                    }
                }

                if (Window.QueryPressedKey(Pencil.Gaming.Key.Left)) {
                    Cam.Move(new Vector3(-0.1f, 0, 0));
                }

                if (Window.QueryPressedKey(Pencil.Gaming.Key.Right)) {
                    Cam.Move(new Vector3(0.1f, 0, 0));
                }

                if (Window.QueryPressedKey(Pencil.Gaming.Key.Up)) {
                    Cam.Move(new Vector3(0, 0.1f, 0));
                }

                if (Window.QueryPressedKey(Pencil.Gaming.Key.Down)) {
                    Cam.Move(new Vector3(0, -0.1f, 0));
                }

                // TODO: Zoom Input

                /*
                 *  Graphics
                 */
                DoGraphics();

                if (!Window.Tick(elapsedTime)) {
                    break;
                }
            }
        }

        private void DoGraphics() {
            GL.Viewport(0, 0, Window.Width, Window.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Prog.Use();
            Prog.SetUniform("projection_matrix", Cam.ProjMat);
            Prog.SetUniform("view_matrix", Cam.ViewMat);
            GL.ActiveTexture(TextureUnit.Texture0);
            Prog.SetUniform1("tex", 0);

            Quad.Bind();

            // draw fields
            for (uint w = 0; w < Grid.Width; ++w) {
                for (uint h = 0; h < Grid.Height; ++h) {
                    Prog.SetUniform("model_matrix", Matrix.CreateTranslation(new Vector3(w, h, 0)));
                    Field f = Grid.GetField<Field>(new Utility.GlobalCoordinate(w, h));
                    GL.BindTexture(TextureTarget.Texture2D, f.Texture.Handle);
                    Quad.Draw();
                }
            }

            // draw agents
            foreach (var agent in People.Agent.Agents) {
                if (agent.IsVisible) {
                    var pos = agent.Trace.Last();
                    Prog.SetUniform("model_matrix", Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(new Vector3((pos.X - 0.5f) / 8, (pos.Y - 0.5f) / 8, 0)));
                    GL.BindTexture(TextureTarget.Texture2D, agent.Texture.Handle);
                    Quad.Draw();
                }
            }

            Quad.Unbind();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.UseProgram(0);
        }
    }
}
