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
    public class InteractionTestScript : Script
    {
        public InteractionTestScript()
            : base("Interaction -> Peacebloom", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            foreach (var p in ObjectManager.Objects.Where(x => x.IsGameObject).Cast<WoWGameObject>().Where(x => x.Name == "Peacebloom").OrderBy(x => x.Location.DistanceTo(ObjectManager.LocalPlayer.Location)))
            {
                Print("-- {0}", p.Name);
                Print("\tInteracting...");
                p.Interact();
            }

            Stop();
        }
    }
}
