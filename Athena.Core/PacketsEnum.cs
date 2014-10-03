using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core
{
    public enum PacketsEnum
    {
        Unknown = 0x0,
        MoveHeartbeat = 0x1F2,
        MoveStop = 0x8F1,
        MoveStart = 0x95A,
        MoveJump = 0x1153,
        MoveLand = 0x8FA,
        MoveJumpHitHead = 0xD9,

        SetActiveMover = 0x9F0,
        PostSetActiveMover = 0x11D9,
    }
}
