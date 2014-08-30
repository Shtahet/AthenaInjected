using System;

namespace Athena.Core.Internal.DirectX
{
    public abstract class D3DHook
    {
        #region Delegates

        public delegate void OnFrameDelegate();

        #endregion

        protected static readonly object _frameLock = new object();
        public IntPtr DevicePointer; // Added by Ryuk

        public static event EventHandler OnFrame;

        public static event OnFrameDelegate OnFrameOnce;

        public abstract void Initialize();

        protected void RaiseEvent()
        {
            lock (_frameLock)
            {
                if (OnFrame != null)
                    OnFrame(null, new EventArgs());

                if (OnFrameOnce != null)
                {
                    OnFrameOnce();
                    OnFrameOnce = null;
                }
            }
        }
    }
}
