using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.Scripts;

namespace Athena.Core.Scripts.Tests
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
