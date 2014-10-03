using System.Linq;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Scripts;

namespace Athena.Core.Scripts.Tests
{
    public class SetTargetTestsScript : Script
    {
        public SetTargetTestsScript()
            : base("SetTarget -> Closest Mob", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            WoWUnit obj =
                ObjectManager.Objects.Where(x => x.IsUnit && !x.Guid.Equals(ObjectManager.LocalPlayer.Guid))
                    .Cast<WoWUnit>()
                    .OrderBy(x => x.Location.DistanceTo(ObjectManager.LocalPlayer.Location))
                    .FirstOrDefault();
            
                Print("-- {0}", obj.Name);
                Print("\tSetting Target");
                obj.SetAsTarget();
            

            Stop();
        }
    }
}
