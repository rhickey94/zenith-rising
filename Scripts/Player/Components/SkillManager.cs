using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Skill execution orchestrator for player character.
/// Responsibilities:
/// - Cooldown tracking (6 skill slots)
/// - Input buffering (recovery window: last 15% of animation accepts early inputs)
/// - Animation lock enforcement (first 85% blocks non-dash inputs)
/// - Skill validation (class restrictions, slot validation)
/// - CDR (Cooldown Reduction) application
/// - Routing: Animation-driven skills → Player.TryCastSkill(), Instant skills → ExecuteInstantSkill()
/// - Combo tracking integration (basic attack chains)
/// - Instant skill effects (buff application, visual spawning)
/// Does NOT handle: Animation callbacks (SkillEffectController), buff lifecycle (BuffManager)
/// </summary>
[GlobalClass]
public partial class SkillManager : Node
{
    // ═══════════════════════════════════════════════════════════════
    // SKILL SLOTS (6 total - configured in Godot editor)
    // ═══════════════════════════════════════════════════════════════
    [Export] public Skill BasicAttackSkill { get; set; }      // LMB
    [Export] public Skill SpecialAttackSkill { get; set; }    // RMB
    [Export] public Skill PrimarySkill { get; set; }          // 1 Key
    [Export] public Skill SecondarySkill { get; set; }        // 2 Key
    [Export] public Skill UltimateSkill { get; set; }         // 3 Key
    [Export] public Skill UtilitySkill { get; set; }          // Spacebar

    // ═══════════════════════════════════════════════════════════════
    // INPUT BUFFERING CONFIG
    // ═══════════════════════════════════════════════════════════════
    [Export] public float BufferWindowSeconds = 0.15f; // Input buffer duration

    // ═══════════════════════════════════════════════════════════════
    // COOLDOWN TIMERS (counted down in Update())
    // ═══════════════════════════════════════════════════════════════
    private float _basicAttackCooldownTimer = 0.0f;
    private float _specialAttackCooldownTimer = 0.0f;
    private float _primarySkillCooldownTimer = 0.0f;
    private float _secondarySkillCooldownTimer = 0.0f;
    private float _ultimateSkillCooldownTimer = 0.0f;
    private float _utilitySkillCooldownTimer = 0.0f;

    // ═══════════════════════════════════════════════════════════════
    // INPUT BUFFER STATE
    // ═══════════════════════════════════════════════════════════════
    private SkillSlot? _bufferedSkillSlot = null;
    private double _bufferTimestamp = 0.0;

    // ═══════════════════════════════════════════════════════════════
    // DEPENDENCIES
    // ═══════════════════════════════════════════════════════════════
    private Player _player;
    private BuffManager _buffManager;
    private StatsManager _statsManager;
    private AnimationController _animationController;

    // ═══════════════════════════════════════════════════════════════
    // LIFECYCLE METHODS
    // ═══════════════════════════════════════════════════════════════
    public override void _Ready()
    {
        // Wait for Initialize() - dependencies injected by Player._Ready()
    }

