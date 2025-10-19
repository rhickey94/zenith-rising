using Godot;
using ZenithRising.Scripts.Core.Config;
using ZenithRising.Scripts.Skills.Balance;

namespace ZenithRising.Scripts.Core;

public partial class GameBalance : Node
{
    public static GameBalance Instance { get; private set; }

    // Individual .tres files (assigned in Godot editor)
    [Export] public PlayerStatsConfig PlayerStatsResource { get; set; }
    [Export] public CharacterProgressionConfig CharacterProgressionResource { get; set; }
    [Export] public CombatSystemConfig CombatSystemResource { get; set; }
    [Export] public EnemyConfig EnemyResource { get; set; }
    [Export] public UpgradeSystemConfig UpgradeSystemResource { get; set; }
    [Export] public SkillBalanceDatabase SkillDatabase { get; set; }

    // Wrapper populated in _Ready()
    public BalanceConfig Config { get; private set; }

    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            GD.PrintErr("Multiple GameBalance instances detected! Destroying duplicate.");
            QueueFree();
            return;
        }

        Instance = this;

        if (PlayerStatsResource == null)
        {
            GD.PrintErr("GameBalance: PlayerStats not assigned!");
        }

        if (CharacterProgressionResource == null)
        {
            GD.PrintErr("GameBalance: CharacterProgression not assigned!");
        }

        if (CombatSystemResource == null)
        {
            GD.PrintErr("GameBalance: CombatSystem not assigned!");
        }

        if (EnemyResource == null)
        {
            GD.PrintErr("GameBalance: Enemy not assigned!");
        }

        if (UpgradeSystemResource == null)
        {
            GD.PrintErr("GameBalance: UpgradeSystem not assigned!");
        }

        Config = new()
        {
            PlayerStats = PlayerStatsResource,
            CharacterProgression = CharacterProgressionResource,
            CombatSystem = CombatSystemResource,
            Enemy = EnemyResource,
            UpgradeSystem = UpgradeSystemResource
        };

        if (SkillDatabase == null)
        {
            GD.PrintErr("GameBalance: SkillBalanceDatabase not assigned!");
        }
    }
}
