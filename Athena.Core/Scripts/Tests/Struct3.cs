using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Runtime.InteropServices;
namespace Yates.Core.Tests
{

    [StructLayout(LayoutKind.Sequential)]
    internal struct Struct3
    {
        public static readonly VertexFormat vertexFormat_0;
        public static readonly int int_0;
        public Vector3 vector3_0;
        public int int_1;
        public Struct3(Vector3 pos, int col)
        {
            this.vector3_0 = pos;
            this.int_1 = col;
        }

        public Struct3(float x, float y, float z, int col)
        {
            this.vector3_0 = new Vector3(x, y, z);
            this.int_1 = col;
        }

        static Struct3()
        {
            vertexFormat_0 = VertexFormat.Diffuse | VertexFormat.Position;
            int_0 = Vector3.SizeInBytes + 4;
        }
    }

}
