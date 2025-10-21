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
    [Signal] public delegate void ResourcesChangedEventHandler(int gold, int cores, int components, int fragments);
    [Signal] public delegate void FloorInfoChangedEventHandler(int floorNumber, string floorName);
    [Signal] public delegate void WaveInfoChangedEventHandler(int waveNumber, int enemiesRemaining);
    [Signal] public delegate void ShowLevelUpPanelEventHandler(Godot.Collections.Array<Upgrade> upgrades);

    // ===== PRIVATE FIELDS - Components =====
    private InputManager _inputManager;
    private SkillManager _skillManager;
    private StatsManager _statsManager;
    private BuffManager _buffManager;
    private UpgradeManager _upgradeManager;
    private SkillAnimationController _skillAnimationController;
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;
    private Vector2 _lastDirection = Vector2.Down;
    private PlayerState _currentState = PlayerState.Idle;
    private Skill _currentCastingSkill;
    private bool _isCastingWhileMoving = false;
    private bool _movementControlledBySkill = false;
    private PlayerState _previousState = PlayerState.Idle;
    private Tween _invincibilityTween;

    // Dash state
    private bool _isDashing = false;
    private Vector2 _dashStartPos = Vector2.Zero;
    private Vector2 _dashEndPos = Vector2.Zero;
    private float _dashElapsed = 0f;
    private float _dashDuration = 0f;

    public Vector2 GetLastDirection() => _lastDirection;

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
        _skillAnimationController = GetNode<SkillAnimationController>("SkillAnimationController");
        if (_skillAnimationController == null)
        {
            GD.PrintErr("Player: SkillAnimationController not found!");
        }
        else
        {
            _skillAnimationController.Initialize(this, _statsManager, _animationPlayer, _buffManager);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        _inputManager?.HandleInputEvent(@event);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_statsManager != null && _sprite != null)
        {
            if (_statsManager.IsInvincible && _invincibilityTween == null)
            {
                // Start flashing
                _invincibilityTween = CreateTween();
                _invincibilityTween.SetLoops();
                _invincibilityTween.TweenProperty(_sprite, "modulate:a", 0.5, 0.1);
                _invincibilityTween.TweenProperty(_sprite, "modulate:a", 1.0, 0.1);
            }
            else if (!_statsManager.IsInvincible && _invincibilityTween != null)
            {
                // Stop flashing
                _invincibilityTween.Kill();
                _invincibilityTween = null;
                _sprite.Modulate = new Color(1, 1, 1, 1); // Reset to full opacity
            }
        }

        Vector2 direction = Vector2.Zero;

        // Code-driven dash movement overrides normal movement
        if (_isDashing)
        {
            _dashElapsed += (float)delta;
            float t = Mathf.Clamp(_dashElapsed / _dashDuration, 0f, 1f);

            // Ease-out curve for smooth deceleration
            t = (float)Mathf.Ease(t, -2.0);

            // Interpolate position
            GlobalPosition = _dashStartPos.Lerp(_dashEndPos, t);

            // Check if dash completed
            if (t >= 1.0f)
            {
                _isDashing = false;
            }

            // Skip normal movement logic
            MoveAndSlide();
            return;
        }

        // Skip manual movement if skill controls it (other forced movement skills)
        if (_movementControlledBySkill)
        {
            // For other skills that might need forced movement in the future
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
            if (direction != Vector2.Zero)
            {
                _lastDirection = direction;
                PlayWalkAnimation(GetMouseDirection());

                if (_currentState != PlayerState.Running)
                {
                    ChangeState(PlayerState.Running);
                }
            }
            else
            {
                PlayIdleAnimation(GetMouseDirection());

                if (_currentState != PlayerState.Idle)
                {
                    ChangeState(PlayerState.Idle);
                }
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

        EmitResourcesUpdate(0, 0, 0, 0);
        EmitFloorInfoUpdate(1, "Initialization");
        EmitWaveInfoUpdate(1, 0);
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
        return GetMouseDirection();
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
        _skillAnimationController?.SetCurrentSkill(skill);

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
        else if (skill.MovementBehavior == MovementBehavior.Forced)
        {
            _movementControlledBySkill = true;
        }

        // Play animation with speed scaling based on skill category
        string animName = GetSkillAnimationName(skill);
        _animationPlayer.Play(animName);

        // Apply animation speed based on skill category
        if (_statsManager != null)
        {
            if (skill.Category == SkillCategory.Attack)
            {
                // Attacks: scale with attack speed
                float attackSpeedRatio = _statsManager.CurrentAttackRate / _statsManager.BaseAttackRate;
                _animationPlayer.SpeedScale = attackSpeedRatio;
            }
            else // SkillCategory.Spell
            {
                // Spells: scale with cast speed
                float castSpeedRatio = _statsManager.CurrentCastSpeed / _statsManager.BaseCastSpeed;
                _animationPlayer.SpeedScale = castSpeedRatio;
            }
        }

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
            GD.Print($"[DEBUG] TryInstantSkill: Applying buff for skill {skill.SkillId}");  // ← Add this

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
                GD.Print($"[DEBUG] Spawning effect: {skill.SkillEffectScene.ResourcePath}");  // ← Add this

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

    /// <summary>
    /// Starts a dash movement in the specified direction.
    /// Called by SkillAnimationController via animation callback.
    /// </summary>
    public void StartDash(Vector2 direction, float distance, float duration)
    {
        _isDashing = true;
        _dashStartPos = GlobalPosition;
        _dashEndPos = GlobalPosition + (direction.Normalized() * distance);
        _dashDuration = duration;
        _dashElapsed = 0f;
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        _upgradeManager?.ApplyUpgrade(upgrade);
    }

    /// <summary>
    /// Forcibly ends the dash movement.
    /// Called by SkillAnimationController via animation callback.
    /// </summary>
    public void EndDash()
    {
        if (_isDashing)
        {
            _isDashing = false;
        }
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

    // ===== PRIVATE HELPERS - Signal Emission =====
    private void EmitResourcesUpdate(int gold, int cores, int components, int fragments)
    {
        EmitSignal(SignalName.ResourcesChanged, gold, cores, components, fragments);
    }

    private void EmitFloorInfoUpdate(int floorNumber, string floorName)
    {
        EmitSignal(SignalName.FloorInfoChanged, floorNumber, floorName);
    }

    private void EmitWaveInfoUpdate(int waveNumber, int enemiesRemaining)
    {
        EmitSignal(SignalName.WaveInfoChanged, waveNumber, enemiesRemaining);
    }

    // ===== PRIVATE HELPERS - Animation =====
    private Vector2 GetMouseDirection()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        Vector2 direction = mousePosition - GlobalPosition;

        // Guard against mouse exactly on player
        if (direction.LengthSquared() < 0.01f)
        {
            return _lastDirection; // Fallback to movement direction
        }

        return direction.Normalized();
    }

    private void PlayWalkAnimation(Vector2 direction)
    {
        if (_animationPlayer == null)
        {
            return;
        }

        string animName = GetDirectionalAnimationName("walk", direction);

        if (_animationPlayer.CurrentAnimation != animName)
        {
            _animationPlayer.Play(animName);
        }
    }

    private void PlayIdleAnimation(Vector2 direction)
    {
        if (_animationPlayer == null)
        {
            return;
        }

        string animName = GetDirectionalAnimationName("idle", direction);

        if (_animationPlayer.CurrentAnimation != animName)
        {
            _animationPlayer.Play(animName);
        }
    }

    private string GetDirectionalAnimationName(string baseAnimation, Vector2 direction)
    {
        string directionSuffix;

        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            directionSuffix = direction.X > 0 ? "right" : "left";
        }
        else
        {
            directionSuffix = direction.Y > 0 ? "down" : "up";
        }

        return $"{baseAnimation}_{directionSuffix}";
    }

    private string GetSkillAnimationName(Skill skill)
    {
        if (string.IsNullOrEmpty(skill.AnimationBaseName))
        {
            GD.PrintErr($"No AnimationBaseName for skill: {skill.SkillId}");
            return "idle_down";
        }

        return skill.UsesDirectionalAnimation
            ? GetDirectionalAnimationName(skill.AnimationBaseName, GetMouseDirection())
            : skill.AnimationBaseName;
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
            _movementControlledBySkill = false;
            _currentCastingSkill = null;
            _skillAnimationController.ClearCurrentSkill();

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
}
