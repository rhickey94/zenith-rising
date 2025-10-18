using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Skills.Balance.Data;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Balance;

[GlobalClass]
public partial class SkillBalanceEntry : Resource
{
    [ExportGroup("Skill Components")]
    [Export] public ProjectileData Projectile { get; set; }
    [Export] public MeleeData Melee { get; set; }
    [Export] public AOEData AOE { get; set; }
    [Export] public ExplosionData Explosion { get; set; }

    [ExportGroup("Animation")]
    [Export] public string AnimationBaseName { get; set; } = "";
    [Export] public bool UsesDirectionalAnimation { get; set; } = true;

    [ExportGroup("Identity")]
    [Export] public string SkillId { get; set; } = "";
    [Export] public string SkillName { get; set; } = "";
    [Export] public SkillBalanceType BalanceType { get; set; }
    [Export] public CastBehavior CastBehavior { get; set; } = CastBehavior.Instant;
    [Export] public DamageSource DamageSource { get; set; } = DamageSource.EffectCollision;
    [Export] public MovementBehavior MovementBehavior { get; set; } = MovementBehavior.Allowed;

    [ExportGroup("Base Stats")]
    [Export] public float BaseDamage { get; set; } = 10f;
    [Export] public float Cooldown { get; set; } = 1.0f;
    [Export] public DamageType DamageType { get; set; } = DamageType.Physical;

    [ExportGroup("Scaling")]
    [Export] public float StrengthScaling { get; set; } = 0f; // Physical skills
    [Export] public float IntelligenceScaling { get; set; } = 0f; // Magical skills

    [ExportGroup("Mastery Bonuses")]
    [Export] public float BronzeDamageBonus { get; set; } = 0.05f; // +5%
    [Export] public float SilverDamageBonus { get; set; } = 0.10f; // +10%
    [Export] public float GoldDamageBonus { get; set; } = 0.15f; // +15%
    [Export] public float DiamondDamageBonus { get; set; } = 0.25f; // +25%

}
