using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.Scripts;

namespace Athena.Core.Scripts.Tests
{
    public class PlayerInsideOrOutTestScript : Script
    {
        public PlayerInsideOrOutTestScript()
            : base("PlayerInsideOrOut", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;


            Print("-- {0}, IsOutdoors: {1}", ObjectManager.LocalPlayer.Name, ObjectManager.LocalPlayer.IsOutdoors);
            Stop();
        }
    }
}
