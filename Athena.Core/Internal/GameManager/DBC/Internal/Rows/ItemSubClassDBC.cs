using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class ItemSubClassDBC : DBCRow
    {
        public uint Id { get; private set; }

        public uint ItemClassId { get; private set; }

        public uint SubClassId { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.ItemClassId = GeneralHelper.Memory.Read<uint>(pRow + 4U);
            this.SubClassId = GeneralHelper.Memory.Read<uint>(pRow + 8U);
        }

        public override uint RowSize
        {
            get { return 0x34; }
        }
        public override uint Entry { get { return Id; } }
    }
}
