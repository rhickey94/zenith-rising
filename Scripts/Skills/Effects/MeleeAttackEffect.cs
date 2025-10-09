using System.Collections.Generic;
using Godot;
using SpaceTower.Scripts.Enemies.Base;

namespace SpaceTower.Scripts.Skills.Effects;

public partial class MeleeAttackEffect : Area2D
{
    private float _damage = 10.0f;
    private float _lifetime = 0.3f;
    private readonly HashSet<Enemy> _hitEnemies = [];

    public override void _Ready()
    {
        // Connect to body_entered signal

        BodyEntered += OnBodyEntered;
    }

    public void Initialize(Vector2 direction, float baseDamage, float lifetime, float damageBonus)
    {
        _damage = baseDamage * (1 + damageBonus);
        _lifetime = lifetime;

        Rotation = direction.Angle();

        GD.Print($"Melee initialized with {_damage} damage");
    }

    public override void _PhysicsProcess(double delta)
    {
        _lifetime -= (float)delta;

        if (_lifetime <= 0)
        {
            QueueFree();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print($"Body entered: {body.Name} (type: {body.GetType().Name})");

        if (body is Enemy enemy && !_hitEnemies.Contains(enemy))
        {
            enemy.TakeDamage(_damage);
            _hitEnemies.Add(enemy);
            GD.Print($"Melee hit {enemy.Name} for {_damage} damage!");
        }
    }
}
