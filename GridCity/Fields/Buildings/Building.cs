using System;
using System.Drawing;

namespace GridCity.Fields.Buildings {
    abstract class Building : ConnectableField {
        public enum Type { RESIDENTIAL, WORK, SCHOOL }
        private string Name { get; set; }
        public Building(Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, string name) : base(pos, layout, orientation) {
            Name = name;
            var tex = (Bitmap)Properties.Resources.ResourceManager.GetObject(Name);
            switch (orientation) {
                case Orientation_CW.NINETY:
                    tex.RotateFlip(RotateFlipType.Rotate90FlipY);
                    break;
                case Orientation_CW.ONEEIGHTY:
                    tex.RotateFlip(RotateFlipType.Rotate180FlipY);
                    break;
                case Orientation_CW.TWOSEVENTY:
                    tex.RotateFlip(RotateFlipType.Rotate270FlipY);
                    break;
                default:
                    tex.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
            }
            Texture = new Graphics.Texture(Name + "_" + orientation.ToString(), tex);
        }
        public override string ToString() {
            return Name;
        }
    }
}
