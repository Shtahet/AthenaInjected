using System;
using System.Data;
using System.Runtime.InteropServices;
using Athena.Core.Internal.Objects;
using Athena.Core.Patchables;

namespace Athena.Core.Internal.GameManager
{
    public static class WoWFunctions
    {
        #region WowObject Functions
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void GetObjectLocationDelegate(uint objectPointer, out Location loc);
        public static GetObjectLocationDelegate _getObjectFunctionLocation;
        #endregion

        #region ObjectManager Functions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint EnumVisibleObjectsDelegate(IntPtr callback, int filter);
        public static EnumVisibleObjectsDelegate _enumVisibleObjects;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint GetActivePlayerObejctDeledate();
        public static GetActivePlayerObejctDeledate _getActivePlayer;
        #endregion

        #region UncataloguedFunctions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SetUITargetDelegate(ulong guidPointer);
        public static SetUITargetDelegate _setTarget;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate bool GetObjectIsOutdoorsDelegate(uint pointer);
        public static GetObjectIsOutdoorsDelegate _GetObjectIsOutdoors;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int TrackingStopDelegate(uint pointer, uint one, uint one2);
        public static TrackingStopDelegate _TrackingStop;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate char TrackingStartDelegate(
           uint pointer, int clickType, ref ulong interactGuid, ref Location clickLocation, float precision);
        public static TrackingStartDelegate _TrackingStart;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool GetSpellCooldownDelegate(
            uint spellId, bool isPet, ref int duration, ref int start, ref bool isEnabled, ref int zero);
        public static GetSpellCooldownDelegate _getSpellCooldown;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int UnitReactionDelegate(uint thisObj, uint unitToCompare);
        public static UnitReactionDelegate _unitReaction;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate char UnitThreatInfoDelegate(
            uint player, uint target, ref uint threatStatus, ref uint rawPct, ref uint scaledPct,
            ref uint threatValue);
        public static UnitThreatInfoDelegate _unitThreatInfo;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SetActiveMover(ulong guid, bool _true);
        public static SetActiveMover _SetActiveMover;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate bool Packet_SendJamDelegate(uint _this, uint a2, int always2);
        public static Packet_SendJamDelegate Packet_SendJam;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint ClientConnectionDelegate();
        public static ClientConnectionDelegate _ClientConnection;

        #endregion

        #region luaFunctions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DoStringDelegate(string command, string commandagain, uint zero);
        public static DoStringDelegate _doString;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate uint GetLocalizedTextDelegate(uint pointer, string returnString, int negativeone);
        public static GetLocalizedTextDelegate _GetLocalizedText;
        #endregion

        public static void Initialize()
        {
            #region WowObject Functions
            _getObjectFunctionLocation =
                GeneralHelper.Memory.CreateFunction<GetObjectLocationDelegate>(
                    Offsets.UncataloguedFunctions.CGObject__GetObjectLocation);
            #endregion

            #region ObjectManager Functions

            _enumVisibleObjects =
                GeneralHelper.Memory.CreateFunction<EnumVisibleObjectsDelegate>(
                Offsets.ObjectManagerOffsets.EnumVisibleObjects);

            _getActivePlayer =
                GeneralHelper.Memory.CreateFunction<GetActivePlayerObejctDeledate>(
                    Offsets.ObjectManagerOffsets.GetActivePlayerObject);

            #endregion

            #region UncataloguedFunctions

            _setTarget =
                GeneralHelper.Memory.CreateFunction<SetUITargetDelegate>(
                    Offsets.UncataloguedFunctions.CGGameUI__Target);

            _GetObjectIsOutdoors =
                GeneralHelper.Memory.CreateFunction<GetObjectIsOutdoorsDelegate>(
                    Offsets.UncataloguedFunctions.CGObject__IsOutdoors);

            _TrackingStop =
                GeneralHelper.Memory.CreateFunction<TrackingStopDelegate>(
                    Offsets.UncataloguedFunctions.CGUnit_C__TrackingStopInternal);

            _TrackingStart =
                GeneralHelper.Memory.CreateFunction<TrackingStartDelegate>(
                    Offsets.UncataloguedFunctions.CGUnit_C__InitializeTrackingStateWrapper);

            _getSpellCooldown =
                GeneralHelper.Memory.CreateFunction<GetSpellCooldownDelegate>(
                    Offsets.UncataloguedFunctions.CGUnit_C__InitializeTrackingStateWrapper);

            _unitReaction =
                GeneralHelper.Memory.CreateFunction<UnitReactionDelegate>(
                    Offsets.UncataloguedFunctions.CGUnit_C__UnitReaction);

            _unitThreatInfo =
                GeneralHelper.Memory.CreateFunction<UnitThreatInfoDelegate>(
                    Offsets.UncataloguedFunctions.CGUnit_C__CalculateThreat);

            _SetActiveMover =
                GeneralHelper.Memory.CreateFunction<SetActiveMover>(
                    Offsets.UncataloguedFunctions.CGUnit_C__SetActiveMover);

            Packet_SendJam =
  GeneralHelper.Memory.CreateFunction<Packet_SendJamDelegate>(Offsets.Packet.SendJam);

            _ClientConnection =
  GeneralHelper.Memory.CreateFunction<ClientConnectionDelegate>(Offsets.Packet.ClientConection);
            #endregion

            #region Lua Functions
            _doString = GeneralHelper.Memory.CreateFunction<DoStringDelegate>(
                    Offsets.LuaFunctions.ExecuteBuffer);

            _GetLocalizedText = GeneralHelper.Memory.CreateFunction<GetLocalizedTextDelegate>(
                    Offsets.LuaFunctions.GetLocalizedText);
            #endregion
        }
    }
}
