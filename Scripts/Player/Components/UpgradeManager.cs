using System.Collections.Generic;
using System.Linq;
using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Progression.Upgrades;

namespace ZenithRising.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class UpgradeManager : Node
{
    private readonly Dictionary<UpgradeType, float> _activeUpgrades = [];

    // Available upgrades pool
    private readonly List<Upgrade> _availableUpgrades = [];

    private Player _player;
    private StatsManager _statsManager;
    private BuffManager _buffManager;

    public override void _Ready()
    {
        _player = GetParent<Player>();
        if (_player == null)
        {
            GD.PrintErr("UpgradeManager: Could not find Player parent!");
        }

        _statsManager = _player.GetNode<StatsManager>("StatsManager");
        if (_statsManager == null)
        {
            GD.PrintErr("UpgradeManager: Could not find StatsManager on Player!");
        }

        _buffManager = _player.GetNode<BuffManager>("BuffManager");
        if (_buffManager == null)
        {
            GD.PrintErr("UpgradeManager: BuffManager not found!");
        }

        InitializeUpgradeList();
    }

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

    public float GetUpgradeValue(UpgradeType type)
    {
        return _activeUpgrades.GetValueOrDefault(type, 0f);
    }

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

    public Dictionary<UpgradeType, float> GetActiveUpgrades()
    {
        return new Dictionary<UpgradeType, float>(_activeUpgrades);
    }

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

    public void ClearUpgrades()
    {
        _activeUpgrades.Clear();
    }

    private StatModifiers GetUpgradeModifiers()
    {
        float movementSpeedPercent = GetUpgradeValue(UpgradeType.MovementSpeed);
        float attackSpeedPercent = GetUpgradeValue(UpgradeType.AttackSpeed);
        float healthFlat = GetUpgradeValue(UpgradeType.MaxHealth);
        float speedFlat = GameBalance.Instance.Config.UpgradeSystem.BaseSpeedPerLevel * _statsManager.PowerLevel;
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
}
