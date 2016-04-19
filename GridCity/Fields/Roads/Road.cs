namespace GridCity.Fields.Roads {

    using System.Drawing;

    internal class Road : ConnectableField {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        internal Road(Utility.GlobalCoordinate pos, Pathfinding.BaseNodeLayout layout, Orientation_CW orientation, Bitmap tex, string name) : base(pos, layout, orientation) {
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

            Texture = new GridCity.Graphics.Texture(name + "_" + orientation.ToString(), tex);
        }
    }
}
