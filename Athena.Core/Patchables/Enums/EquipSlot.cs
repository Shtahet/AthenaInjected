namespace Athena.Core.Patchables.Enums
{
    public enum EquipSlot : int
    {
        Head = 0,
        Neck,
        Shoulder,
        Body,
        Chest,
        Waist,
        Legs,
        Feet,
        Wrist,
        Hand,
        Finger1,
        Finger2,
        Trinket1,
        Trinket2,
        Back,
        MainHand,
        OffHand,
        Ranged,
        Tabard,

        FIRST_EQUIPPED = EquipSlot.Head,
        LAST_EQUIPPED = EquipSlot.Tabard,
    }

}
