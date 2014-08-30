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
    public class TrackingStopTestScript : Script
    {
        public TrackingStopTestScript()
            : base("TrackingStop", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            ObjectManager.LocalPlayer.TrackingStop();
            Print("Stopping Tracking...");
            Stop();
        }
    }
}
