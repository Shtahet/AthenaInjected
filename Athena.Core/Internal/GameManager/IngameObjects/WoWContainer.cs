using System.Collections.Generic;
using Athena.Core.Internal.Objects;
using Athena.Core.Patchables;

namespace Athena.Core.Internal.GameManager.IngameObjects
{
    public class WoWContainer : WoWItem
    {
        public WoWContainer(uint pointer)
            : base(pointer)
        {

        }

        public uint Slots
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWContainerFields.NumSlots);
            }
        }

        public ulong GetItemGuid(int index)
        {
            if (index > 35 || index >= Slots || index <= 0)
                return 0;

            ulong low = GetDescriptor<ulong>((int)Descriptors.WoWContainerFields.Slots + (index * 8));
            //ulong high = GetDescriptor<ulong>((int)Descriptors.WoWContainerFields.Slots + (index * 8) + 0x8);

            //return new WoWGuidWoD(low, high);
            return low;
        }

        public WoWItem GetItem(int index)
        {
            return ObjectManager.GetObjectByGuid(GetItemGuid(index)) as WoWItem;
        }

        public List<WoWItem> Items
        {
            get
            {
                var ret = new List<WoWItem>((int)Slots);
                for (int i = 0; i < Slots; i++)
                {
                    //WoWGuidWoD guid = GetItemGuid(i);
                    ulong guid = GetItemGuid(i);
                    if (guid != 0)
                    {
                        var obj = ObjectManager.GetObjectByGuid(guid);
                        if (obj == null || !obj.IsValid || !obj.IsItem)
                            continue;
                        ret.Add(obj as WoWItem);
                    }
                }
                return ret;
            }
        }
    }
}
