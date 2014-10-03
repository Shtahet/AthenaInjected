using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class SpellCastTimesDBC : DBCRow
    {
        public uint Id { get; private set; }

        public uint CastTime { get; private set; }

        public uint SpellCastTimes { get; private set; }

        public uint MinCastTime { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.CastTime = GeneralHelper.Memory.Read<uint>(pRow + 4U);
            this.SpellCastTimes = GeneralHelper.Memory.Read<uint>(pRow + 8U);
            this.MinCastTime = GeneralHelper.Memory.Read<uint>(pRow + 12U);
        }

        public override uint RowSize
        {
            get { return 0x10; }
        }
        public override uint Entry { get { return Id; } }
    }
}
