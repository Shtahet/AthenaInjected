using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.Scripts;

namespace Athena.Core.Scripts.Tests
{
    public class LineOfSightTestScript : Script
    {
        public LineOfSightTestScript()
            : base("LineOfSight -> Target", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            var target = ObjectManager.LocalPlayer.Target;
            if (target.IsValid)
            {
                Print("-- {0}", target.Name);

                Print("\tILOS: {0}", target.IsInLineOfSight);
            }
            else
            {
                Print("Null obj");
            }


            Stop();
        }
    }
}
