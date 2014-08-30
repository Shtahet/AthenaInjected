using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.Core.Internal;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Scripts;

namespace Athena.Core.Scripts
{
    public class LuaTestTestScript : Script
    {
        public LuaTestTestScript()
            : base("LuaTest", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            Print("Attempting jump via lua");
            WoWLua.ExecuteBuffer("JumpOrAscendStart()");

            Print("\nGetting Player Name via lua");

            string luaCommand ="nameString = GetUnitName(\"PLAYER\", true)";
            WoWLua.ExecuteBuffer(luaCommand);

            var name = WoWLua.GetLocalizedText("nameString");
            Print("\t - {0}", name);

            Print("\n Getting free bag slots via WoWLua.GetReturnValues");
            string[] results = WoWLua.GetReturnValues("nbSlots = 0; for i = 0, 4 do if GetContainerNumFreeSlots(i) ~= nil then nbSlots = nbSlots + GetContainerNumFreeSlots(i); end end ", "nbSlots");

            foreach (var r in results)
            {
                Print("\t - {0}", r);
            }

            Stop();
        }
    }
}
