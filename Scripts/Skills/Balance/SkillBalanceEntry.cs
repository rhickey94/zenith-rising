using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Skills.Balance.Data;
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
    [Export] public SkillCategory Category { get; set; } = SkillCategory.Attack;
    [Export] public DamageSource DamageSource { get; set; } = DamageSource.EffectCollision;
    [Export] public MovementBehavior MovementBehavior { get; set; } = MovementBehavior.Allowed;
    [Export] public SkillType SkillType { get; set; } = SkillType.Active;

    [ExportGroup("Skill Components")]
    [Export] public ProjectileData Projectile { get; set; }
    [Export] public MeleeData Melee { get; set; }
    [Export] public AOEData AOE { get; set; }
    [Export] public ExplosionData Explosion { get; set; }
    [Export] public BuffData Buff { get; set; }

    [ExportGroup("Combo System")]
    [Export] public ComboStrikeData[] ComboStrikes { get; set; } // Array of strikes for combo attacks

    [ExportGroup("Animation")]
    [Export] public string AnimationBaseName { get; set; } = "";
    [Export] public bool UsesDirectionalAnimation { get; set; } = true;

    [ExportGroup("Base Stats")]
    [Export] public float BaseDamage { get; set; } = 10f;
    [Export] public float Cooldown { get; set; } = 1.0f;
    [Export] public float CastTime { get; set; } = 0f; // For spells only
    [Export] public DamageType DamageType { get; set; } = DamageType.Physical;
    [Export] public bool IsDashSkill { get; set; } = false;

    [ExportGroup("Scaling")]
    [Export] public float StrengthScaling { get; set; } = 0f; // Physical skills
    [Export] public float IntelligenceScaling { get; set; } = 0f; // Magical skills

    [ExportGroup("Mastery Bonuses")]
    [Export] public float BronzeDamageBonus { get; set; } = 0.05f; // +5%
    [Export] public float SilverDamageBonus { get; set; } = 0.10f; // +10%
    [Export] public float GoldDamageBonus { get; set; } = 0.15f; // +15%
    [Export] public float DiamondDamageBonus { get; set; } = 0.25f; // +25%

    [ExportGroup("Description")]
    [Export(PropertyHint.MultilineText)] public string Description { get; set; } = "";

}
