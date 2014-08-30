using System;
using Athena.Core.Internal.Objects;
using Athena.Core.Patchables;
using Athena.Core.Patchables.Enums;

namespace Athena.Core.Internal.GameManager.IngameObjects
{
    public class WoWUnit : WoWObject
    {

        public WoWUnit(uint pointer)
            : base(pointer)
        {
        }

        public WoWGuid TargetGuid
        {
            get
            {
                ulong low = GetDescriptor<ulong>((int) Descriptors.WoWUnitFields.Target);
                ulong high = GetDescriptor<ulong>((int) (Descriptors.WoWUnitFields.Target + 0x8));
                return new WoWGuid(low, high);
            }
        }

        public WoWObject Target
        {
            get
            {
                return ObjectManager.GetObjectByGuid(TargetGuid);
            }
        }

        public bool IsDead
        {
            get { return Health <= 0; }
        }

        public uint InfoFlags
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.Sex);
            }
        }

        public WoWRace Race
        {

            get { return (WoWRace)((int)this.InfoFlags & (int)byte.MaxValue); }
        }

        public WoWClass Class
        {
            get { return (WoWClass)((int)(this.InfoFlags >> 8) & (int)byte.MaxValue); }
        }

        public bool IsLootable
        {
            get { return (DynamicFlags & WoWObjectDynamicFlags.Lootable) != 0; }
        }

        public bool IsTapped
        {
            get { return (DynamicFlags & WoWObjectDynamicFlags.TaggedByOther) != 0; }
        }

        public bool IsTappedByMe
        {
            get { return (DynamicFlags & WoWObjectDynamicFlags.TaggedByMe) != 0; }
        }

        public bool IsInCombat
        {
            get { return (Flags & UnitFlags.Combat) != 0; }
        }

        public uint Health
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.Health);
            }
        }

        public uint MaxHealth
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MaxHealth);
            }
        }

        public double HealthPercentage
        {
            get
            {
                return (Health / (double)MaxHealth) * 100;
            }
        }

        public double PowerPercentage
        {
            get
            {
                return (Power / (double)MaxPower) * 100;
            }
        }

        public uint Level
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.Level);
            }
        }

        public UnitFlags Flags
        {
            get
            {
                return (UnitFlags)GetDescriptor<uint>((int)Descriptors.WoWUnitFields.Flags);
            }
        }

        public UnitFlags2 Flags2
        {
            get
            {
                return (UnitFlags2)GetDescriptor<uint>((int)Descriptors.WoWUnitFields.Flags2);
            }
        }

        public uint NpcFlags
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.NpcFlags);
            }
        }

        public uint Faction
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.FactionTemplate);
            }
        }

        public uint BaseAttackTime
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.AttackRoundBaseTime);
            }
        }

        public uint RangedAttackTime
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.RangedAttackRoundBaseTime);
            }
        }

        public float BoundingRadius
        {
            get
            {
                return GetDescriptor<float>((int)Descriptors.WoWUnitFields.BoundingRadius);
            }
        }

        public float CombatReach
        {
            get
            {
                return GetDescriptor<float>((int)Descriptors.WoWUnitFields.CombatReach);
            }
        }

        public uint DisplayId
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.DisplayID);
            }
        }

        public uint MountDisplayId
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MountDisplayID);
            }
        }

        public uint NativeDisplayId
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.NativeDisplayID);
            }
        }

        public uint MinDamage
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MinDamage);
            }
        }

        public uint MaxDamage
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MaxDamage);
            }
        }

        public uint MinOffhandDamage
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MaxOffHandDamage);
            }
        }

        public uint MaxOffhandDamage
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MaxOffHandDamage);
            }
        }

        public uint PetExperience
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.PetExperience);
            }
        }

        public uint PetNextLevelExperience
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.PetNextLevelExperience);
            }
        }

        public uint BaseMana
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.BaseMana);
            }
        }

        public uint BaseHealth
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.BaseHealth);
            }
        }

        public uint AttackPower
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.AttackPower);
            }
        }

        public uint RangedAttackPower
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.RangedAttackPower);
            }
        }

        public uint MinRangedDamage
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MinRangedDamage);
            }
        }

        public uint MaxRangedDamage
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MaxRangedDamage);
            }
        }

        public uint MaxItemLevel
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MaxItemLevel);
            }
        }

        // 4.1: Contains mana, energy, rage and runic power. Thanks to JuJu.
        public uint Power
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.Power);
            }
        }

        public uint MaxPower
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWUnitFields.MaxPower);
            }
        }

        public WoWGuid SummonedBy
        {
            get
            {
                ulong low = GetDescriptor<ulong>((int)Descriptors.WoWUnitFields.SummonedBy);
                ulong high = GetDescriptor<ulong>((int)Descriptors.WoWUnitFields.SummonedBy + 0x8);
                return new WoWGuid(low, high);
            }
        }

        public WoWGuid CreatedBy
        {
            get
            {
                ulong low = GetDescriptor<ulong>((int)Descriptors.WoWUnitFields.CreatedBy);
                ulong high = GetDescriptor<ulong>((int)Descriptors.WoWUnitFields.CreatedBy + 0x8);
                return new WoWGuid(low, high);
            }
        }

        public WowUnitReaction ReactionToPlayer
        {
            get { return (WowUnitReaction) WoWFunctions._unitReaction(ObjectManager.LocalPlayer.Pointer, Pointer); }
        }

        public bool HaveThreatWith(uint objectPointer)
        {
            //if (!this.InCombat)
            //    return false;
            uint Status = 0;
            uint RawPercentage = 0;
            uint ThreatValue = 0;
            uint ScaledPercent = 0;
            WoWFunctions._unitThreatInfo(Pointer, objectPointer, ref Status, ref RawPercentage, ref ScaledPercent, ref ThreatValue);
            return Status > 0;
        }

        public bool HaveThreatWithPlayer
        {
            get { return HaveThreatWith(ObjectManager.LocalPlayer.Pointer); }
        }

        public bool HaveAggroWith(uint objectPointer)
        {
            //if (!this.InCombat)
            //    return false;
            uint Status = 0;
            uint RawPercentage = 0;
            uint ThreatValue = 0;
            uint ScaledPercent = 0;
            WoWFunctions._unitThreatInfo(Pointer, objectPointer, ref Status, ref RawPercentage, ref ScaledPercent, ref ThreatValue);
            return Status >=3;
        }

        public bool HaveAggroWithPlayer
        {
            get { return HaveAggroWith(ObjectManager.LocalPlayer.Pointer); }
        }

        public override string ToString()
        {
            return "[\"" + Name + "\", Distance = " + (int)Distance + ", Type = " + Type + "]";
        }


        public static implicit operator uint(WoWUnit self)
        {
            return self != null ? self.Pointer : 0;
        }

        public static new WoWUnit Invalid = new WoWUnit(0);
    }
}
