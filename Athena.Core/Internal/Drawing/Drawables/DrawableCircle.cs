using System.Drawing;
using Athena.Core.Internal.Objects;

namespace Athena.Core.Internal.Drawing.Drawables
{
    public class DrawableCircle : IResource
    {
        public Location Center;
        public float Radius;
        public Color InsideColor;
        public Color OutsideColor;
        public int Complexity;
        public bool IsFilled;
        
        public DrawableCircle(Location center, float radius, Color inside, Color outside, int complex = 24, bool filled = true)
        {
            Center = center;
            Radius = radius;
            InsideColor = inside;
            OutsideColor = outside;
            Complexity = complex;
            IsFilled = filled;
        }
        public void Draw()
        {
            Rendering.DrawCircle(Center, Radius, InsideColor, OutsideColor, Complexity, IsFilled);
        }

        public bool Remove { get; set; }
        public void OnBeforeRemove()
        {
            
        }
    }
}
