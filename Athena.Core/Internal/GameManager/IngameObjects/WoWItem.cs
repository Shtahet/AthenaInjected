using System.Collections.Generic;
using Athena.Core.Internal.Objects;
using Athena.Core.Patchables;
using Athena.Core.Patchables.Enums;

namespace Athena.Core.Internal.GameManager.IngameObjects
{
    public class WoWItem : WoWObject
    {

        public WoWItem(uint pointer)
            : base(pointer)
        { }

        public List<uint> Enchants
        {
            get
            {
                var ret = new List<uint>();
                for (var i = 0; i < 12; i++)
                {
                    var id = GetDescriptor<uint>((int)Descriptors.WoWItemFields.Enchantment + (i * 12));
                    if (id > 0)
                        ret.Add(id);
                }
                return ret;
            }
        }

        public WoWGuid OwnerGuid
        {
            get
            {
                ulong low = GetDescriptor<ulong>((int)Descriptors.WoWItemFields.Owner);
                ulong high = GetDescriptor<ulong>((int)Descriptors.WoWItemFields.Owner + 0x8);

                return new WoWGuid(low, high);
            }
        }

        public WoWGuid CreatorGuid
        {
            get
            {
                ulong low = GetDescriptor<ulong>((int)Descriptors.WoWItemFields.Creator);
                ulong high = GetDescriptor<ulong>((int)Descriptors.WoWItemFields.Creator + 0x8);

                return new WoWGuid(low, high);
            }
        }

        public uint StackCount
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWItemFields.StackCount);
            }
        }

        public ItemFlags Flags
        {
            get
            {
                return (ItemFlags)GetDescriptor<uint>((int)Descriptors.WoWItemFields.DynamicFlags);
            }
        }

        public uint RandomPropertiesId
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWItemFields.RandomPropertiesID);
            }
        }

        public uint Durability
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWItemFields.Durability);
            }
        }

        public uint MaxDurability
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWItemFields.MaxDurability);
            }
        }

        public uint EnchantId
        {
            get { return GetDescriptor<uint>((int)Descriptors.WoWItemFields.Enchantment); }
        }


        public static implicit operator uint(WoWItem self)
        {
            return self != null ? self.Pointer : 0;
        }

        public static new WoWItem Invalid = new WoWItem(0);
    }
}
