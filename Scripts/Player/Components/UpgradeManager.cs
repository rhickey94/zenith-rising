using System.Collections.Generic;
using System.Linq;
using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Progression.Upgrades;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Permanent upgrade management for per-run progression (resets on death/victory).
/// Responsibilities:
/// - Upgrade pool generation (8 upgrade types from GameBalance config)
/// - Upgrade selection and stacking (values accumulate per stack)
/// - Stat modifier aggregation (upgrades + buffs combined)
/// - Triggering stat recalculation when upgrades/buffs change
/// - Save/load active upgrades for run persistence
/// Pattern: Collects upgrade + buff modifiers → StatsManager.RecalculateStats()
/// Signal flow: BuffManager.BuffsChanged → OnBuffsChanged → RecalculateAllStats
/// Does NOT handle: Buff lifecycle (BuffManager), stat calculation (StatsManager)
/// </summary>
[GlobalClass]
public partial class UpgradeManager : Node
{
    // ═══════════════════════════════════════════════════════════════
    // UPGRADE TRACKING
    // ═══════════════════════════════════════════════════════════════
    private readonly Dictionary<UpgradeType, float> _activeUpgrades = [];
    private readonly List<Upgrade> _availableUpgrades = [];

    // ═══════════════════════════════════════════════════════════════
    // DEPENDENCIES
    // ═══════════════════════════════════════════════════════════════
    private Player _player;
    private StatsManager _statsManager;
    private BuffManager _buffManager;

    // ═══════════════════════════════════════════════════════════════
    // LIFECYCLE METHODS
    // ═══════════════════════════════════════════════════════════════
    public override void _Ready()
    {
        // Dependencies injected via Initialize() - no initialization needed here
    }

