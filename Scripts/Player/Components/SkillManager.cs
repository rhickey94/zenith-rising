using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class SkillManager : Node
{
    [Export] public Skill BasicAttackSkill { get; set; }
    [Export] public Skill SpecialAttackSkill { get; set; }

    [Export] public Skill PrimarySkill { get; set; }
    [Export] public Skill SecondarySkill { get; set; }
    [Export] public Skill UltimateSkill { get; set; }

    [Export] public Skill UtilitySkill { get; set; }

    private float _basicAttackCooldownTimer = 0.0f;
    private float _specialAttackCooldownTimer = 0.0f;
    private float _primarySkillCooldownTimer = 0.0f;
    private float _secondarySkillCooldownTimer = 0.0f;
    private float _ultimateSkillCooldownTimer = 0.0f;
    private float _utilitySkillCooldownTimer = 0.0f;

    private Player _player;
    private StatsManager _statsManager;

    public override void _Ready()
    {
        _player = GetParent<Player>();
        if (_player == null)
        {
            GD.PrintErr("SkillManager: Could not find Player parent!");
        }

        _statsManager = _player.GetNode<StatsManager>("StatsManager");
        if (_statsManager == null)
        {
            GD.PrintErr("SkillManager: StatsManager not found!");
        }

        ValidateSkill(PrimarySkill, SkillSlot.Primary);
        ValidateSkill(SecondarySkill, SkillSlot.Secondary);
        ValidateSkill(UltimateSkill, SkillSlot.Ultimate);
        ValidateSkill(BasicAttackSkill, SkillSlot.BasicAttack);
        ValidateSkill(SpecialAttackSkill, SkillSlot.SpecialAttack);
        ValidateSkill(UtilitySkill, SkillSlot.Utility);

        InitializeAllSkills();
    }

    public void Update(float delta)
    {
        if (_basicAttackCooldownTimer > 0)
        {
            _basicAttackCooldownTimer -= delta;
        }

        if (_specialAttackCooldownTimer > 0)
        {
            _specialAttackCooldownTimer -= delta;
        }

        if (_primarySkillCooldownTimer > 0)
        {
            _primarySkillCooldownTimer -= delta;
        }

        if (_secondarySkillCooldownTimer > 0)
        {
            _secondarySkillCooldownTimer -= delta;
        }

        if (_ultimateSkillCooldownTimer > 0)
        {
            _ultimateSkillCooldownTimer -= delta;
        }

        if (_utilitySkillCooldownTimer > 0)
        {
            _utilitySkillCooldownTimer -= delta;
        }
    }

    public void UseSkill(SkillSlot slot)
    {
        switch (slot)
        {
            case SkillSlot.BasicAttack:
                UseSkill(BasicAttackSkill, ref _basicAttackCooldownTimer);
                break;
            case SkillSlot.SpecialAttack:
                UseSkill(SpecialAttackSkill, ref _specialAttackCooldownTimer);
                break;
            case SkillSlot.Primary:
                UseSkill(PrimarySkill, ref _primarySkillCooldownTimer);
                break;
            case SkillSlot.Secondary:
                UseSkill(SecondarySkill, ref _secondarySkillCooldownTimer);
                break;
            case SkillSlot.Ultimate:
                UseSkill(UltimateSkill, ref _ultimateSkillCooldownTimer);
                break;
            case SkillSlot.Utility:
                UseSkill(UtilitySkill, ref _utilitySkillCooldownTimer);
                break;
        }
    }

    private void UseSkill(Skill skill, ref float cooldownRemaining)
    {
        if (skill == null || _statsManager == null)
        {
            return;
        }

        if (cooldownRemaining > 0)
        {
            return;
        }

        // Initialize skill from database
        skill.Initialize();

        // Calculate actual cooldown (if skill has one)
        float actualCooldown = 0f;
        if (skill.Cooldown > 0)
        {
            // Apply CDR to cooldown
            actualCooldown = skill.Cooldown * (1 - _statsManager.CooldownReduction);
        }

        // Route based on CastBehavior
        if (skill.CastBehavior == CastBehavior.AnimationDriven)
        {
            // Request player to play animation
            if (_player.TryCastSkill(skill))
            {
                // If skill has cooldown, apply it
                if (actualCooldown > 0)
                {
                    cooldownRemaining = actualCooldown;
                }
                else
                {
                    // If no cooldown, limit by attack rate (for Attack category skills)
                    if (skill.Category == SkillCategory.Attack)
                    {
                        cooldownRemaining = 1.0f / _statsManager.CurrentAttackRate;
                    }
                    // Spells with no cooldown don't have artificial delay
                }
            }
        }
        else // CastBehavior.Instant
        {
            bool success = _player.TryInstantSkill(skill);
            if (success)
            {
                // Instant skills always use cooldown (if they have one)
                cooldownRemaining = actualCooldown;
            }
        }
    }

    private void ValidateSkill(Skill skill, SkillSlot expectedSlot)
    {
        if (skill == null)
        {
            return;
        }

        bool isAllowed = skill.AllowedClass == PlayerClass.All || skill.AllowedClass == _player.CurrentClass;
        if (!isAllowed)
        {
            GD.PrintErr($"Skill {skill.SkillName} is for {skill.AllowedClass}, but player is {_player.CurrentClass}!");
        }

        if (skill.Slot != expectedSlot)
        {
            GD.PrintErr($"Skill {skill.SkillName} is a {skill.Slot} skill, but equipped in {expectedSlot} slot!");
        }
    }

    /// <summary>
    /// Gets the remaining cooldown time for a skill slot.
    /// </summary>
    public float GetCooldownRemaining(SkillSlot slot)
    {
        return slot switch
        {
            SkillSlot.BasicAttack => _basicAttackCooldownTimer,
            SkillSlot.SpecialAttack => _specialAttackCooldownTimer,
            SkillSlot.Primary => _primarySkillCooldownTimer,
            SkillSlot.Secondary => _secondarySkillCooldownTimer,
            SkillSlot.Ultimate => _ultimateSkillCooldownTimer,
            SkillSlot.Utility => _utilitySkillCooldownTimer,
            _ => 0f
        };
    }

    /// <summary>
    /// Gets the total cooldown duration for a skill slot.
    /// </summary>
    public float GetCooldownTotal(SkillSlot slot)
    {
        var skill = GetSkill(slot);
        if (skill == null) return 0f;

        // Apply CDR to get actual cooldown
        if (_statsManager != null && skill.Cooldown > 0)
        {
            return skill.Cooldown * (1 - _statsManager.CooldownReduction);
        }

        return skill.Cooldown;
    }

    /// <summary>
    /// Gets the skill equipped in a specific slot.
    /// </summary>
    private Skill GetSkill(SkillSlot slot)
    {
        return slot switch
        {
            SkillSlot.BasicAttack => BasicAttackSkill,
            SkillSlot.SpecialAttack => SpecialAttackSkill,
            SkillSlot.Primary => PrimarySkill,
            SkillSlot.Secondary => SecondarySkill,
            SkillSlot.Ultimate => UltimateSkill,
            SkillSlot.Utility => UtilitySkill,
            _ => null
        };
    }

    /// <summary>
    /// Initializes all equipped skills by loading their data from the balance database.
    /// Called during _Ready() to ensure skill metadata is available before UI needs it.
    /// </summary>
    private void InitializeAllSkills()
    {
        BasicAttackSkill?.Initialize();
        SpecialAttackSkill?.Initialize();
        PrimarySkill?.Initialize();
        SecondarySkill?.Initialize();
        UltimateSkill?.Initialize();
        UtilitySkill?.Initialize();
    }
}
