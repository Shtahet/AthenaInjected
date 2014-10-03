using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal
{
    public abstract class DBCRow :IEntry
    {
        internal uint Pointer = 0;
        public abstract void Initialize(uint pRow);

        public abstract uint RowSize { get; }

        internal string GetString(uint offset, int length = 128)
        {
            return GeneralHelper.Memory.ReadString(this.Pointer + GeneralHelper.Memory.Read<uint>(this.Pointer + offset) + offset, new UTF8Encoding());
        }

        public abstract uint Entry { get;  }
    }

    interface IEntry
    {
        uint Entry { get; } 
    }
}
