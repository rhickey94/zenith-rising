using System.Collections.Generic;
using Godot;
using SpaceTower.Scripts.Enemies.Base;

namespace SpaceTower.Scripts.Effects;

public partial class MeleeAttack : Area2D
{
    [Export] public float BaseDamage = 10.0f;
    [Export] public float Lifetime = 0.3f;
    private readonly HashSet<Enemy> _hitEnemies = [];

    public override void _Ready()
    {
        // Connect to body_entered signal

        BodyEntered += OnBodyEntered;
    }

    public void Initialize(float damageBonus, Vector2 direction)
    {
        BaseDamage *= 1 + damageBonus;

        Rotation = direction.Angle();

        GD.Print($"Melee initialized with {BaseDamage} damage");
    }

    public override void _PhysicsProcess(double delta)
    {
        Lifetime -= (float)delta;

        if (Lifetime <= 0)
        {
            QueueFree();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print($"Body entered: {body.Name} (type: {body.GetType().Name})");

        if (body is Enemy enemy && !_hitEnemies.Contains(enemy))
        {
            enemy.TakeDamage(BaseDamage);
            _hitEnemies.Add(enemy);
            GD.Print($"Melee hit {enemy.Name} for {BaseDamage} damage!");
        }
    }
}
