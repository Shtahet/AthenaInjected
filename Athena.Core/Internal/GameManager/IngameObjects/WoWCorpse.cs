using Athena.Core.Patchables;

namespace Athena.Core.Internal.GameManager.IngameObjects
{
    public class WoWCorpse : WoWObject
    {
        public WoWCorpse(uint pointer)
            : base(pointer)
        {

        }

        public ulong OwnerGuid
        {
            get
            {
                return GetDescriptor<ulong>((int)Descriptors.WoWCorpseFields.Owner);
            }
        }

        public uint DisplayId
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWCorpseFields.DisplayID);
            }
        }

        public uint Flags
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWCorpseFields.Flags);
            }
        }

        public uint DynamicFlags
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWCorpseFields.DynamicFlags);
            }
        }


        public static implicit operator uint(WoWCorpse self)
        {
            return self != null ? self.Pointer : 0;
        }

        public static new WoWCorpse Invalid = new WoWCorpse(0);
    }
}
