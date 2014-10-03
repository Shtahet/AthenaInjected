using System.Drawing;
using Athena.Core.Internal.GameManager.IngameObjects;

namespace Athena.Core.Internal.DirectX.Drawing.Drawables
{
    class DrawableBox : IResource
    {
        private WoWObject Object;
        private float Heading;
        private float Width;
        private float Height;
        private Color Color;
        private int Alpha;
        public DrawableBox(WoWObject obj, float heading, float width, float height, Color col, int alpha)
        {
            Object = obj;
            Heading = heading;
            Width = width;
            Height = height;
            Color = col;
            Alpha = alpha;
        }
        public void Draw()
        {
            if (!Object.IsValid)
            {
                Remove = true;
            }
            else
            {
                Rendering.DrawBox(Object.Location, Heading, Width, Height, Color, Alpha);
            }
        }

        public bool Remove { get; set; }
        public void OnBeforeRemove()
        {
        }
    }
}
