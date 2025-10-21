using Godot;

namespace ZenithRising.Scripts.Skills.Balance.Data;

[GlobalClass]
public partial class ProjectileData : Resource
{
    [ExportGroup("Projectile Properties")]
    [Export] public float ProjectileSpeed { get; set; } = 0f;
    [Export] public int PierceCount { get; set; } = 0;
    [Export] public float ProjectileLifetime { get; set; } = 5f;
    [Export] public int ProjectileCount { get; set; } = 0;
    [Export] public float ProjectileDamage { get; set; } = 0f;
    [Export] public float ProjectileSpreadAngle { get; set; } = 0f;

    [ExportGroup("Diamond Tier Mastery Bonuses")]
    [Export] public float DiamondSpeedBonus { get; set; } = 0.5f; // +50% speed
    [Export] public int DiamondPierceBonus { get; set; } = 2; // +2 pierce
}
