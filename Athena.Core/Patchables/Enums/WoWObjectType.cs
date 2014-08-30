using System;

namespace Athena.Core.Patchables.Enums
{
    [Flags]
    public enum WoWObjectType
    {
        Object = 0x1,
        Item = 0x2,
        Container = 0x4,
        Unit = 0x8,
        Player = 0x10,
        GameObject = 0x20,
        DynamicObject = 0x40,
        Corpse = 0x80,
        AiGroup = 0x100,
        AreaTrigger = 0x200,
    }
}
