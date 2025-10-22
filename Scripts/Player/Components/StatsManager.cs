using System;
using Godot;
using ZenithRising.Scripts.Core;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Stat modifier aggregation structure.
/// Collected by UpgradeManager from upgrades + buffs, passed to StatsManager.RecalculateStats().
/// Formula: FinalStat = (BaseStat + FlatBonus) * (1 + PercentBonus)
/// </summary>
public struct StatModifiers
{
    // Flat bonuses (added before percent multipliers)
    public float FlatHealth;
    public float FlatSpeed;
    public float FlatDamage;

    // Percent bonuses (0.10 = +10%, multiplicative with base)
    public float PercentHealth;
    public float PercentSpeed;
    public float PercentAttackSpeed;
    public float PercentCastSpeed;
    public float PercentDamage;
    public float PercentCritChance;
    public float PercentCritDamage;
    public float PercentCooldownReduction;

    // Special bonuses
    public float PickupRadius;
    public float HealthRegenPerSecond;
    public int ProjectilePierceCount;
}

/// <summary>
/// The 5 core character attributes.
/// STR: Physical damage, HP
/// INT: Magical damage, Cast speed, CDR
/// AGI: Attack speed, Crit chance
/// VIT: HP, HP regen
/// FOR: Crit damage
/// </summary>
public enum StatType
{
    Strength,
    Intelligence,
    Agility,
    Vitality,
    Fortune
}

/// <summary>
/// Character stat management and calculation.
/// Responsibilities:
/// - Core stat storage (STR/INT/AGI/VIT/FOR) and derived stat calculation
/// - Character/Power progression tracking (dual XP system)
/// - Health/damage management
/// - Save/load data serialization
/// - Stat recalculation when upgrades/buffs change
/// Formula: FinalStat = (BaseStat + AttributeBonus + FlatModifier) * (1 + PercentModifier)
/// Does NOT handle: Buff lifecycle (BuffManager), upgrade tracking (UpgradeManager)
/// </summary>
[GlobalClass]
public partial class StatsManager : Node
{
    // ═══════════════════════════════════════════════════════════════
    // BASE STATS (Loaded from GameBalance config)
    // ═══════════════════════════════════════════════════════════════
    public float BaseMaxHealth { get; set; } = 100f;
    public float BaseSpeed { get; set; } = 300f;
    public float BasePickupRadius { get; private set; } = 80f;
    public float BaseAttackRate { get; set; } = 2.0f;      // Attacks per second
    public float BaseCastSpeed { get; set; } = 1.0f;       // Cast speed multiplier
    public float BaseDamage { get; private set; } = 10f;

    // ═══════════════════════════════════════════════════════════════
    // CORE ATTRIBUTES (5 stats - allocated by player)
    // ═══════════════════════════════════════════════════════════════
    [ExportGroup("Core Attributes (Debug Visibility)")]
    [Export] public int Strength { get => _strength; private set => _strength = Mathf.Max(0, value); }
    [Export] public int Intelligence { get => _intelligence; private set => _intelligence = Mathf.Max(0, value); }
    [Export] public int Agility { get => _agility; private set => _agility = Mathf.Max(0, value); }
    [Export] public int Vitality { get => _vitality; private set => _vitality = Mathf.Max(0, value); }
    [Export] public int Fortune { get => _fortune; private set => _fortune = Mathf.Max(0, value); }

    [ExportGroup("Character Progression")]
    [Export] public int CharacterLevel { get; private set; } = 1;
    [Export] public int CharacterExperience { get; private set; } = 0;
    [Export] public int AvailableStatPoints { get; private set; } = 15;

    [ExportGroup("Power Progression (Per-Run)")]
    [Export] public int PowerLevel { get; set; } = 1;
    [Export] public int PowerExperience { get; set; } = 0;

    [ExportGroup("Highest Floor")]
    [Export] public int HighestFloor { get; private set; } = 1;

    // ═══════════════════════════════════════════════════════════════
    // DERIVED STATS (Calculated from base + attributes + modifiers)
    // ═══════════════════════════════════════════════════════════════
    // XP Requirements (calculated from progression formulas)
    public int CharacterExperienceRequired { get; private set; } = 100;
    public int PowerExperienceRequired { get; private set; } = 100;

