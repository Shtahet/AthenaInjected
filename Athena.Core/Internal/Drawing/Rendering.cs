using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Athena.Core.Internal.Objects;
using SlimDX;
using SlimDX.Direct3D9;

namespace Athena.Core.Internal.Drawing
{
    public static class Rendering
    {
        private static readonly List<IResource> _Drawables = new List<IResource>();
        private static IntPtr _usedDevicePointer = IntPtr.Zero;

        private static Object locker = new object();

        public static Device Device { get; private set; }
        public static SlimDX.Direct3D9.Font DrawingFont;

        public static void Initialize(IntPtr devicePointer)
        {
            if (_usedDevicePointer != devicePointer)
            {
                //Debug.WriteLine("Rendering: Device initialized on " + devicePointer);
                Device = Device.FromPointer(devicePointer);
                _usedDevicePointer = devicePointer;
                DrawingFont = new SlimDX.Direct3D9.Font(Device, new System.Drawing.Font(FontFamily.GenericSansSerif, 14));
            }

            Camera.Initialize();
        }

        public static void AddDrawable(IResource source)
        {
            lock (locker)
            {
                _Drawables.Add(source);
            }
        }

        public static IEnumerable<IResource> GetDrawables()
        {
            return _Drawables;
        } 

        private static void InternalRender(Vector3 target)
        {
            if (!Rendering.IsInitialized)
                return;

            var viewport = Device.Viewport;
            viewport.MinZ = 0.0f;
            viewport.MaxZ = 0.94f;
            Device.Viewport = viewport;

            Device.SetTransform(TransformState.World, Matrix.Translation(target));
            Device.SetTransform(TransformState.View, Camera.View);
            Device.SetTransform(TransformState.Projection, Camera.Projection);

            Device.VertexShader = null;
            Device.PixelShader = null;
            Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            Device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);//
            Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);

            Device.SetRenderState(RenderState.Lighting, false);
            Device.SetTexture(0, null);

            Device.SetRenderState(RenderState.ZEnable, true);
            Device.SetRenderState(RenderState.ZWriteEnable, true);
            Device.SetRenderState(RenderState.ZFunc, Compare.LessEqual);
            
