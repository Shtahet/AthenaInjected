using Athena.Core.Internal.Objects;
using SlimDX.Direct3D9;

namespace Athena.Core.Internal.Drawing.Drawables
{
    public class DrawableImageInWorld :IResource
    {
        private Texture Texture { get; set; }

        private Location Location { get; set; }

        public DrawableImageInWorld(string filePath, Location loc)
        {
            Texture = SlimDX.Direct3D9.Texture.FromFile(Rendering.Device, filePath);
            Location = loc;
        }

        public void Draw()
        {
            Rendering.DrawImageInWorld(Texture, Location);
        }

        public bool Remove { get; set; }
        public void OnBeforeRemove()
        {
            Texture.Dispose();
            Texture = null;
        }
    }
}
