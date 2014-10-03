using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Athena.Core.Internal.DirectX;
using Athena.Core.Internal.Objects;
using Athena.Core.Patchables;

namespace Athena.Core.Internal.GameManager
{
    public class SpellManager : IPulsable
    {
        public List<WoWSpell> CachedSpellRequests = new List<WoWSpell>(); 
        public List<WoWSpell> KnownSpells = new List<WoWSpell>(); 
        private bool Update = false;

        public SpellManager()
        {
            KnownSpells = new List<WoWSpell>();
            Update = true;
        }

        public void OnPulse()
        {
            if (!ObjectManager.IsInGame)
                return;

            if (!Update)
                return;

            UpdateSpellbook();

            Update = false;
        }

        private void UpdateSpellbook()
        {
            List<uint> foundIds = new List<uint>();
            List<WoWSpell> tempList = new List<WoWSpell>();
            uint nbSpells = GeneralHelper.Memory.Read<uint>(Offsets.SpellBook.NumberOfSpells);
            uint SpellBookInfoPtr = GeneralHelper.Memory.Read<uint>(Offsets.SpellBook.Book);
            for (uint index = 0U; index < nbSpells; ++index)
            {
                uint Struct = GeneralHelper.Memory.Read<uint>(SpellBookInfoPtr + index*4U);
                //uint IsKnown = GeneralHelper.Memory.Read<uint>(Struct);
                uint ID = GeneralHelper.Memory.Read<uint>(Struct + 0x4);
                foundIds.Add(ID);
            }

            string LuaString = "local t = {}; ";

            foreach (var foundId in foundIds)
            {
                LuaString = LuaString +
                            string.Format("name{0} = GetSpellInfo({1}) table.insert(t, name{2}); ", foundId, foundId,
                                foundId);
            }

            LuaString = LuaString + "w = table.concat(t, '|'); return w";

            string[] ret = WoWLua.GetReturnValues(LuaString, "w");

            string[] nameSplit = ret[0].Split(Convert.ToChar("|"));

            for (int i = 0; i < foundIds.Count; i++)
            {
                tempList.Add(new WoWSpell(nameSplit[i], foundIds[i]));
            }
            KnownSpells = tempList;
        }

        public WoWSpell GetWoWSpellFromId(uint id)
        {
            WoWSpell tempSpell = CachedSpellRequests.FirstOrDefault(x => x.Id == id);

            if (tempSpell == null)
            {
                string LuaString = "local t = {}; ";
                LuaString = LuaString + string.Format("name{0} = GetSpellInfo({1}) table.insert(t, name{2}); ", id, id,id);
                LuaString = LuaString + "w = table.concat(t, '|'); return w";
                string[] ret = WoWLua.GetReturnValues(LuaString, "w");
                string[] nameSplit = ret[0].Split(Convert.ToChar("|"));

                WoWSpell t = new WoWSpell(nameSplit[0], id);
                CachedSpellRequests.Add(t);

                return t;
            }
            else
            {
                return tempSpell;
            }
        }

    }
}
