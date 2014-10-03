using Athena.Core.Internal.Objects;
using Athena.Core.Patchables;
using Athena.Core.Patchables.Enums;

namespace Athena.Core.Internal.GameManager.IngameObjects
{
    public class WoWGameObject : WoWObject
    {
        public WoWGameObject(uint pointer)
            : base(pointer)
        {

        }

        public uint DisplayId
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWGameObjectFields.DisplayID);
            }
        }

        public uint Flags
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWGameObjectFields.Flags);
            }
        }

        public uint Level
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWGameObjectFields.Level);
            }
        }

        public uint Faction
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWGameObjectFields.FactionTemplate);
            }
        }

        public bool Locked
        {
            get
            {
                return (Flags & (uint)GameObjectFlags.Locked) > 0;
            }
        }

        public bool InUse
        {
            get
            {
                return (Flags & (uint)GameObjectFlags.InUse) > 0;
            }
        }

        public bool IsTransport
        {
            get
            {
                return (Flags & (uint)GameObjectFlags.Transport) > 0;
            }
        }

        public ulong CreatedBy
        {
            get
            {
                ulong low = GetDescriptor<ulong>((int)Descriptors.WoWGameObjectFields.CreatedBy);
                //ulong high = GetDescriptor<ulong>((int)Descriptors.WoWGameObjectFields.CreatedBy + 0x8);

                //return new WoWGuidWoD(low, high);
                return low;
            }
        }

        public bool CreatedByMe
        {
            get
            {
                return CreatedBy.Equals(ObjectManager.LocalPlayer.Guid);
                return false;
            }
        }

        public bool Gatherable
        {
            get
            {
                return false;
            }
        }

        public static implicit operator uint(WoWGameObject self)
        {
            return self != null ? self.Pointer : 0;
        }

        public static new WoWGameObject Invalid = new WoWGameObject(0);
    }
}
