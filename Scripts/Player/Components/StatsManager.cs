using System;
using Godot;
using ZenithRising.Scripts.Core;

namespace ZenithRising.Scripts.PlayerScripts.Components;

public struct StatModifiers
{
    // Flat bonuses
    public float FlatHealth;
    public float FlatSpeed;
    public float FlatDamage;

    // Percent bonuses (0.10 = +10%)
    public float PercentHealth;
    public float PercentSpeed;
    public float PercentAttackSpeed;  // now uses same direction as other stats
    public float PercentCastSpeed;  // now uses same direction as other stats
    public float PercentDamage;
    public float PercentCritChance;
    public float PercentCritDamage;
    public float PercentCooldownReduction;

    // Special bonuses
    public float PickupRadius;
    public float HealthRegenPerSecond;
    public int ProjectilePierceCount;
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
    // ═══════════════════════════════════════════════════════════════
    // CHARACTER STATS (Loaded from Config - Not Exported)
    // ═══════════════════════════════════════════════════════════════
    // Core Stats
    public float BaseMaxHealth { get; set; } = 100f;
    public float BaseSpeed { get; set; } = 300f;
    public float BasePickupRadius { get; private set; } = 80f;

    // Combat Stats
    public float BaseAttackRate { get; set; } = 2.0f; // 2 attacks per second (0.5s between attacks)
    public float BaseCastSpeed { get; set; } = 1.0f; // 1.0x multiplier
    public float BaseDamage { get; private set; } = 10f;

    // ═══════════════════════════════════════════════════════════════
    // ATTRIBUTE STATS (Exported for Debug Visibility Only)
    // ═══════════════════════════════════════════════════════════════
    [ExportGroup("Attribute Stats (Debug Visibility)")]
    [Export] public int Strength { get => _strength; private set => _strength = Mathf.Max(0, value); }       // STR: +3% Physical Dmg, +10 HP
    [Export] public int Intelligence { get => _intelligence; private set => _intelligence = Mathf.Max(0, value); }   // INT: +3% Magical Dmg, +2% CDR
    [Export] public int Agility { get => _agility; private set => _agility = Mathf.Max(0, value); }        // AGI: +2% Attack Speed, +1% Crit
    [Export] public int Vitality { get => _vitality; private set => _vitality = Mathf.Max(0, value); }       // VIT: +25 HP, +0.5 HP/sec
    [Export] public int Fortune { get => _fortune; private set => _fortune = Mathf.Max(0, value); }        // FOR: +2% Crit Dmg, +1% Drop Rate

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
    // CALCULATED STATS (Runtime Only - Not Exported)
    // ═══════════════════════════════════════════════════════════════
    // Experience Requirements (calculated from formulas)
    public int CharacterExperienceRequired { get; private set; } = 100;
    public int PowerExperienceRequired { get; private set; } = 100;

    // Current Stats (after all modifiers)
    public float CurrentMaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float CurrentSpeed { get; private set; }
    public float CurrentDamage { get; private set; }
    public float CurrentPickupRadius { get; private set; }
    public float CurrentCritChance { get; private set; }
    public float CurrentCritMultiplier { get; private set; }

    // New Combat Stats
    public float CurrentAttackRate { get; private set; }  // Final attacks per second
    public float CurrentCastSpeed { get; private set; }   // Final cast speed multiplier
    public float CooldownReduction { get; private set; } = 0f; // 0% - 40% cap

    // Damage Multipliers (used by CombatSystem)
    public float DamageMultiplier { get; private set; } = 1.0f;
    public float PhysicalDamageMultiplier { get; private set; } = 1.0f;
    public float MagicalDamageMultiplier { get; private set; } = 1.0f;

    // Special Stats
    public float HealthRegenPerSecond { get; private set; } = 0f;
    public int ProjectilePierceCount { get; private set; } = 0;
    public bool IsInvincible { get; private set; } = false;

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE FIELDS
    // ═══════════════════════════════════════════════════════════════
    private Player _player;
    private int _strength = 0;
    private int _intelligence = 0;
    private int _agility = 0;
    private int _vitality = 0;
    private int _fortune = 0;

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

    // ===== PUBLIC API - Progression =====
    public void AddPowerExperience(int amount)
    {
        PowerExperience += amount;

        while (PowerExperience >= PowerExperienceRequired)
        {
            PowerLevelUp();
        }

        EmitExperienceUpdate();
    }

    public void AddCharacterExperience(int amount)
    {
        CharacterExperience += amount;

        // Handle level ups (support multiple levels in one award)
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

    // ===== PUBLIC API - Stats =====
    public void RecalculateStats(StatModifiers modifiers)
    {
        // Calculate health percentage BEFORE changing MaxHealth
        float healthPercentage = CurrentMaxHealth > 0 ? CurrentHealth / CurrentMaxHealth : 1f;
        var config = GameBalance.Instance.Config.CharacterProgression;

        // ═══════════════════════════════════════════════════════════════
        // STEP 1: Calculate Attribute Bonuses
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

    public bool CanAllocateStat()
    {
        return AvailableStatPoints > 0;
    }

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

    // New method for skill control
    public void SetInvincible(bool invincible)
    {
        IsInvincible = invincible;

        if (invincible)
        {
            // Trigger visual feedback (flash effect)
            // _player?.EmitSignal("InvincibilityStarted");
        }
        else
        {
            // _player?.EmitSignal("InvincibilityEnded");
        }
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

        // Recalculate base stats (upgrades will be applied separately by Game.cs)
        RecalculateStats(new StatModifiers());
    }

    // ===== PRIVATE HELPERS - Calculations =====
    private float CalculateRunLevelHealthBonus()
    {
        return GameBalance.Instance.Config.CharacterProgression.PowerLevelHealthBonus * (PowerLevel - 1);
    }

    // ===== PRIVATE HELPERS - Progression =====
    private void PowerLevelUp()
    {
        var config = GameBalance.Instance.Config.CharacterProgression;
        PowerExperience -= PowerExperienceRequired;
        PowerLevel++;
        CalculateExperienceRequirements();

        // Recalculate MaxHealth with new run level (UpgradeManager will call RecalculateStats, but update now for heal)
        float characterHealthBonus = (Vitality * config.VitalityHealthPerPoint) + (Strength * config.StrengthHealthPerPoint);
        CurrentMaxHealth = BaseMaxHealth + CalculateRunLevelHealthBonus() + characterHealthBonus;
        CurrentHealth = CurrentMaxHealth;  // Full heal on level up

        EmitHealthUpdate();
        EmitExperienceUpdate();

        EmitSignal(SignalName.LeveledUp);
    }

    private void Die()
    {
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
        _player?.EmitSignal(Player.SignalName.ExperienceChanged, PowerExperience, PowerExperienceRequired, PowerLevel);
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
