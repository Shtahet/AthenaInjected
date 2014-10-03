using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athena.Core.Patchables;
using Athena.Core.Patchables.Enums;
using GreyMagic.Internals;
using SlimDX.XAudio2;

namespace Athena.Core.Tests
{
    public static class WoWEvents
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int WoWEventInternalDelegate(uint ebp8, uint ebpC, uint eax, uint ebp);
        public static WoWEventInternalDelegate _WoWEventInternal;
        private static Detour _WoWEventInternalHook;

        public delegate void WoWEventHandler(FrameXMLEvents eventCode, List<string> args);
        private static readonly Dictionary<FrameXMLEvents, List<WoWEventHandler>> _eventHandler = new Dictionary<FrameXMLEvents, List<WoWEventHandler>>();


        public static void Initialize()
        {
            _WoWEventInternal =
                GeneralHelper.Memory.CreateFunction<WoWEventInternalDelegate>(
                    Offsets.UncataloguedFunctions.FrameScript_SignalEventHandler);

            _WoWEventInternalHook = GeneralHelper.Memory.Detours.CreateAndApply(_WoWEventInternal,
                                                                      new WoWEventInternalDelegate(Callback), "WoWEvents");

            Register(FrameXMLEvents.PLAYER_LEVEL_UP, HandlePlayerLevelUp);
            Register(FrameXMLEvents.AMPLIFY_UPDATE, HandleARCHAEOLOGY);
        }

        #region
        private static void HandlePlayerLevelUp(FrameXMLEvents ev, List<string> args)
        {
            GeneralHelper.MainLog(string.Join(",", args.ToArray()), "PLAYER_LEVEL_UP");
        }

        private static void HandleARCHAEOLOGY(FrameXMLEvents ev, List<string> args)
        {
            GeneralHelper.MainLog(string.Join(",", args.ToArray()), "ARCHAEOLOGY_CLOSED");
        }

        #endregion

        #region private functions

        private static int Callback(uint ebp8, uint ebpC, uint eax, uint ebp)
        {
            FrameXMLEvents eventCode = (FrameXMLEvents) ebp8;

            if (_eventHandler.ContainsKey(eventCode))
            {
                uint ArgsFormatPtr = ebpC;
                string ArgsFormat = GeneralHelper.Memory.ReadString(ArgsFormatPtr, new UTF8Encoding());

                HandleEvent(eventCode, ArgsFormat, eax);
            }

           return (int)_WoWEventInternalHook.CallOriginal(ebp8, ebpC, eax, ebp);
        }

        private static List<string> ParseArguments(string Format, uint ArgsPtr)
        {
            List<string> list = new List<string>();
            string[] strArray = Format.Split(new char[] { '%' });
            uint num = 0;
            foreach (string str in strArray)
            {
                if (str.Length == 1)
                {
                    string str2;
                    if (str == "s")
                    {
                        uint u = ArgsPtr + (num * 4);
                        uint stringPtr = GeneralHelper.Memory.Read<uint>(u);
                        str2 = GeneralHelper.Memory.ReadString(stringPtr, new UTF8Encoding());
                        list.Add(str2);
                    }
                    else if (str == "f")
                    {
                        str2 = GeneralHelper.Memory.Read<float>(ArgsPtr + (num * 4)).ToString();
                        list.Add(str2);
                    }
                    else if (str == "u")
                    {
                        str2 = GeneralHelper.Memory.Read<uint>(ArgsPtr + (num * 4)).ToString();
                        list.Add(str2);
                    }
                    else if (str == "d")
                    {
                        str2 = GeneralHelper.Memory.Read<int>(ArgsPtr + (num * 4)).ToString();
                        list.Add(str2);
                    }
                    else if (str == "b")
                    {
                        str2 = Convert.ToBoolean(GeneralHelper.Memory.Read<int>(ArgsPtr + (num * 4))).ToString();
                        list.Add(str2);
                    }
                    else
                    {
                        list.Add(string.Empty);
                    }
                    num++;
                }
            }
            return list;
        }

        private static void HandleEvent(FrameXMLEvents eventCode, string ArgsFormat, uint ArgsPtr)
        {
            List<string> parsedArgs = ParseArguments(ArgsFormat, ArgsPtr);
            foreach (WoWEventHandler handler in _eventHandler[eventCode])
                handler(eventCode, parsedArgs);
        }
        #endregion

        #region public functions
        public static void Register(FrameXMLEvents EventCode, WoWEventHandler handler)
        {
            if (_eventHandler.ContainsKey(EventCode))
                _eventHandler[EventCode].Add(handler);
            else
                _eventHandler.Add(EventCode, new List<WoWEventHandler> { handler });
        }

        public static void Remove(FrameXMLEvents EventCode, WoWEventHandler handler)
        {
            if (_eventHandler.ContainsKey(EventCode))
                _eventHandler[EventCode].Remove(handler);
        }
        #endregion
    }
}
