using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class SpellCategoriesDBC : DBCRow
    {
        public uint Id { get; private set; }

        public uint SpellDispelTypeId { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.SpellDispelTypeId = GeneralHelper.Memory.Read<uint>(pRow + 20U);
        }

        public override uint RowSize
        {
            get { return 0x28; }
        }
        public override uint Entry { get { return Id; } }
    }
}
