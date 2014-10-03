using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.GameManager.DBC.Internal
{
    public class DBC
    {
        private readonly WoWClientDB m_dbInfo;
        private readonly WowDBCFile m_fileHdr;

        private uint RowSize;
        public DBC(IntPtr dbcBase, uint _RowSize)
        {
            IntPtr ptr = dbcBase;
            this.m_dbInfo = GeneralHelper.Memory.Read<WoWClientDB>((uint)((int)ptr));
            this.m_fileHdr = GeneralHelper.Memory.Read<WowDBCFile>((uint)((int)this.m_dbInfo.Data));
            RowSize = _RowSize;
        }

        public uint GetRowEntry(int index)
        {
            int rowNumber = index;
            uint RowPointer = (uint)(m_dbInfo.FirstRow + (int)(rowNumber * RowSize)); //Rows not FirstRow in WoD

            uint indexRead = GeneralHelper.Memory.Read<uint>(RowPointer);

            return indexRead;
        }

        public uint GetRowPtr(int index)
        {
            int rowNumber = index;
            uint RowPointer = (uint)(m_dbInfo.FirstRow + (int)(rowNumber * RowSize));//Rows not FirstRow in WoD

            return RowPointer;
        }

        public int MaxIndex
        {
            get { return this.m_dbInfo.MaxIndex; }
        }

        public int MinIndex
        {
            get { return this.m_dbInfo.MinIndex; }
        }

        public int NumRows
        {
            get { return this.m_dbInfo.NumRows; }
        }

        public IntPtr vTable
        {
            get { return this.m_dbInfo.VTable; }
        }

    }
}
