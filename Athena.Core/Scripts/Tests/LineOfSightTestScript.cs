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
            
            Print("-- {0}", target.Name);
            Print("\tILOS: {0}", target.IsInLineOfSight);
            
            Stop();
        }
    }
}
