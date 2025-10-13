using System.Collections.Generic;
using Godot;
using SpaceTower.Progression.Upgrades;
using SpaceTower.Scripts.Core;
using SpaceTower.Scripts.Enemies.Base;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;
using SpaceTower.Scripts.Skills.Data;

namespace SpaceTower.Scripts.Skills.Effects;

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

        var skill = sourceSkill as MeleeAttackSkill;
        if (skill == null)
        {
            GD.PrintErr("MeleeAttackEffect: sourceSkill is not MeleeAttackSkill!");
            return;
        }

        _damage = skill.Damage;
        _lifetime = skill.Lifetime;

        // Apply mastery bonuses
        ApplyMasteryBonuses();

        Rotation = direction.Angle();

        GD.Print($"Melee initialized with {_damage} damage");
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
        GD.Print($"Body entered: {body.Name} (type: {body.GetType().Name})");

        if (body is Enemy enemy && !_hitEnemies.Contains(enemy))
        {
            float healthBefore = enemy.Health;
            enemy.TakeDamage(CombatSystem.CalculateDamage(_damage, _caster));
            _hitEnemies.Add(enemy);

            GD.Print($"Melee hit {enemy.Name} for {_damage} damage!");

            // Track kill if enemy died
            if (healthBefore > 0 && enemy.Health <= 0)
            {
                OnEnemyKilled(enemy);
            }
        }
    }
}
