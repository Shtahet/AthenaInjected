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
