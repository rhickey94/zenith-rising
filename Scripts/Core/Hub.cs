using Godot;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.PlayerScripts.Components;

namespace ZenithRising.Scripts.Core;

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
    }

    private void InitializePlayer()
    {
        Player.Initialize();
    }
}
