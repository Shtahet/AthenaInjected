using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;

namespace Athena.Core.Internal.Objects
{
    public class WoWSpell
    {
        public WoWSpell(string name, uint id)
        {
            Name = name;
            Id = id;
        }

        public bool IsValid
        {
            get { return Id != 0; }
        }

        public uint Id { get; private set; }

        public string Name { get; private set; }

        public void Cast()
        {
            Cast(ObjectManager.LocalPlayer);
        }

        public void Cast(WoWUnit target)
        {
            if (!IsValid)
                return;

            if (target == null || !target.IsValid)
                return;

            WoWFunctions._setTarget(target.Guid);
            WoWLua.ExecuteBuffer(string.Format("CastSpellByID(\"{0}\")", Id));
        }
    }
}
