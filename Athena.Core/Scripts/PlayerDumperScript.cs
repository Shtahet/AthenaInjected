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
    public class PlayerDumperScript : Script
    {
        public PlayerDumperScript()
            : base("Players", "Dumper")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            foreach (var p in ObjectManager.Objects.Where(x => x.IsPlayer).Cast<WoWPlayer>())
            {
                Print("-- {0}", p.Name);
                Print("\tGUID: 0x{0}", p.Guid.ToString("X"));
                Print("\tLevel {0} {1} {2}", p.Level, p.Race, p.Class);
                Print("\tHealth: {0}/{1} ({2}%)", p.Health, p.MaxHealth, (int)p.HealthPercentage);
                //Print("\t{0}: {1}/{2} ({3}%)", p.PowerType, p.Power, p.MaxPower, (int)p.PowerPercentage);
                Print("\tPosition: {0}", p.Location);
            }

            Stop();
        }
    }
}
