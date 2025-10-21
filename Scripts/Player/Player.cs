using System.Collections.Generic;
using Godot;
using ZenithRising.Scripts.Progression.Upgrades;
using ZenithRising.Scripts.PlayerScripts.Components;
using ZenithRising.Scripts.UI.Panels;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts;

public enum PlayerState
{
    Idle,
    Running,
    CastingSkill,
    Hurt,
    Dead
}

public partial class Player : CharacterBody2D
{
    // ===== EXPORT FIELDS - Config =====
    [Export] public PlayerClass CurrentClass = PlayerClass.Warrior;

    // ===== SIGNALS =====
    [Signal] public delegate void HealthChangedEventHandler(float currentHealth, float maxHealth);
    [Signal] public delegate void ExperienceChangedEventHandler(int currentXP, int requiredXP, int level);
    [Signal] public delegate void ShowLevelUpPanelEventHandler(Godot.Collections.Array<Upgrade> upgrades);

    // ===== PRIVATE FIELDS - Components =====
    private InputManager _inputManager;
    private SkillManager _skillManager;
    private StatsManager _statsManager;
    private BuffManager _buffManager;
    private UpgradeManager _upgradeManager;
    private SkillEffectController _skillEffectController;
    private AnimationController _animationController;
    private VisualFeedbackController _visualFeedbackController;
    private ForcedMovementController _movementController;
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;
    private PlayerState _currentState = PlayerState.Idle;
    private Skill _currentCastingSkill;
    private bool _isCastingWhileMoving = false;
    private PlayerState _previousState = PlayerState.Idle;

    // ===== LIFECYCLE METHODS =====
    public override void _Ready()
    {
        AddToGroup("player");

        // Get components
        _inputManager = GetNode<InputManager>("InputManager");
        if (_inputManager == null)
        {
            GD.PrintErr("Player: InputManager component not found!");
        }
        else
        {
            _inputManager.SkillPressed += OnSkillPressed;
        }

        _skillManager = GetNode<SkillManager>("SkillManager");
        if (_skillManager == null)
        {
            GD.PrintErr("Player: SkillManager component not found!");
        }

        _statsManager = GetNode<StatsManager>("StatsManager");
        if (_statsManager == null)
        {
            GD.PrintErr("Player: StatsManager component not found!");
        }
        else
        {
            // Subscribe to level up event
            _statsManager.LeveledUp += OnLeveledUp;
            _statsManager.HealthChanged += OnStatsManagerHealthChanged;       // 🆕 NEW
            _statsManager.ExperienceChanged += OnStatsManagerExperienceChanged; // 🆕 NEW
        }

        _upgradeManager = GetNode<UpgradeManager>("UpgradeManager");
        if (_upgradeManager == null)
        {
            GD.PrintErr("Player: UpgradeManager component not found!");
        }

        _buffManager = GetNode<BuffManager>("BuffManager");
        if (_buffManager == null)
        {
            GD.PrintErr("Player: BuffManager component not found!");
        }

        _sprite = GetNode<Sprite2D>("Sprite2D");
        if (_sprite == null)
        {
            GD.PrintErr("Player: AnimatedSprite2D not found!");
        }

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        if (_animationPlayer == null)
        {
            GD.PrintErr("Player: AnimationPlayer not found!");
        }
        else
        {
            _animationPlayer.AnimationFinished += OnAnimationFinished;
        }

        // Get or create SkillAnimationController
        _skillEffectController = GetNode<SkillEffectController>("SkillEffectController");
        if (_skillEffectController == null)
        {
            GD.PrintErr("Player: SkillEffectController not found!");
        }

        // Get AnimationController
        _animationController = GetNode<AnimationController>("AnimationController");
        if (_animationController == null)
        {
            GD.PrintErr("Player: AnimationController not found!");
        }

        // Get VisualFeedbackController
        _visualFeedbackController = GetNode<VisualFeedbackController>("VisualFeedbackController");
        if (_visualFeedbackController == null)
        {
            GD.PrintErr("Player: VisualFeedbackController not found!");
        }

        // Get ForcedMovementController
        _movementController = GetNode<ForcedMovementController>("ForcedMovementController");
        if (_movementController == null)
        {
            GD.PrintErr("Player: ForcedMovementController not found!");
        }

        // Initialize SkillEffectController with ForcedMovementController
        _skillEffectController.Initialize(this, _statsManager, _buffManager, _movementController);

        // 🆕 Inject dependencies into managers
        _upgradeManager?.Initialize(this, _statsManager, _buffManager);
        _skillManager?.Initialize(this, _statsManager);
        _animationController?.Initialize(this, _animationPlayer);
        _visualFeedbackController?.Initialize(_sprite, _statsManager);
        _movementController?.Initialize(this);

    }

