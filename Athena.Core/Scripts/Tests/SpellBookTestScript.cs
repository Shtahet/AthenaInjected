using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.DBC;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Objects;
using Athena.Core.Internal.Scripts;

namespace Athena.Core.Scripts.Tests
{
    public class SpellBookTestScript : Script
    {
        public SpellBookTestScript()
            : base("SpellBook -> Dump", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            var spells = GeneralHelper.SpellManager.KnownSpells;
            Print("Number of known spells {0}", spells.Count);

            uint FrostboltID = 126201;
            WoWSpell tempSpell = GeneralHelper.SpellManager.GetWoWSpellFromId(FrostboltID);
            Print("Frostbolt local name: {0}", tempSpell.Name);


            uint sealID = 20154;
            WoWSpell tempSpell2 = GeneralHelper.SpellManager.GetWoWSpellFromId(sealID);
            tempSpell2.Cast();
            Stop();
        }
    }
}
