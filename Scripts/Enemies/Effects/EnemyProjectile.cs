using Godot;
using SpaceTower.Scripts.PlayerScripts;

namespace SpaceTower.Scripts.Enemies.Effects;

public partial class EnemyProjectile : Area2D
{
    [Export] public float Speed = 300.0f;
    [Export] public float Damage = 10.0f;
    [Export] public float Lifetime = 5.0f;
    private Vector2 _direction;

    public void Initialize(Vector2 direction, float damage)
    {
        _direction = direction.Normalized();
        Damage = damage;
        Rotation = _direction.Angle();
    }

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        GetTree().CreateTimer(Lifetime).Timeout += () => QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direction * Speed * (float)delta;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.TakeDamage(Damage);
            QueueFree();  // Destroy on hit (no pierce for enemies)
        }
    }
}
