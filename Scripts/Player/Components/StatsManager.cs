using System;
using Godot;

namespace SpaceTower.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class StatsManager : Node
{
    // Base stats (set in editor)
    [Export] public float BaseMaxHealth { get; set; } = 100.0f;
    [Export] public float BaseSpeed { get; set; } = 300.0f;
    [Export] public float BaseFireRate { get; set; } = 0.2f;
    [Export] public float BaseMeleeRate { get; set; } = 0.5f;

    // Calculated stats (modified by upgrades)
    public float MaxHealth { get; private set; } = 100.0f;
    public float Health { get; private set; } = 100.0f;
    public float Speed { get; private set; }
    public float FireRate { get; private set; }
    public float MeleeRate { get; private set; }

    // Progression
    [Export] public int Level { get; private set; } = 1;
    [Export] public int Experience { get; private set; } = 0;
    [Export] public int ExperienceToNextLevel { get; private set; } = 100;

    // Event for level up (Player will listen)
    public event Action LeveledUp;

    private Player _player;

    public override void _Ready()
    {
        _player = GetParent<Player>();
        if (_player == null)
        {
            GD.PrintErr("StatsManager: Could not find Player parent!");
        }

        MaxHealth = BaseMaxHealth;
        Health = MaxHealth;
    }

    public void Initialize()
    {
        // Set initial stat values (no upgrades yet)
        RecalculateStats(0f, 0f, 0f, 0f);

        EmitHealthUpdate();
        EmitExperienceUpdate();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }

        EmitHealthUpdate();

        if (Health <= 0)
        {
            Die();
        }
    }

    public void AddExperience(int amount)
    {
        Experience += amount;

        while (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }

        EmitExperienceUpdate();
    }

    public void RecalculateStats(float movementSpeedBonus, float attackSpeedBonus, float maxHealthBonus, float baseSpeedBonus)
    {
        GD.Print($"RecalculateStats called: maxHealthBonus={maxHealthBonus}, Level={Level}"); // ← ADD THIS
        // Apply upgrade bonuses to base values
        Speed = (BaseSpeed + baseSpeedBonus) * (1 + movementSpeedBonus);
        FireRate = BaseFireRate * (1 - attackSpeedBonus);
        MeleeRate = BaseMeleeRate * (1 - attackSpeedBonus);
        MaxHealth = BaseMaxHealth + maxHealthBonus + CalculateLevelHealthBonus();

        GD.Print($"New MaxHealth: {MaxHealth}"); // ← ADD THIS

        // Don't reduce current health, only increase max if needed
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }

        EmitHealthUpdate();
    }

    private float CalculateLevelHealthBonus()
    {
        // +20 HP per level (Level 1 = 0 bonus, Level 2 = 20, Level 3 = 40, etc.)
        return 20f * (Level - 1);
    }

    private void LevelUp()
    {
        Experience -= ExperienceToNextLevel;
        Level++;
        ExperienceToNextLevel = (int)(ExperienceToNextLevel * 1.5);

        // Update MaxHealth immediately with level bonus (upgrade bonuses added when RecalculateStats is called)
        MaxHealth = BaseMaxHealth + CalculateLevelHealthBonus();
        Health = MaxHealth;  // Full heal on level up

        GD.Print($"Leveled up to {Level}!");

        EmitHealthUpdate();
        EmitExperienceUpdate();

        // Notify Player that level up occurred (this will trigger upgrade panel)
        LeveledUp?.Invoke();
    }

    private void Die()
    {
        GD.Print("Player died!");
        GetTree().ReloadCurrentScene();
    }

    private void EmitHealthUpdate()
    {
        _player.EmitSignal(Player.SignalName.HealthChanged, Health, MaxHealth);
    }

    private void EmitExperienceUpdate()
    {
        _player.EmitSignal(Player.SignalName.ExperienceChanged, Experience, ExperienceToNextLevel, Level);
    }
}
