using System;
using System.Runtime.InteropServices;

namespace Athena.Core.Internal.DirectX.Required
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PresentParameters
    {
        public readonly uint BackBufferWidth;
        public readonly uint BackBufferHeight;
        public uint BackBufferFormat;
        public readonly uint BackBufferCount;
        public readonly uint MultiSampleType;
        public readonly uint MultiSampleQuality;
        public uint SwapEffect;
        public readonly IntPtr hDeviceWindow;
        [MarshalAs(UnmanagedType.Bool)]
        public bool Windowed;
        [MarshalAs(UnmanagedType.Bool)]
        public readonly bool EnableAutoDepthStencil;
        public readonly uint AutoDepthStencilFormat;
        public readonly uint Flags;
        public readonly uint FullScreen_RefreshRateInHz;
        public readonly uint PresentationInterval;
    }
}
