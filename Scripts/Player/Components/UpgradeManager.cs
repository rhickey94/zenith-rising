using System.Collections.Generic;
using System.Linq;
using Godot;
using SpaceTower.Progression;

namespace SpaceTower.Scripts.PlayerScripts.Components;

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
        // Track upgrade
        if (_activeUpgrades.ContainsKey(upgrade.Type))
        {
            _activeUpgrades[upgrade.Type] += upgrade.Value;
        }
        else
        {
            _activeUpgrades[upgrade.Type] = upgrade.Value;
        }

        // Apply upgrade immediately
        ApplyUpgradeEffect(upgrade.Type);

        GD.Print($"Selected: {upgrade.UpgradeName}");
        GD.Print($"New Value: {_activeUpgrades[upgrade.Type]}");
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

    private void ApplyUpgradeEffect(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.DamagePercent:
                // Apply damage percent upgrade logic
                break;
            case UpgradeType.AttackSpeed:
                _player.FireRate *= 1 - _activeUpgrades[upgradeType];
                break;
            case UpgradeType.MovementSpeed:
                _player.Speed *= 1 + _activeUpgrades[upgradeType];
                break;
            case UpgradeType.MaxHealth:
                _statsManager.IncreaseMaxHealth(_activeUpgrades[upgradeType]);
                break;
            case UpgradeType.PickupRadius:
                // Apply pickup radius upgrade logic
                break;
            case UpgradeType.ProjectilePierce:
                // Apply projectile pierce upgrade logic
                break;
            case UpgradeType.CritChance:
                // Apply crit chance upgrade logic
                break;
            case UpgradeType.HealthRegen:
                // Apply health regen upgrade logic
                break;
        }
    }
}