    // Final Stats (after RecalculateStats())
    public float CurrentMaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float CurrentSpeed { get; private set; }
    public float CurrentDamage { get; private set; }
    public float CurrentPickupRadius { get; private set; }
    public float CurrentCritChance { get; private set; }
    public float CurrentCritMultiplier { get; private set; }
    public float CurrentAttackRate { get; private set; }
    public float CurrentCastSpeed { get; private set; }
    public float CooldownReduction { get; private set; } = 0f;

    // Damage Multipliers (used by CombatSystem.CalculateDamage())
    public float DamageMultiplier { get; private set; } = 1.0f;
    public float PhysicalDamageMultiplier { get; private set; } = 1.0f;
    public float MagicalDamageMultiplier { get; private set; } = 1.0f;

    // Special Stats
    public float HealthRegenPerSecond { get; private set; } = 0f;
    public int ProjectilePierceCount { get; private set; } = 0;
    public bool IsInvincible { get; private set; } = false;

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE FIELDS (Backing fields for attribute properties)
    // ═══════════════════════════════════════════════════════════════
    private int _strength = 0;
    private int _intelligence = 0;
    private int _agility = 0;
    private int _vitality = 0;
    private int _fortune = 0;

    // ═══════════════════════════════════════════════════════════════
    // SIGNALS
    // ═══════════════════════════════════════════════════════════════
    [Signal] public delegate void LeveledUpEventHandler();
    [Signal] public delegate void CharacterLeveledUpEventHandler();
    [Signal] public delegate void StatAllocatedEventHandler(int statType);
    [Signal] public delegate void HealthChangedEventHandler(float currentHealth, float maxHealth);
    [Signal] public delegate void ExperienceChangedEventHandler(int currentXP, int requiredXP, int level);
    [Signal] public delegate void PlayerDiedEventHandler();

