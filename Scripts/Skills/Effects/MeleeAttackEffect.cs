using System.Collections.Generic;
using Godot;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Effects;

public partial class MeleeAttackEffect : CollisionSkillEffect
{
    private float _damage;
    private float _lifetime;
    private readonly HashSet<Enemy> _hitEnemies = [];

    public override void _Ready()
    {
        // Connect to body_entered signal
        BodyEntered += OnBodyEntered;
    }

    public override void Initialize(Skill sourceSkill, Player caster, Vector2 direction)
    {
        base.Initialize(sourceSkill, caster, direction);

        _damage = sourceSkill.Damage;
        _lifetime = sourceSkill.Cooldown;

        // Apply mastery bonuses
        ApplyMasteryBonuses();

        Rotation = direction.Angle();
    }

    private void ApplyMasteryBonuses()
    {
        switch (_sourceSkill.CurrentTier)
        {
            case SkillMasteryTier.Silver:
                _damage *= 1.5f;
                break;
            case SkillMasteryTier.Gold:
                _damage *= 2.0f;
                _lifetime += 0.2f; // Lasts longer
                break;
            case SkillMasteryTier.Diamond:
                _damage *= 3.0f;
                _lifetime += 0.5f;
                break;
        }
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
        if (body is Enemy enemy && !_hitEnemies.Contains(enemy))
        {
            float healthBefore = enemy.Health;
            float damage = CalculateDamage(_damage);
            enemy.TakeDamage(damage);
            GD.Print($"Melee attack hit enemy for {damage} damage");
            _hitEnemies.Add(enemy);

            // Track kill if enemy died
            if (healthBefore > 0 && enemy.Health <= 0)
            {
                OnEnemyKilled(enemy);
            }
        }
    }
}