    public override void _UnhandledInput(InputEvent @event)
    {
        _inputManager?.HandleInputEvent(@event);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Vector2.Zero;

        // Forced movement (dash, leap, etc.) overrides normal movement
        if (_movementController != null && _movementController.UpdateMovement(delta))
        {
            MoveAndSlide();
            return;
        }

        // Allow movement in locomotion states OR while casting with MovementAllowed
        if (_currentState == PlayerState.Idle ||
            _currentState == PlayerState.Running ||
            _isCastingWhileMoving)
        {
            direction = _inputManager?.GetMovementInput() ?? Vector2.Zero;
        }
        else if (_currentState == PlayerState.CastingSkill)
        {
            // Movement locked during cast
            direction = Vector2.Zero;
        }

        if (_statsManager != null)
        {
            Velocity = direction * _statsManager.CurrentSpeed;
        }

        // Update locomotion animations only in pure locomotion states
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Running)
        {
            _animationController?.PlayLocomotion(_currentState, direction);

            if (direction != Vector2.Zero && _currentState != PlayerState.Running)
            {
                ChangeState(PlayerState.Running);
            }
            else if (direction == Vector2.Zero && _currentState != PlayerState.Idle)
            {
                ChangeState(PlayerState.Idle);
            }
        }

        _skillManager?.Update((float)delta);
        MoveAndSlide();
    }

    // ===== PUBLIC API =====
    public void Initialize()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.SaveExists())
        {
            // Existing save game - load it
            SaveData? saveData = SaveManager.Instance.LoadGame();
            if (saveData.HasValue)
            {
                _statsManager?.LoadSaveData(saveData.Value);

                // Restore upgrades if active run
                if (saveData.Value.HasActiveRun)
                {
                    var upgradeManager = GetNode<UpgradeManager>("UpgradeManager");
                    upgradeManager?.LoadActiveUpgrades(saveData.Value.ActiveUpgrades);
                }
            }
        }
        else
        {
            // New game - initialize fresh stats
            _statsManager?.Initialize();
        }
    }

    public void TakeDamage(float damage)
    {
        _statsManager?.TakeDamage(damage);
    }

    public void AddExperience(int amount)
    {
        _statsManager?.AddPowerExperience(amount);
    }

    public Vector2 GetAttackDirection()
    {
        return _animationController?.GetFacingDirection() ?? Vector2.Down;
    }

    public bool TryCastSkill(Skill skill)
    {
        if (_currentState == PlayerState.Dead)
        {
            return false;
        }

        // Store previous state for returning after cast
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Running)
        {
            _previousState = _currentState;
        }

        // Store skill reference
        _currentCastingSkill = skill;
        _skillEffectController?.SetCurrentSkill(skill);

        // Handle movement based on skill's MovementBehavior
        if (_currentState != PlayerState.Idle && _currentState != PlayerState.Running)
        {
            return false;
        }

        ChangeState(PlayerState.CastingSkill);

        // Set movement flags based on behavior
        if (skill.MovementBehavior == MovementBehavior.Allowed)
        {
            _isCastingWhileMoving = true;
        }

        float speedScale = 1.0f;
        if (_statsManager != null)
        {
            if (skill.Category == SkillCategory.Attack)
            {
                speedScale = _statsManager.CurrentAttackRate / _statsManager.BaseAttackRate;
            }
            else // SkillCategory.Spell
            {
                speedScale = _statsManager.CurrentCastSpeed / _statsManager.BaseCastSpeed;
            }
        }

        // Delegate animation playback to AnimationController
        _animationController?.PlaySkillAnimation(skill, speedScale);

        return true;
    }

    public bool TryInstantSkill(Skill skill)
    {
        if (_currentState == PlayerState.Dead)
        {
            return false;
        }

        // Instant skills don't change FSM state (non-interrupting)

        // Check if skill has buff data (data-driven!)

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

            // ✅ NEW: Spawn visual effect if skill has one
            if (skill.SkillEffectScene != null)
            {
                var effect = skill.SkillEffectScene.Instantiate<Node2D>();
                GetParent().AddChild(effect);
                effect.GlobalPosition = GlobalPosition;
            }

            return true;
        }

        // Future: Check for other instant effect types
        // if (skill.HealAmount > 0) { _statsManager.Heal(skill.HealAmount); return true; }
        // if (skill.SummonScene != null) { SpawnSummon(skill.SummonScene); return true; }

        GD.PrintErr($"TryInstantSkill: Skill {skill.SkillId} has no instant effect data!");
        return false;
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        _upgradeManager?.ApplyUpgrade(upgrade);
    }

    public void StartDash(Vector2 direction, float distance, float duration)
    {
        _movementController?.StartDash(direction, distance, duration);
    }

    public void EndDash()
    {
        _movementController?.EndMovement();
    }

    // ===== PRIVATE HELPERS - Event Handlers =====
    private void OnLeveledUp()
    {
        var upgradeOptions = GetRandomUpgrades(3);

        // Emit signal for UI to handle (decoupled from LevelUpPanel)
        var upgradesArray = new Godot.Collections.Array<Upgrade>();
        foreach (var upgrade in upgradeOptions)
        {
            upgradesArray.Add(upgrade);
        }

        EmitSignal(SignalName.ShowLevelUpPanel, upgradesArray);
    }


    // ===== PRIVATE HELPERS - Upgrades =====
    private List<Upgrade> GetRandomUpgrades(int count)
    {
        return _upgradeManager?.GetRandomUpgrades(count);
    }

    private bool CanTransitionTo(PlayerState newState)
    {
        if (_currentState == PlayerState.Dead)
        {
            return false;
        }

        if (newState == PlayerState.Hurt || newState == PlayerState.Dead)
        {
            return true;
        }

        return _currentState switch
        {
            PlayerState.Idle => true,
            PlayerState.Running => true,
            PlayerState.CastingSkill => newState == PlayerState.Idle || newState == PlayerState.Running,
            PlayerState.Hurt => newState == PlayerState.Idle || newState == PlayerState.Running,
            _ => false
        };
    }

    private void ChangeState(PlayerState newState)
    {
        if (!CanTransitionTo(newState))
        {
            return;
        }

        _currentState = newState;
    }

    private void OnAnimationFinished(StringName animName)
    {
        // Clear casting flags
        bool wasCasting = _currentState == PlayerState.CastingSkill || _isCastingWhileMoving;

        if (wasCasting)
        {
            _isCastingWhileMoving = false;
            _currentCastingSkill = null;
            _skillEffectController.ClearCurrentSkill();

            // Return to previous state
            if (_currentState == PlayerState.CastingSkill)
            {
                ChangeState(_previousState);
            }
        }

        _animationPlayer.SpeedScale = 1.0f;
    }

    private void OnSkillPressed(int skillSlot)
    {
        _skillManager?.UseSkill((SkillSlot)skillSlot);
    }

    private void OnStatsManagerHealthChanged(float current, float max)
    {
        EmitSignal(SignalName.HealthChanged, current, max);
    }

    private void OnStatsManagerExperienceChanged(int currentXP, int requiredXP, int level)
    {
        EmitSignal(SignalName.ExperienceChanged, currentXP, requiredXP, level);
    }
}
