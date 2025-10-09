using Godot;
using SpaceTower.Scripts.Enemies.Base;

namespace SpaceTower.Scripts.Skills.Effects;

public partial class BasicProjectile : Area2D
{
    [Export] public float Speed = 600.0f;
    [Export] public float BaseDamage = 25.0f;
    [Export] public float Lifetime = 3.0f;

    private Vector2 _direction;
    private int _pierceCount = 0; // Number of enemies this projectile pierced

    private int _maxPierce = 0; // Maximum number of enemies this projectile can pierce

    private float _damageMultiplier = 1.0f;

    public void Initialize(Vector2 direction, float damageBonus = 0f, int pierce = 0)
    {
        _direction = direction.Normalized();
        Rotation = direction.Angle();
        _damageMultiplier = 1.0f + damageBonus; // Reset multiplier for each projectile

        _maxPierce = pierce;
    }

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;

        GetTree().CreateTimer(Lifetime).Timeout += QueueFree;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direction * Speed * (float)delta;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Enemy enemy)
        {
            var totalDamage = BaseDamage * _damageMultiplier;

            enemy.TakeDamage(totalDamage);

            _pierceCount++;
            if (_pierceCount >= _maxPierce)
            {
                QueueFree();
            }
        }
    }
}
