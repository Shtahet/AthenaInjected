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
    public class UnitDumperScript : Script
    {
        public UnitDumperScript()
            : base("Units", "Dumper")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            foreach (var u in ObjectManager.Objects.Where(x => x.IsUnit).Cast<WoWUnit>())
            {
                Print("-- {0}", u.Name);
                Print("\tGUID: 0x{0}", u.Guid.ToString("X"));
                Print("\tHealth: {0}/{1} ({2}%)", u.Health, u.MaxHealth, (int)u.HealthPercentage);
                //Print("\tReaction: {0}", u.Reaction);
                Print("\tPosition: {0}", u.Location);
            }

            Stop();
        }
    }
}
