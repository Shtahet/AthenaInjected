using System.Collections.Generic;
using System.Linq;
using Athena.Core.Patchables;
using Athena.Core.Patchables.Enums;

namespace Athena.Core.Internal.GameManager.IngameObjects
{
    public class WoWLocalPlayer : WoWPlayer
    {
        public new static void Initialize()
        {
            WoWObject.Initialize();
        }

        public WoWLocalPlayer(uint pointer)
            : base(pointer)
        {

        }

        public bool IsLooting
        {
            get { return (Flags & UnitFlags.Looting) != 0; }
        }


        public List<WoWItem> Items
        {
            get
            {
                return ObjectManager.Objects
                    .Where(x => x.IsValid && x.IsItem)
                    .Select(x => x as WoWItem)
                    .Where(x => x.OwnerGuid.Equals(Guid))
                    .ToList();
            }
        }

        public WoWItem GetEquippedItem(EquipSlot slot)
        {
            var entry = GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.VisibleItems + ((int)slot * 0x8));
            var item = Items.Where(x => x.Entry == entry).FirstOrDefault() ?? WoWItem.Invalid;
            return item;
        }

        public void TrackingStop()
        {
            WoWFunctions._TrackingStop(Pointer, 1, 1);
        }

        public static implicit operator uint(WoWLocalPlayer self)
        {
            return self != null ? self.Pointer : 0;
        }

        public static new WoWLocalPlayer Invalid = new WoWLocalPlayer(0);
    }
}
