using System.Drawing;

namespace Athena.Core.Internal.Drawing.Drawables
{
    class DrawableString : IResource
    {
        private string Message;
        private int X;
        private int Y;
        private Color DrawColor = Color.GreenYellow;
        public DrawableString(string message, int x, int y, Color? color = null)
        {
            Message = message;
            X = x;
            Y = y;

            if(color.HasValue)
                DrawColor = color.Value;
        }

        public void Draw()
        {
            Rendering.DrawingFont.DrawString(null, Message, X, Y, DrawColor);
        }

        public bool Remove { get; set; }
        public void OnBeforeRemove()
        {
            
        }
    }
}
