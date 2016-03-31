using GridCity.Fields;
using GridCity.Graphics;
using GridCity.Graphics.Gl;
using GridCity.Properties;
using GridCity.Utility.Units;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Diagnostics;
using System.Linq;

namespace GridCity {
    class Game {
        private Grid Grid { get; }
        private Date Date { get; } = new Date();
        private Window Window { get; }
        private Program Prog { get; }
        private Quad Quad { get; }
        private Camera Cam { get; }
        public Game(uint gridWidth, uint gridHeight, uint windowWidth, uint windowHeight) {
            Window = new Window(windowWidth, windowHeight);
            Prog = new Program();
            Prog.attachShaders(new Shader(ShaderType.VertexShader, Resources.test_vert), new Shader(ShaderType.FragmentShader, Resources.test_frag));
            Quad = new Quad();
            float div = windowWidth / gridWidth;
            Cam = new OrthographicCamera(new Vector3(gridWidth / 2, gridHeight / 2, 1), windowWidth / div, windowHeight / div, 0.1f, 1000f);

            Grid = new Grid(gridWidth, gridHeight);
            var scene = new Scene(Grid);
            scene.printResidents();
        }
        public void loop() {
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
                Date.tick(elapsedTime);
                accumulatedTime += elapsedTime;
                if (accumulatedTime.Seconds > 1f) {
                    accumulatedTime.Seconds %= 1f;
                    Console.WriteLine(Date);
                }

                /*
                 *  Simulation
                 */
                var simulatedSeconds = elapsedTime * Date.SpeedFactor;
                var rbs = Grid.getFields<Fields.Buildings.ResidentialBuilding>();
                foreach (var rb in rbs) {
                    foreach (var hh in rb.Households) {
                        foreach (var res in hh.Residents) {
                            res.checkTime(simulatedSeconds, Date);
                        }
                    }
                }
                foreach (var agent in People.Agent.Agents) {
                    agent.tick(simulatedSeconds);
                }

                /*
                 *  Input
                 */
                if (Window.queryPressedKey(Pencil.Gaming.Key.PageUp)) {
                    if (Date.SpeedFactor < 2048)
                        Date.SpeedFactor *= 2;
                }
                if (Window.queryPressedKey(Pencil.Gaming.Key.PageDown)) {
                    if (Date.SpeedFactor > 1)
                        Date.SpeedFactor /= 2;
                }
                if (Window.queryPressedKey(Pencil.Gaming.Key.Left)) {
                    Cam.move(new Vector3(-0.1f, 0, 0));
                }
                if (Window.queryPressedKey(Pencil.Gaming.Key.Right)) {
                    Cam.move(new Vector3(0.1f, 0, 0));
                }
                if (Window.queryPressedKey(Pencil.Gaming.Key.Up)) {
                    Cam.move(new Vector3(0, 0.1f, 0));
                }
                if (Window.queryPressedKey(Pencil.Gaming.Key.Down)) {
                    Cam.move(new Vector3(0, -0.1f, 0));
                }
                // TODO: Zoom Input

                /*
                 *  Graphics
                 */
                doGraphics();

                if (!Window.tick(elapsedTime)) {
                    break;
                }
            }
        }
        private void doGraphics() {
            GL.Viewport(0, 0, Window.Width, Window.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Prog.use();
            Prog.setUniform("projection_matrix", Cam.ProjMat);
            Prog.setUniform("view_matrix", Cam.ViewMat);
            GL.ActiveTexture(TextureUnit.Texture0);
            Prog.setUniform1("tex", 0);

            Quad.bind();

            // draw fields
            for (uint w = 0; w < Grid.Width; ++w) {
                for (uint h = 0; h < Grid.Height; ++h) {
                    Prog.setUniform("model_matrix", Matrix.CreateTranslation(new Vector3(w, h, 0)));
                    Field f = Grid.getField<Field>(new Utility.GlobalCoordinate(w, h));
                    GL.BindTexture(TextureTarget.Texture2D, f.Texture.Handle);
                    Quad.draw();
                }
            }

            // draw agents
            foreach (var agent in People.Agent.Agents) {
                if (agent.IsVisible) {
                    var pos = agent.Trace.Last();
                    Prog.setUniform("model_matrix", Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(new Vector3((pos.X - 0.5f) / 8, (pos.Y - 0.5f) / 8, 0)));
                    GL.BindTexture(TextureTarget.Texture2D, agent.Texture.Handle);
                    Quad.draw();
                }
            }

            Quad.unbind();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.UseProgram(0);
        }
    }
}
