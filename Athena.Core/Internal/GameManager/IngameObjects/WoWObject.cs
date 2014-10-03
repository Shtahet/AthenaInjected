using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Athena.Core.Internal.Objects;
using Athena.Core.Patchables;
using Athena.Core.Patchables.Enums;

namespace Athena.Core.Internal.GameManager.IngameObjects
{
    public class WoWObject
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate uint GetObjectNameDelegate(uint objectPointer);
        public GetObjectNameDelegate _getObjectName;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void InteractDelegate(uint objectPointer);
        public InteractDelegate _Interact;

        public uint Pointer { get; set; }

        public static void Initialize()
        {
          
        }

        public WoWObject(uint pointer)
        {
            Pointer = pointer;

            if (IsValid)
            {
                _getObjectName = RegisterVirtualFunction<GetObjectNameDelegate>(Offsets.vTableOffsets.GetObjectName);
                _Interact = RegisterVirtualFunction<InteractDelegate>(Offsets.vTableOffsets.Interact);               
            
            }
        }

        protected T RegisterVirtualFunction<T>(uint offset) where T : class
        {
            var pointer = GeneralHelper.Memory.GetVFTableEntry(Pointer, offset / 4);
            if (pointer ==  0)
                return null;
            return GeneralHelper.Memory.CreateFunction<T>(pointer);
        }


        public string Name
        {
            get
            {
                var pointer = _getObjectName(Pointer);
                if (pointer == 0)
                    return "UNKNOWN";
                return GeneralHelper.Memory.ReadString(pointer, new UTF8Encoding());
            }
        }
        
        public Location Location
        {
            get
            {
                Location ret;
                WoWFunctions._getObjectFunctionLocation(Pointer, out ret);
                return ret;
            }
        }

        public void Interact()
        {
            _Interact(Pointer);
        }

        public void SetAsTarget()
        {
            //var allocatedMemory = (uint) Marshal.AllocHGlobal(0x10);

            //GeneralHelper.WriteGUID(allocatedMemory, Guid);
            //WoWFunctions._setTarget(allocatedMemory);
            WoWFunctions._setTarget(Guid);
            //Marshal.FreeHGlobal((IntPtr)allocatedMemory);
        }

        public bool IsOutdoors
        {
            get { return WoWFunctions._GetObjectIsOutdoors(Pointer); }
        }

        public bool IsInLineOfSight
        {//Not sure if this works or not...
            get
            {
                var res = WoWWorld.LineOfSightTest(ObjectManager.LocalPlayer.Location, Location);
                return res != WoWWorld.TracelineResult.Collided;
            }
        }

        public bool IsValid
        {
            get
            {
                if (ObjectManager.Objects == null)
                    return Pointer != 0;
                else
                    return Pointer != 0 && ObjectManager.Objects.Any(x => x.Pointer == Pointer);
                
            }
        }

        public WoWObjectType Type
        {
            get
            {
                return (WoWObjectType)GetDescriptor<uint>((int)Descriptors.WoWObjectFields.Type);
            }
        }

        public WoWObjectDynamicFlags DynamicFlags
        {
            get { return (WoWObjectDynamicFlags)GetDescriptor<uint>((int)Descriptors.WoWObjectFields.DynamicFlags); }
        }

        public uint StorageField
        {
            get
            {
                return GeneralHelper.Memory.Read<uint>(Pointer + Offsets.DescriptorOffset);
            }
        }


        public ulong Guid
        {
            get
            {
                ulong low = GeneralHelper.Memory.Read<ulong>(this.StorageField + (uint)Descriptors.WoWObjectFields.Guid);
                //ulong high = GeneralHelper.Memory.Read<ulong>(this.StorageField + (uint)Descriptors.WoWObjectFields.Guid + 0x8);

               //WoWGuidWoD tempGuid = new WoWGuidWoD(low, high);
                return low;
                //return tempGuid;
            }
        }

        public uint Data
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWObjectFields.Data);
            }
        }

        public uint Entry
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWObjectFields.EntryID);
            }
        }

        public float Distance
        {
            get
            {
                var local = ObjectManager.LocalPlayer;
                if (local == null || !local.IsValid)
                    return float.NaN;
                return (float)local.Location.DistanceTo(Location);
            }
        }

        protected unsafe T GetDescriptor<T>(int offset)
        {
            uint descriptorArray = *(uint*)(Pointer + (int) Offsets.DescriptorOffset);
            int size = Marshal.SizeOf(typeof(T));
            object ret = null;
            switch (size)
            {
                case 1:
                    ret = *(byte*)(descriptorArray + offset);
                    break;

                case 2:
                    ret = *(short*)(descriptorArray + offset);
                    break;

                case 4:
                    ret = *(uint*)(descriptorArray + offset);
                    break;

                case 8:
                    ret = *(ulong*)(descriptorArray + offset);
                    break;
            }
            return (T)ret;
        }

        public bool IsContainer { get { return Type.HasFlag(WoWObjectType.Container); } }
        public bool IsCorpse { get { return Type.HasFlag(WoWObjectType.Corpse); } }
        public bool IsGameObject { get { return Type.HasFlag(WoWObjectType.GameObject); } }
        public bool IsDynamicObject { get { return Type.HasFlag(WoWObjectType.DynamicObject); } }
        public bool IsUnit { get { return Type.HasFlag(WoWObjectType.Unit); } }
        public bool IsPlayer { get { return Type.HasFlag(WoWObjectType.Player); } }
        public bool IsItem { get { return Type.HasFlag(WoWObjectType.Item); } }

        public override string ToString()
        {
            return "[\"" + Name + "\", Distance = " + (int)Distance + ", Type = " + Type + "]";
        }

        public static implicit operator uint(WoWObject self)
        {
            return self != null ? self.Pointer : 0;
        }

        public static WoWObject Invalid = new WoWObject(0);
    }
}
