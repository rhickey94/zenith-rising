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

        // Build upgrade list from config
        if (GameBalance.Instance == null || GameBalance.Instance.Config == null)
        {
            GD.PrintErr("UpgradeManager: GameBalance not ready yet! Deferring upgrade list initialization.");
            // Defer initialization - will be called when first needed
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

        // Get all current upgrade totals

        float movementSpeedBonus = GetUpgradeValue(UpgradeType.MovementSpeed);
        float attackSpeedBonus = GetUpgradeValue(UpgradeType.AttackSpeed);
        float maxHealthBonus = GetUpgradeValue(UpgradeType.MaxHealth);
        float baseSpeedBonus = GameBalance.Instance.Config.UpgradeSystem.BaseSpeedPerLevel * _statsManager.PowerLevel; // Level up bonus (+10 speed per level)

        float damagePercentBonus = GetUpgradeValue(UpgradeType.DamagePercent);
        float critChanceBonus = GetUpgradeValue(UpgradeType.CritChance);
        float pickupRadiusBonus = GetUpgradeValue(UpgradeType.PickupRadius);
        float healthRegenBonus = GetUpgradeValue(UpgradeType.HealthRegen);
        float projectilePierceBonus = GetUpgradeValue(UpgradeType.ProjectilePierce);

        // Tell StatsManager to recalculate everything from scratch
        _statsManager.RecalculateStats(new StatModifiers
        {
            MovementSpeedBonus = movementSpeedBonus,
            AttackSpeedBonus = attackSpeedBonus,
            MaxHealthBonus = maxHealthBonus,
            BaseSpeedBonus = baseSpeedBonus,
            DamagePercentBonus = damagePercentBonus,
            CritChanceBonus = critChanceBonus,
            PickupRadiusBonus = pickupRadiusBonus,
            HealthRegenBonus = healthRegenBonus,
            ProjectilePierceBonus = (int)projectilePierceBonus
        });
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
}
