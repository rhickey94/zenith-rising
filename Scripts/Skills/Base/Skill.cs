using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Balance;
using ZenithRising.Scripts.Skills.Balance.Data;

namespace ZenithRising.Scripts.Skills.Base;

[GlobalClass]
public partial class Skill : Resource
{
    // ═══════════════════════════════════════════════════════════════
    // METADATA (Exported - Scene References & Persistence)
    // ═══════════════════════════════════════════════════════════════
    [ExportGroup("Skill Identity")]
    [Export] public string SkillId { get; set; } = "";
    [Export] public PlayerClass AllowedClass { get; set; }
    [Export] public SkillSlot Slot { get; set; }
    [Export] public PackedScene SkillEffectScene { get; set; } // Optional visual effect scene

    [ExportGroup("Mastery (Persistent)")]
    [Export] public int KillCount { get; set; } = 0;
    [Export] public SkillMasteryTier CurrentTier { get; set; } = SkillMasteryTier.Bronze;

    // ═══════════════════════════════════════════════════════════════
    // DATABASE-LOADED PROPERTIES (Not Exported - Single Source of Truth)
    // ═══════════════════════════════════════════════════════════════

    // Identity
    public string SkillName { get; private set; } = "";
    public SkillBalanceType BalanceType { get; private set; }
    public CastBehavior CastBehavior { get; private set; }
    public SkillCategory Category { get; private set; }
    public DamageSource DamageSource { get; private set; }
    public MovementBehavior MovementBehavior { get; private set; }
    public SkillType SkillType { get; private set; }
    public string Description { get; private set; } = "";

    // Base Stats
    public float BaseDamage { get; private set; }
    public float Cooldown { get; private set; }
    public float CastTime { get; private set; }
    public DamageType DamageType { get; private set; }
    public bool IsDashSkill { get; private set; }

    // Scaling

    // Animation
    public string AnimationBaseName { get; private set; }
    public bool UsesDirectionalAnimation { get; private set; }

    // Projectile Properties (loaded from ProjectileData)
    public float ProjectileSpeed { get; private set; }
    public int PierceCount { get; private set; }
    public float ProjectileLifetime { get; private set; }
    public int ProjectileCount { get; private set; }
    public float ProjectileDamage { get; private set; }
    public float ProjectileSpreadAngle { get; private set; }
    // Projectile Diamond Bonuses (loaded from ProjectileData)
    public float ProjectileDiamondSpeedBonus { get; private set; }
    public int ProjectileDiamondPierceBonus { get; private set; }

    // Melee Properties (loaded from MeleeData)
    public float Range { get; private set; }
    public float Width { get; private set; }

    // AOE Properties (loaded from AOEData)
    public float Radius { get; private set; }
    public float Duration { get; private set; }
    public int RotationCount { get; private set; }

    // Explosion Properties (loaded from ExplosionData)
    public float ExplosionDamage { get; private set; }
    public float ExplosionRadius { get; private set; }
    public float ExplosionKnockback { get; private set; }

    // Buff Properties (loaded from BuffData)
    public float BuffDuration { get; private set; }
    public float BuffAttackSpeed { get; private set; }
    public float BuffMoveSpeed { get; private set; }
    public float BuffCastSpeed { get; private set; }
    public float BuffDamage { get; private set; }
    public float BuffCDR { get; private set; }
    public float BuffDamageReduction { get; private set; }

    // Mastery Bonuses
    public float BronzeDamageBonus { get; private set; }
    public float SilverDamageBonus { get; private set; }
    public float GoldDamageBonus { get; private set; }
    public float DiamondDamageBonus { get; private set; }

    // Combo System
    public ComboStrikeData[] ComboStrikes { get; private set; }

    private bool _initialized = false;

    private static readonly int[] _masteryThresholds = [100, 500, 2000, 10000];

