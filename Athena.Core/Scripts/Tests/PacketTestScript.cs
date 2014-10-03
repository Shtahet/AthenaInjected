using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Athena.Core.Internal;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.Scripts;
using Athena.Core.Patchables;
using GreyMagic.Internals;

namespace Athena.Core.Scripts.Tests
{
    public class PacketTestScript : Script
    {
        public PacketTestScript()
            : base("Packet", "Tests")
        { }

        private bool isFreshPacket = false;
        private bool isBlacklisted = false;
        private List<uint> packetwhitelist = new List<uint>();
        private uint CurrentOpcode = 0;
        public override void OnStart()
        {
            /*packetwhitelist.Add((uint)PacketsEnum.MoveHeartbeat);
            packetwhitelist.Add((uint)PacketsEnum.MoveJump);
            packetwhitelist.Add((uint)PacketsEnum.MoveLand);
            packetwhitelist.Add((uint)PacketsEnum.MoveStart);
            packetwhitelist.Add((uint)PacketsEnum.MoveStop);
            packetwhitelist.Add((uint)PacketsEnum.MoveJumpHitHead);*/
            packetwhitelist.Add((uint)PacketsEnum.SetActiveMover);
            packetwhitelist.Add((uint)PacketsEnum.PostSetActiveMover);

            Packet_PutFloat =
               GeneralHelper.Memory.CreateFunction<Packet_PutFloatDelegate>(Offsets.Packet.PutFloat);
            Packet_PutInt16 =
               GeneralHelper.Memory.CreateFunction<Packet_PutInt16Delegate>(Offsets.Packet.PutInt16);
            Packet_PutInt32 =
               GeneralHelper.Memory.CreateFunction<Packet_PutInt32Delegate>(Offsets.Packet.PutInt32);
            Packet_PutInt64 =
               GeneralHelper.Memory.CreateFunction<Packet_PutInt64Delegate>(Offsets.Packet.PutInt64);
            Packet_PutInt8 =
              GeneralHelper.Memory.CreateFunction<Packet_PutInt8Delegate>(Offsets.Packet.PutInt8);
            Packet_PutString =
              GeneralHelper.Memory.CreateFunction<Packet_PutStringDelegate>(Offsets.Packet.PutString);
            Packet_PutArray =
              GeneralHelper.Memory.CreateFunction<Packet_PutArrayDelegate>(Offsets.Packet.PutArray);
            Packet_PutData =
              GeneralHelper.Memory.CreateFunction<Packet_PutDataDelegate>(Offsets.Packet.PutData);


            Packet_PutFloat_Detour = GeneralHelper.Memory.Detours.CreateAndApply(Packet_PutFloat,
                                                                     new Packet_PutFloatDelegate(Packet_PutFloatCallback), "PutFloat");
            Packet_PutInt16_Detour = GeneralHelper.Memory.Detours.CreateAndApply(Packet_PutInt16,
                                                                     new Packet_PutInt16Delegate(Packet_PutInt16Callback), "PutInt16");
            Packet_PutInt32_Detour = GeneralHelper.Memory.Detours.CreateAndApply(Packet_PutInt32,
                                                                     new Packet_PutInt32Delegate(Packet_PutInt32Callback), "PutInt32");
            Packet_PutInt64_Detour = GeneralHelper.Memory.Detours.CreateAndApply(Packet_PutInt64,
                                                                     new Packet_PutInt64Delegate(Packet_PutInt64Callback), "PutInt64");
            Packet_PutInt8_Detour = GeneralHelper.Memory.Detours.CreateAndApply(Packet_PutInt8,
                                                                     new Packet_PutInt8Delegate(Packet_PutInt8Callback), "PutInt8");
            Packet_PutString_Detour = GeneralHelper.Memory.Detours.CreateAndApply(Packet_PutString,
                                                                     new Packet_PutStringDelegate(Packet_PutStringCallback), "PutString");
            Packet_PutArray_Detour = GeneralHelper.Memory.Detours.CreateAndApply(Packet_PutArray,
                                                                     new Packet_PutArrayDelegate(Packet_PutArrayCallback), "PutArray");
            Packet_PutData_Detour = GeneralHelper.Memory.Detours.CreateAndApply(Packet_PutData,
                                                                     new Packet_PutDataDelegate(Packet_PutDataCallback), "PutData");
            Packet_SendJam_Detour = GeneralHelper.Memory.Detours.CreateAndApply(WoWFunctions.Packet_SendJam,
                                                                     new WoWFunctions.Packet_SendJamDelegate(Packet_SendJamCallback), "SendJam");



        }

        private int Packet_PutFloatCallback(uint _this, float a2)
        {
            if (!isBlacklisted)
                Print("PutFloat {0}", a2);
            return (int)Packet_PutFloat_Detour.CallOriginal(_this, a2);
        }

        private int Packet_PutInt16Callback(uint _this, Int16 a2)
        {
            if (!isBlacklisted)
                Print("PutInt16: {0} ({1})", a2, a2.ToString("X"));
            return (int)Packet_PutInt16_Detour.CallOriginal(_this, a2);
        }

