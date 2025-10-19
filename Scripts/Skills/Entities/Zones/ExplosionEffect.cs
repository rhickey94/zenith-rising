using Godot;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Entities.Zones;

public partial class ExplosionEffect : DamageEntityBase
{
    private float _explosionRadius;
    private float _explosionDamage;
    private CollisionShape2D _collisionShape;

    public override void Initialize(Skill skill, Player caster, Vector2 direction)
    {
        base.Initialize(skill, caster, direction);
        _explosionDamage = skill.ExplosionDamage;
        _explosionRadius = skill.ExplosionRadius;
    }

    public override void _Ready()
    {
        // Set collision shape to match explosion radius
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        if (_collisionShape?.Shape is CircleShape2D circle)
        {
            circle.Radius = _explosionRadius;
        }

        // Connect collision signal
        BodyEntered += OnBodyEntered;

        var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        if (animationPlayer != null)
        {
            animationPlayer.Play("explode");
        }

        // Visual effect duration, then cleanup
        GetTree().CreateTimer(0.5f).Timeout += QueueFree;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Enemy enemy)
        {
            float healthBefore = enemy.Health;
            float damage = CalculateDamage(_explosionDamage);
            enemy.TakeDamage(damage);

            // Track kill if enemy died
            if (healthBefore > 0 && enemy.Health <= 0)
            {
                OnEnemyKilled(enemy);
            }
        }
    }
}
