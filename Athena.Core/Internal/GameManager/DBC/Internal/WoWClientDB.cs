using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WoWClientDB
    {
        public IntPtr VTable;
        public int NumRows;
        public int MaxIndex;
        public int MinIndex;
        public IntPtr Data;
        public IntPtr FirstRow;
        public IntPtr Rows;
        public IntPtr Unk1;
        public uint Unk2;
        public IntPtr Unk3;
        public uint Unk4;
        public uint RowEntrySize;
    }
}
