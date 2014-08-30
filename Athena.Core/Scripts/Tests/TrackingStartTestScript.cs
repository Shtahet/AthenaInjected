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
    public class TrackingStartTestScript : Script
    {
        public TrackingStartTestScript()
            : base("Tracking Start - > Move to Target Loc", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            var target = ObjectManager.LocalPlayer.Target;
            
            Print("-- {0}", target.Name);
            Print("Moving...");
            WoWWorld.ClickToMove(target.Location);

            Stop();
        }
    }
}
