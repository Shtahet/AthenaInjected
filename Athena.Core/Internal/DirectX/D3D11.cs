using System;
using System.Runtime.InteropServices;
using GreyMagic.Internals;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;

namespace Athena.Core.Internal.DirectX
{
    public class D3D11 : D3DHook
    {
        private const int VMT_PRESENT = 8;
        private const int VMT_RESIZETARGET = 14;

        public uint PresentPointer = 0;
        public uint ResetTargetPointer = 0;
        private Direct3D11Present _presentDelegate;
        private Detour _presentHook;

        public override void Initialize()
        {
            Device tmpDevice;
            SwapChain sc;
            using (var rf = new RenderForm())
            {
                var desc = new SwapChainDescription
                {
                    BufferCount = 1,
                    Flags = SwapChainFlags.None,
                    IsWindowed = true,
                    ModeDescription =
                        new ModeDescription(100, 100, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                    OutputHandle = rf.Handle,
                    SampleDescription = new SampleDescription(1, 0),
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput
                };

                Result res = Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc,
                                                        out tmpDevice, out sc);
                if (res.IsSuccess)
                {
                    using (tmpDevice)
                    {
                        using (sc)
                        {
                            PresentPointer = GeneralHelper.Memory.GetVFTableEntry(sc.ComPointer, VMT_PRESENT);
                            ResetTargetPointer = GeneralHelper.Memory.GetVFTableEntry(sc.ComPointer,
                                                                                             VMT_RESIZETARGET);
                        }
                    }
                }
            }

            _presentDelegate = GeneralHelper.Memory.CreateFunction<Direct3D11Present>(PresentPointer);
            _presentHook = GeneralHelper.Memory.Detours.CreateAndApply(_presentDelegate, new Direct3D11Present(Callback),
                                                                      "D11Present");
        }

        private int Callback(IntPtr swapChainPtr, int syncInterval, PresentFlags flags)
        {
            RaiseEvent();
            return (int)_presentHook.CallOriginal(swapChainPtr, syncInterval, flags);
        }

        #region Nested type: Direct3D11Present

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate int Direct3D11Present(IntPtr swapChainPtr, int syncInterval, PresentFlags flags);

        #endregion

        #region Nested type: Direct3D11ResizeTarget

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate int Direct3D11ResizeTarget(IntPtr swapChainPtr, ref Required.DXGI_MODE_DESC newTargetParameters);

        #endregion
    }
}
