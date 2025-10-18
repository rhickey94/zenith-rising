using Godot;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.PlayerScripts.Components;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Entities.Projectiles;

/// <summary>
/// Base class for skill effects that need collision detection (Area2D).
/// Use this for projectiles, melee attacks, and other effects that interact with Area2D/CharacterBody2D.
/// For instant effects without collision (like Whirlwind), use SkillEffect instead.
/// </summary>
public abstract partial class DamageEntityBase : Area2D
{
    protected Skill _sourceSkill;
    protected Player _caster;
    protected StatsManager _statsManager;
    protected Vector2 _direction;

    /// <summary>
    /// Standardized initialization for all collision-based skill effects.
    /// Called by executors after instantiating the effect.
    /// </summary>
    /// <param name="sourceSkill">The skill resource that spawned this effect</param>
    /// <param name="caster">The player who cast the skill</param>
    /// <param name="direction">The direction the skill was cast (normalized)</param>
    public virtual void Initialize(Skill sourceSkill, Player caster, Vector2 direction)
    {
        _sourceSkill = sourceSkill;
        _caster = caster;
        _direction = direction;

        _statsManager = _caster.GetNode<StatsManager>("StatsManager");
        if (_statsManager == null)
        {
            GD.PrintErr("CollisionSkillEffect.Initialize: Could not find StatsManager on caster!");
        }
    }

    // Helper method for standardized damage calculation
    protected float CalculateDamage(float baseDamage, bool forceCrit = false)
    {
        if (_sourceSkill == null || _statsManager == null)
        {
            GD.PrintErr($"{GetType().Name}: Cannot calculate damage - missing skill or stats!");
            return baseDamage;
        }

        return CombatSystem.CalculateDamage(baseDamage, _statsManager, _sourceSkill.DamageType, forceCrit);
    }

    /// <summary>
    /// Called when this effect kills an enemy.
    /// Reports the kill to the source skill for mastery tracking.
    /// </summary>
    /// <param name="enemy">The enemy that was killed</param>
    protected void OnEnemyKilled(Enemy enemy)
    {
        if (_sourceSkill != null)
        {
            _sourceSkill.RecordKill();
            GD.Print($"{_sourceSkill.SkillName} kill count: {_sourceSkill.KillCount}");
        }
    }
}
