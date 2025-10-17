using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.PlayerScripts.Components;
using ZenithRising.Scripts.Skills.Base;
using ZenithRising.Scripts.Skills.Data;

namespace ZenithRising.Scripts.Skills.Effects;

public partial class BasicProjectile : CollisionSkillEffect
{
    [Export] public float Speed = 600.0f;
    [Export] public float BaseDamage = 25.0f;
    [Export] public float Lifetime = 3.0f;

    private int _pierceCount = 0;
    private int _maxPierce = 0;
    private float _totalDamage;

    public override void Initialize(Skill sourceSkill, Player caster, Vector2 direction)
    {
        base.Initialize(sourceSkill, caster, direction);

        if (sourceSkill is not ProjectileSkill skill)
        {
            GD.PrintErr("BasicProjectile: sourceSkill is not ProjectileSkill!");
            return;
        }

        BaseDamage = skill.DirectDamage;
        _totalDamage = BaseDamage;

        // Apply upgrade bonuses
        _maxPierce = _statsManager.ProjectilePierceCount;

        // Apply mastery bonuses
        ApplyMasteryBonuses();

        Rotation = direction.Angle();
    }

    private void ApplyMasteryBonuses()
    {
        switch (_sourceSkill.CurrentTier)
        {
            case SkillMasteryTier.Silver:
                _totalDamage *= 1.5f;
                break;
            case SkillMasteryTier.Gold:
                _totalDamage *= 2.0f;
                Speed *= 1.2f;
                break;
            case SkillMasteryTier.Diamond:
                _totalDamage *= 3.0f;
                Speed *= 1.5f;
                _maxPierce += 2; // +2 pierce at Diamond
                break;
        }
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
