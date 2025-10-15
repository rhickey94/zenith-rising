using Godot;
using SpaceTower.Scripts.Core;

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

public enum StatType
{
    Strength,
    Intelligence,
    Agility,
    Vitality,
    Fortune
}

[GlobalClass]
public partial class StatsManager : Node
{
    // Base stats (set in editor)
    [Export] public float BaseMaxHealth { get; set; } = 100.0f;
    [Export] public float BaseSpeed { get; set; } = 300.0f;
    [Export] public float BaseFireRate { get; set; } = 0.2f;
    [Export] public float BaseMeleeRate { get; set; } = 0.5f;

    // ===== PERMANENT CHARACTER PROGRESSION (save these) =====
    [ExportGroup("Character Progression")]
    [Export] public int CharacterLevel { get; private set; } = 1;
    [Export] public int UnallocatedStatPoints { get; private set; } = 15;
    [Export] public int Strength { get; private set; } = 0;       // STR: +3% Physical Dmg, +10 HP
    [Export] public int Intelligence { get; private set; } = 0;   // INT: +3% Magical Dmg, +2% CDR
    [Export] public int Agility { get; private set; } = 0;        // AGI: +2% Attack Speed, +1% Crit
    [Export] public int Vitality { get; private set; } = 0;       // VIT: +25 HP, +0.5 HP/sec
    [Export] public int Fortune { get; private set; } = 0;        // FOR: +2% Crit Dmg, +1% Drop Rate
    [Export] public int HighestFloorReached { get; private set; } = 0;

    // Calculated stats (modified by upgrades)
    public float MaxHealth { get; private set; } = 100.0f;
    public float Health { get; private set; } = 100.0f;
    public float Speed { get; private set; }
    public float FireRate { get; private set; }
    public float MeleeRate { get; private set; }

    // Combat stats (cached from upgrades)
    public float DamageMultiplier { get; private set; } = 1.0f;
    public float PhysicalDamageMultiplier { get; private set; } = 1.0f;
    public float MagicalDamageMultiplier { get; private set; } = 1.0f;
    public float CritDamageMultiplier { get; private set; } = 1.5f;
    public float CritChance { get; private set; } = 0f;
    public float PickupRadius { get; private set; } = 80f;
    public float HealthRegenPerSecond { get; private set; } = 0f;
    public int ProjectilePierceCount { get; private set; } = 0;

    // Progression
    [Export] public int RunLevel { get; private set; } = 1;
    [Export] public int Experience { get; private set; } = 0;
    [Export] public int ExperienceToNextLevel { get; private set; } = 100;

    // Event for level up (Player will listen)
    [Signal] public delegate void LeveledUpEventHandler();
    [Signal] public delegate void CharacterLeveledUpEventHandler();
    [Signal] public delegate void StatAllocatedEventHandler(int statType);

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
        // Calculate health percentage BEFORE changing MaxHealth
        float healthPercentage = MaxHealth > 0 ? Health / MaxHealth : 1f;

        // Apply upgrade bonuses to base values
        Speed = (BaseSpeed + modifiers.BaseSpeedBonus) * (1 + modifiers.MovementSpeedBonus);

        // Attack speed: AGI bonus + upgrade bonuses
        float agiSpeedBonus = Agility * 0.02f;
        float totalAttackSpeedBonus = modifiers.AttackSpeedBonus + agiSpeedBonus;
        FireRate = BaseFireRate * (1 - totalAttackSpeedBonus);
        MeleeRate = BaseMeleeRate * (1 - totalAttackSpeedBonus);

        // Max Health: Base + Run level + Upgrades + Character stats (VIT + STR)
        float characterHealthBonus = (Vitality * 25f) + (Strength * 10f);
        MaxHealth = BaseMaxHealth + CalculateRunLevelHealthBonus() + modifiers.MaxHealthBonus + characterHealthBonus;

        // Damage: Upgrades + Character stats (STR/INT)
        DamageMultiplier = 1.0f + modifiers.DamagePercentBonus;
        PhysicalDamageMultiplier = 1.0f + (Strength * 0.03f);
        MagicalDamageMultiplier = 1.0f + (Intelligence * 0.03f);

