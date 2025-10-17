using System.Collections.Generic;
using System.Linq;
using Godot;
using ZenithRising.Scripts.Progression.Upgrades;

namespace ZenithRising.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class UpgradeManager : Node
{
    private readonly Dictionary<UpgradeType, float> _activeUpgrades = [];

    // Available upgrades pool
    private readonly List<Upgrade> _availableUpgrades =
    [
        new Upgrade { UpgradeName = "Damage Boost", Description = "+15% Damage", Type = UpgradeType.DamagePercent, Value = 0.15f },
        new Upgrade { UpgradeName = "Attack Speed", Description = "+20% Fire Rate", Type = UpgradeType.AttackSpeed, Value = 0.20f },
        new Upgrade { UpgradeName = "Swift Feet", Description = "+15% Movement Speed", Type = UpgradeType.MovementSpeed, Value = 0.15f },
        new Upgrade { UpgradeName = "Vitality", Description = "+50 Max Health", Type = UpgradeType.MaxHealth, Value = 50f },
        new Upgrade { UpgradeName = "Magnet", Description = "+30 Pickup Radius", Type = UpgradeType.PickupRadius, Value = 30f },
        new Upgrade { UpgradeName = "Piercing Shots", Description = "Projectiles Pierce +1", Type = UpgradeType.ProjectilePierce, Value = 1f },
        new Upgrade { UpgradeName = "Critical Hit", Description = "+10% Crit Chance", Type = UpgradeType.CritChance, Value = 0.10f },
        new Upgrade { UpgradeName = "Regeneration", Description = "+2 HP/sec", Type = UpgradeType.HealthRegen, Value = 2f }
    ];

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

        GD.Print($"Selected: {upgrade.UpgradeName}");
        GD.Print($"New Total: {_activeUpgrades[upgrade.Type]}");

        // Recalculate all stats from base + all upgrades
        RecalculateAllStats();
    }

    public List<Upgrade> GetRandomUpgrades(int count)
    {
        var shuffled = _availableUpgrades.OrderBy(x => GD.Randi()).ToList();
        return [.. shuffled.Take(count)];
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

        // Get all current upgrade totals

        float movementSpeedBonus = GetUpgradeValue(UpgradeType.MovementSpeed);
        float attackSpeedBonus = GetUpgradeValue(UpgradeType.AttackSpeed);
        float maxHealthBonus = GetUpgradeValue(UpgradeType.MaxHealth);
        float baseSpeedBonus = 10f * _statsManager.PowerLevel; // Level up bonus (+10 speed per level)

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

            GD.Print($"Loaded {_activeUpgrades.Count} active upgrades");

            // Recalculate stats with loaded upgrades
            RecalculateAllStats();
        }
    }

    public void ClearUpgrades()
    {
        _activeUpgrades.Clear();
        GD.Print("Cleared all active upgrades");
    }
}
