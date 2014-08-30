using SlimDX.Direct3D9;

namespace Athena.Core.Internal.Drawing.Drawables
{
    public class DrawableImage :IResource
    {
        private Texture Texture { get; set; }
        public DrawableImage(string filePath)
        {
            Texture = SlimDX.Direct3D9.Texture.FromFile(Rendering.Device, filePath);
        }
        public void Draw()
        {
            Rendering.DrawImage(Texture);
        }

        public bool Remove { get; set; }
        public void OnBeforeRemove()
        {
            Texture.Dispose();
            Texture = null;
        }
    }
}