    // ═══════════════════════════════════════════════════════════════
    // LIFECYCLE METHODS
    // ═══════════════════════════════════════════════════════════════
    public override void _Ready()
    {
        LoadCharacterStatsFromConfig();

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

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - INITIALIZATION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Initializes stats for new game (no save data).
    /// Calculates initial derived stats and emits UI update signals.
    /// </summary>
    public void Initialize()
    {
        RecalculateStats(new StatModifiers());
        EmitHealthUpdate();
        EmitExperienceUpdate();
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - COMBAT
    // ═══════════════════════════════════════════════════════════════
    public void TakeDamage(float damage)
    {
        if (IsInvincible)
        {
            return; // Early exit, no damage taken
        }

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

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - PROGRESSION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Awards Power XP (per-run progression). Handles overflow for multiple level-ups.
    /// </summary>
    public void AddPowerExperience(int amount)
    {
        PowerExperience += amount;

        while (PowerExperience >= PowerExperienceRequired)
        {
            PowerLevelUp();
        }

        EmitExperienceUpdate();
    }

    /// <summary>
    /// Awards Character XP (permanent progression). Handles overflow for multiple level-ups.
    /// </summary>
    public void AddCharacterExperience(int amount)
    {
        CharacterExperience += amount;

        while (CharacterExperience >= CharacterExperienceRequired)
        {
            AddCharacterLevel(1);
        }
    }

    public void AddCharacterLevel(int levels = 1)
    {
        CharacterExperience -= CharacterExperienceRequired;
        CharacterLevel += levels;
        AvailableStatPoints += levels;
        CalculateExperienceRequirements();
        EmitSignal(SignalName.CharacterLeveledUp);
    }

    public void UpdateHighestFloor(int floor)
    {
        if (floor > HighestFloor)
        {
            HighestFloor = floor;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - STAT CALCULATION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Recalculates all derived stats from base + attributes + modifiers.
    /// Called by UpgradeManager whenever upgrades or buffs change.
    /// Formula: FinalStat = (Base + Attributes + FlatMod) * (1 + PercentMod)
    /// Preserves health percentage when MaxHealth changes.
    /// </summary>
    public void RecalculateStats(StatModifiers modifiers)
    {
        // Preserve health percentage across MaxHealth changes
        float healthPercentage = CurrentMaxHealth > 0 ? CurrentHealth / CurrentMaxHealth : 1f;
        var config = GameBalance.Instance.Config.CharacterProgression;

        // ═══════════════════════════════════════════════════════════════
        // STEP 1: Calculate Attribute Bonuses (STR/INT/AGI/VIT/FOR)
        // ═══════════════════════════════════════════════════════════════
        // STR bonuses
        float strDamagePercent = Strength * config.StrengthDamagePerPoint;
        float strHealthFlat = Strength * config.StrengthHealthPerPoint;

        // INT bonuses
        float intDamagePercent = Intelligence * config.IntelligenceDamagePerPoint;
        float intCastSpeedPercent = Intelligence * config.IntelligenceCastSpeedPerPoint; // NEW
        float intCDRPercent = Intelligence * config.IntelligenceCDRPerPoint; // NEW

        // AGI bonuses
        float agiAttackSpeedPercent = Agility * config.AgilityAttackSpeedPerPoint;
        float agiCritPercent = Agility * config.AgilityCritPerPoint;

        // VIT bonuses
        float vitHealthFlat = Vitality * config.VitalityHealthPerPoint;
        float vitRegenFlat = Vitality * config.VitalityRegenPerPoint;

        // FOR bonuses
        float forCritDamagePercent = Fortune * config.FortuneCritDamagePerPoint;

        // ═══════════════════════════════════════════════════════════════
        // STEP 2: Apply Unified Formula (CharacterStat + Flat) * (1 + Percent)
        // ═══════════════════════════════════════════════════════════════
        // HEALTH
        float flatHealth = strHealthFlat + vitHealthFlat + CalculateRunLevelHealthBonus() + modifiers.FlatHealth;
        float percentHealth = modifiers.PercentHealth;
        CurrentMaxHealth = (BaseMaxHealth + flatHealth) * (1 + percentHealth);

        // MOVEMENT SPEED
        float flatSpeed = modifiers.FlatSpeed;
        float percentSpeed = modifiers.PercentSpeed;
        CurrentSpeed = (BaseSpeed + flatSpeed) * (1 + percentSpeed);

        // ATTACK SPEED (attacks per second)
        float percentAttackSpeed = agiAttackSpeedPercent + modifiers.PercentAttackSpeed;
        CurrentAttackRate = BaseAttackRate * (1 + percentAttackSpeed);

        // CAST SPEED (multiplier for cast times)
        float percentCastSpeed = intCastSpeedPercent + modifiers.PercentCastSpeed;
        CurrentCastSpeed = BaseCastSpeed * (1 + percentCastSpeed);

        // COOLDOWN REDUCTION (percent reduction, capped at 40%)
        float totalCDR = intCDRPercent + modifiers.PercentCooldownReduction;
        CooldownReduction = Mathf.Clamp(totalCDR, 0f, 0.40f);

        // DAMAGE MULTIPLIERS (used by CombatSystem)
        float percentPhysicalDamage = strDamagePercent + modifiers.PercentDamage;
        PhysicalDamageMultiplier = 1.0f + percentPhysicalDamage;

        float percentMagicalDamage = intDamagePercent + modifiers.PercentDamage;
        MagicalDamageMultiplier = 1.0f + percentMagicalDamage;

        DamageMultiplier = 1.0f + modifiers.PercentDamage;

        // CRIT CHANCE
        float percentCritChance = agiCritPercent + modifiers.PercentCritChance;
        CurrentCritChance = Mathf.Clamp(percentCritChance, 0f, 0.75f);

        // CRIT DAMAGE
        float baseCritDamage = 0.5f; // Base crit = 150%
        float percentCritDamage = forCritDamagePercent + modifiers.PercentCritDamage;
        CurrentCritMultiplier = 1.0f + baseCritDamage + percentCritDamage;

        // HEALTH REGEN
        float flatRegen = vitRegenFlat + modifiers.HealthRegenPerSecond;
        HealthRegenPerSecond = flatRegen;

        // PICKUP RADIUS
        CurrentPickupRadius = BasePickupRadius + modifiers.PickupRadius;

        // PROJECTILE PIERCE
        ProjectilePierceCount = modifiers.ProjectilePierceCount;

        // ═══════════════════════════════════════════════════════════════
        // STEP 3: Calculate Experience Requirements
        // ═══════════════════════════════════════════════════════════════

        CalculateExperienceRequirements();

        // ═══════════════════════════════════════════════════════════════
        // STEP 4: Maintain Health Percentage
        // ═══════════════════════════════════════════════════════════════

        CurrentHealth = CurrentMaxHealth * healthPercentage;
        if (CurrentHealth > CurrentMaxHealth)
        {
            CurrentHealth = CurrentMaxHealth;
        }

        EmitHealthUpdate();
    }

    /// <summary>
    /// Checks if player has unallocated stat points.
    /// </summary>
    public bool CanAllocateStat()
    {
        return AvailableStatPoints > 0;
    }

    /// <summary>
    /// Allocates stat points to a specific attribute (STR/INT/AGI/VIT/FOR).
    /// Called by StatAllocationPanel when player spends points.
    /// </summary>
    public void AllocateStat(StatType statType, int amount = 1)
    {
        if (!CanAllocateStat())
        {
            GD.PrintErr("StatsManager: No unallocated stat points!");
            return;
        }

        if (amount > AvailableStatPoints)
        {
            GD.PrintErr($"StatsManager: Trying to allocate {amount} points but only {AvailableStatPoints} available!");
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

        AvailableStatPoints -= amount;

        EmitSignal(SignalName.StatAllocated, (int)statType);
    }

    /// <summary>
    /// Sets invincibility state for skills (Dash, Leap Slam).
    /// Called by SkillEffectController during forced movement.
    /// </summary>
    public void SetInvincible(bool invincible)
    {
        IsInvincible = invincible;
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - SAVE/LOAD
    // ═══════════════════════════════════════════════════════════════
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
            UnallocatedStatPoints = AvailableStatPoints,
            HighestFloorReached = HighestFloor,

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
        CalculateExperienceRequirements();
        AvailableStatPoints = data.UnallocatedStatPoints;
        HighestFloor = data.HighestFloorReached;

        // Restore run state (PowerLevel only - StatsManager owns this)
        PowerLevel = data.PowerLevel;
        PowerExperience = 0;
        PowerExperienceRequired = (int)(100 * Mathf.Pow(1.5f, PowerLevel - 1));

        // Recalculate base stats (upgrades applied separately by Dungeon.cs)
        RecalculateStats(new StatModifiers());
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - CALCULATIONS
    // ═══════════════════════════════════════════════════════════════
    private float CalculateRunLevelHealthBonus()
    {
        return GameBalance.Instance.Config.CharacterProgression.PowerLevelHealthBonus * (PowerLevel - 1);
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - PROGRESSION
    // ═══════════════════════════════════════════════════════════════
    private void PowerLevelUp()
    {
        var config = GameBalance.Instance.Config.CharacterProgression;
        PowerExperience -= PowerExperienceRequired;
        PowerLevel++;
        CalculateExperienceRequirements();

        // Full heal on level-up
        float characterHealthBonus = (Vitality * config.VitalityHealthPerPoint) + (Strength * config.StrengthHealthPerPoint);
        CurrentMaxHealth = BaseMaxHealth + CalculateRunLevelHealthBonus() + characterHealthBonus;
        CurrentHealth = CurrentMaxHealth;

        EmitHealthUpdate();
        EmitExperienceUpdate();
        EmitSignal(SignalName.LeveledUp);
    }

    private void Die()
    {
        EmitSignal(SignalName.PlayerDied);
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - SIGNAL EMISSION
    // ═══════════════════════════════════════════════════════════════
    private void EmitHealthUpdate()
    {
        EmitSignal(SignalName.HealthChanged, CurrentHealth, CurrentMaxHealth);
    }

    private void EmitExperienceUpdate()
    {
        EmitSignal(SignalName.ExperienceChanged, PowerExperience, PowerExperienceRequired, PowerLevel);
    }

    private void CalculateExperienceRequirements()
    {
        if (GameBalance.Instance?.Config?.CharacterProgression == null)
        {
            CharacterExperienceRequired = 100;
            PowerExperienceRequired = 100;
            return;
        }

        var config = GameBalance.Instance.Config.CharacterProgression;

        // Character XP formula (inline calculation)
        CharacterExperienceRequired = (int)(config.CharacterXPBase * Mathf.Pow(config.CharacterXPGrowth, CharacterLevel - 1));

        // Power XP formula
        PowerExperienceRequired = (int)(100 * Mathf.Pow(1.5f, PowerLevel - 1));
    }

    private void LoadCharacterStatsFromConfig()
    {
        if (GameBalance.Instance?.Config?.PlayerStats == null)
        {
            GD.PrintErr("StatsManager: GameBalance config not available, using defaults");
            return;
        }

        var config = GameBalance.Instance.Config.PlayerStats;

        // Load all character stats from config
        BaseMaxHealth = config.BaseMaxHealth;
        BaseSpeed = config.BaseSpeed;
        BasePickupRadius = config.BasePickupRadius;
        BaseAttackRate = config.BaseAttackRate;
        BaseCastSpeed = config.BaseCastSpeed;
        BaseDamage = config.BaseDamage;

        GD.Print($"StatsManager: Loaded character stats from config (BaseMaxHealth={BaseMaxHealth}, BaseSpeed={BaseSpeed})");
    }
}
