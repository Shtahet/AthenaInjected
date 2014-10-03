using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal.Rows
{
    public class ResearchSiteDBC : DBCRow
    {
        public uint Id { get; private set; }

        public uint MapID { get; private set; }

        public uint POI { get; private set; }

        public string Name { get; private set; }

        public uint Flags { get; private set; }

        public override void Initialize(uint pRow)
        {
            this.Id = GeneralHelper.Memory.Read<uint>(pRow);
            this.MapID = GeneralHelper.Memory.Read<uint>(pRow + 0x4);
            this.POI = GeneralHelper.Memory.Read<uint>(pRow + 0x8);
            this.Name = this.GetString(0xC, 128);
            this.Flags = GeneralHelper.Memory.Read<uint>(pRow + 0x10);

        }

        public override uint RowSize
        {
            get { return 0x14; }
        }
        public override uint Entry { get { return Id; } }
    }
}
