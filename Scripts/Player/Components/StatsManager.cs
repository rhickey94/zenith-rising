using System;
using Godot;
using ZenithRising.Scripts.Core;

namespace ZenithRising.Scripts.PlayerScripts.Components;

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
    // ===== PRIVATE BACKING FIELDS =====
    private Player _player;
    private int _strength = 0;
    private int _intelligence = 0;
    private int _agility = 0;
    private int _vitality = 0;
    private int _fortune = 0;

    // ===== EXPORT PROPERTIES - Base Stats =====
    [Export] public float BaseMaxHealth { get; set; } = 100f;
    [Export] public float BaseSpeed { get; set; } = 300f;
    [Export] public float BaseFireRate { get; set; } = 0.2f;
    [Export] public float BaseMeleeRate { get; set; }

    // ===== EXPORT PROPERTIES - Character Progression =====
    [ExportGroup("Character Progression")]
    [Export] public int CharacterLevel { get; private set; } = 1;
    [Export] public int CharacterExperience { get; private set; } = 0;
    [Export] public int CharacterExperienceToNextLevel { get; private set; } = 500;
    [Export] public int UnallocatedStatPoints { get; private set; } = 15;
    [Export] public int Strength { get => _strength; private set => _strength = Mathf.Max(0, value); }       // STR: +3% Physical Dmg, +10 HP
    [Export] public int Intelligence { get => _intelligence; private set => _intelligence = Mathf.Max(0, value); }   // INT: +3% Magical Dmg, +2% CDR
    [Export] public int Agility { get => _agility; private set => _agility = Mathf.Max(0, value); }        // AGI: +2% Attack Speed, +1% Crit
    [Export] public int Vitality { get => _vitality; private set => _vitality = Mathf.Max(0, value); }       // VIT: +25 HP, +0.5 HP/sec
    [Export] public int Fortune { get => _fortune; private set => _fortune = Mathf.Max(0, value); }        // FOR: +2% Crit Dmg, +1% Drop Rate
    [Export] public int HighestFloorReached { get; private set; } = 0;

    // ===== EXPORT PROPERTIES - Run Progression =====
    [ExportGroup("Run Progression")]
    [Export] public int PowerLevel { get; set; } = 1;
    [Export] public int PowerExperience { get; set; } = 0;
    [Export] public int PowerExperienceToNextLevel { get; private set; } = 100;

    // ===== PUBLIC PROPERTIES - Calculated Stats =====
    public float CurrentMaxHealth { get; private set; } = 100.0f;
    public float CurrentHealth { get; private set; } = 100.0f;
    public float CurrentSpeed { get; private set; }
    public float CurrentFireRate { get; private set; }
    public float CurrentMeleeRate { get; private set; }

    // ===== PUBLIC PROPERTIES - Combat Stats =====
    public float DamageMultiplier { get; private set; } = 1.0f;
    public float PhysicalDamageMultiplier { get; private set; } = 1.0f;
    public float MagicalDamageMultiplier { get; private set; } = 1.0f;
    public float CritDamageMultiplier { get; private set; } = 1.5f;
    public float CritChance { get; private set; } = 0f;
    public float PickupRadius { get; private set; } = 80f;
    public float HealthRegenPerSecond { get; private set; } = 0f;
    public int ProjectilePierceCount { get; private set; } = 0;

    // ===== SIGNALS =====
    [Signal] public delegate void LeveledUpEventHandler();
    [Signal] public delegate void CharacterLeveledUpEventHandler();
    [Signal] public delegate void StatAllocatedEventHandler(int statType);

    // ===== LIFECYCLE METHODS =====
    public override void _Ready()
    {
        _player = GetParent<Player>();
        if (_player == null)
        {
            GD.PrintErr("StatsManager: Could not find Player parent!");
        }

        // Initialize base stats from config (allows inspector overrides if non-zero)
        if (GameBalance.Instance != null && GameBalance.Instance.Config != null)
        {
            var config = GameBalance.Instance.Config.PlayerStats;

            // Only override if not set in inspector (check against defaults)
            if (BaseMaxHealth == 100.0f)
            {
                BaseMaxHealth = config.BaseMaxHealth;
            }

            if (BaseSpeed == 300.0f)
            {
                BaseSpeed = config.BaseSpeed;
            }

            if (BaseFireRate == 0.2f)
            {
                BaseFireRate = config.BaseFireRate;
            }

            if (BaseMeleeRate == 0.5f)
            {
                BaseMeleeRate = config.BaseMeleeRate;
            }
        }
        else
        {
            GD.PrintErr("StatsManager: GameBalance not initialized! Using default values.");
        }

        CurrentMaxHealth = BaseMaxHealth;
        CurrentHealth = CurrentMaxHealth;
    }

    public override void _Process(double delta)
    {
        if (CurrentHealth < CurrentMaxHealth && HealthRegenPerSecond > 0)
        {
            CurrentHealth += HealthRegenPerSecond * (float)delta;
            if (CurrentHealth > CurrentMaxHealth)
            {
                CurrentHealth = CurrentMaxHealth;
            }

            EmitHealthUpdate();
        }
    }

    // ===== PUBLIC API - Initialization =====
    public void Initialize()
    {
        // Set initial stat values (no upgrades yet)
        RecalculateStats(new StatModifiers());

        EmitHealthUpdate();
        EmitExperienceUpdate();
    }

    // ===== PUBLIC API - Combat =====
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        EmitHealthUpdate();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // ===== PUBLIC API - Progression =====
    public void AddPowerExperience(int amount)
    {
        PowerExperience += amount;

        while (PowerExperience >= PowerExperienceToNextLevel)
        {
            PowerLevelUp();
        }

        EmitExperienceUpdate();
    }

    public void AddCharacterExperience(int amount)
    {
        CharacterExperience += amount;

        // Handle level ups (support multiple levels in one award)
        while (CharacterExperience >= CharacterExperienceToNextLevel)
        {
            AddCharacterLevel(1);
        }
    }

    public void AddCharacterLevel(int levels = 1)
    {
        CharacterExperience -= CharacterExperienceToNextLevel;
        CharacterLevel += levels;
        UnallocatedStatPoints += levels;

        GD.Print($"Character leveled up to {CharacterLevel}! Gained {levels} stat point(s).");
        CharacterExperienceToNextLevel = CalculateCharacterXPForNextLevel();

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

    // ===== PUBLIC API - Stats =====
    public void RecalculateStats(StatModifiers modifiers)
    {
        // Calculate health percentage BEFORE changing MaxHealth
        float healthPercentage = CurrentMaxHealth > 0 ? CurrentHealth / CurrentMaxHealth : 1f;
        var config = GameBalance.Instance.Config.CharacterProgression;

        // Apply upgrade bonuses to base values
        CurrentSpeed = (BaseSpeed + modifiers.BaseSpeedBonus) * (1 + modifiers.MovementSpeedBonus);

        // Attack speed: AGI bonus + upgrade bonuses
        float agiSpeedBonus = Agility * config.AgilityAttackSpeedPerPoint;
        float totalAttackSpeedBonus = modifiers.AttackSpeedBonus + agiSpeedBonus;
        CurrentFireRate = BaseFireRate * (1 - totalAttackSpeedBonus);
        CurrentMeleeRate = BaseMeleeRate * (1 - totalAttackSpeedBonus);

        // Max Health: Base + Run level + Upgrades + Character stats (VIT + STR)
        float characterHealthBonus = (Vitality * config.VitalityHealthPerPoint) + (Strength * config.StrengthHealthPerPoint);
        CurrentMaxHealth = BaseMaxHealth + CalculateRunLevelHealthBonus() + modifiers.MaxHealthBonus + characterHealthBonus;

        // Damage: Upgrades + Character stats (STR/INT)
        DamageMultiplier = 1.0f + modifiers.DamagePercentBonus;
        PhysicalDamageMultiplier = 1.0f + (Strength * config.StrengthDamagePerPoint);
        MagicalDamageMultiplier = 1.0f + (Intelligence * config.IntelligenceDamagePerPoint);

        // Crit: Upgrades + Character stats (AGI + FOR)
        float characterCritChance = Mathf.Min(Agility * config.AgilityCritPerPoint, 0.5f); // Cap at 50%
        CritChance = Mathf.Clamp(modifiers.CritChanceBonus + characterCritChance, 0f, 1f);
        CritDamageMultiplier = 1.5f + (Fortune * config.FortuneCritDamagePerPoint); // Base 150% + FOR bonus

        // Regen: Upgrades + Character stats (VIT)
        float characterRegen = Vitality * config.VitalityRegenPerPoint;
        HealthRegenPerSecond = modifiers.HealthRegenBonus + characterRegen;

        PickupRadius = 80f + modifiers.PickupRadiusBonus;
        ProjectilePierceCount = modifiers.ProjectilePierceBonus;

        // Maintain health percentage
        CurrentHealth = CurrentMaxHealth * healthPercentage;
        if (CurrentHealth > CurrentMaxHealth)
        {
            CurrentHealth = CurrentMaxHealth;
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

    public float GetCooldownReduction()
    {
        return (1.0f - (1.0f / (1.0f + Intelligence * GameBalance.Instance.Config.CharacterProgression.IntelligenceCDRPerPoint))) * 100f;
    }

    // ===== PUBLIC API - Save/Load =====
    public SaveData GetSaveData()
    {
        return new SaveData
        {
            Version = 1,
            LastSaved = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),

            // Character stats (StatsManager owns this)
            Strength = Strength,
            Intelligence = Intelligence,
            Agility = Agility,
            Vitality = Vitality,
            Fortune = Fortune,

            // Character progression (StatsManager owns this)
            CharacterLevel = CharacterLevel,
            CharacterExperience = CharacterExperience,
            UnallocatedStatPoints = UnallocatedStatPoints,
            HighestFloorReached = HighestFloorReached,

            // Run state (StatsManager owns PowerLevel)
            PowerLevel = PowerLevel,

            // These will be filled in by Game.cs:
            CurrentFloor = 0,  // Game.cs sets this
            HasActiveRun = false,  // Game.cs sets this
            ActiveUpgrades = null  // Game.cs sets this
        };
    }


    public void LoadSaveData(SaveData data)
    {
        // Restore character stats
        _strength = data.Strength;
        _intelligence = data.Intelligence;
        _agility = data.Agility;
        _vitality = data.Vitality;
        _fortune = data.Fortune;

        // Restore character progression
        CharacterLevel = data.CharacterLevel;
        CharacterExperience = data.CharacterExperience;
        CharacterExperienceToNextLevel = CalculateCharacterXPForNextLevel();
        UnallocatedStatPoints = data.UnallocatedStatPoints;
        HighestFloorReached = data.HighestFloorReached;

        // Restore run state (PowerLevel only - StatsManager owns this)
        PowerLevel = data.PowerLevel;
        PowerExperience = 0;
        PowerExperienceToNextLevel = (int)(100 * Mathf.Pow(1.5f, PowerLevel - 1));

        GD.Print($"Loaded character: Level {CharacterLevel}, STR {Strength}, INT {Intelligence}, AGI {Agility}, VIT{Vitality}, FOR {Fortune}");
        GD.Print($"Run state: Power Level {PowerLevel}");

        // Recalculate base stats (upgrades will be applied separately by Game.cs)
        RecalculateStats(new StatModifiers());
    }


    // ===== PRIVATE HELPERS - Calculations =====
    private int CalculateCharacterXPForNextLevel()
    {
        var config = GameBalance.Instance.Config.CharacterProgression;
        return (int)(config.CharacterXPBase * Mathf.Pow(config.CharacterXPGrowth, CharacterLevel - 1));
    }

    private float CalculateRunLevelHealthBonus()
    {
        return GameBalance.Instance.Config.CharacterProgression.PowerLevelHealthBonus * (PowerLevel - 1);
    }

    // ===== PRIVATE HELPERS - Progression =====
    private void PowerLevelUp()
    {
        var config = GameBalance.Instance.Config.CharacterProgression;
        PowerExperience -= PowerExperienceToNextLevel;
        PowerLevel++;
        PowerExperienceToNextLevel = (int)(PowerExperienceToNextLevel * 1.5);

        // Recalculate MaxHealth with new run level (UpgradeManager will call RecalculateStats, but update now for heal)
        float characterHealthBonus = (Vitality * config.VitalityHealthPerPoint) + (Strength * config.StrengthHealthPerPoint);
        CurrentMaxHealth = BaseMaxHealth + CalculateRunLevelHealthBonus() + characterHealthBonus;
        CurrentHealth = CurrentMaxHealth;  // Full heal on level up

        GD.Print($"Run leveled up to {PowerLevel}!");

        EmitHealthUpdate();
        EmitExperienceUpdate();

        EmitSignal(SignalName.LeveledUp);
    }

    private void Die()
    {
        GD.Print("Player died!");

        // Find Dungeon node and notify it
        var dungeon = GetTree().Root.GetNode<Dungeon>("Dungeon");
        if (dungeon != null)
        {
            dungeon.OnPlayerDeath();
        }
        else
        {
            GD.PrintErr("Could not find Dungeon node!");
            GetTree().ReloadCurrentScene(); // Fallback
        }
    }

    // ===== PRIVATE HELPERS - Signals =====
    private void EmitHealthUpdate()
    {
        _player?.EmitSignal(Player.SignalName.HealthChanged, CurrentHealth, CurrentMaxHealth);
    }

    private void EmitExperienceUpdate()
    {
        _player?.EmitSignal(Player.SignalName.ExperienceChanged, PowerExperience, PowerExperienceToNextLevel, PowerLevel);
    }
}
