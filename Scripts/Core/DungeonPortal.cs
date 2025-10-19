using Godot;

namespace ZenithRising.Scripts.Core;

public partial class DungeonPortal : Area2D
{
    [Export] public string DungeonScenePath = "res://Scenes/Core/dungeon.tscn";

    private bool _playerInRange = false;
    private Label _promptLabel;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;

        // Create prompt label
        _promptLabel = new Label
        {
            Text = "[E] Enter Dungeon",
            Position = new Vector2(-60, -40), // Above portal
            Visible = false
        };
        AddChild(_promptLabel);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (_playerInRange && @event.IsActionPressed("interact"))
        {
            EnterDungeon();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            _playerInRange = true;
            _promptLabel.Visible = true;
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            _playerInRange = false;
            _promptLabel.Visible = false;
        }
    }

    private void EnterDungeon()
    {
        GetTree().ChangeSceneToFile(DungeonScenePath);
    }
}
