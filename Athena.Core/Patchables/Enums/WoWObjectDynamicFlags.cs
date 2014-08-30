namespace Athena.Core.Patchables.Enums
{
    public enum WoWObjectDynamicFlags : uint
    {
        None = 0U,
        Invisible = 1U,
        Lootable = 2U,
        TrackUnit = 4U,
        TaggedByOther = 8U,
        TaggedByMe = 16U,
        SpecialInfo = 32U,
        ReferAFriendLinked = 128U,
        IsTappedByAllThreatList = 256U,
    }

}
