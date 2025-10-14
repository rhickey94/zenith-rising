using Godot;

namespace SpaceTower.Scripts.Progression.Upgrades;

[GlobalClass]
public partial class Upgrade : Resource
{
    [Export] public string UpgradeName { get; set; }
    [Export] public string Description { get; set; }
    [Export] public UpgradeType Type { get; set; }
    [Export] public float Value { get; set; }
}

public enum UpgradeType
{
    DamagePercent,
    AttackSpeed,
    MovementSpeed,
    MaxHealth,
    PickupRadius,
    ProjectilePierce,
    CritChance,
    HealthRegen
}
