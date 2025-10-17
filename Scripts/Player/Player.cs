using System.Collections.Generic;
using Godot;
using SpaceTower.Scripts.Progression.Upgrades;
using SpaceTower.Scripts.PlayerScripts.Components;
using SpaceTower.Scripts.UI.Panels;
using SpaceTower.Scripts.Core;

namespace SpaceTower.Scripts.PlayerScripts;

public enum PlayerState
{
    Idle,
    Running,
    BasicAttacking,
    CastingSkill,
    Hurt,
    Dead
}

public partial class Player : CharacterBody2D
{
    // ===== EXPORT FIELDS - Config =====
    [Export] public PlayerClass CurrentClass = PlayerClass.Warrior;

    // ===== EXPORT FIELDS - Scenes =====
    [Export] public PackedScene ProjectileScene;
    [Export] public PackedScene MeleeAttackScene;

    // ===== EXPORT FIELDS - UI Dependencies =====
    [Export] public LevelUpPanel LevelUpPanel;
    [Export] public StatAllocationPanel StatAllocationPanel;

    // ===== SIGNALS =====
    [Signal] public delegate void HealthChangedEventHandler(float currentHealth, float maxHealth);
    [Signal] public delegate void ExperienceChangedEventHandler(int currentXP, int requiredXP, int level);
    [Signal] public delegate void ResourcesChangedEventHandler(int gold, int cores, int components, int fragments);
    [Signal] public delegate void FloorInfoChangedEventHandler(int floorNumber, string floorName);
    [Signal] public delegate void WaveInfoChangedEventHandler(int waveNumber, int enemiesRemaining);

    // ===== PRIVATE FIELDS - Components =====
    private SkillManager _skillManager;
    private StatsManager _statsManager;
    private UpgradeManager _upgradeManager;
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;
    private Vector2 _lastDirection = Vector2.Down;
    private PlayerState _currentState = PlayerState.Idle;

    // ===== LIFECYCLE METHODS =====
    public override void _Ready()
    {
        AddToGroup("player");

        if (ProjectileScene == null)
        {
            GD.PrintErr("Player: ProjectileScene not assigned!");
        }

        // Get components
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

        // Connect to LevelUpPanel
        if (LevelUpPanel != null)
        {
            LevelUpPanel.UpgradeSelected += HandleUpgradeSelection;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        _skillManager?.HandleInput(@event);

        // Open stat allocation panel with 'C' key
        if (@event.IsActionPressed("open_stats"))
        {
            StatAllocationPanel?.ShowPanel(_statsManager, _upgradeManager);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Vector2.Zero;

        // Only allow movement input in locomotion states
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Running)
        {
            direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        }
        else if (_currentState == PlayerState.CastingSkill)
        {
            // Block movement during skill cast
            direction = Vector2.Zero;
        }
        else if (_currentState == PlayerState.BasicAttacking)
        {
            // Allow movement during basic attack for responsiveness
            direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        }

        if (_statsManager != null)
        {
            Velocity = direction * _statsManager.CurrentSpeed;
        }

        // Update locomotion animations only in locomotion states
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Running)
        {
            if (direction != Vector2.Zero)
            {
                _lastDirection = direction;
                PlayWalkAnimation(direction);

                if (_currentState != PlayerState.Running)
                {
                    ChangeState(PlayerState.Running);
                }
            }
            else
            {
                PlayIdleAnimation(_lastDirection);

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

                GD.Print("Save data loaded successfully!");
            }
        }
        else
        {
            // New game - initialize fresh stats
            _statsManager?.Initialize();
            GD.Print("Initialized fresh character stats");
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

    public bool TryBasicAttack()
    {
        if (!CanTransitionTo(PlayerState.BasicAttacking))
        {
            return false;
        }

        ChangeState(PlayerState.BasicAttacking);

        string attackAnim = GetDirectionalAnimationName("warrior_attack", _lastDirection);
        _animationPlayer.Play(attackAnim);

        return true;
    }

    public bool TryCastSkill(string skillName)
    {
        if (!CanTransitionTo(PlayerState.CastingSkill))
        {
            return false;
        }

        ChangeState(PlayerState.CastingSkill);

        // For now, we'll handle specific skill animations
        if (skillName == "Whirlwind")
        {
            _animationPlayer.Play("warrior_whirlwind");
        }

        return true;
    }

    // ===== PRIVATE HELPERS - Event Handlers =====
    private void OnLeveledUp()
    {
        if (LevelUpPanel != null)
        {
            var upgradeOptions = GetRandomUpgrades(3);

            LevelUpPanel.ShowUpgrades(upgradeOptions);
        }
    }

    private void HandleUpgradeSelection(Upgrade upgrade)
    {
        _upgradeManager?.ApplyUpgrade(upgrade);
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
            PlayerState.BasicAttacking => newState == PlayerState.Idle || newState == PlayerState.Running,
            PlayerState.CastingSkill => newState == PlayerState.Idle || newState == PlayerState.Running,
            PlayerState.Hurt => newState == PlayerState.Idle || newState == PlayerState.Running,
            _ => false
        };
    }

    private void ChangeState(PlayerState newState)
    {
        if (!CanTransitionTo(newState))
        {
            GD.Print($"Cannot transition from {_currentState} to {newState}");
            return;
        }

        GD.Print($"State: {_currentState} → {newState}");
        _currentState = newState;
    }

    private void OnAnimationFinished(StringName animName)
    {
        string anim = animName.ToString();

        // Return to locomotion state after attacks/skills
        if (anim.StartsWith("warrior_attack") || anim.StartsWith("warrior_whirlwind"))
        {
            // Check if player is moving to determine next state
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

            if (direction != Vector2.Zero)
            {
                ChangeState(PlayerState.Running);
            }
            else
            {
                ChangeState(PlayerState.Idle);
            }
        }
    }

}
