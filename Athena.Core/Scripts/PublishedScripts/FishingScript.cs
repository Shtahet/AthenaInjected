using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.Core.Internal;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Scripts;
using GreyMagic.Internals;

namespace Athena.Core.Scripts.PublishedScripts
{
    class FishingScript : Script
    {
        public FishingScript()
            : base("Fishing", "Bot")
        {
        }

        private FishingState CurrentState
        {
            get;
            set;
        }
        private int NumberOfFishCaught = 0;
        private WoWSpell FishingSpell = null;
        private DateTime LootTimer;

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
            {
                Print("You can't start this script without being logged into your character!");
                return;
            }

            RegisterEvents();
        }

        public override void OnTerminate()
        {
            DeregisterEvents();
            CurrentState = FishingState.Lure;
            FishingSpell = null;

            Print("Fishing has finished! You caught {0} fish", NumberOfFishCaught);
        }

        public override void OnTick()
        {
            if (!ObjectManager.IsInGame)
                return;

            switch (CurrentState)
            {
                case FishingState.Lure:
                    //TODO: Lure logic
                    //TODO: Check Training
                    CurrentState = FishingState.Cast;
                    break;
                case FishingState.Cast:
                    Print("Casting Fishing Pole");
                    FishingSpell.Cast();
                    CurrentState = FishingState.Fishing;
                    break;
                case FishingState.Fishing:
                    if (IsFishing)
                    {
                        if (IsBobbing)
                            CurrentState = FishingState.Loot;
                    }
                    else
                        CurrentState = FishingState.Lure;
                    break;
                case FishingState.Loot:
                    Print("Getting Fishing Bobber");
                    Bobber.Interact();
                    LootTimer = DateTime.Now;
                    CurrentState = FishingState.Looting;
                    break;
                case FishingState.Looting:
                    var span = DateTime.Now - LootTimer;
                    if (span.TotalSeconds > 3)
                    {
                        Print("No loot detected?");
                        CurrentState = FishingState.Lure;
                    }
                    break;
                case FishingState.Combat:
                    //TODO: Add combat logic here
                    break;
                case FishingState.Training:
                    //TODO: Add training logic here
                    break;
            }
        }

        #region Properties

        private WoWGameObject Bobber
        {
            get
            {
                //Maybe there is another check we could do here?
                //Entry ID maybe?
                return ObjectManager.Objects.Where(b => b.IsValid && b.IsGameObject)
                    .Select(b => b as WoWGameObject).FirstOrDefault(b => b.CreatedByMe);
            }
        }

        private bool IsBobbing
        {
            get { return (Bobber.IsValid ? Manager.Memory.Read<byte>(new IntPtr(Bobber.Pointer.ToInt64() + Pointers.Other.IsBobbing)) == 1 : false); }
        }

        private bool IsFishing
        {
            get { return ObjectManager.LocalPlayer.ChanneledCastingId == Fishing.Id; }
        }

        private bool HasBait
        {
            get { return WoWLua.GetReturnValues("GetWeaponEnchantInfo()", "HasEnchant")[0] == "1"; }
        }

        #endregion

        #region Events
        public override void RegisterEvents()
        {
            base.RegisterEvents();
        }

        public override void DeregisterEvents()
        {
            base.DeregisterEvents();
        }
        #endregion

        #region Enums

        public enum FishingState
        {
            Lure,
            Cast,
            Fishing,
            Loot,
            Looting,
            Combat,
            Training,
        }
        #endregion
    }
}
