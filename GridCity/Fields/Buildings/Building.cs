namespace GridCity.Fields.Buildings {

    using System;
    using System.Drawing;

    internal abstract class Building : ConnectableField {

        public Building(Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, string name, Tuple<uint, uint> size) : base(pos, layout, orientation) {
            Name = name;
            Size = size;
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

            Texture = new GridCity.Graphics.Texture(Name + "_" + orientation.ToString(), tex);
        }

        public enum Type {
            RESIDENTIAL, WORK, SCHOOL
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        private string Name { get; set; }

        private Tuple<uint, uint> Size { get; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public override string ToString() {
            return Name;
        }
    }
}
