using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena.Core.Patchables
{
    internal static class Offsets
    {
        internal static void Initialize()
        {
            //Ill let you make some beautiful function to do this automattically ;)
            //We rebased all now as rebasing on every read/write will eventually waste time. If your botting all day, you will have waisted a few minutes but yeah...

            #region DrawingOffsets
            DrawingOffsets.CGWorldFrame__GetActiveCamera = GeneralHelper.RebaseAddress(DrawingOffsets.CGWorldFrame__GetActiveCamera);
            //DrawingOffsets.Possible_AspectRatio = GeneralHelper.RebaseAddress(DrawingOffsets.Possible_AspectRatio);
            DrawingOffsets.aspect1 = GeneralHelper.RebaseAddress(DrawingOffsets.aspect1);
            #endregion

            #region ObjectManager Functions
            ObjectManagerOffsets.EnumVisibleObjects = GeneralHelper.RebaseAddress(ObjectManagerOffsets.EnumVisibleObjects);
            ObjectManagerOffsets.GetActivePlayerObject = GeneralHelper.RebaseAddress(ObjectManagerOffsets.GetActivePlayerObject);
            #endregion

            #region UncataloguedFunctions
            UncataloguedFunctions.CGGameUI__Target = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGGameUI__Target);
            UncataloguedFunctions.CGObject__IsOutdoors = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGObject__IsOutdoors);
            UncataloguedFunctions.CGWorldFrame__Intersect = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGWorldFrame__Intersect);
            UncataloguedFunctions.CGUnit_C__TrackingStopInternal = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGUnit_C__TrackingStopInternal);
            UncataloguedFunctions.CGUnit_C__InitializeTrackingStateWrapper = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGUnit_C__InitializeTrackingStateWrapper);
            UncataloguedFunctions.CGUnit_C__UnitReaction = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGUnit_C__UnitReaction);
            UncataloguedFunctions.CGUnit_C__CalculateThreat = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGUnit_C__CalculateThreat);
            UncataloguedFunctions.FrameScript_SignalEventHandler = GeneralHelper.RebaseAddress(UncataloguedFunctions.FrameScript_SignalEventHandler);
            UncataloguedFunctions.CGObject__GetObjectLocation = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGObject__GetObjectLocation);
            UncataloguedFunctions.DBCGetCompressedRows = GeneralHelper.RebaseAddress(UncataloguedFunctions.DBCGetCompressedRows);
            UncataloguedFunctions.CGUnit_C__SetActiveMover = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGUnit_C__SetActiveMover);
            
            
            #endregion

            #region Lua Functions
            LuaFunctions.ExecuteBuffer = GeneralHelper.RebaseAddress(LuaFunctions.ExecuteBuffer);
            LuaFunctions.GetLocalizedText = GeneralHelper.RebaseAddress(LuaFunctions.GetLocalizedText);
            #endregion

            #region ClientDB

            ClientDbOffsets.ResearchSite = GeneralHelper.RebaseAddress(ClientDbOffsets.ResearchSite);
            ClientDbOffsets.ItemSubClass = GeneralHelper.RebaseAddress(ClientDbOffsets.ItemSubClass);
            ClientDbOffsets.SpellRange = GeneralHelper.RebaseAddress(ClientDbOffsets.SpellRange);
            ClientDbOffsets.SpellCategories = GeneralHelper.RebaseAddress(ClientDbOffsets.SpellCategories);
            ClientDbOffsets.Lock = GeneralHelper.RebaseAddress(ClientDbOffsets.Lock);
            ClientDbOffsets.SpellCastTimes = GeneralHelper.RebaseAddress(ClientDbOffsets.SpellCastTimes);
            ClientDbOffsets.SpellCooldowns = GeneralHelper.RebaseAddress(ClientDbOffsets.SpellCooldowns);
            ClientDbOffsets.QuestPOIPoint = GeneralHelper.RebaseAddress(ClientDbOffsets.QuestPOIPoint);

            //ClientDbOffsets.Spell = GeneralHelper.RebaseAddress(ClientDbOffsets.Spell);
            //ClientDbOffsets.SpellEffect = GeneralHelper.RebaseAddress(ClientDbOffsets.SpellEffect);
            #endregion

            #region Spellbook

            SpellBook.Book = GeneralHelper.RebaseAddress(SpellBook.Book);
            SpellBook.NumberOfSpells = GeneralHelper.RebaseAddress(SpellBook.NumberOfSpells);

            #endregion

            #region Packet 
            Packet.PutArray = GeneralHelper.RebaseAddress(Packet.PutArray);//
            Packet.PutData = GeneralHelper.RebaseAddress(Packet.PutData);//
            Packet.PutFloat = GeneralHelper.RebaseAddress(Packet.PutFloat); //
            Packet.PutInt16 = GeneralHelper.RebaseAddress(Packet.PutInt16); //
            Packet.PutInt32 = GeneralHelper.RebaseAddress(Packet.PutInt32); //
            Packet.PutInt64 = GeneralHelper.RebaseAddress(Packet.PutInt64);//
            Packet.PutInt8 = GeneralHelper.RebaseAddress(Packet.PutInt8); //
            Packet.PutString = GeneralHelper.RebaseAddress(Packet.PutString); //
            Packet.SendJam = GeneralHelper.RebaseAddress(Packet.SendJam); //

            Packet.ClientConection = GeneralHelper.RebaseAddress(Packet.ClientConection); //
            Packet.OsTick = GeneralHelper.RebaseAddress(Packet.OsTick); //
            #endregion
        }

        public static uint DescriptorOffset = 0x4;

        internal static class DrawingOffsets
        {
            internal static uint aspect1 = 0xE31294; //5.4.8
            internal static uint aspect2 = 0x24C;    //5.4.8

            internal static uint CGWorldFrame__GetActiveCamera = 0x4D408F;//6.4.8
            //internal static uint Possible_AspectRatio = 0xC5EC9C;//6.0.2
        }

        internal static class ObjectManagerOffsets
        {
            internal static uint EnumVisibleObjects = 0x39B686; //5.4.8
            internal static uint GetActivePlayerObject = 0x4F84; //5.4.8
        }

        internal static class UncataloguedFunctions
        {
            internal static uint CGGameUI__Target = 0x8CE510;//5.4.8

            internal static uint CGWorldFrame__Intersect = 0x5EEF7B;//5.4.8

            internal static uint CGUnit_C__UnitReaction = 0x4153C3;//5.4.8
            internal static uint CGUnit_C__CalculateThreat = 0x418816;//5.4.8
            internal static uint CGUnit_C__TrackingStopInternal = 0x418660;//5.4.8
            internal static uint CGUnit_C__InitializeTrackingStateWrapper = 0x41FB57;//5.4.8

            internal static uint FrameScript_SignalEventHandler = 0x52792;//0x52792;//5.4.8
            
            internal static uint CGObject__GetObjectLocation = 0x39D3FD; //5.4.8
            internal static uint CGObject__IsOutdoors = 0x4142AC;//5.4.8
            internal static uint DBCGetCompressedRows = 0x203D64;//5.4.8

            internal static uint CGUnit_C__SetActiveMover = 0x4173CC;//5.4.8
        }

        internal static class LuaFunctions
        {
            internal static uint ExecuteBuffer = 0x4FD12; //5.4.8
            internal static uint GetLocalizedText = 0x414267; //5.4.8
        }

        internal static class vTableOffsets
        {
            //vTables don't need to be rebased!
            internal static uint GetObjectName = 0x154 - 0x20;//5.4.8
            internal static uint Interact = 0x130 - 0x20;//5.4.8
        }

        internal static class ClientDbOffsets
        {
            internal static uint ResearchSite = 0xC8C690;
            internal static uint ItemSubClass = 0xC8BB90;
            internal static uint SpellRange = 0xC8D05C;
            internal static uint SpellCategories = 0xC8CB34;
            internal static uint Lock = 0xC8BF58;
            internal static uint SpellCastTimes = 0xC8CB08;
            internal static uint SpellCooldowns = 0xC8CBB8;
            internal static uint QuestPOIPoint = 0xC8C4D8;

            //
            internal static uint Spell = 0xC8D0E0;
            internal static uint SpellEffect = 0xC8CC68;
        }

        internal class SpellBook
        {
            internal static uint Book = 0xDC25E4;
            internal static uint NumberOfSpells = 0xDC25E0;
        }

        internal class Packet
        {
            internal static uint PutInt8 = 0x0000F018;
            internal static uint PutInt16 = 0x0000F045;
            internal static uint PutInt32 = 0x0000F075;
            internal static uint PutInt64 = 0x0000F0A3;
            internal static uint PutFloat = 0x0000F0D8; 
            internal static uint PutArray = 0x0000F10A;
            internal static uint PutData = 0x0000F20F;
            internal static uint PutString = 0x0000F2AE;
            internal static uint SendJam = 0x00399FBF;

            internal static uint ClientConection = 0x66580B;

            internal static uint OsTick = 0xBA5EF8;
        }
    }
}
