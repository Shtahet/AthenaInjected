using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Athena.Core.Internal.DirectX;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Objects;
using Athena.Core.Patchables;
using Athena.Core.Patchables.Enums;

namespace Athena.Core.Internal.GameManager
{
    public class ObjectManager : IPulsable
    {
        #region Callback delegates
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int EnumVisibleObjectsCallback(uint objectPointer, uint arg);
        private static IntPtr _ourCallback;
        private static readonly EnumVisibleObjectsCallback _callback = Callback;
        #endregion]

        private static readonly Dictionary<ulong, WoWObject> _objects = new Dictionary<ulong, WoWObject>();

        public static WoWLocalPlayer LocalPlayer { get; private set; }
        public static List<WoWObject> Objects { get; private set; }

        public static void Initialize()
        {
            _ourCallback = Marshal.GetFunctionPointerForDelegate(_callback);
        }

        public static WoWObject GetObjectByGuid(ulong guid)
        {
            if (_objects.ContainsKey(guid))
                return _objects[guid];
            return WoWObject.Invalid;
        }

        public static bool IsInGame
        {
            get
            {
                return LocalPlayer != null;
            }
        }

        public void OnPulse()
        {
            uint localPlayerPointer = WoWFunctions._getActivePlayer();

            if (localPlayerPointer == 0)
                return;
            LocalPlayer = new WoWLocalPlayer(localPlayerPointer);

            foreach (var obj in _objects.Values)
                obj.Pointer = 0;

            WoWFunctions._enumVisibleObjects(_ourCallback, 0);
            
            foreach (var pair in _objects.Where(p => p.Value.Pointer == 0).ToList())
                _objects.Remove(pair.Key);

            Objects = _objects.Values.ToList();
        }

        private static int Callback(uint ObjectPointer, uint arg)
        {
            /*uint guidhigh = (uint) (guid >> 32);
            var pointer = _getObjectByGuid((uint)guid, guidhigh, -1);
            if (pointer == IntPtr.Zero)
                return 1;*/

            var obj = new WoWObject(ObjectPointer);

            //WoWGuidWoD guid = obj.Guid;
            ulong guid = obj.Guid;
            if (_objects.ContainsKey(guid))
                _objects[guid].Pointer = ObjectPointer;
            else
            {
                //var obj = new WoWObject(pointer);
                var type = obj.Type;

                if (type.HasFlag(WoWObjectType.Player))
                    _objects.Add(guid, new WoWPlayer(ObjectPointer));
                else if (type.HasFlag(WoWObjectType.Unit))
                    _objects.Add(guid, new WoWUnit(ObjectPointer));
                else if (type.HasFlag(WoWObjectType.Container))
                    _objects.Add(guid, new WoWContainer(ObjectPointer));
                else if (type.HasFlag(WoWObjectType.Item))
                    _objects.Add(guid, new WoWItem(ObjectPointer));
                else if (type.HasFlag(WoWObjectType.Corpse))
                    _objects.Add(guid, new WoWCorpse(ObjectPointer));
                else if (type.HasFlag(WoWObjectType.GameObject))
                    _objects.Add(guid, new WoWGameObject(ObjectPointer));
                else if (type.HasFlag(WoWObjectType.DynamicObject))
                    _objects.Add(guid, new WoWDynamicObject(ObjectPointer));
                else
                    _objects.Add(guid, obj);
            }
            return 1;
        }
    }
}