            Device.SetRenderState(RenderState.CullMode, Cull.None);

            
            /*Device.SetTransform(TransformState.World, Matrix.Translation(target));
            Device.SetTransform(TransformState.View, Camera.View);
            Device.SetTransform(TransformState.Projection, Camera.Projection);

            Device.VertexShader = null;
            Device.PixelShader = null;
            Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            Device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
            Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            Device.SetRenderState(RenderState.Lighting, 0);
            Device.SetTexture(0, null);
            Device.SetRenderState(RenderState.CullMode, Cull.None);*/
        }

        /*public static void DrawLine(Location from, Location to, Color color)
        {
            var vertices = new PositionColored[2];
            vertices[0] = new PositionColored(Vector3.Zero, color.ToArgb());
            vertices[1] = new PositionColored(to.ToVector3() - from.ToVector3(), color.ToArgb());

            InternalRender(from.ToVector3());

            Device.DrawUserPrimitives(PrimitiveType.LineList, vertices.Length / 2, vertices);
        }*/

        public static void DrawLine(Location from, Location to, Color color)
        {
            try
            {
                int col = Color.FromArgb(255, color).ToArgb();
                PositionColored[] data = new PositionColored[] { new PositionColored(Vector3.Zero, col), new PositionColored(to.ToVector3() - from.ToVector3(), col) };

                InternalRender(from.ToVector3());

                Device.DrawUserPrimitives<PositionColored>(PrimitiveType.LineList, data.Length / 2, data);
            }
            catch
            {
            }
        }


        public static void DrawBox(Location position, float heading, float width, float height, Color color, int alpha)
        {
            try
            {
                //This is "Borrowed" code ;)

                int col = Color.FromArgb(alpha, color).ToArgb();

                PositionColored[] data = new PositionColored[] { 
            new PositionColored(width / 2f, width / 2f, 0f, col), new PositionColored(-(width / 2f), width / 2f, 0f, col), new PositionColored(width / 2f, -(width / 2f), 0f, col), new PositionColored(-(width / 2f), -(width / 2f), 0f, col), new PositionColored(-(width / 2f), width / 2f, 0f, col), new PositionColored(width / 2f, -(width / 2f), 0f, col), new PositionColored(width / 2f, width / 2f, 0f, col), new PositionColored(-(width / 2f), width / 2f, 0f, col), new PositionColored(width / 2f, width / 2f, height, col), new PositionColored(width / 2f, width / 2f, height, col), new PositionColored(-(width / 2f), width / 2f, 0f, col), new PositionColored(-(width / 2f), width / 2f, height, col), new PositionColored(-(width / 2f), -(width / 2f), 0f, col), new PositionColored(-(width / 2f), width / 2f, 0f, col), new PositionColored(-(width / 2f), width / 2f, height, col), new PositionColored(-(width / 2f), -(width / 2f), height, col), 
            new PositionColored(-(width / 2f), -(width / 2f), 0f, col), new PositionColored(-(width / 2f), width / 2f, height, col), new PositionColored(width / 2f, width / 2f, 0f, col), new PositionColored(width / 2f, -(width / 2f), 0f, col), new PositionColored(width / 2f, -(width / 2f), height, col), new PositionColored(width / 2f, width / 2f, 0f, col), new PositionColored(width / 2f, width / 2f, height, col), new PositionColored(width / 2f, -(width / 2f), height, col), new PositionColored(width / 2f, -(width / 2f), 0f, col), new PositionColored(-(width / 2f), -(width / 2f), 0f, col), new PositionColored(width / 2f, -(width / 2f), height, col), new PositionColored(-(width / 2f), -(width / 2f), 0f, col), new PositionColored(-(width / 2f), -(width / 2f), height, col), new PositionColored(width / 2f, -(width / 2f), height, col), new PositionColored(width / 2f, width / 2f, height, col), new PositionColored(-(width / 2f), width / 2f, height, col), 
            new PositionColored(width / 2f, -(width / 2f), height, col), new PositionColored(-(width / 2f), -(width / 2f), height, col), new PositionColored(-(width / 2f), width / 2f, height, col), new PositionColored(width / 2f, -(width / 2f), height, col)
         };
                InternalRender(position.ToVector3());

                Device.DrawUserPrimitives<PositionColored>(PrimitiveType.TriangleList, 12, data);
                data = null;
            }
            catch
            {
            }
        }


        public static void DrawImage(Texture bitmap)
        {
            Sprite sprite = new Sprite(Device);
            sprite.Begin(SpriteFlags.None);
            Vector3? vv = new Vector3(200.0f, 50.0f, 10.0f);
            sprite.Draw(bitmap, vv, vv, new Color4(Color.White));
            sprite.End();
        }

        public static void DrawImageInWorld(Texture bitmap, Location loc)
        {
            /*Sprite sprite = new Sprite(Device);
            sprite.Begin(SpriteFlags.None);

            Vector3 Location = new Vector3(loc.X, loc.Y, loc.Z);

            InternalRender(Location);
            //Vector3? vv = new Vector3(200.0f, 50.0f, 10.0f);
            sprite.Draw(bitmap, Location, Location, new Color4(Color.White));


            sprite.End();
            sprite.Dispose();
            sprite = null;*/

            PositionImage[] data = new PositionImage[]
            {
                new PositionImage(loc, bitmap)
            };

            InternalRender(loc.ToVector3());


            Device.DrawUserPrimitives<PositionImage>(PrimitiveType.TriangleFan, 1, data);
        }

        public static unsafe void DrawCircle(Location center, float radius, Color innerColor, Color outerColor, int complexity = 24, bool isFilled = true)
        {
            var vertices = new List<PositionColored>();

            if (isFilled)
                vertices.Add(new PositionColored(Vector3.Zero, innerColor.ToArgb()));

            double stepAngle = (Math.PI * 2) / complexity;
            for (int i = 0; i <= complexity; i++)
            {
                double angle = (Math.PI * 2) - (i * stepAngle);
                float x = (float)(radius * Math.Cos(angle));
                float y = (float)(-radius * Math.Sin(angle));
                vertices.Add(new PositionColored(new Vector3(x, y, 0), outerColor.ToArgb()));
            }

            var buffer = vertices.ToArray();

            InternalRender(center.ToVector3() + new Vector3(0, 0, 0.3f));

            if (isFilled)
                Device.DrawUserPrimitives(PrimitiveType.TriangleFan, buffer.Length - 2, buffer);
            else
                Device.DrawUserPrimitives(PrimitiveType.LineStrip, buffer.Length - 1, buffer);
        }

        public static bool IsInitialized
        {
            get { return Device != null; }
        }

        public static void DrawDrawables()
        {
            lock (locker)
            {
                for (int i = _Drawables.Count - 1; i >= 0; i--)
                {
                    if (_Drawables[i].Remove)
                    {
                        _Drawables[i].OnBeforeRemove();
                        _Drawables.RemoveAt(i);

                        continue;
                    }

                    _Drawables[i].Draw();
                }
            }
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColored
    {
        public static readonly VertexFormat FVF = VertexFormat.Position | VertexFormat.Diffuse;
        public static readonly int Stride = Vector3.SizeInBytes + sizeof(int);

        public Vector3 Position;
        public int Color;

        public PositionColored(Vector3 pos, int col)
        {
            Position = pos;
            Color = col;
        }

        public PositionColored(float x, float y, float z, int col)
        {
            Position = new Vector3(x,y,z);
            Color = col;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct PositionImage
    {
        public static readonly VertexFormat FVF = VertexFormat.Position | VertexFormat.Diffuse;
        public static readonly int Stride = Vector3.SizeInBytes + sizeof(int);

        public Location Position;
        public Texture Texture;

        public PositionImage(Location pos, Texture texture)
        {
            Position = pos;
            Texture = texture;
        }
    }
}
