using Godot;
using SpaceTower.Scripts.Enemies.Base;

namespace SpaceTower.Scripts.Skills.SkillEffects;

public partial class WhirlwindEffect : Node2D
{
    [Export] public float Duration = 0.5f;  // Visual animation duration
    [Export] public float RotationSpeed = 10f;  // Rotation speed (radians/sec)

    private float _damage;
    private float _radius;

    public void Initialize(float damage, float radius)
    {
        _damage = damage;
        _radius = radius;
    }

    public override void _Ready()
    {
        // Apply damage immediately on spawn
        ApplyDamage();

        // Destroy after visual animation duration
        GetTree().CreateTimer(Duration).Timeout += QueueFree;
    }

    public override void _Process(double delta)
    {
        // Rotate for visual effect
        Rotation += RotationSpeed * (float)delta;
    }

    private void ApplyDamage()
    {
        var enemies = GetTree().GetNodesInGroup("enemies");
        int hitCount = 0;

        foreach (Node node in enemies)
        {
            if (node is Enemy enemy)
            {
                float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
                if (distance < _radius)
                {
                    enemy.TakeDamage(_damage);
                    hitCount++;
                }
            }
        }

        GD.Print($"Whirlwind hit {hitCount} enemies!");
    }
}