    public void Initialize(Player player, StatsManager statsManager, AnimationController animationController, BuffManager buffManager)
    {
        _player = player;
        _statsManager = statsManager;
        _animationController = animationController;
        _buffManager = buffManager;

        // Validate all equipped skills
        ValidateSkill(PrimarySkill, SkillSlot.Primary);
        ValidateSkill(SecondarySkill, SkillSlot.Secondary);
        ValidateSkill(UltimateSkill, SkillSlot.Ultimate);
        ValidateSkill(BasicAttackSkill, SkillSlot.BasicAttack);
        ValidateSkill(SpecialAttackSkill, SkillSlot.SpecialAttack);
        ValidateSkill(UtilitySkill, SkillSlot.Utility);

        // Load skill data from SkillBalanceDatabase
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

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - SKILL EXECUTION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Primary entry point for skill execution. Called by InputManager via Player.OnSkillPressed().
    /// Handles: Proactive input buffering, cooldown routing, dash interrupts.
    /// New behavior: Always buffers inputs during animations for smooth combos (latest input wins).
    /// </summary>
    public void UseSkill(SkillSlot slot)
    {
        // Input Cancellation: Latest input always wins
        if (_bufferedSkillSlot.HasValue && _bufferedSkillSlot.Value != slot)
        {
            ClearInputBuffer();
        }

        // Special case: Dash can interrupt animations
        bool isDash = (slot == SkillSlot.Utility);

        // Proactive Buffering: If player is casting, buffer the input for execution after animation ends
        if (_player.IsCastingSkill() && !isDash)
        {
            _bufferedSkillSlot = slot;
            _bufferTimestamp = Time.GetTicksMsec() / 1000.0;
            return;
        }

        // Try to execute skill immediately (player is idle/running or this is a dash interrupt)
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

        // Clear buffer on successful execution
        if (success)
        {
            ClearInputBuffer();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - SKILL EXECUTION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Attempts to execute a skill if cooldown allows.
    /// Handles: Database initialization, combo tracking, CDR application, routing to Player/ExecuteInstantSkill.
    /// </summary>
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

        // Load skill data from SkillBalanceDatabase
        skill.Initialize();

        // Combo tracking: Get next strike number for basic attack chains
        int strikeNumber = 1;
        if (skill.Slot == SkillSlot.BasicAttack)
        {
            var comboTracker = _player.GetComboTracker();
            if (comboTracker != null)
            {
                strikeNumber = comboTracker.GetNextComboStrike(skill);
            }
        }

        // Apply CDR (Cooldown Reduction) from Intelligence stat
        float actualCooldown = 0f;
        if (skill.Cooldown > 0)
        {
            actualCooldown = skill.Cooldown * (1 - _statsManager.CooldownReduction);
        }

        // Route based on CastBehavior (AnimationDriven vs Instant)
        bool success = false;

        if (skill.CastBehavior == CastBehavior.AnimationDriven)
        {
            if (_player.TryCastSkill(skill, strikeNumber))
            {
                success = true;

                if (actualCooldown > 0)
                {
                    cooldownRemaining = actualCooldown;
                }
                else
                {
                    // No-cooldown attacks limited by attack rate stat
                    if (skill.Category == SkillCategory.Attack)
                    {
                        cooldownRemaining = 1.0f / _statsManager.CurrentAttackRate;
                    }
                }
            }
        }
        else // CastBehavior.Instant
        {
            if (_player.IsDead())
            {
                return false;
            }

            if (ExecuteInstantSkill(skill))
            {
                success = true;
                cooldownRemaining = actualCooldown;
            }
        }

        return success;
    }

    /// <summary>
    /// Executes instant (non-interrupting) skill effects.
    /// Currently supports: Buff application + visual spawning.
    /// Future: Heals, summons, teleports.
    /// </summary>
    private bool ExecuteInstantSkill(Skill skill)
    {
        if (skill.Slot != SkillSlot.BasicAttack)
        {
            _player.GetComboTracker()?.ResetCombo();
        }

        // Data-driven buff application
        if (skill.BuffDuration > 0)
        {
            _buffManager?.ApplyBuff(
                buffId: skill.SkillId,
                duration: skill.BuffDuration,
                attackSpeedBonus: skill.BuffAttackSpeed,
                moveSpeedBonus: skill.BuffMoveSpeed,
                castSpeedBonus: skill.BuffCastSpeed,
                damageBonus: skill.BuffDamage,
                cooldownReductionBonus: skill.BuffCDR,
                damageReductionBonus: skill.BuffDamageReduction
            );

            if (skill.SkillEffectScene != null)
            {
                var effect = skill.SkillEffectScene.Instantiate<Node2D>();
                _player.GetParent().AddChild(effect);
                effect.GlobalPosition = _player.GlobalPosition;
            }

            return true;
        }

        // Future: Heals, summons, teleports
        GD.PrintErr($"ExecuteInstantSkill: Skill {skill.SkillId} has no instant effect data!");
        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - VALIDATION
    // ═══════════════════════════════════════════════════════════════
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

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - COOLDOWN QUERIES (Used by SkillBarHUD)
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Gets remaining cooldown time for UI display.
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
    /// Gets total cooldown duration (after CDR) for UI display.
    /// </summary>
    public float GetCooldownTotal(SkillSlot slot)
    {
        var skill = GetSkill(slot);
        if (skill == null) return 0f;

        if (_statsManager != null && skill.Cooldown > 0)
        {
            return skill.Cooldown * (1 - _statsManager.CooldownReduction);
        }

        return skill.Cooldown;
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - INPUT BUFFERING
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Immediately attempts to execute buffered input (frame-perfect transitions).
    /// Called by Player.OnAnimationFinished() for instant skill chaining.
    /// Returns true if buffered input was executed successfully.
    /// </summary>
    public bool TryExecuteBufferedInput()
    {
        if (_bufferedSkillSlot == null) return false;

        // Check if buffer is still valid (within buffer window)
        double currentTime = Time.GetTicksMsec() / 1000.0;
        double age = currentTime - _bufferTimestamp;

        if (age > BufferWindowSeconds)
        {
            ClearInputBuffer();
            return false;
        }

        // Execute buffered skill immediately
        SkillSlot bufferedSlot = _bufferedSkillSlot.Value;
        ClearInputBuffer(); // Clear before execution to prevent infinite loops
        UseSkill(bufferedSlot);
        return true;
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - INPUT BUFFERING
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Processes buffered input if within buffer window.
    /// Called every frame from Update() as fallback (TryExecuteBufferedInput is preferred).
    /// </summary>
    public void ProcessInputBuffer(double delta)
    {
        if (_bufferedSkillSlot == null) return;

        double currentTime = Time.GetTicksMsec() / 1000.0;
        double age = currentTime - _bufferTimestamp;

        if (age > BufferWindowSeconds)
        {
            ClearInputBuffer();
            return;
        }

        UseSkill(_bufferedSkillSlot.Value);
    }

    /// <summary>
    /// Clears the buffered input (used when dash interrupts animations).
    /// </summary>
    public void ClearBuffer()
    {
        _bufferedSkillSlot = null;
        _bufferTimestamp = 0.0;
    }

    private void ClearInputBuffer()
    {
        ClearBuffer();
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - SKILL QUERIES
    // ═══════════════════════════════════════════════════════════════
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
    /// Loads skill data from SkillBalanceDatabase for all equipped skills.
    /// Called during Initialize() to ensure metadata available before first use.
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

