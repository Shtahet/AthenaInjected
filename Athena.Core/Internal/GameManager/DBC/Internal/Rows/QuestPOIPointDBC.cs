using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class QuestPOIPointDBC : DBCRow
    {
        public uint Id { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public uint pos_Id { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.X = GeneralHelper.Memory.Read<int>(pRow + 4U);
            this.Y = GeneralHelper.Memory.Read<int>(pRow + 8U);
            this.pos_Id = GeneralHelper.Memory.Read<uint>(pRow + 12U);

        }

        public override uint RowSize
        {
            get { return 0x10; }
        }
        public override uint Entry { get { return Id; } }
    }
}
