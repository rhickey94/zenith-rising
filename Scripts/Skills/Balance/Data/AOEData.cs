using Godot;

namespace ZenithRising.Scripts.Skills.Balance.Data;

[GlobalClass]
public partial class AOEData : Resource
{
    [ExportGroup("Area/Range Properties")]
    [Export] public float Radius { get; set; } = 0f; // For AOE skills
    [Export] public float Duration { get; set; } = 0f; // For persistent zones/buffs
    [Export] public int RotationCount { get; set; } = 1; // Base: 1 rotation
    [Export] public int BronzeRotationBonus { get; set; } = 0; // Mastery tier bonuses
    [Export] public int SilverRotationBonus { get; set; } = 1; // +1 rotation at Silver
    [Export] public int GoldRotationBonus { get; set; } = 2;   // +2 rotations at Gold
    [Export] public int DiamondRotationBonus { get; set; } = 3; // +3 rotations at Diamond
}
