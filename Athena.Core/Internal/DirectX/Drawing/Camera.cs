using System.Runtime.InteropServices;
using Athena.Core.Patchables;
using SlimDX;

namespace Athena.Core.Internal.DirectX.Drawing
{
    public static unsafe class Camera
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint GetActiveCameraDelegate();
        private static GetActiveCameraDelegate _getActiveCamera;

        public static void Initialize()
        {
            _getActiveCamera = GeneralHelper.Memory.CreateFunction<GetActiveCameraDelegate>(Offsets.DrawingOffsets.CGWorldFrame__GetActiveCamera);
            _GetFov = GeneralHelper.Memory.CreateFunction<GetFovDelegate>(GeneralHelper.Memory.GetVFTableEntry(Pointer, 0));
            _Forward = GeneralHelper.Memory.CreateFunction<ForwardDelegate>(GeneralHelper.Memory.GetVFTableEntry(Pointer, 1));
            _Right = GeneralHelper.Memory.CreateFunction<RightDelegate>(GeneralHelper.Memory.GetVFTableEntry(Pointer, 2));
            _Up = GeneralHelper.Memory.CreateFunction<UpDelegate>(GeneralHelper.Memory.GetVFTableEntry(Pointer, 3));
        }

        private static uint Pointer
        {
            get { return _getActiveCamera(); }
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate float GetFovDelegate(uint ptr);
        private static GetFovDelegate _GetFov;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* ForwardDelegate(uint ptr, Vector3* vecOut);
        private static ForwardDelegate _Forward;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* RightDelegate(uint ptr, Vector3* vecOut);
        private static RightDelegate _Right;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* UpDelegate(uint ptr, Vector3* vecOut);
        private static UpDelegate _Up;

        public static float FieldOfView
        {
            get
            {
                return _GetFov(Pointer);
            }
        }

        public static Vector3 Forward
        {
            get
            {
                var res = new Vector3();
                _Forward(Pointer, &res);
                return res;
            }
        }

        public static Vector3 Right
        {
            get
            {
                var res = new Vector3();
                _Right(Pointer, &res);
                return res;
            }
        }

        public static Vector3 Up
        {
            get
            {
                var res = new Vector3();
                _Up(Pointer, &res);
                return res;
            }
        }

        public static Matrix Projection
        {
            get
            {
                var cam = GetCamera();
                return Matrix.PerspectiveFovRH(FieldOfView * 0.6f, AspectRatio, 1, cam.FarZ); //cam.NearZ, cam.FarZ);
            }
        }

        public static Matrix View
        {
            get
            {
                var cam = GetCamera();
                var eye = cam.Position;
                var at = eye + Camera.Forward;
                return Matrix.LookAtRH(eye, at, new Vector3(0, 0, 1));
            }
        }

        public static float AspectRatio
        {
            get
            {
                float aspect = GeneralHelper.Memory.Read<float>(GeneralHelper.Memory.Read<uint>(Offsets.DrawingOffsets.aspect1) +
                                                 Offsets.DrawingOffsets.aspect2);
                //float aspect = 1.77863777f;
                //float aspect = GeneralHelper.Memory.Read<float>(Offsets.DrawingOffsets.Possible_AspectRatio);

                return aspect;
            }
        }
        public static CameraInfo GetCamera()
        {
            CameraInfo info = GeneralHelper.Memory.Read<CameraInfo>(Pointer);

            return info;
        }
    }

    public struct CameraInfo
    {
        public uint VTable;
        public int Unknown2;
        public Vector3 Position;
        public CameraMatrix Matrix;
        public float FieldOfView;
        public uint Model;
        public long Timestamp;
        public float NearZ;
        public float FarZ;
        public float AspectRatio;
    }

    public struct CameraMatrix
    {
        private float M11;
        private float M12;
        private float M13;
        private float M21;
        private float M22;
        private float M23;
        private float M31;
        private float M32;
        private float M33;
    };
}
