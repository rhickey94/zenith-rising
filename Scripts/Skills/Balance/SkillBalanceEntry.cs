using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Balance;

[GlobalClass]
public partial class SkillBalanceEntry : Resource
{
    [ExportGroup("Identity")]
    [Export] public string SkillId { get; set; } = "";
    [Export] public string SkillName { get; set; } = "";
    [Export] public SkillBalanceType BalanceType { get; set; }
    [Export] public CastBehavior CastBehavior { get; set; } = CastBehavior.Instant;
    [Export] public DamageSource DamageSource { get; set; } = DamageSource.EffectCollision;

    [ExportGroup("Base Stats")]
    [Export] public float BaseDamage { get; set; } = 10f;
    [Export] public float Cooldown { get; set; } = 1.0f;
    [Export] public DamageType DamageType { get; set; } = DamageType.Physical;

    [ExportGroup("Area/Range")]
    [Export] public float Radius { get; set; } = 0f; // For AOE skills
    [Export] public float Range { get; set; } = 0f; // For melee/projectile
    [Export] public float Width { get; set; } = 0f; // For melee arcs

    [ExportGroup("Projectile Properties")]
    [Export] public float ExplosionDamage { get; set; } = 0f;
    [Export] public float ProjectileSpeed { get; set; } = 0f;
    [Export] public int PierceCount { get; set; } = 0;
    [Export] public float ProjectileLifetime { get; set; } = 5f;

    [ExportGroup("Timing")]
    [Export] public float CastTime { get; set; } = 0f; // Animation duration
    [Export] public float Duration { get; set; } = 0f; // For persistent zones/buffs

    [ExportGroup("Scaling")]
    [Export] public float StrengthScaling { get; set; } = 0f; // Physical skills
    [Export] public float IntelligenceScaling { get; set; } = 0f; // Magical skills

    [ExportGroup("Mastery Bonuses")]
    [Export] public float BronzeDamageBonus { get; set; } = 0.05f; // +5%
    [Export] public float SilverDamageBonus { get; set; } = 0.10f; // +10%
    [Export] public float GoldDamageBonus { get; set; } = 0.15f; // +15%
    [Export] public float DiamondDamageBonus { get; set; } = 0.25f; // +25%
}
