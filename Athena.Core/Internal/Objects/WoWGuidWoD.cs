using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Internal.Objects
{
    public class WoWGuidWoD
    {
        public WoWGuidWoD(ulong low, ulong high)
        {
            Low = low;
            High = high;
        }

        public WoWGuidWoD()
        {
            Low = 0;
            High = 0;
        }

        public bool IsValid
        {
            get
            {
                return Low != 0;
            }
        }

        public ulong Low { get; set; }
        public ulong High { get; set; }

        public GuidType Type
        {
            get { return (GuidType)(High >> 58); }
            set { High |= (ulong)value << 58; }
        }

        public GuidSubType SubType
        {
            get { return (GuidSubType)(Low >> 56); }
            set { Low |= (ulong)value << 56; }
        }

        public ushort RealmId
        {
            get { return (ushort)((High >> 42) & 0xFFFF); }
            set { High |= (ulong)value << 42; }
        }

        public ushort ServerId
        {
            get { return (ushort)((Low >> 40) & 0xFFFF); }
            set { Low |= (ulong)value << 40; }
        }

        public ushort MapId
        {
            get { return (ushort)((High >> 29) & 0x1FFF); }
            set { High |= (ulong)value << 29; }
        }

        public uint dbId
        {
            get { return (uint)(High & 0xFFFFFF) >> 6; }
            set { High |= (ulong)value << 6; }
        }

        public ulong CreationBits
        {
            get { return Low & 0xFFFFFFFFFF; }
            set { Low |= value; }
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, SubType: {1}, RealmId: {2}, ServerId: {3}, MapId: {4}, dbId: {5} --- low: {6} high: {7}", Type,
                SubType, RealmId, ServerId, MapId, dbId, Low, High);
        }

        public override bool Equals(object obj)
        {
            WoWGuidWoD o;
            try
            {
                o = (WoWGuidWoD)obj;
            }
            catch (Exception)
            {
                o = new WoWGuidWoD();
            }

            if (this.Low != o.Low)
                return false;

            if (this.High != o.High)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return (int)(Low ^ High);
        }
    }

    public enum GuidSubType : byte
    {
        None = 0
    }

    //This may not be complete
    public enum GuidType : byte
    {
        Null = 0,
        Uniq = 1,
        Player = 2,
        Item = 3,
        StaticDoor = 4,
        Transport = 5,
        Conversation = 6,
        Creature = 7,
        Vehicle = 8,
        Pet = 9,
        GameObject = 10,
        DynamicObject = 11,
        AreaTrigger = 12,
        Corpse = 13,
        LootObject = 14,
        SceneObject = 15,
        Scenario = 16,
        AIGroup = 17,
        DynamicDoor = 18,
        ClientActor = 19,
        Vignette = 20,
        CallForHelp = 21,
        AIResource = 22,
        AILock = 23,
        AILockTicket = 24,
        ChatChannel = 25,
        Party = 26,
        Guild = 27,
        WowAccount = 28,
        BNetAccount = 29,
        GMTask = 30,
        MobileSession = 31,
        RaidGroup = 32,
        Spell = 33,
        Mail = 34,
        WebObj = 35,
        LFGObject = 36,
        LFGList = 37,
        UserRouter = 38,
        PVPQueueGroup = 39,
        UserClient = 40,
        PetBattle = 41,
        UniqueUserClient = 42,
        BattlePet = 43
    }
}
