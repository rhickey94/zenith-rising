using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Skills.Balance;

namespace SpaceTower.Scripts.Core;

public partial class GameBalance : Node
{
    public static GameBalance Instance { get; private set; }

    [Export] public BalanceConfig Config { get; set; }
    [Export] public SkillBalanceDatabase SkillDatabase { get; set; }

    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            GD.PrintErr("Multiple GameBalance instances detected! Destroying duplicate.");
            QueueFree();
            return;
        }

        Instance = this;

        if (Config == null)
        {
            GD.PrintErr("GameBalance: BalanceConfig not assigned!");
        }

        if (SkillDatabase == null)
        {
            GD.PrintErr("GameBalance: SkillBalanceDatabase not assigned!");
        }

        GD.Print("GameBalance initialized successfully");
    }
}
