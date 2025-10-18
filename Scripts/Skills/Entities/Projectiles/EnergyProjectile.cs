using Godot;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Entities.Projectiles;

public partial class EnergyProjectile : DamageEntityBase
{
    private float _speed;
    private float _lifetime;
    private int _pierceCount = 0;
    private int _maxPierce = 0;
    private float _totalDamage;

    public override void Initialize(Skill skill, Player caster, Vector2 direction)
    {
        base.Initialize(skill, caster, direction);
        _speed = skill.ProjectileSpeed;
        _lifetime = skill.ProjectileLifetime;

        float damageToUse = skill.ProjectileDamage > 0 ? skill.ProjectileDamage : skill.BaseDamage;
        _totalDamage = damageToUse;

        // Apply upgrade bonuses
        _maxPierce = _statsManager.ProjectilePierceCount;

        // Apply mastery bonuses
        ApplyMasteryBonuses();

        Rotation = direction.Angle();
    }

    private void ApplyMasteryBonuses()
    {
        if (_sourceSkill == null)
        {
            return;
        }

        float damageMultiplier = _sourceSkill.CurrentTier switch
        {
            SkillMasteryTier.Bronze => 1f + _sourceSkill.BronzeDamageBonus,
            SkillMasteryTier.Silver => 1f + _sourceSkill.SilverDamageBonus,
            SkillMasteryTier.Gold => 1f + _sourceSkill.GoldDamageBonus,
            SkillMasteryTier.Diamond => 1f + _sourceSkill.DiamondDamageBonus,
            _ => 1f
        };

        _totalDamage *= damageMultiplier;

        // Diamond tier bonuses: +50% speed, +2 pierce
        // (These could also be data-driven in the future)
        if (_sourceSkill.CurrentTier == SkillMasteryTier.Diamond)
        {
            _speed *= 1.5f;
            _maxPierce += 2;
        }
    }

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;

        GetTree().CreateTimer(_lifetime).Timeout += QueueFree;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direction * _speed * (float)delta;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Enemy enemy)
        {
            float healthBefore = enemy.Health;
            float damage = CalculateDamage(_totalDamage);
            enemy.TakeDamage(damage);
            GD.Print($"Basic projectile hit enemy for {damage} damage");

            // Track kill if enemy died
            if (healthBefore > 0 && enemy.Health <= 0)
            {
                OnEnemyKilled(enemy);
            }

            _pierceCount++;
            if (_pierceCount > _maxPierce)
            {
                QueueFree();
            }
        }
    }
}
