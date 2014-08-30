using System.Drawing;
using Athena.Core.Internal.Objects;

namespace Athena.Core.Internal.Drawing.Drawables
{
    internal class DrawableLine : IResource
    {
        public Location From;
        public Location To;
        public Color Color;

        public DrawableLine(Location from, Location to, Color color)
        {
            From = from;
            To = to;
            Color = color;
        }
        public void Draw()
        {
            Rendering.DrawLine(From, To, Color);
        }

        public bool Remove { get; set; }
        public void OnBeforeRemove()
        {
            
        }
    }
}
