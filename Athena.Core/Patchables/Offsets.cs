using System.Windows.Forms;

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
            //DrawingOffsets.WorldFrame = GeneralHelper.RebaseAddress(DrawingOffsets.WorldFrame);
            //DrawingOffsets.aspect1 = GeneralHelper.RebaseAddress(DrawingOffsets.aspect1);
            DrawingOffsets.Possible_AspectRatio = GeneralHelper.RebaseAddress(DrawingOffsets.Possible_AspectRatio);
            #endregion
            
            #region ObjectManager Functions
            ObjectManagerOffsets.EnumVisibleObjects = GeneralHelper.RebaseAddress(ObjectManagerOffsets.EnumVisibleObjects);
            ObjectManagerOffsets.GetActivePlayerObject = GeneralHelper.RebaseAddress(ObjectManagerOffsets.GetActivePlayerObject);
            #endregion

            #region WowObject Functions
            WowObjectOffsets.GetObjectLocation = GeneralHelper.RebaseAddress(WowObjectOffsets.GetObjectLocation);
            #endregion

            #region UncataloguedFunctions
            UncataloguedFunctions.CGGameUI__Target = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGGameUI__Target);
            UncataloguedFunctions.WowObject__IsOutdoors = GeneralHelper.RebaseAddress(UncataloguedFunctions.WowObject__IsOutdoors);
            UncataloguedFunctions.CGWorldFrame__Intersect = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGWorldFrame__Intersect);
            UncataloguedFunctions.CGUnit_C__TrackingStopInternal = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGUnit_C__TrackingStopInternal);
            UncataloguedFunctions.CGUnit_C__InitializeTrackingStateWrapper = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGUnit_C__InitializeTrackingStateWrapper);
            UncataloguedFunctions.Spell_C_GetSpellCooldown = GeneralHelper.RebaseAddress(UncataloguedFunctions.Spell_C_GetSpellCooldown);
            UncataloguedFunctions.CGUnit_C__UnitReaction = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGUnit_C__UnitReaction);
            UncataloguedFunctions.CGUnit_C__CalculateThreat = GeneralHelper.RebaseAddress(UncataloguedFunctions.CGUnit_C__CalculateThreat);
            #endregion

            #region Lua Functions
            LuaFunctions.ExecuteBuffer = GeneralHelper.RebaseAddress(LuaFunctions.ExecuteBuffer);
            LuaFunctions.GetLocalizedText = GeneralHelper.RebaseAddress(LuaFunctions.GetLocalizedText);
            #endregion
        }

        public static uint DescriptorOffset = 0x4;

        internal static class DrawingOffsets
        {
            //internal static uint WorldFrame = 0xE9FDBC; //6.0.2
            //internal static uint ActiveCamera = 0x7610; //6.0.2

            //internal static uint aspect1 = 0xE31294; //5.4.8
            //internal static uint aspect2 = 0x24C;    //5.4.8

            internal static uint CGWorldFrame__GetActiveCamera = 0x42CBA1;//6.0.2
            internal static uint Possible_AspectRatio = 0xC5EC9C;//6.0.2
        }

        internal static class ObjectManagerOffsets
        {
            internal static uint EnumVisibleObjects = 0x2AD181; //6.0.2
            internal static uint GetActivePlayerObject = 0x04268; //6.0.2
        }

        internal static class UncataloguedFunctions
        {
            internal static uint CGGameUI__Target = 0x9D7AE3;//6.0.2
            internal static uint WowObject__IsOutdoors = 0x33EB12;//6.0.2
            internal static uint CGWorldFrame__Intersect = 0x5A1232;//6.0.2
            internal static uint CGUnit_C__TrackingStopInternal = 0x3412A0;//6.0.2
            internal static uint CGUnit_C__InitializeTrackingStateWrapper = 0x34EB4F;//6.0.2
            internal static uint Spell_C_GetSpellCooldown = 0x291289;//6.0.2
            internal static uint CGUnit_C__UnitReaction = 0x352E21;//6.0.2
            internal static uint CGUnit_C__CalculateThreat = 0x345AB4;//6.0.2
        }

        internal static class WowObjectOffsets
        {
            internal static uint GetObjectLocation = 0x2AF7E2; //6.0.2
        }

        internal static class LuaFunctions
        {
            internal static uint ExecuteBuffer = 0x29F20; //6.0.2
            internal static uint GetLocalizedText = 0x33EA54; //6.0.2
        }

        internal static class vTableOffsets
        {
            //vTables don't need to be rebased!
            internal static uint GetObjectName = 0x154;//6.0.2
            internal static uint Interact = 0x130;//6.0.2
        }
    }
}
