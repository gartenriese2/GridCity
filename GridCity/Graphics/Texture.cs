namespace GridCity.Graphics {

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Pencil.Gaming.Graphics;
    
    internal class Texture {

        public Texture(string id) {
            ID = id;
            if (!Map.ContainsKey(id)) {
                var bmp = (Bitmap)Properties.Resources.ResourceManager.GetObject(id);
                Size = Tuple.Create(bmp.Width, bmp.Height);
                Map.Add(id, GL.Utils.LoadImage(bmp));
            }
        }

        public Texture(string id, Bitmap bmp) {
            ID = id;
            if (!Map.ContainsKey(id)) {
                Size = Tuple.Create(bmp.Width, bmp.Height);
                Map.Add(id, GL.Utils.LoadImage(bmp));
            }
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public string ID { get; }

        public Tuple<int, int> Size { get; }

        public int Handle => Map.Where(x => x.Key == ID).Single().Value;

        private static Dictionary<string, int> Map { get; } = new Dictionary<string, int>();
    }
}
