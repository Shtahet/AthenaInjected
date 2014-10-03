using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WowDBCFile
    {
        public uint Magic;
        public int RecordsCount;
        public int FieldsCount;
        public int RecordSize;
        public int StringTableSize;
    }
}
