using Godot;

namespace ZenithRising.Scripts.Core.Config;

public class BalanceConfig
{
    public PlayerStatsConfig PlayerStats { get; set; }
    public CharacterProgressionConfig CharacterProgression { get; set; }
    public CombatSystemConfig CombatSystem { get; set; }
    public EnemyConfig Enemy { get; set; }
    public UpgradeSystemConfig UpgradeSystem { get; set; }
}
