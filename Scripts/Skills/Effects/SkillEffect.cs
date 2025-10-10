using Godot;
using SpaceTower.Scripts.Enemies.Base;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;

namespace SpaceTower.Scripts.Skills.Effects;

/// <summary>
/// Base class for all skill effects. Provides standardized initialization and kill tracking.
/// Inherits from Node2D - use CollisionSkillEffect for effects that need Area2D collision detection.
/// </summary>
public abstract partial class SkillEffect : Node2D
{
    protected Skill _sourceSkill;
    protected Player _caster;
    protected Vector2 _direction;

    /// <summary>
    /// Standardized initialization for all skill effects.
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
