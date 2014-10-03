using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class SpellCooldownsDBC : DBCRow
    {
        public uint Id { get; private set; }

        public uint SpellId { get; private set; }

        public uint Cooldown1 { get; private set; }

        public uint Cooldown2 { get; private set; }

        public uint GlobalCooldown { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.SpellId = GeneralHelper.Memory.Read<uint>(pRow + 4U);
            this.Cooldown1 = GeneralHelper.Memory.Read<uint>(pRow + 12U);
            this.Cooldown2 = GeneralHelper.Memory.Read<uint>(pRow + 16U);
            this.GlobalCooldown = GeneralHelper.Memory.Read<uint>(pRow + 20U);
        }

        public override uint RowSize
        {
            get { return 0x18; }
        }
        public override uint Entry { get { return Id; } }
    }
}
