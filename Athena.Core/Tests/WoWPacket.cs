using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Athena.Core.Internal.GameManager;

namespace Athena.Core.Tests
{
    public class WoWPacket
    {
        private uint Address = 0;
        public uint Cave;
        public WoWPacket(uint packetCtor)
        {
            Cave = (uint) Marshal.AllocHGlobal(0x1000);
            Address = GeneralHelper.RebaseAddress(packetCtor);

            _PacketCtorDelgate = GeneralHelper.Memory.CreateFunction<PacketCtorDelgate>(Address);

            _PacketCtorDelgate(Cave);
        }


        public void Set<T>(uint offset, T val) where T : struct
        {
            GeneralHelper.Memory.Write((uint)Cave + offset, val);
        }

        public void Send()
        {
            WoWFunctions.Packet_SendJam(WoWFunctions._ClientConnection(), Cave, 2);
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int PacketCtorDelgate(uint cave);
        public static PacketCtorDelgate _PacketCtorDelgate;
    }
}
