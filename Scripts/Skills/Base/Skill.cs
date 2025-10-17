using Godot;
using ZenithRising.Scripts.Core;
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
    public float BaseDamage { get; private set; }
    public float Range { get; private set; }
    public float Radius { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public int PierceCount { get; private set; }
    public SkillBalanceType BalanceType { get; private set; }
    public CastBehavior CastBehavior { get; private set; }
    public DamageSource DamageSource { get; private set; }

    public float Damage
    {
        get => BaseDamage;
        set => BaseDamage = value;
    }


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
        SkillName = entry.SkillName;
        Cooldown = entry.Cooldown;
        DamageType = entry.DamageType;
        BaseDamage = entry.BaseDamage;
        Range = entry.Range;
        Radius = entry.Radius;
        ProjectileSpeed = entry.ProjectileSpeed;
        PierceCount = entry.PierceCount;
        BalanceType = entry.BalanceType;
        Damage = entry.BaseDamage;

        CastBehavior = entry.CastBehavior;
        DamageSource = entry.DamageSource;

        _initialized = true;
        GD.Print($"Skill '{SkillName}' ({SkillId}) initialized from database");
    }

    public void Execute(Player player)
    {
        // Instantiate executor based on skill name

        _executor ??= CreateExecutor();

        _executor?.ExecuteSkill(player, this);
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
            SkillBalanceType.Projectile => new ProjectileSkillExecutor(),
            SkillBalanceType.PersistentZone => null, // Future implementation
            SkillBalanceType.CastSpawn => null, // Future implementation
            _ => null
        };
    }
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
    Ultimate    // R key
}

public enum SkillMasteryTier
{
    Bronze,   // 0-99 kills
    Silver,   // 100-499 kills
    Gold,     // 500-1999 kills
    Diamond   // 2000+ kills
}
