using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class LockDBC : DBCRow
    {
        public uint Id { get; private set; }

        public uint LockTypeId { get; private set; }

        public uint RequiredLevel { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.LockTypeId = GeneralHelper.Memory.Read<uint>(pRow + 36U);
            this.RequiredLevel = GeneralHelper.Memory.Read<uint>(pRow + 68U);
        }

        public override uint RowSize
        {
            get { return 0x84; }
        }
        public override uint Entry { get { return Id; } }
    }
}
