using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class SpellDBC : DBCRow
    {
        public uint SpellId { get; private set; }

        public string SpellName { get; private set; }

        public int SpellCategoriesId { get; private set; }

        public int SpellCooldownsId { get; private set; }

        public int SpellMiscId { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.SpellId = GeneralHelper.Memory.Read<uint>(pRow);
            this.SpellName = this.GetString(4U, 128);
            this.SpellCategoriesId = GeneralHelper.Memory.Read<int>(pRow + 52U);
            this.SpellCooldownsId = GeneralHelper.Memory.Read<int>(pRow + 60U);
            this.SpellMiscId = GeneralHelper.Memory.Read<int>(pRow + 96U);
        }

        public override uint RowSize
        {
            get { return 0x80; }
        }

        public override uint Entry { get { return SpellId; } }
    }
}
