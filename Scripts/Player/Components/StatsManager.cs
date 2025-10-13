using System;
using Godot;

namespace SpaceTower.Scripts.PlayerScripts.Components;

public struct StatModifiers
{
    public float MovementSpeedBonus; // Percentage increase (e.g., 0.1 for +10%)
    public float AttackSpeedBonus;   // Percentage decrease in attack interval (e.g., 0.1 for -10%)
    public float MaxHealthBonus;     // Flat increase in max health
    public float BaseSpeedBonus;     // Flat increase in base speed
    public float DamagePercentBonus;
    public float CritChanceBonus;
    public float PickupRadiusBonus;
    public float HealthRegenBonus;
    public int ProjectilePierceBonus;
}

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
    // Combat stats (cached from upgrades)
    public float DamageMultiplier { get; private set; } = 1.0f;
    public float CritChance { get; private set; } = 0f;
    public float PickupRadius { get; private set; } = 80f;
    public float HealthRegenPerSecond { get; private set; } = 0f;
    public int ProjectilePierceCount { get; private set; } = 0;

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

    public override void _Process(double delta)
    {
        if (Health < MaxHealth && HealthRegenPerSecond > 0)
        {
            Health += HealthRegenPerSecond * (float)delta;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            EmitHealthUpdate();
        }
    }

    public void Initialize()
    {

        // Set initial stat values (no upgrades yet)
        RecalculateStats(new StatModifiers());

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

    public void RecalculateStats(StatModifiers modifiers)
    {
        GD.Print($"RecalculateStats called: maxHealthBonus={modifiers.MaxHealthBonus}, Level={Level}");

        // Calculate health percentage BEFORE changing MaxHealth
        float healthPercentage = MaxHealth > 0 ? Health / MaxHealth : 1f;

        // Apply upgrade bonuses to base values
        Speed = (BaseSpeed + modifiers.BaseSpeedBonus) * (1 + modifiers.MovementSpeedBonus);
        FireRate = BaseFireRate * (1 - modifiers.AttackSpeedBonus);
        MeleeRate = BaseMeleeRate * (1 - modifiers.AttackSpeedBonus);
        MaxHealth = BaseMaxHealth + modifiers.MaxHealthBonus + CalculateLevelHealthBonus();
        DamageMultiplier = 1.0f + modifiers.DamagePercentBonus;
        CritChance = Mathf.Clamp(modifiers.CritChanceBonus, 0f, 1f);
        PickupRadius = 80f + modifiers.PickupRadiusBonus;
        HealthRegenPerSecond = modifiers.HealthRegenBonus;
        ProjectilePierceCount = modifiers.ProjectilePierceBonus;

        GD.Print($"New MaxHealth: {MaxHealth}");

        // Maintain health percentage
        Health = MaxHealth * healthPercentage;

        // Ensure we don't exceed max (safety check)
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