    public void Initialize(Player player, StatsManager statsManager, BuffManager buffManager)
    {
        _player = player;
        _statsManager = statsManager;
        _buffManager = buffManager;

        if (_buffManager != null)
        {
            _buffManager.BuffsChanged += OnBuffsChanged;
        }

        InitializeUpgradeList();
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - UPGRADE APPLICATION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Applies an upgrade (stacks value if already active).
    /// Triggers immediate stat recalculation.
    /// Called by Player when user selects upgrade from level-up panel.
    /// </summary>
    public void ApplyUpgrade(Upgrade upgrade)
    {
        // Track upgrade (stack values)
        if (_activeUpgrades.ContainsKey(upgrade.Type))
        {
            _activeUpgrades[upgrade.Type] += upgrade.Value;
        }
        else
        {
            _activeUpgrades[upgrade.Type] = upgrade.Value;
        }

        // Recalculate all stats from base + all upgrades
        RecalculateAllStats();
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - UPGRADE SELECTION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Returns random upgrades for level-up panel choices.
    /// Uses lazy initialization if upgrade list not ready.
    /// </summary>
    public List<Upgrade> GetRandomUpgrades(int count)
    {
        // Lazy initialize if not done in _Ready() due to timing
        if (_availableUpgrades.Count == 0)
        {
            InitializeUpgradeList();
        }

        var shuffled = _availableUpgrades.OrderBy(x => GD.Randi()).ToList();
        return [.. shuffled.Take(count)];
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - UPGRADE POOL INITIALIZATION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Initializes upgrade pool from GameBalance config.
    /// 8 upgrade types: Damage, Attack Speed, Movement Speed, Max Health, Pickup Radius, Pierce, Crit Chance, Regen.
    /// </summary>
    private void InitializeUpgradeList()
    {
        if (GameBalance.Instance == null || GameBalance.Instance.Config == null)
        {
            GD.PrintErr("UpgradeManager: Cannot initialize upgrade list - GameBalance not available!");
            return;
        }

        var config = GameBalance.Instance.Config.UpgradeSystem;
        _availableUpgrades.Clear();
        _availableUpgrades.AddRange(
        [
        new Upgrade { UpgradeName = "Damage Boost", Description = $"+{config.DamageBoostPerStack * 100}% Damage", Type = UpgradeType.DamagePercent, Value = config.DamageBoostPerStack },
        new Upgrade { UpgradeName = "Attack Speed", Description = $"+{config.AttackSpeedPerStack * 100}% Fire Rate", Type = UpgradeType.AttackSpeed, Value = config.AttackSpeedPerStack },
        new Upgrade { UpgradeName = "Swift Feet", Description = $"+{config.MovementSpeedPerStack * 100}% Movement Speed", Type = UpgradeType.MovementSpeed, Value = config.MovementSpeedPerStack },
        new Upgrade { UpgradeName = "Vitality", Description = $"+{config.MaxHealthPerStack} Max Health", Type = UpgradeType.MaxHealth, Value = config.MaxHealthPerStack },
        new Upgrade { UpgradeName = "Magnet", Description = $"+{config.PickupRadiusPerStack} Pickup Radius", Type = UpgradeType.PickupRadius, Value = config.PickupRadiusPerStack },
        new Upgrade { UpgradeName = "Piercing Shots", Description = $"Projectiles Pierce +{config.ProjectilePiercePerStack}", Type = UpgradeType.ProjectilePierce, Value = config.ProjectilePiercePerStack },
        new Upgrade { UpgradeName = "Critical Hit", Description = $"+{config.CritChancePerStack * 100}% Crit Chance", Type = UpgradeType.CritChance, Value = config.CritChancePerStack },
        new Upgrade { UpgradeName = "Regeneration", Description = $"+{config.HealthRegenPerStack} HP/sec", Type = UpgradeType.HealthRegen, Value = config.HealthRegenPerStack }
    ]);
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - UPGRADE QUERIES
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Gets current stacked value for an upgrade type.
    /// Returns 0 if upgrade not active.
    /// </summary>
    public float GetUpgradeValue(UpgradeType type)
    {
        return _activeUpgrades.GetValueOrDefault(type, 0f);
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - STAT RECALCULATION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Recalculates all player stats from upgrades + buffs.
    /// Called when: Upgrades applied, buffs change (via BuffManager.BuffsChanged signal).
    /// Pattern: Collects upgrade modifiers + buff modifiers → combines additively → StatsManager.RecalculateStats().
    /// </summary>
    public void RecalculateAllStats()
    {
        if (_statsManager == null)
        {
            return;
        }

        if (GameBalance.Instance == null || GameBalance.Instance.Config == null)
        {
            GD.PrintErr("UpgradeManager.RecalculateAllStats: GameBalance not available!");
            return;
        }

        // Collect UPGRADE modifiers
        var upgradeModifiers = GetUpgradeModifiers();

        // Collect BUFF modifiers
        var buffModifiers = _buffManager?.GetStatModifiers() ?? new StatModifiers();

        // Combine them (additive)
        var combined = new StatModifiers
        {
            FlatHealth = upgradeModifiers.FlatHealth + buffModifiers.FlatHealth,
            FlatSpeed = upgradeModifiers.FlatSpeed + buffModifiers.FlatSpeed,
            FlatDamage = upgradeModifiers.FlatDamage + buffModifiers.FlatDamage,

            PercentHealth = upgradeModifiers.PercentHealth + buffModifiers.PercentHealth,
            PercentSpeed = upgradeModifiers.PercentSpeed + buffModifiers.PercentSpeed,
            PercentAttackSpeed = upgradeModifiers.PercentAttackSpeed + buffModifiers.PercentAttackSpeed,
            PercentCastSpeed = upgradeModifiers.PercentCastSpeed + buffModifiers.PercentCastSpeed,
            PercentDamage = upgradeModifiers.PercentDamage + buffModifiers.PercentDamage,
            PercentCritChance = upgradeModifiers.PercentCritChance + buffModifiers.PercentCritChance,
            PercentCritDamage = upgradeModifiers.PercentCritDamage + buffModifiers.PercentCritDamage,
            PercentCooldownReduction = upgradeModifiers.PercentCooldownReduction + buffModifiers.PercentCooldownReduction,

            PickupRadius = upgradeModifiers.PickupRadius + buffModifiers.PickupRadius,
            HealthRegenPerSecond = upgradeModifiers.HealthRegenPerSecond + buffModifiers.HealthRegenPerSecond,
            ProjectilePierceCount = upgradeModifiers.ProjectilePierceCount + buffModifiers.ProjectilePierceCount
        };

        // Single call to StatsManager with combined modifiers
        _statsManager.RecalculateStats(combined);
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - SAVE/LOAD
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Returns copy of active upgrades for save system.
    /// </summary>
    public Dictionary<UpgradeType, float> GetActiveUpgrades()
    {
        return new Dictionary<UpgradeType, float>(_activeUpgrades);
    }

    /// <summary>
    /// Loads saved upgrades and recalculates stats.
    /// Called when loading active run from save file.
    /// </summary>
    public void LoadActiveUpgrades(Dictionary<UpgradeType, float> upgrades)
    {
        _activeUpgrades.Clear();

        if (upgrades != null)
        {
            foreach (var kvp in upgrades)
            {
                _activeUpgrades[kvp.Key] = kvp.Value;
            }

            // Recalculate stats with loaded upgrades
            RecalculateAllStats();
        }
    }

    /// <summary>
    /// Clears all active upgrades (used when starting new run).
    /// </summary>
    public void ClearUpgrades()
    {
        _activeUpgrades.Clear();
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - MODIFIER AGGREGATION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Converts active upgrades to StatModifiers structure.
    /// Called by RecalculateAllStats() to combine with buff modifiers.
    /// </summary>
    private StatModifiers GetUpgradeModifiers()
    {
        float movementSpeedPercent = GetUpgradeValue(UpgradeType.MovementSpeed);
        float attackSpeedPercent = GetUpgradeValue(UpgradeType.AttackSpeed);
        float healthFlat = GetUpgradeValue(UpgradeType.MaxHealth);
        float speedFlat = GameBalance.Instance.Config.UpgradeSystem.BaseSpeedPerLevel * (_statsManager?.PowerLevel ?? 1);
        float damagePercent = GetUpgradeValue(UpgradeType.DamagePercent);
        float critChancePercent = GetUpgradeValue(UpgradeType.CritChance);
        float pickupRadiusFlat = GetUpgradeValue(UpgradeType.PickupRadius);
        float healthRegenFlat = GetUpgradeValue(UpgradeType.HealthRegen);
        float projectilePierceFlat = GetUpgradeValue(UpgradeType.ProjectilePierce);

        return new StatModifiers
        {
            FlatHealth = healthFlat,
            FlatSpeed = speedFlat,
            FlatDamage = 0f,

            PercentHealth = 0f,
            PercentSpeed = movementSpeedPercent,
            PercentAttackSpeed = attackSpeedPercent,
            PercentCastSpeed = 0f, // Add upgrade if needed later
            PercentDamage = damagePercent,
            PercentCritChance = critChancePercent,
            PercentCritDamage = 0f,
            PercentCooldownReduction = 0f, // Add upgrade if needed later

            PickupRadius = pickupRadiusFlat,
            HealthRegenPerSecond = healthRegenFlat,
            ProjectilePierceCount = (int)projectilePierceFlat
        };
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - EVENT HANDLERS
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Signal handler for BuffManager.BuffsChanged.
    /// Triggers stat recalculation when buffs are added/removed/expired.
    /// </summary>
    private void OnBuffsChanged()
    {
        RecalculateAllStats();
    }
}
