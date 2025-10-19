using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Balance;
using ZenithRising.Scripts.Skills.Executors;

namespace ZenithRising.Scripts.Skills.Base;

[GlobalClass]
public partial class Skill : Resource
{
    [Export] public string SkillId { get; set; } = "";
    [Export] public string SkillName { get; set; }
    [Export] public string Description { get; set; }
    [Export] public SkillType Type { get; set; }
    [Export] public float Cooldown { get; set; } = 0f; // For active skills
    [Export] public DamageType DamageType { get; set; } = DamageType.Physical;

    [Export] public PlayerClass AllowedClass { get; set; }
    [Export] public SkillSlot Slot { get; set; }

    [Export] public PackedScene SkillEffectScene { get; set; } // Optional visual effect scene

    // Mastery tracking
    [Export] public int KillCount { get; set; } = 0;
    [Export] public SkillMasteryTier CurrentTier { get; set; } = SkillMasteryTier.Bronze;

    // Runtime properties (loaded from database)
    public string AnimationBaseName { get; private set; }
    public bool UsesDirectionalAnimation { get; private set; }

    public float BaseDamage { get; private set; }
    public float Range { get; private set; }
    public float Radius { get; private set; }
    public float Duration { get; private set; }
    public int ProjectileCount { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public float ProjectileDamage { get; private set; }
    public float ProjectileSpreadAngle { get; private set; }
    public float ProjectileLifetime { get; private set; }
    public int PierceCount { get; private set; }
    public SkillBalanceType BalanceType { get; private set; }
    public CastBehavior CastBehavior { get; private set; }
    public DamageSource DamageSource { get; private set; }
    public MovementBehavior MovementBehavior { get; private set; }
    public float BronzeDamageBonus { get; private set; }
    public float SilverDamageBonus { get; private set; }
    public float GoldDamageBonus { get; private set; }
    public float DiamondDamageBonus { get; private set; }
    public int WhirlwindRotations { get; private set; }
    public int BronzeRotationBonus { get; private set; }
    public int SilverRotationBonus { get; private set; }
    public int GoldRotationBonus { get; private set; }
    public int DiamondRotationBonus { get; private set; }
    public float ExplosionDamage { get; private set; }
    public float ExplosionRadius { get; private set; }

    private bool _initialized = false;

    private static readonly int[] _masteryThresholds = { 100, 500, 2000, 10000 };

    private ISkillExecutor _executor;

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

        // Load balance values
        AnimationBaseName = entry.AnimationBaseName;
        UsesDirectionalAnimation = entry.UsesDirectionalAnimation;

        SkillName = entry.SkillName;
        DamageType = entry.DamageType;
        CastBehavior = entry.CastBehavior;
        DamageSource = entry.DamageSource;
        BalanceType = entry.BalanceType;
        MovementBehavior = entry.MovementBehavior;

        Cooldown = entry.Cooldown;
        BaseDamage = entry.BaseDamage;

        LoadProjectileData(entry);
        LoadMeleeData(entry);
        LoadAOEData(entry);
        LoadExplosionData(entry);

        _initialized = true;
        GD.Print($"Skill '{SkillName}' ({SkillId}) initialized from database");
    }

    public bool Execute(Player player)
    {
        if (CastBehavior == CastBehavior.AnimationDriven)
        {
            GD.PrintErr($"{SkillName} is AnimationDriven - should not call Execute()!");
            return false;
        }
        _executor ??= CreateExecutor();
        return _executor?.ExecuteSkill(player, this) ?? false;
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
            GD.Print($"{SkillName} advanced to {CurrentTier} tier! ({KillCount} kills)");
        }
    }

    private ISkillExecutor CreateExecutor()
    {
        // Only used for EffectCollision damage source
        if (DamageSource != DamageSource.EffectCollision)
        {
            return null; // PlayerHitbox and None don't use executors
        }

        // Route by BalanceType for effect-based skills
        return BalanceType switch
        {
            SkillBalanceType.Projectile => new InstantProjectileExecutor(),
            SkillBalanceType.PersistentZone => null, // Future implementation
            SkillBalanceType.CastSpawn => null, // Future implementation
            _ => null
        };
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
    }

    private void LoadMeleeData(SkillBalanceEntry entry)
    {
        if (entry.Melee == null)
        {
            return;
        }

        Range = entry.Melee.Range;
        Duration = entry.Melee.CastTime;
    }

    private void LoadAOEData(SkillBalanceEntry entry)
    {
        if (entry.AOE == null)
        {
            return;
        }

        Radius = entry.AOE.Radius;
        Duration = entry.AOE.Duration;
        WhirlwindRotations = entry.AOE.RotationCount;

        // Apply mastery rotation bonuses (if skill uses rotations)
        if (entry.AOE.RotationCount > 0)  // ← Generic check
        {
            WhirlwindRotations += CurrentTier switch
            {
                SkillMasteryTier.Bronze => entry.AOE.BronzeRotationBonus,
                SkillMasteryTier.Silver => entry.AOE.SilverRotationBonus,
                SkillMasteryTier.Gold => entry.AOE.GoldRotationBonus,
                SkillMasteryTier.Diamond => entry.AOE.DiamondRotationBonus,
                _ => 0
            };
        }
    }

    private void LoadExplosionData(SkillBalanceEntry entry)
    {
        if (entry.Explosion == null)
        {
            return;
        }

        ExplosionDamage = entry.Explosion.ExplosionDamage;
        ExplosionRadius = entry.Explosion.ExplosionRadius;
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
    Instant,           // Execute immediately on input
    AnimationDriven    // Requires animation playback
}

public enum DamageSource
{
    PlayerHitbox,      // Damage from player-attached Area2D
    EffectCollision,   // Damage from spawned effect's collision
    None              // No damage (buffs/utility)
}

public enum SkillType
{
    Active,
    Passive
}

public enum SkillSlot
{
    BasicAttack,
    SpecialAttack,

    Primary,    // Q key
    Secondary,  // E key
    Ultimate,    // R key
    Utility
}

public enum SkillMasteryTier
{
    Bronze,   // 0-99 kills
    Silver,   // 100-499 kills
    Gold,     // 500-1999 kills
    Diamond   // 2000+ kills
}
