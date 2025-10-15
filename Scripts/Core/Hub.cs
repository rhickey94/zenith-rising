using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.PlayerScripts.Components;

namespace SpaceTower.Scripts.Core;

public partial class Hub : Node2D
{
    [Export] public string DungeonScenePath = "res://Scenes/Core/dungeon.tscn";
    [Export] public Player Player;

    public override void _Ready()
    {
        if (Player == null)
        {
            GD.PrintErr("Hub: Player not assigned!");
            return;
        }

        // Initialize player (loads save if exists)
        CallDeferred(MethodName.InitializePlayer);

        GD.Print("Hub loaded - safe zone");
    }

    private void InitializePlayer()
    {
        Player.Initialize();
        GD.Print($"Player initialized - CurrentSpeed: {Player.GetNode<StatsManager>("StatsManager").CurrentSpeed}");
    }
}