    /// <summary>
    /// Loads skill balance data from GameBalance.SkillDatabase.
    /// Call this before first use (done automatically by SkillManager).
    /// </summary>
    public void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        var entry = GameBalance.Instance?.SkillDatabase?.GetSkillBalance(SkillId);
        if (entry == null)
        {
            GD.PrintErr($"Skill.Initialize(): Failed to load balance data for '{SkillId}'!");
            return;
        }

        // Load Identity
        SkillName = entry.SkillName;
        BalanceType = entry.BalanceType;
        CastBehavior = entry.CastBehavior;
        Category = entry.Category;  // ✅ ADDED
        DamageSource = entry.DamageSource;
        MovementBehavior = entry.MovementBehavior;
        SkillType = entry.SkillType;  // ✅ ADDED
        Description = entry.Description;  // ✅ ADDED

        // Load Base Stats
        BaseDamage = entry.BaseDamage;
        Cooldown = entry.Cooldown;
        CastTime = entry.CastTime;  // ✅ ADDED
        DamageType = entry.DamageType;
        IsDashSkill = entry.IsDashSkill;

        // Load Scaling
        // StrengthScaling = entry.StrengthScaling;
        // IntelligenceScaling = entry.IntelligenceScaling;

        // Load Animation
        AnimationBaseName = entry.AnimationBaseName;
        UsesDirectionalAnimation = entry.UsesDirectionalAnimation;

        // Load Mastery Bonuses
        BronzeDamageBonus = entry.BronzeDamageBonus;
        SilverDamageBonus = entry.SilverDamageBonus;
        GoldDamageBonus = entry.GoldDamageBonus;
        DiamondDamageBonus = entry.DiamondDamageBonus;

