namespace GridCity.Graphics {

    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    internal class Texture {

        public Texture(string id) {
            ID = id;
            if (!Map.ContainsKey(id)) {
                Map.Add(id, Pencil.Gaming.Graphics.GL.Utils.LoadImage((Bitmap)Properties.Resources.ResourceManager.GetObject(id)));
            }
        }

        public Texture(string id, Bitmap bmp) {
            ID = id;
            if (!Map.ContainsKey(id)) {
                Map.Add(id, Pencil.Gaming.Graphics.GL.Utils.LoadImage(bmp));
            }
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public string ID { get; }

        public int Handle => Map.Where(x => x.Key == ID).Single().Value;

        private static Dictionary<string, int> Map { get; } = new Dictionary<string, int>();
    }
}
