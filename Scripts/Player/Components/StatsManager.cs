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
    // Attack Speed System (NEW naming)
    public float BaseAttackRate { get; set; } = 2.0f; // 2 attacks per second (0.5s between attacks)
    public float CurrentAttackRate { get; private set; } = 2.0f; // Calculated

    // Cast Speed System (NEW)
    public float BaseCastSpeed { get; set; } = 1.0f; // 1.0x multiplier
    public float CurrentCastSpeed { get; private set; } = 1.0f; // Calculated

    // Cooldown Reduction System (NEW)
    public float CooldownReduction { get; private set; } = 0f; // 0% - 40% cap

    // DEPRECATED (will remove after refactor)
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
    public bool IsInvincible { get; private set; } = false;

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

        CharacterExperienceToNextLevel = CalculateCharacterXPForNextLevel();

        EmitSignal(SignalName.CharacterLeveledUp);
    }

    public void UpdateHighestFloor(int floor)
    {
        if (floor > HighestFloorReached)
        {
            HighestFloorReached = floor;
        }
    }

    // ===== PUBLIC API - Stats =====
    public void RecalculateStats(StatModifiers modifiers)
    {
        // Calculate health percentage BEFORE changing MaxHealth
        float healthPercentage = CurrentMaxHealth > 0 ? CurrentHealth / CurrentMaxHealth : 1f;
        var config = GameBalance.Instance.Config.CharacterProgression;

        // ===== CALCULATE ATTRIBUTE BONUSES =====

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

        // ===== UNIFIED FORMULA: (CharacterStat + Flat) * (1 + Percent) =====

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
        CooldownReduction = Mathf.Clamp(totalCDR, 0f, 0.40f); // Cap at 40%

        // DAMAGE (additive pool)
        float percentPhysicalDamage = strDamagePercent + modifiers.PercentDamage;
        PhysicalDamageMultiplier = 1.0f + percentPhysicalDamage;

        float percentMagicalDamage = intDamagePercent + modifiers.PercentDamage;
        MagicalDamageMultiplier = 1.0f + percentMagicalDamage;

        DamageMultiplier = 1.0f + modifiers.PercentDamage;

        // CRIT CHANCE
        float percentCritChance = agiCritPercent + modifiers.PercentCritChance;
        CritChance = Mathf.Clamp(percentCritChance, 0f, 0.75f); // Cap at 75%

        // CRIT DAMAGE
        float baseCritDamage = 0.5f; // Base crit = 150%
        float percentCritDamage = forCritDamagePercent + modifiers.PercentCritDamage;
        CritDamageMultiplier = 1.0f + baseCritDamage + percentCritDamage;

        // HEALTH REGEN
        float flatRegen = vitRegenFlat + modifiers.HealthRegenPerSecond;
        HealthRegenPerSecond = flatRegen;

        // PICKUP RADIUS
        float basePickupRadius = 80f;
        PickupRadius = basePickupRadius + modifiers.PickupRadius;

        // PROJECTILE PIERCE
        ProjectilePierceCount = modifiers.ProjectilePierceCount;

        // ===== MAINTAIN HEALTH PERCENTAGE =====
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

        EmitSignal(SignalName.StatAllocated, (int)statType);
    }

    public float GetCooldownReduction()
    {
        return (1.0f - (1.0f / (1.0f + Intelligence * GameBalance.Instance.Config.CharacterProgression.IntelligenceCDRPerPoint))) * 100f;
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
        _player?.EmitSignal(Player.SignalName.ExperienceChanged, PowerExperience, PowerExperienceToNextLevel, PowerLevel);
    }
}