        private int Packet_PutInt32Callback(uint _this, Int32 a2)
        {
            if (isFreshPacket)
            {
                isBlacklisted = !packetwhitelist.Contains((uint)a2);
                if (!isBlacklisted)
                {
                    PacketsEnum packetName = (PacketsEnum) a2;

                    Print("[JAM Packet: {0} ({1})]", packetName, a2.ToString("X"));
                    CurrentOpcode = (uint) a2;
                }
                isFreshPacket = false;
            }
            else
            {
                if (!isBlacklisted)
                {
                    int tick = GeneralHelper.Memory.Read<int>(Offsets.Packet.OsTick);
                    Print("PutInt32: {0} ({1}) - {2}", a2, a2.ToString("X"), tick);
                }
            }
            
            return (int)Packet_PutInt32_Detour.CallOriginal(_this, a2);
        }

        private int Packet_PutInt64Callback(uint _this, Int64 a2)
        {
            if(!isBlacklisted)
                Print("PutInt64: {0} ({1})", a2, a2.ToString("X"));
            return (int)Packet_PutInt64_Detour.CallOriginal(_this, a2);
        }

        private int Packet_PutInt8Callback(uint _this, sbyte a2)
        {
            if (!isBlacklisted)
                Print("PutInt8: {0} ({1})", a2, a2.ToString("X"));
            return (int)Packet_PutInt8_Detour.CallOriginal(_this, a2);
        }

        private int Packet_PutStringCallback(uint _this, string a2)
        {
            if (!isBlacklisted) 
                Print("PutString {0}", a2);
            return (int)Packet_PutString_Detour.CallOriginal(_this, a2);
        }

        private int Packet_PutArrayCallback(uint _this, uint a2, int size)
        {
            if (!isBlacklisted)
            {
                byte[] data = GeneralHelper.Memory.ReadBytes(a2, size);
                string dataString = ByteArrayToString(data);
                Print("PutArray: {0}", dataString);
            }
            return (int)Packet_PutArray_Detour.CallOriginal(_this, a2, size);
        }

        private int Packet_PutDataCallback(uint _this, uint a2, int size)
        {
            if (!isBlacklisted)
            {
                byte[] data = GeneralHelper.Memory.ReadBytes(a2, size);
                string dataString = ByteArrayToString(data);
                Print("PutData: {0}", dataString);
            }
            return (int)Packet_PutData_Detour.CallOriginal(_this, a2, size);
        }


        private bool Packet_SendJamCallback(uint _this, uint a2, int always2)
        {
            Print("---------------------------------------");
            //Print("SendJam: {0} {1} {2}", _this, a2, always2);
            isFreshPacket = true;
            isBlacklisted = false;
            return (bool)Packet_SendJam_Detour.CallOriginal(_this, a2, always2);
        }

        public override void OnTerminate()
        {
            Packet_PutFloat_Detour.Remove();
            Packet_PutInt16_Detour.Remove();
            Packet_PutInt32_Detour.Remove();
            Packet_PutInt64_Detour.Remove();
            Packet_PutInt8_Detour.Remove();
            Packet_PutString_Detour.Remove();
            Packet_PutArray_Detour.Remove();
            Packet_PutData_Detour.Remove();
            Packet_SendJam_Detour.Remove();
        }


        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        #region Crap
        //int __thiscall Packet_PutFloat(int this, int a2)
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int Packet_PutFloatDelegate(uint _this, float a2);
        public static Packet_PutFloatDelegate Packet_PutFloat;
        private static Detour Packet_PutFloat_Detour;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int Packet_PutInt16Delegate(uint _this, Int16 a2);
        public static Packet_PutInt16Delegate Packet_PutInt16;
        private static Detour Packet_PutInt16_Detour;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int Packet_PutInt32Delegate(uint _this, Int32 a2);
        public static Packet_PutInt32Delegate Packet_PutInt32;
        private static Detour Packet_PutInt32_Detour;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int Packet_PutInt64Delegate(uint _this, Int64 a2);
        public static Packet_PutInt64Delegate Packet_PutInt64;
        private static Detour Packet_PutInt64_Detour;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int Packet_PutInt8Delegate(uint _this, sbyte a2);
        public static Packet_PutInt8Delegate Packet_PutInt8;
        private static Detour Packet_PutInt8_Detour;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int Packet_PutStringDelegate(uint _this, string a2);
        public static Packet_PutStringDelegate Packet_PutString;
        private static Detour Packet_PutString_Detour;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int Packet_PutArrayDelegate(uint _this, uint a2, int size);
        public static Packet_PutArrayDelegate Packet_PutArray;
        private static Detour Packet_PutArray_Detour;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int Packet_PutDataDelegate(uint _this, uint a2, int size);
        public static Packet_PutDataDelegate Packet_PutData;
        private static Detour Packet_PutData_Detour;


        private static Detour Packet_SendJam_Detour;
        #endregion
    }
}
