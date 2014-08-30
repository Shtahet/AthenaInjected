using System;
using System.Collections.Generic;
using Athena.Core.Internal.DirectX.Required;
using Athena.Core.Internal.Drawing;
using Athena.Core.Internal.GameManager;

namespace Athena.Core.Internal.DirectX
{
    public static class Pulsator
    {
        internal static D3DHook Hook;

        public static D3DVersion HookVersion;

        private static readonly object _pulseLock = new object();

        public static DateTime SkipUntil = DateTime.Now;

        public static void Initialize(D3DVersion ver)
        {
            switch (ver)
            {
                case D3DVersion.Direct3D9:
                    Hook = new D3D9();
                    break;
                case D3DVersion.Direct3D11:
                    Hook = new D3D11();
                    break;
            }

            if (Hook == null)
                throw new Exception("Hook = null!");

            Hook.Initialize();

        }

        public static void Initialize()
        {
            Initialize(D3DVersion.Direct3D9);

            D3DHook.OnFrameOnce += D3DHook_OnFrameOnce;
            D3DHook.OnFrame += D3DHook_OnFrame;
        }

        private static void D3DHook_OnFrameOnce()
        {
            //This method will only be called once, when the hook is Initialized
            lock (_pulseLock)
            {
                //Logging.Write("D3DHook_OnFrameOnce");
            }
        }

        private static void D3DHook_OnFrame(object sender, EventArgs e)
        {
            //Everything here is called on the main thread...

            //For D3D9 this is our Endscene hook
            //For D3D10/11 this is our Present hook
            lock (_pulseLock)
            {
                //We want to draw on ever frame..
                if (Rendering.IsInitialized)
                {
                    Rendering.DrawDrawables();
                }
                else
                {
                    Rendering.Initialize(Hook.DevicePointer);
                }

                if (SkipUntil < DateTime.Now)
                {
                    //We dont want to call this EVERY frame, or we might kill the host PC. Skipping 300 ms means 3~ calls/second
                    SkipUntil = DateTime.Now.AddMilliseconds(300);

                    foreach (var pulsable in _pulsables)
                        pulsable.OnPulse();
                }
            }
        }

        private static LinkedList<IPulsable> _pulsables = new LinkedList<IPulsable>();
        public static void RegisterCallback(IPulsable pulsable)
        {
            _pulsables.AddLast(pulsable);
        }

        public static void RegisterCallbacks(params IPulsable[] pulsables)
        {
            foreach (var pulsable in pulsables)
                RegisterCallback(pulsable);
        }

        public static void RemoveCallback(IPulsable pulsable)
        {
            if (_pulsables.Contains(pulsable))
                _pulsables.Remove(pulsable);
        }

    }
}
