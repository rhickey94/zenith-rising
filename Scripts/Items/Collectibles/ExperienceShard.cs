using Godot;
using SpaceTower.Scripts.PlayerScripts;

namespace SpaceTower.Scripts.Items.Collectibles;

public partial class ExperienceShard : Area2D
{
    [Export] public int ExperienceValue = 10;
    [Export] public float MoveSpeed = 300.0f;
    [Export] public float PickupRadius = 80.0f;

    private Player _player;
    private bool _isBeingCollected = false;

    public override void _Ready()
    {
        // Find player

        _player = GetTree().GetFirstNodeInGroup("player") as Player;

        // Detect collision with player

        BodyEntered += OnBodyEntered;

        // Add floating animation

        var tween = CreateTween();
        tween.SetLoops();
        tween.TweenProperty(this, "position:y", Position.Y - 3, 0.5f);
        tween.TweenProperty(this, "position:y", Position.Y + 3, 0.5f);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null || !IsInstanceValid(_player) || _isBeingCollected)
        {
            return;
        }

        // Move toward player if close enough


        float distance = GlobalPosition.DistanceTo(_player.GlobalPosition);
        if (distance < PickupRadius)
        {
            Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
            GlobalPosition += direction * MoveSpeed * (float)delta;
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player player && !_isBeingCollected)
        {
            _isBeingCollected = true;

            // Give XP

            player.AddExperience(ExperienceValue);

            // Pickup animation

            var tween = CreateTween();
            tween.TweenProperty(this, "scale", Scale * 1.3f, 0.1f);
            tween.TweenProperty(this, "modulate:a", 0.0f, 0.1f);
            tween.TweenCallback(Callable.From(QueueFree));
        }
    }
}
