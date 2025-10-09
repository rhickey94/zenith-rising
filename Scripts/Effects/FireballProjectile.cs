using Godot;
using SpaceTower.Scripts.Enemies.Base;

namespace SpaceTower.Scripts.Effects;

public partial class FireballProjectile : Area2D
{
    [Export] public float Speed = 400.0f;
    [Export] public float DirectDamage = 40.0f;
    [Export] public float ExplosionDamage = 25.0f;
    [Export] public float ExplosionRadius = 100.0f;
    [Export] public float MaxRange = 500.0f;

    [Export] public PackedScene ExplosionEffectScene; // Optional visual effect

    private Vector2 _direction;
    private Vector2 _startPosition;
    private bool _hasExploded = false;

    public void Initialize(Vector2 direction, Vector2 startPosition)
    {
        _direction = direction.Normalized();
        _startPosition = startPosition;
        Rotation = direction.Angle();
    }

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move forward
        Position += _direction * Speed * (float)delta;

        // Check if we've traveled max distance
        float distanceTraveled = GlobalPosition.DistanceTo(_startPosition);
        if (distanceTraveled >= MaxRange)
        {
            Explode();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (_hasExploded) return;

        if (body is Enemy enemy)
        {
            // Deal direct hit damage
            enemy.TakeDamage(DirectDamage);

            // Then explode
            Explode();
        }
    }

    private void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;

        GD.Print($"Fireball exploded at {GlobalPosition}");

        // Spawn explosion visual effect
        if (ExplosionEffectScene != null)
        {
            var explosion = ExplosionEffectScene.Instantiate<Node2D>();
            explosion.GlobalPosition = GlobalPosition;
            GetTree().Root.AddChild(explosion);
        }

        // Damage all enemies in radius
        var enemies = GetTree().GetNodesInGroup("enemies");
        int hitCount = 0;

        foreach (Node node in enemies)
        {
            if (node is Enemy enemy)
            {
                float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
                if (distance <= ExplosionRadius)
                {
                    enemy.TakeDamage(ExplosionDamage);
                    hitCount++;
                }
            }
        }

        GD.Print($"Explosion hit {hitCount} enemies");

        // Destroy the fireball
        QueueFree();
    }
}
