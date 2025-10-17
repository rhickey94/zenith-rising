using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Base;
using ZenithRising.Scripts.Skills.Data;

namespace ZenithRising.Scripts.Skills.Effects;

public partial class FireballProjectile : CollisionSkillEffect
{
    [Export] public float Speed = 400.0f;
    [Export] public float MaxRange = 500.0f;

    [Export] public PackedScene ExplosionEffectScene; // Optional visual effect

    private float _directDamage;
    private float _explosionDamage;
    private float _explosionRadius;

    private Vector2 _startPosition;
    private bool _hasExploded = false;

    public override void Initialize(Skill sourceSkill, Player caster, Vector2 direction)
    {
        base.Initialize(sourceSkill, caster, direction);

        _directDamage = sourceSkill.BaseDamage;
        _explosionDamage = sourceSkill.BaseDamage;
        _explosionRadius = sourceSkill.Radius;
        _startPosition = caster.GlobalPosition;

        Rotation = direction.Angle();

        // Apply mastery bonuses
        ApplyMasteryBonuses();
    }

    private void ApplyMasteryBonuses()
    {
        switch (_sourceSkill.CurrentTier)
        {
            case SkillMasteryTier.Silver:
                _directDamage *= 1.5f;
                _explosionDamage *= 1.5f;
                break;
            case SkillMasteryTier.Gold:
                _directDamage *= 2.0f;
                _explosionDamage *= 2.0f;
                _explosionRadius *= 1.2f;
                break;
            case SkillMasteryTier.Diamond:
                _directDamage *= 3.0f;
                _explosionDamage *= 3.0f;
                _explosionRadius *= 1.5f;
                Speed *= 1.3f;
                break;
        }
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
        if (_hasExploded)
        {
            return;
        }

        if (body is Enemy enemy)
        {
            // Deal direct hit damage
            float healthBefore = enemy.Health;
            float damage = CalculateDamage(_directDamage);
            enemy.TakeDamage(damage);
            GD.Print($"Fireball hit enemy for {damage} damage");

            // Track kill if enemy died from direct hit
            if (healthBefore > 0 && enemy.Health <= 0)
            {
                OnEnemyKilled(enemy);
            }

            // Then explode
            Explode();
        }
    }

    private void Explode()
    {
        if (_hasExploded)
        {
            return;
        }

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
                if (distance <= _explosionRadius)
                {
                    float healthBefore = enemy.Health;
                    float damage = CalculateDamage(_explosionDamage);
                    enemy.TakeDamage(damage);
                    GD.Print($"Explosion hit enemy for {damage} damage");
                    hitCount++;

                    // Track kill if enemy died from explosion
                    if (healthBefore > 0 && enemy.Health <= 0)
                    {
                        OnEnemyKilled(enemy);
                    }
                }
            }
        }

        GD.Print($"Explosion hit {hitCount} enemies");

        // Destroy the fireball
        QueueFree();
    }
}
