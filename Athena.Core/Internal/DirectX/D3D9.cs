using System;
using System.Runtime.InteropServices;
using GreyMagic.Internals;
using SlimDX.Direct3D9;

namespace Athena.Core.Internal.DirectX
{
    public class D3D9 : D3DHook
    {
        private const int VMT_ENDSCENE = 42;
        private const int VMT_RESET = 16;
        public uint EndScenePointer = 0;
        public uint ResetExPointer = 0;
        public uint ResetPointer = 0;
        private Direct3D9EndScene _endSceneDelegate;
        private Detour _endSceneHook;

        private Direct3D9Reset _resetDelegate;
        private static Detour _resetHook;

        public override void Initialize()
        {
            using (var d3d = new Direct3D())
            {
                using (var tmpDevice = new Device(d3d, 0, DeviceType.Hardware, IntPtr.Zero,
                                               CreateFlags.HardwareVertexProcessing,
                                               new PresentParameters { BackBufferWidth = 1, BackBufferHeight = 1 }))
                {
                    EndScenePointer = GeneralHelper.Memory.GetVFTableEntry(tmpDevice.ComPointer, VMT_ENDSCENE);
                    ResetPointer = GeneralHelper.Memory.GetVFTableEntry(tmpDevice.ComPointer, VMT_RESET);
                }
            }

            _endSceneDelegate = GeneralHelper.Memory.CreateFunction<Direct3D9EndScene>(EndScenePointer);
            _endSceneHook = GeneralHelper.Memory.Detours.CreateAndApply(_endSceneDelegate,
                                                                       new Direct3D9EndScene(Callback), "D9EndScene");

           /* _resetDelegate = GeneralHelper.Memory.CreateFunction<Direct3D9Reset>(ResetPointer);
            _resetHook = GeneralHelper.Memory.Detours.CreateAndApply(_resetDelegate,
                                                                       new Direct3D9Reset(ResetHook), "D9Reset"); 
            
            */
            //We really need RESET hook to, so we dont crash wow when hiding the window...
            
            return;
        }

       /* private int ResetHook(IntPtr device, Required.PresentParameters pp)
        {
            base.ResetDrawingObjects();
            return (int)_resetHook.CallOriginal(device, pp);
        }*/

        private int Callback(IntPtr device)
        {
            DevicePointer = device;
            RaiseEvent();
            return (int)_endSceneHook.CallOriginal(device);
        }

        #region Nested type: Direct3D9EndScene

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int Direct3D9EndScene(IntPtr device);

        #endregion

        #region Nested type: Direct3D9Reset

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int Direct3D9Reset(IntPtr device, Required.PresentParameters presentationParameters);

        #endregion

        #region Nested type: Direct3D9ResetEx

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int Direct3D9ResetEx(IntPtr presentationParameters, IntPtr displayModeEx);

        #endregion
    }
}
