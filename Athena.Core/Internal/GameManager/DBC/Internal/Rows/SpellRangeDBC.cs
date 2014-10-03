using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class SpellRangeDBC : DBCRow
    {
        public uint Id { get; private set; }

        public float MinRangeHostile { get; private set; }

        public float MinRangeFriendly { get; private set; }

        public float MaxRangeHostile { get; private set; }

        public float MaxRangeFriendly { get; private set; }

        public uint Flags { get; private set; }

        public string Name { get; private set; }

        public string ShortName { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.MinRangeHostile = GeneralHelper.Memory.Read<float>(pRow + 4U);
            this.MinRangeFriendly = GeneralHelper.Memory.Read<float>(pRow + 8U);
            this.MaxRangeHostile = GeneralHelper.Memory.Read<float>(pRow + 12U);
            this.MaxRangeFriendly = GeneralHelper.Memory.Read<float>(pRow + 16U);
            this.Flags = GeneralHelper.Memory.Read<uint>(pRow + 20U);
            this.Name = this.GetString(24U, 128);
            this.ShortName = this.GetString(28U, 128);
        }

        public override uint RowSize
        {
            get { return 0x20; }
        }
        public override uint Entry { get { return Id; } }
    }
}