        // Crit: Upgrades + Character stats (AGI + FOR)
        float characterCritChance = Mathf.Min(Agility * 0.01f, 0.5f); // Cap at 50%
        CritChance = Mathf.Clamp(modifiers.CritChanceBonus + characterCritChance, 0f, 1f);
        CritDamageMultiplier = 1.5f + (Fortune * 0.02f); // Base 150% + FOR bonus

        // Regen: Upgrades + Character stats (VIT)
        float characterRegen = Vitality * 0.5f;
        HealthRegenPerSecond = modifiers.HealthRegenBonus + characterRegen;

        PickupRadius = 80f + modifiers.PickupRadiusBonus;
        ProjectilePierceCount = modifiers.ProjectilePierceBonus;

        // Maintain health percentage
        Health = MaxHealth * healthPercentage;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }

        EmitHealthUpdate();
    }

    public bool CanAllocateStat()
    {
        return UnallocatedStatPoints > 0;
    }

    public void AllocateStat(StatType statType, int amount = 1)
    {
        if (!CanAllocateStat())
        {
            GD.PrintErr("StatsManager: No unallocated stat points!");
            return;
        }

        if (amount > UnallocatedStatPoints)
        {
            GD.PrintErr($"StatsManager: Trying to allocate {amount} points but only {UnallocatedStatPoints} available!");
            return;
        }

        switch (statType)
        {
            case StatType.Strength:
                Strength += amount;
                break;
            case StatType.Intelligence:
                Intelligence += amount;
                break;
            case StatType.Agility:
                Agility += amount;
                break;
            case StatType.Vitality:
                Vitality += amount;
                break;
            case StatType.Fortune:
                Fortune += amount;
                break;
        }

        UnallocatedStatPoints -= amount;

        GD.Print($"Allocated {amount} point(s) to {statType}. {UnallocatedStatPoints} points remaining.");

        EmitSignal(SignalName.StatAllocated, (int)statType);
    }

    public void AddCharacterLevel(int levels = 1)
    {
        CharacterLevel += levels;
        UnallocatedStatPoints += levels;

        GD.Print($"Character leveled up to {CharacterLevel}! Gained {levels} stat point(s).");

        EmitSignal(SignalName.CharacterLeveledUp);
    }

    public void UpdateHighestFloor(int floor)
    {
        if (floor > HighestFloorReached)
        {
            HighestFloorReached = floor;
            GD.Print($"New highest floor reached: {HighestFloorReached}");
        }
    }

    private float CalculateRunLevelHealthBonus()
    {
        // +20 HP per level (Level 1 = 0 bonus, Level 2 = 20, Level 3 = 40, etc.)
        return 20f * (RunLevel - 1);
    }

    private void LevelUp()
    {
        Experience -= ExperienceToNextLevel;
        RunLevel++;
        ExperienceToNextLevel = (int)(ExperienceToNextLevel * 1.5);

        // Recalculate MaxHealth with new run level (UpgradeManager will call RecalculateStats, but update now for heal)
        float characterHealthBonus = (Vitality * 25f) + (Strength * 10f);
        MaxHealth = BaseMaxHealth + CalculateRunLevelHealthBonus() + characterHealthBonus;
        Health = MaxHealth;  // Full heal on level up

        GD.Print($"Run leveled up to {RunLevel}!");

        EmitHealthUpdate();
        EmitExperienceUpdate();

        EmitSignal(SignalName.LeveledUp);
    }

    private void Die()
    {
        GD.Print("Player died!");

        // Find Game node and notify it
        var game = GetTree().Root.GetNode<Game>("Game");
        if (game != null)
        {
            game.OnPlayerDeath();
        }
        else
        {
            GD.PrintErr("Could not find Game node!");
            GetTree().ReloadCurrentScene(); // Fallback
        }
    }

    private void EmitHealthUpdate()
    {
        _player.EmitSignal(Player.SignalName.HealthChanged, Health, MaxHealth);
    }

    private void EmitExperienceUpdate()
    {
        _player.EmitSignal(Player.SignalName.ExperienceChanged, Experience, ExperienceToNextLevel, RunLevel);
    }
}
