using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.Scripts;

namespace Athena.Core.Scripts.Tests
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
