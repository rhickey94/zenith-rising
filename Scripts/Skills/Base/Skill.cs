using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Executors;

namespace SpaceTower.Scripts.Skills.Base;

[GlobalClass]
public partial class Skill : Resource
{
    [Export] public string SkillName { get; set; }
    [Export] public string Description { get; set; }
    [Export] public SkillType Type { get; set; }
    [Export] public float Cooldown { get; set; } = 0f; // For active skills

    [Export] public PlayerClass AllowedClass { get; set; }
    [Export] public SkillSlot Slot { get; set; }

    [Export] public PackedScene SkillEffectScene { get; set; } // Optional visual effect scene

    // Mastery tracking
    [Export] public int KillCount { get; set; } = 0;
    [Export] public SkillMasteryTier CurrentTier { get; set; } = SkillMasteryTier.Bronze;

    private static readonly int[] _masteryThresholds = { 100, 500, 2000, 10000 };

    private ISkillExecutor _executor;

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
        // Type-based factory - automatically matches skill to executor based on data type

        return this switch
        {
            Data.ProjectileSkill => new ProjectileSkillExecutor(),
            Data.InstantAOESkill => new InstantAOESkillExecutor(),
            Data.MeleeAttackSkill => new MeleeSkillExecutor(),
            Data.StunSkill => new MeleeSkillExecutor(), // Reuses melee executor
            _ => null
        };
    }
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