        // Load Sub-Resources
        LoadProjectileData(entry);
        LoadMeleeData(entry);
        LoadAOEData(entry);
        LoadExplosionData(entry);
        LoadBuffData(entry);
        LoadComboSkillData(entry);
        _initialized = true;
    }

    /// <summary>
    /// Records a kill for this skill and checks for mastery tier upgrades.
    /// Called by skill effects when they kill an enemy.
    /// </summary>
    public void RecordKill()
    {
        KillCount++;
        CheckMasteryTierUp();
    }

    /// <summary>
    /// Gets the configuration for a specific strike in a combo.
    /// </summary>
    /// <param name="strikeIndex">0-indexed strike number (0 = first strike)</param>
    /// <returns>ComboStrikeData or null if not found</returns>
    public ComboStrikeData GetStrikeConfig(int strikeIndex)
    {
        if (ComboStrikes == null || strikeIndex < 0 || strikeIndex >= ComboStrikes.Length)
        {
            return null;
        }
        return ComboStrikes[strikeIndex];
    }

    /// <summary>
    /// Checks if the skill should advance to the next mastery tier based on kill count.
    /// </summary>
    private void CheckMasteryTierUp()
    {
        SkillMasteryTier newTier = CurrentTier;

        if (KillCount >= _masteryThresholds[3])
        {
            newTier = SkillMasteryTier.Diamond;
        }
        else if (KillCount >= _masteryThresholds[2])
        {
            newTier = SkillMasteryTier.Gold;
        }
        else if (KillCount >= _masteryThresholds[1])
        {
            newTier = SkillMasteryTier.Silver;
        }
        else if (KillCount >= _masteryThresholds[0])
        {
            newTier = SkillMasteryTier.Bronze;
        }

        if (newTier != CurrentTier)
        {
            CurrentTier = newTier;
        }
    }

    private void LoadProjectileData(SkillBalanceEntry entry)
    {
        if (entry.Projectile == null)
        {
            return;
        }

        ProjectileSpeed = entry.Projectile.ProjectileSpeed;
        PierceCount = entry.Projectile.PierceCount;
        ProjectileLifetime = entry.Projectile.ProjectileLifetime;
        ProjectileCount = entry.Projectile.ProjectileCount;
        ProjectileDamage = entry.Projectile.ProjectileDamage;
        ProjectileSpreadAngle = entry.Projectile.ProjectileSpreadAngle;

        ProjectileDiamondSpeedBonus = entry.Projectile.DiamondSpeedBonus;
        ProjectileDiamondPierceBonus = entry.Projectile.DiamondPierceBonus;
    }

    private void LoadMeleeData(SkillBalanceEntry entry)
    {
        if (entry.Melee == null)
        {
            return;
        }

        Range = entry.Melee.Range;
        Width = entry.Melee.Width;
    }

    private void LoadAOEData(SkillBalanceEntry entry)
    {
        if (entry.AOE == null)
        {
            return;
        }

        Radius = entry.AOE.Radius;
        Duration = entry.AOE.Duration;
        RotationCount = entry.AOE.RotationCount;

        // Apply mastery rotation bonuses (if skill uses rotations)
        if (entry.AOE.RotationCount > 0)  // ← Generic check
        {
            RotationCount += CurrentTier switch
            {
                SkillMasteryTier.Bronze => entry.AOE.BronzeRotationBonus,
                SkillMasteryTier.Silver => entry.AOE.SilverRotationBonus,
                SkillMasteryTier.Gold => entry.AOE.GoldRotationBonus,
                SkillMasteryTier.Diamond => entry.AOE.DiamondRotationBonus,
                _ => 0
            };
        }
    }

    private void LoadBuffData(SkillBalanceEntry entry)
    {
        if (entry.Buff == null)
        {
            GD.Print($"[DEBUG] Skill {SkillId}: No BuffData found");  // ← Add this

            return;
        }

        BuffDuration = entry.Buff.Duration;
        GD.Print($"[DEBUG] Skill {SkillId}: Loaded BuffDuration = {BuffDuration}");  // ← Add this

        BuffAttackSpeed = entry.Buff.AttackSpeedBonus;
        BuffMoveSpeed = entry.Buff.MoveSpeedBonus;
        BuffCastSpeed = entry.Buff.CastSpeedBonus;
        BuffDamage = entry.Buff.DamageBonus;
        BuffCDR = entry.Buff.CooldownReductionBonus;
        BuffDamageReduction = entry.Buff.DamageReductionBonus;
    }

    private void LoadExplosionData(SkillBalanceEntry entry)
    {
        if (entry.Explosion == null)
        {
            return;
        }

        ExplosionDamage = entry.Explosion.ExplosionDamage;
        ExplosionRadius = entry.Explosion.ExplosionRadius;
        ExplosionKnockback = entry.Explosion.ExplosionKnockback;
    }

    private void LoadComboSkillData(SkillBalanceEntry entry)
    {
        if (entry.ComboStrikes == null) return;

        ComboStrikes = entry.ComboStrikes;
    }
}

public enum MovementBehavior
{
    Locked,
    Allowed,
    Forced
}

public enum CastBehavior
{
    Instant,           // Non-interrupting (can use while doing other actions)
    AnimationDriven,   // Interrupting (locks FSM to CastingSkill state)
    Channeled,         // Hold button for continuous effect
    Charged,           // Hold to power up, release to fire
    Combo,             // Input sequence with timing windows
    Toggle             // On/off state (Overwatch precision mode)
}

public enum DamageSource
{
    PlayerHitbox,       // Damage from player-attached Area2D
    EffectCollision,    // Damage from spawned effect's collision
    None                // No damage (buffs/utility)
}

public enum SkillType
{
    Active,
    Passive
}

public enum SkillCategory
{
    Attack,
    Spell
}

public enum SkillSlot
{
    BasicAttack,    // Left click
    SpecialAttack,  // Right click

    Primary,        // Q key / 1
    Secondary,      // E key / 2
    Tertiary,       // W key / 3
    Ultimate,       // R key / 4
    Utility         // Space
}

public enum SkillMasteryTier
{
    Bronze,   // 0-99 kills
    Silver,   // 100-499 kills
    Gold,     // 500-1999 kills
    Diamond   // 2000+ kills
}
