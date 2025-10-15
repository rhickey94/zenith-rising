namespace SpaceTower.Scripts.Core;

public struct SaveData
{
    public int Strength;
    public int Intelligence;
    public int Agility;
    public int Vitality;
    public int Fortune;

    public int Version { get; set; }
    public int CharacterLevel { get; set; }
    public int UnallocatedStatPoints { get; set; }
    public int HighestFloorReached { get; set; }
    public string LastSaved { get; set; }
}
