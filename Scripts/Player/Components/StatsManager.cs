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
    [Export] public int CharacterExperience { get; private set; } = 0;
    [Export] public int CharacterExperienceToNextLevel { get; private set; } = 500;
    [Export] public int UnallocatedStatPoints { get; private set; } = 15;
    [Export] public int Strength { get => _strength; private set => _strength = Mathf.Max(0, value); }       // STR: +3% Physical Dmg, +10 HP
    [Export] public int Intelligence { get => _intelligence; private set => _intelligence = Mathf.Max(0, value); }   // INT: +3% Magical Dmg, +2% CDR
    [Export] public int Agility { get => _agility; private set => _agility = Mathf.Max(0, value); }        // AGI: +2% Attack Speed, +1% Crit
    [Export] public int Vitality { get => _vitality; private set => _vitality = Mathf.Max(0, value); }       // VIT: +25 HP, +0.5 HP/sec
    [Export] public int Fortune { get => _fortune; private set => _fortune = Mathf.Max(0, value); }        // FOR: +2% Crit Dmg, +1% Drop Rate
    [Export] public int HighestFloorReached { get; private set; } = 0;

    // Progression
    [ExportGroup("Run Progression")]
    [Export] public int PowerLevel { get; private set; } = 1;
    [Export] public int PowerExperience { get; private set; } = 0;
    [Export] public int PowerExperienceToNextLevel { get; private set; } = 100;

    // Calculated stats (modified by upgrades)
    public float CurrentMaxHealth { get; private set; } = 100.0f;
    public float CurrentHealth { get; private set; } = 100.0f;
    public float CurrentSpeed { get; private set; }
    public float CurrentFireRate { get; private set; }
    public float CurrentMeleeRate { get; private set; }

    // Combat stats (cached from upgrades)
    public float DamageMultiplier { get; private set; } = 1.0f;
    public float PhysicalDamageMultiplier { get; private set; } = 1.0f;
    public float MagicalDamageMultiplier { get; private set; } = 1.0f;
    public float CritDamageMultiplier { get; private set; } = 1.5f;
    public float CritChance { get; private set; } = 0f;
    public float PickupRadius { get; private set; } = 80f;
    public float HealthRegenPerSecond { get; private set; } = 0f;
    public int ProjectilePierceCount { get; private set; } = 0;

    // Event for level up (Player will listen)
    [Signal] public delegate void LeveledUpEventHandler();
    [Signal] public delegate void CharacterLeveledUpEventHandler();
    [Signal] public delegate void StatAllocatedEventHandler(int statType);

    private Player _player;
    private int _strength = 0;
    private int _intelligence = 0;
    private int _agility = 0;
    private int _vitality = 0;
    private int _fortune = 0;
    private const float STR_DAMAGE_PER_POINT = 0.03f;
    private const float STR_HEALTH_PER_POINT = 10f;
    private const float INT_DAMAGE_PER_POINT = 0.03f;
    private const float INT_CDR_PER_POINT = 0.02f;
    private const float AGI_ATTACK_SPEED_PER_POINT = 0.02f;
    private const float AGI_CRIT_PER_POINT = 0.01f;
    private const float VIT_HEALTH_PER_POINT = 25f;
    private const float VIT_REGEN_PER_POINT = 0.5f;
    private const float FOR_CRIT_DMG_PER_POINT = 0.02f;

    public override void _Ready()
    {
        _player = GetParent<Player>();
        if (_player == null)
        {
            GD.PrintErr("StatsManager: Could not find Player parent!");
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

    public void Initialize()
    {
        // Set initial stat values (no upgrades yet)
        RecalculateStats(new StatModifiers());

        EmitHealthUpdate();
        EmitExperienceUpdate();
    }

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

    public void RecalculateStats(StatModifiers modifiers)
    {
        // Calculate health percentage BEFORE changing MaxHealth
        float healthPercentage = CurrentMaxHealth > 0 ? CurrentHealth / CurrentMaxHealth : 1f;

        // Apply upgrade bonuses to base values
        CurrentSpeed = (BaseSpeed + modifiers.BaseSpeedBonus) * (1 + modifiers.MovementSpeedBonus);

        // Attack speed: AGI bonus + upgrade bonuses
        float agiSpeedBonus = Agility * AGI_ATTACK_SPEED_PER_POINT;
        float totalAttackSpeedBonus = modifiers.AttackSpeedBonus + agiSpeedBonus;
        CurrentFireRate = BaseFireRate * (1 - totalAttackSpeedBonus);
        CurrentMeleeRate = BaseMeleeRate * (1 - totalAttackSpeedBonus);

        // Max Health: Base + Run level + Upgrades + Character stats (VIT + STR)
        float characterHealthBonus = (Vitality * VIT_HEALTH_PER_POINT) + (Strength * STR_HEALTH_PER_POINT);
        CurrentMaxHealth = BaseMaxHealth + CalculateRunLevelHealthBonus() + modifiers.MaxHealthBonus + characterHealthBonus;

        // Damage: Upgrades + Character stats (STR/INT)
        DamageMultiplier = 1.0f + modifiers.DamagePercentBonus;
        PhysicalDamageMultiplier = 1.0f + (Strength * STR_DAMAGE_PER_POINT);
        MagicalDamageMultiplier = 1.0f + (Intelligence * INT_DAMAGE_PER_POINT);

        // Crit: Upgrades + Character stats (AGI + FOR)
        float characterCritChance = Mathf.Min(Agility * AGI_CRIT_PER_POINT, 0.5f); // Cap at 50%
        CritChance = Mathf.Clamp(modifiers.CritChanceBonus + characterCritChance, 0f, 1f);
        CritDamageMultiplier = 1.5f + (Fortune * FOR_CRIT_DMG_PER_POINT); // Base 150% + FOR bonus

        // Regen: Upgrades + Character stats (VIT)
        float characterRegen = Vitality * VIT_REGEN_PER_POINT;
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

    public float GetCooldownReduction()
    {
        return (1.0f - (1.0f / (1.0f + Intelligence * INT_CDR_PER_POINT))) * 100f;
    }

    private int CalculateCharacterXPForNextLevel()
    {
        // Exponential curve: each level requires ~10% more XP than previous
        // Level 1→2: 500 XP
        // Level 10→11: ~1,300 XP
        // Level 50→51: ~58,000 XP
        // Level 100: Total ~13.8 million XP (balanced for long-term play)
        return (int)(500 * Mathf.Pow(1.1f, CharacterLevel - 1));
    }

    private float CalculateRunLevelHealthBonus()
    {
        // +20 HP per level (Level 1 = 0 bonus, Level 2 = 20, Level 3 = 40, etc.)
        return 20f * (PowerLevel - 1);
    }

    private void PowerLevelUp()
    {
        PowerExperience -= PowerExperienceToNextLevel;
        PowerLevel++;
        PowerExperienceToNextLevel = (int)(PowerExperienceToNextLevel * 1.5);

        // Recalculate MaxHealth with new run level (UpgradeManager will call RecalculateStats, but update now for heal)
        float characterHealthBonus = (Vitality * 25f) + (Strength * 10f);
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
        _player?.EmitSignal(Player.SignalName.HealthChanged, CurrentHealth, CurrentMaxHealth);
    }

    private void EmitExperienceUpdate()
    {
        _player?.EmitSignal(Player.SignalName.ExperienceChanged, PowerExperience, PowerExperienceToNextLevel, PowerLevel);
    }
}
