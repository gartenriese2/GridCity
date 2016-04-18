namespace GridCity {

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Fields;
    using Graphics;
    using Graphics.Gl;
    using GUI;
    using Pencil.Gaming.Graphics;
    using Pencil.Gaming.MathUtils;
    using Properties;
    using Simulation.Time;
    using Utility;
    using Utility.Units;
    
    internal class Game {

        //---------------------------------------------------------------------
        // Fields
        //---------------------------------------------------------------------
        private bool isInitialized = false;

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Game() {
            DateInfoModel = new DateInfoModel();
            SpeedFactor = DateInfoModel.SpeedFactor;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public DateInfoModel DateInfoModel { get; }

        private SpeedFactor SpeedFactor { get; }

        private Date Date { get; } = new Date(Date.Weekday.MONDAY, new Clock(5));

        private Scene.SceneDescription Scene { get; set; }

        private Window Window { get; set; }

        private Program Prog { get; set; }

        private Quad Quad { get; set; }

        private Camera Cam { get; set; }

        private List<Texture> LoadingTextures { get; } = new List<Texture>();

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public void Init(uint gridWidth, uint gridHeight, uint windowWidth, uint windowHeight, uint windowPosX, uint windowPosY) {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Window = new Window(windowWidth, windowHeight, new Coordinate(windowPosX, windowPosY));
            Prog = new Program();
            Prog.AttachShaders(new Shader(ShaderType.VertexShader, Resources.test_vert), new Shader(ShaderType.FragmentShader, Resources.test_frag));
            Quad = new Quad();
            float div = windowWidth / gridWidth;
            Cam = new OrthographicCamera(new Vector3(gridWidth / 2, gridHeight / 2, 1), windowWidth / div, windowHeight / div, 0.1f, 1000f);
            LoadingTextures.Add(new Texture("Loading1", System.Drawing.RotateFlipType.RotateNoneFlipY));
            LoadingTextures.Add(new Texture("Loading2", System.Drawing.RotateFlipType.RotateNoneFlipY));
            LoadingTextures.Add(new Texture("Loading3", System.Drawing.RotateFlipType.RotateNoneFlipY));
            sw.Stop();
            Console.WriteLine("Graphics initialization took " + sw.ElapsedMilliseconds + "ms");

            sw.Restart();
            Scene = new Scene.SceneDescription(gridWidth, gridHeight);
            sw.Stop();
            Console.WriteLine("Scene initialization took " + sw.ElapsedMilliseconds + "ms");

            Task initSceneTask = Task.Run(() => InitScene());
            sw.Restart();
            while (!initSceneTask.IsCompleted) {
                int idx = ((int)sw.ElapsedMilliseconds / 500) % LoadingTextures.Count;
                StartGraphics();
                DrawLoadingScreen(LoadingTextures[idx]);
                EndGraphics();
                Window.Tick(Time.Zero);
            }

            isInitialized = true;
        }

        public void Loop() {
            Debug.Assert(isInitialized, "Game is not initialized!");

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true) {
                int ms = (int)stopwatch.ElapsedMilliseconds;
                if (ms < 1) {
                    continue;
                }

                stopwatch.Restart();
                Time elapsedTime = Time.FromMilliseconds(ms);

                /*
                 *  Simulation
                 */
                var simulatedTime = elapsedTime * SpeedFactor.Value;

                Date.Tick(simulatedTime);
                DateInfoModel.Date = Date.ToString();

                var rbs = Scene.Grid.GetFields<Fields.Buildings.ResidentialBuilding>();
                foreach (var rb in rbs) {
                    foreach (var hh in rb.Households) {
                        foreach (var res in hh.Residents) {
                            res.CheckTime(simulatedTime, Date);
                        }
                    }
                }

                foreach (var agent in People.Agent.Agents) {
                    agent.Tick(simulatedTime);
                }

                /*
                 *  Input
                 */
                if (Window.QueryPressedKey(Pencil.Gaming.Key.PageUp)) {
                    DateInfoModel.TryAddSpeed();
                }

                if (Window.QueryPressedKey(Pencil.Gaming.Key.PageDown)) {
                    DateInfoModel.TrySubtractSpeed();
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

        private void InitScene() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Scene.InitOccupations();
            sw.Stop();
            Console.WriteLine("Occupation initialization took " + sw.ElapsedMilliseconds + "ms");
            Scene.PrintResidents();
        }

        private void DoGraphics() {
            StartGraphics();

            DrawFields();
            DrawAgents();

            EndGraphics();
        }

        private void StartGraphics() {
            GL.Viewport(0, 0, Window.Width, Window.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Prog.Use();
            Prog.SetUniform("projection_matrix", Cam.ProjMat);
            Prog.SetUniform("view_matrix", Cam.ViewMat);
            GL.ActiveTexture(TextureUnit.Texture0);
            Prog.SetUniform1("tex", 0);

            Quad.Bind();
        }

        private void EndGraphics() {
            Quad.Unbind();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.UseProgram(0);
        }

        private void DrawTexture(Texture tex) {
            GL.BindTexture(TextureTarget.Texture2D, tex.Handle);
            Quad.Draw();
        }

        private void DrawLoadingScreen(Texture tex) {
            float camWidth = ((OrthographicCamera)Cam).Width;
            float camHeight = ((OrthographicCamera)Cam).Height;
            float scale = Math.Max(camWidth, camHeight);
            Matrix modelMatrix = Matrix.CreateScale(scale);
            Prog.SetUniform("model_matrix", modelMatrix);
            DrawTexture(tex);
        }

        private void DrawFields() {
            for (uint w = 0; w < Scene.Grid.Width; ++w) {
                for (uint h = 0; h < Scene.Grid.Height; ++h) {
                    Prog.SetUniform("model_matrix", Matrix.CreateTranslation(new Vector3(w, h, 0)));
                    Field f = Scene.Grid.GetField<Field>(new GlobalCoordinate(w, h));
                    DrawTexture(f.Texture);
                }
            }
        }

        private void DrawAgents() {
            foreach (var agent in People.Agent.Agents) {
                if (agent.IsVisible) {
                    var pos = agent.Trace.Last();
                    Prog.SetUniform("model_matrix", Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(new Vector3((pos.X - 0.5f) / 8, (pos.Y - 0.5f) / 8, 0)));
                    DrawTexture(agent.Texture);
                }
            }
        }
    }
}
