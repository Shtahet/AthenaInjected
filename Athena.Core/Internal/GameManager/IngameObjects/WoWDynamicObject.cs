using Athena.Core.Patchables;

namespace Athena.Core.Internal.GameManager.IngameObjects
{
    public class WoWDynamicObject : WoWObject
    {
        public WoWDynamicObject(uint pointer)
            : base(pointer)
        {

        }

        public uint SpellId
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWDynamicObjectFields.SpellID);
            }
        }

        public ulong CasterGuid
        {
            get
            {
                return GetDescriptor<ulong>((int)Descriptors.WoWDynamicObjectFields.Caster);
            }
        }

        public uint CastTime
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWDynamicObjectFields.CastTime);
            }
        }

        public float Radius
        {
            get
            {
                return GetDescriptor<float>((int)Descriptors.WoWDynamicObjectFields.Radius);
            }
        }

        public static implicit operator uint(WoWDynamicObject self)
        {
            return self != null ? self.Pointer : 0;
        }

        public static new WoWDynamicObject Invalid = new WoWDynamicObject(0);
    }
}
