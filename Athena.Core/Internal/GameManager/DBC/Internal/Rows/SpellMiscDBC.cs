using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class SpellMiscDBC : DBCRow
    {
        public uint Id { get; private set; }

        public uint SpellId { get; private set; }

        public uint SpellCastTimesId { get; private set; }

        public uint SpellRangeId { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.SpellId = GeneralHelper.Memory.Read<uint>(pRow + 4U);
            this.SpellCastTimesId = GeneralHelper.Memory.Read<uint>(pRow + 64U);
            this.SpellRangeId = GeneralHelper.Memory.Read<uint>(pRow + 72U);
        }

        public override uint RowSize
        {
            get { return 0x64; }
        }
        public override uint Entry { get { return Id; } }
    }
}
