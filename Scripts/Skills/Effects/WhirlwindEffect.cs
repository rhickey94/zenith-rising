using Godot;

namespace ZenithRising.Scripts.Skills.Effects;

public partial class WhirlwindEffect : Node2D
{
    [Export] public float Duration = 1.2f;  // Visual animation duration
    [Export] public float RotationSpeed = 10f;  // Rotation speed (radians/sec)

    public override void _Ready()
    {
        GetTree().CreateTimer(Duration).Timeout += QueueFree;
    }

    public override void _Process(double delta)
    {
        Rotation += RotationSpeed * (float)delta;
    }
}
