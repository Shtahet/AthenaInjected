using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.DXGI;

namespace Athena.Core.Internal.DirectX.Required
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DXGI_MODE_DESC
    {
        public int Width;
        public int Height;
        public Rational RefreshRate;
        public Format Format;
        public DisplayModeScanlineOrdering ScanlineOrdering;
        public DisplayModeScaling Scaling;
    }
}
