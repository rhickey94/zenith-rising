using System.Collections.Generic;
using Godot;
using SpaceTower.Progression.Upgrades;
using SpaceTower.Scripts.PlayerScripts.Components;
using SpaceTower.Scripts.Skills.Effects;
using SpaceTower.Scripts.UI.Menus;

namespace SpaceTower.Scripts.PlayerScripts;

public partial class Player : CharacterBody2D
{
    // Stats

    [Export] public float Speed = 300.0f;
    [Export] public float FireRate = 0.2f;
    [Export] public float MeleeRate = 0.5f;

    [Export] public PlayerClass CurrentClass = PlayerClass.Warrior;

    // Scenes

    [Export] public PackedScene ProjectileScene;
    [Export] public PackedScene MeleeAttackScene;

    // UI Dependencies

    [Export] public LevelUpPanel LevelUpPanel;

    // Signals for HUD updates

    [Signal] public delegate void HealthChangedEventHandler(float currentHealth, float maxHealth);
    [Signal] public delegate void ExperienceChangedEventHandler(int currentXP, int requiredXP, int level);
    [Signal] public delegate void ResourcesChangedEventHandler(int gold, int cores, int components, int fragments);
    [Signal] public delegate void FloorInfoChangedEventHandler(int floorNumber, string floorName);
    [Signal] public delegate void WaveInfoChangedEventHandler(int waveNumber, int enemiesRemaining);

    private SkillManager _skillManager;
    private StatsManager _statsManager;
    private UpgradeManager _upgradeManager;

    public override void _Ready()
    {
        AddToGroup("player");

        if (ProjectileScene == null)
        {
            GD.PrintErr("Player: ProjectileScene not assigned!");
        }

        // Get SkillManager component

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

        // Connect to LevelUpPanel

        if (LevelUpPanel != null)
        {
            LevelUpPanel.UpgradeSelected += HandleUpgradeSelection;
        }
    }

    public void Initialize()
    {
        _statsManager?.Initialize();

        EmitResourcesUpdate(0, 0, 0, 0);
        EmitFloorInfoUpdate(1, "Initialization");
        EmitWaveInfoUpdate(1, 0);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        _skillManager?.HandleInput(@event);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Velocity = direction * Speed;

        if (direction != Vector2.Zero)
        {
            Rotation = direction.Angle();
        }

        // Update skill cooldowns
        _skillManager?.Update((float)delta);

        MoveAndSlide();
    }

    public void TakeDamage(float damage)
    {
        _statsManager?.TakeDamage(damage);
    }

    public void AddExperience(int amount)
    {
        _statsManager?.AddExperience(amount);
    }

    private void OnLeveledUp()
    {
        GD.Print("OnLeveledUp called!"); // DEBUG

        GD.Print($"LevelUpPanel is null: {LevelUpPanel == null}"); // DEBUG


        if (LevelUpPanel != null)
        {
            var upgradeOptions = GetRandomUpgrades(3);
            GD.Print($"Got {upgradeOptions.Count} upgrade options"); // DEBUG

            LevelUpPanel.ShowUpgrades(upgradeOptions);
        }
    }

    private void HandleUpgradeSelection(Upgrade upgrade)
    {
        _upgradeManager?.ApplyUpgrade(upgrade);
    }

    private List<Upgrade> GetRandomUpgrades(int count)
    {
        return _upgradeManager?.GetRandomUpgrades(count);
    }

    public float GetUpgradeValue(UpgradeType type)
    {
        return _upgradeManager?.GetUpgradeValue(type) ?? 0f;
    }

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
}
