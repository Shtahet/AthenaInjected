using System;

namespace Athena.Core.Patchables.Enums
{
    [Flags]
    public enum UnitFlags2
    {
        FeignDeath = 0x1,
        NoModel = 0x2,
        Flag_0x4 = 0x4,
        Flag_0x8 = 0x8,
        Flag_0x10 = 0x10,
        Flag_0x20 = 0x20,
        ForceAutoRunForward = 0x40,

        /// <summary>
        /// Treat as disarmed?
        /// Treat main and off hand weapons as not being equipped?
        /// </summary>
        Flag_0x80 = 0x80,

        /// <summary>
        /// Skip checks on ranged weapon?
        /// Treat it as not being equipped?
        /// </summary>
        Flag_0x400 = 0x400,

        Flag_0x800 = 0x800,
        Flag_0x1000 = 0x1000,
    }
}
