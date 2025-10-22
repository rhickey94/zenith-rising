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

    [Export] public float BufferWindowSeconds = 0.15f;

    private float _basicAttackCooldownTimer = 0.0f;
    private float _specialAttackCooldownTimer = 0.0f;
    private float _primarySkillCooldownTimer = 0.0f;
    private float _secondarySkillCooldownTimer = 0.0f;
    private float _ultimateSkillCooldownTimer = 0.0f;
    private float _utilitySkillCooldownTimer = 0.0f;

    private SkillSlot? _bufferedSkillSlot = null;
    private double _bufferTimestamp = 0.0;

    private Player _player;
    private StatsManager _statsManager;
    private AnimationController _animationController;

    public override void _Ready()
    {
        // Don't call GetNode here - wait for Initialize()
    }

    public void Initialize(Player player, StatsManager statsManager, AnimationController animationController)
    {
        _player = player;
        _statsManager = statsManager;
        _animationController = animationController;

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

        ProcessInputBuffer(delta);
    }

    // Public method - routes to appropriate skill based on slot
    public void UseSkill(SkillSlot slot)
    {
        // === INPUT CANCELLATION ===
        // Latest input always wins - cancel buffered skill
        if (_bufferedSkillSlot.HasValue && _bufferedSkillSlot.Value != slot)
        {
            ClearInputBuffer();
        }

        // === ANIMATION LOCK CHECK ===
        // During first 85% of animation, ignore inputs (except Dash)
        bool isDash = (slot == SkillSlot.Utility); // Assuming Dash is utility slot

        if (_player.IsInAnimationLock() && !isDash)
        {
            // Input ignored - player committed to animation
            return;
        }

        bool success = false;

        switch (slot)
        {
            case SkillSlot.BasicAttack:
                success = TryUseSkill(BasicAttackSkill, ref _basicAttackCooldownTimer);
                break;
            case SkillSlot.SpecialAttack:
                success = TryUseSkill(SpecialAttackSkill, ref _specialAttackCooldownTimer);
                break;
            case SkillSlot.Primary:
                success = TryUseSkill(PrimarySkill, ref _primarySkillCooldownTimer);
                break;
            case SkillSlot.Secondary:
                success = TryUseSkill(SecondarySkill, ref _secondarySkillCooldownTimer);
                break;
            case SkillSlot.Ultimate:
                success = TryUseSkill(UltimateSkill, ref _ultimateSkillCooldownTimer);
                break;
            case SkillSlot.Utility:
                success = TryUseSkill(UtilitySkill, ref _utilitySkillCooldownTimer);
                break;
        }

        // If skill failed to execute, buffer the input for retry
        if (!success)
        {
            // Check if in recovery window
            if (_player.IsInRecoveryWindow())
            {
                // Buffer for smooth chaining
                _bufferedSkillSlot = slot;
                _bufferTimestamp = Time.GetTicksMsec() / 1000.0;
            }
            // Otherwise: Input ignored (cooldown, busy, etc.)
        }
        else
        {
            // Skill succeeded - clear buffer
            ClearInputBuffer();
        }
    }

    // Private method - attempts to execute a skill (renamed from UseSkill to TryUseSkill)
    private bool TryUseSkill(Skill skill, ref float cooldownRemaining)
    {
        if (skill == null || _statsManager == null)
        {
            return false;
        }

        if (cooldownRemaining > 0)
        {
            return false;
        }

        // Initialize skill from database
        skill.Initialize();

        // === NEW: Combo handling for basic attacks ===
        int strikeNumber = 1; // Default to strike 1

        if (skill.Slot == SkillSlot.BasicAttack)
        {
            var comboTracker = _player.GetComboTracker();
            if (comboTracker != null)
            {
                strikeNumber = comboTracker.GetNextComboStrike(skill);
            }
        }

        // Calculate actual cooldown (if skill has one)
        float actualCooldown = 0f;
        if (skill.Cooldown > 0)
        {
            // Apply CDR to cooldown
            actualCooldown = skill.Cooldown * (1 - _statsManager.CooldownReduction);
        }

        // Route based on CastBehavior
        bool success = false;

        if (skill.CastBehavior == CastBehavior.AnimationDriven)
        {
            // Request player to play animation
            if (_player.TryCastSkill(skill, strikeNumber))
            {
                success = true;

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
            if (_player.TryInstantSkill(skill))
            {
                success = true;
                // Instant skills always use cooldown (if they have one)
                cooldownRemaining = actualCooldown;
            }
        }

        return success;
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

    public void ProcessInputBuffer(double delta)
    {
        if (_bufferedSkillSlot == null) return;

        double currentTime = Time.GetTicksMsec() / 1000.0;
        double age = currentTime - _bufferTimestamp;

        // Check if buffer expired
        if (age > BufferWindowSeconds)
        {
            ClearInputBuffer();
            return;
        }

        // Try to execute buffered input
        // (temporarily restore mouse position for directional skills)
        UseSkill(_bufferedSkillSlot.Value);
    }

    private void ClearInputBuffer()
    {
        _bufferedSkillSlot = null;
        _bufferTimestamp = 0.0;
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
