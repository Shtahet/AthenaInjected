using Athena.Core.Patchables;

namespace Athena.Core.Internal.GameManager.IngameObjects
{
    public class WoWPlayer : WoWUnit
    {
        public WoWPlayer(uint pointer)
            : base(pointer)
        {

        }

        public uint Experience
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.XP);
            }
        }

        public uint NextLevelExperience
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.NextLevelXP);
            }
        }

        public uint GuildRank
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.GuildRankID);
            }
        }

        public uint GuildLevel
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.GuildLevel);
            }
        }

        public float BlockPercentage
        {
            get
            {
                return GetDescriptor<float>((int)Descriptors.WoWPlayerFields.BlockPercentage);
            }
        }

        public float DodgePercentage
        {
            get
            {
                return GetDescriptor<float>((int)Descriptors.WoWPlayerFields.DodgePercentage);
            }
        }

        public float ParryPercentage
        {
            get
            {
                return GetDescriptor<float>((int)Descriptors.WoWPlayerFields.ParryPercentage);
            }
        }

        public uint Expertise
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.MainhandExpertise);
            }
        }

        public uint OffhandExpertise
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.OffhandExpertise);
            }
        }

        public float CritPercentage
        {
            get
            {
                return GetDescriptor<float>((int)Descriptors.WoWPlayerFields.CritPercentage);
            }
        }

        public float RangedCritPercentage
        {
            get
            {
                return GetDescriptor<float>((int)Descriptors.WoWPlayerFields.RangedCritPercentage);
            }
        }

        public float OffhandCritPercentage
        {
            get
            {
                return GetDescriptor<float>((int)Descriptors.WoWPlayerFields.OffhandCritPercentage);
            }
        }

        public uint Mastery
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.Mastery);
            }
        }

        public uint RestedExperience
        {
            get
            {
                return GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.RestState);
            }
        }

        public ulong Coinage
        {
            get
            {
                return GetDescriptor<ulong>((int)Descriptors.WoWPlayerFields.Coinage);
            }
        }

        public uint PlayerFlags
        {
            get { return GetDescriptor<uint>((int)Descriptors.WoWPlayerFields.PlayerFlags); }
        }

        public bool IsGhost
        {
            get { return (PlayerFlags & (1 << 4)) > 0; }
        }

        public static implicit operator uint(WoWPlayer self)
        {
            return self != null ? self.Pointer : 0;
        }

        public static new WoWPlayer Invalid = new WoWPlayer(0);
    }
}
