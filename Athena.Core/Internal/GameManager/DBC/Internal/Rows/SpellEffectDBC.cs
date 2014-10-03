using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class SpellEffectDBC : DBCRow
    {
        public uint Id { get; private set; }

        public uint Effect { get; private set; }

        public uint TriggerSpell { get; private set; }

        public uint SpellId { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.Effect = GeneralHelper.Memory.Read<uint>(pRow + 4U);
            this.TriggerSpell = GeneralHelper.Memory.Read<uint>(pRow + 16U);
            this.SpellId = GeneralHelper.Memory.Read<uint>(pRow + 108U);
        }

        public override uint RowSize
        {
            get { throw new NotImplementedException(); }
        }
        public override uint Entry { get { return Id; } }
    }
}
