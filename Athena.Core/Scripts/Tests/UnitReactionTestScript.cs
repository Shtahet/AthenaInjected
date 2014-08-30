using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Scripts;

namespace Athena.Core.Scripts
{
    public class UnitReactionTestScript : Script
    {
        public UnitReactionTestScript()
            : base("UnitReaction -> Target", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            WoWUnit target = (WoWUnit)ObjectManager.LocalPlayer.Target;
            
            Print("-- {0}", target.Name);
            Print("\tReaction {0}", target.ReactionToPlayer.ToString());
            
            Stop();
        }
    }
}
