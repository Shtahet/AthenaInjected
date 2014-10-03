using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Athena.Core.Internal.Objects;
using Athena.Core.Patchables;
using Athena.Core.Patchables.Enums;

namespace Athena.Core.Internal.GameManager
{
    public static class WoWWorld
    {
        #region Typedefs & Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int TracelineDelegate(
            ref Location start, ref Location end, out Location result, ref float distanceTravelled, uint flags, uint zero);
        private static TracelineDelegate _traceline;

        #endregion


        public static TracelineResult Traceline(Location start, Location end, out Location result, uint flags)
        {
            if (_traceline == null)
                _traceline = GeneralHelper.Memory.CreateFunction<TracelineDelegate>(Offsets.UncataloguedFunctions.CGWorldFrame__Intersect);

            float dist = 1.0f;
            return (TracelineResult)_traceline(ref start, ref end, out result, ref dist, flags, 0);
        }

        public static TracelineResult Traceline(Location start, Location end)
        {
            Location result;
            return Traceline(start, end, out result, 0x120171);
        }

        public static TracelineResult LineOfSightTest(Location start, Location end)
        {
            start.Z += 1.3f;
            end.Z += 1.3f;
            Location result;
            return Traceline(start, end, out result, 0x120171);
        }

        public static void ClickToMove(Location desLocation, ulong interactionGuid, WowClickToMoveType clickType, float presision)
        {
            WoWFunctions._TrackingStart(ObjectManager.LocalPlayer.Pointer, (int) clickType, ref interactionGuid, ref desLocation,
                presision);
        }

        public static void ClickToMove(Location desLocation)
        {
            ClickToMove(desLocation, 0, WowClickToMoveType.Move, 2f);
        }

        public static void ClickToMove(Location desLocation, WowClickToMoveType clickType)
        {
            ClickToMove(desLocation, 0, clickType, 2f);
        }

        public enum TracelineResult
        {
            NotCollided = 0,
            Collided = 1
        }
    }
}
