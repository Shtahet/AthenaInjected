using System.Drawing;
using Athena.Core.Internal.GameManager.IngameObjects;

namespace Athena.Core.Internal.DirectX.Drawing.Drawables
{
    class DrawableCircleAroundObject : IResource
    {
        public WoWObject Object;
        public float Radius;
        public Color InsideColor;
        public Color OutsideColor;
        public int Complexity;
        public bool IsFilled;

        public DrawableCircleAroundObject(WoWObject obj, float radius, Color inside, Color outside, int complex = 24, bool filled = true)
        {
            Object = obj;
            Radius = radius;
            InsideColor = inside;
            OutsideColor = outside;
            Complexity = complex;
            IsFilled = filled;
        }

        public void Draw()
        {
            Rendering.DrawCircle(Object.Location, Radius, InsideColor, OutsideColor, Complexity, IsFilled);
        }

        public bool Remove { get; set; }
        public void OnBeforeRemove()
        {
            
        }
    }
}
