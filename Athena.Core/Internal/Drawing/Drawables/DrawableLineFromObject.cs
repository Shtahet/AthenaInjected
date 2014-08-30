using System.Drawing;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Objects;

namespace Athena.Core.Internal.Drawing.Drawables
{
    internal class DrawableLineFromObject : IResource
    {
        public WoWObject From;
        public Location To;
        public Color Color;

        public DrawableLineFromObject(WoWObject from, Location to, Color color)
        {
            From = from;
            To = to;
            Color = color;
        }
        public void Draw()
        {
            Rendering.DrawLine(From.Location, To, Color);
        }

        public bool Remove { get; set; }
        public void OnBeforeRemove()
        {
            
        }
    }
}
