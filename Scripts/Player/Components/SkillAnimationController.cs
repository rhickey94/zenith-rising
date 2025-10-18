using Godot;
using System;
using System.Collections.Generic;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.Skills.Base;
using ZenithRising.Scripts.Skills.Entities;

namespace ZenithRising.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class SkillAnimationController : Node
{
    // Injected dependencies
    private Player _player;
    private StatsManager _statsManager;
    private AnimationPlayer _animationPlayer;

    // Hitbox references
    private Area2D _meleeHitbox;
    private Area2D _aoeHitbox;

    // State
    private readonly HashSet<Enemy> _hitEnemiesThisCast = [];
    private Skill _currentCastingSkill;

    // Exports
    [Export] public PackedScene ProjectileScene { get; set; }

    public void Initialize(Player player, StatsManager statsManager, AnimationPlayer animationPlayer)
    {
        _player = player;
        _statsManager = statsManager;
        _animationPlayer = animationPlayer;
    }

    public override void _Ready()
    {
        // Get hitbox nodes from parent Player
        _meleeHitbox = GetParent().GetNode<Area2D>("MeleeHitbox");
        if (_meleeHitbox != null)
        {
            _meleeHitbox.BodyEntered += OnMeleeHitboxBodyEntered;
        }

        _aoeHitbox = GetParent().GetNode<Area2D>("AOEHitbox");
        if (_aoeHitbox != null)
        {
            _aoeHitbox.BodyEntered += OnAOEHitboxBodyEntered;
        }

        if (ProjectileScene == null)
        {
            GD.PrintErr("Player: ProjectileScene not assigned!");
        }
    }

    public void SetCurrentSkill(Skill skill)
    {
        _currentCastingSkill = skill;
    }

    public void ClearCurrentSkill()
    {
        _currentCastingSkill = null;
    }

    // ===== ANIMATION CALLBACKS (Called from AnimationPlayer) =====

    public void EnableMeleeHitbox()
    {
        if (_meleeHitbox == null || _currentCastingSkill == null)
        {
            GD.PrintErr("EnableMeleeHitbox: Hitbox or skill not ready!");
            return;
        }

        _hitEnemiesThisCast.Clear();
        _meleeHitbox.Monitoring = true;
        GD.Print("Melee hitbox enabled");
    }

    public void DisableMeleeHitbox()
    {
        if (_meleeHitbox == null || _currentCastingSkill == null)
        {
            GD.PrintErr("DisableMeleeHitbox: Hitbox or skill not ready!");
            return;
        }

        _meleeHitbox.Monitoring = false;
        GD.Print("Melee hitbox disabled");
    }

    public void EnableAOEHitbox()
    {
        if (_aoeHitbox == null || _currentCastingSkill == null)
        {
            GD.PrintErr("EnableAOEHitbox: Hitbox or skill not ready!");
            return;
        }
        _hitEnemiesThisCast.Clear();
        _aoeHitbox.Monitoring = true;
        GD.Print("AOE hitbox enabled");
    }

    public void DisableAOEHitbox()
    {
        if (_aoeHitbox == null || _currentCastingSkill == null)
        {
            GD.PrintErr("DisableAOEHitbox: Hitbox or skill not ready!");
            return;
        }

        _aoeHitbox.Monitoring = false;
        GD.Print("AOE hitbox disabled");
    }

    public void SpawnWaveProjectiles()
    {
        if (_currentCastingSkill == null || _player == null)
        {
            GD.PrintErr("SpawnWaveProjectiles: Missing skill or player reference!");
            return;
        }

        if (_currentCastingSkill.ProjectileCount <= 0)
        {
            return;
        }


        if (ProjectileScene == null)
        {
            GD.PrintErr("SpawnWaveProjectiles: ProjectileScene not assigned!");
            return;
        }

        Vector2 playerDirection = _player.GetLastDirection();
        Vector2 spawnOffset = playerDirection * 40f;
        Vector2 spawnPos = _player.GlobalPosition + spawnOffset;

        float baseAngle = Mathf.Atan2(playerDirection.Y, playerDirection.X);
        int projectileCount = _currentCastingSkill.ProjectileCount;
        float spreadAngle = _currentCastingSkill.ProjectileSpreadAngle;

        for (int i = 0; i < projectileCount; i++)
        {
            SpawnSingleProjectile(spawnPos, baseAngle, i, projectileCount, spreadAngle);
        }
    }

    // ===== COLLISION HANDLERS =====

    private void OnMeleeHitboxBodyEntered(Node2D body)
    {
        if (body is not Enemy enemy)
        {
            return;
        }

        if (_hitEnemiesThisCast.Contains(enemy))
        {
            return;
        }

        _hitEnemiesThisCast.Add(enemy);
        ApplyHitboxDamage(enemy);
    }

    private void OnAOEHitboxBodyEntered(Node2D body)
    {
        if (body is not Enemy enemy)
        {
            return;
        }

        if (_hitEnemiesThisCast.Contains(enemy))
        {
            return;
        }

        _hitEnemiesThisCast.Add(enemy);
        ApplyHitboxDamage(enemy);
    }

    private void ApplyHitboxDamage(Enemy enemy)
    {
        if (_currentCastingSkill == null || _statsManager == null)
        {
            GD.PrintErr("ApplyHitboxDamage: No active skill or stats!");
            return;
        }

        float baseDamage = _currentCastingSkill.BaseDamage;
        float damage = CombatSystem.CalculateDamage(
            baseDamage,
            _statsManager,
            _currentCastingSkill.DamageType
        );

        float healthBefore = enemy.Health;
        enemy.TakeDamage(damage);
        GD.Print($"{_currentCastingSkill.SkillName} hit {enemy.Name} for {damage} damage");

        if (healthBefore > 0 && enemy.Health <= 0)
        {
            _currentCastingSkill.RecordKill();
        }
    }

    private void SpawnSingleProjectile(Vector2 spawnPos, float baseAngle, int index, int totalCount, float spreadAngle)
    {
        float angleOffset = 0f;

        if (totalCount > 1)
        {
            float step = (spreadAngle * 2) / (totalCount - 1);
            angleOffset = -spreadAngle + (step * index);
        }

        float finalAngle = baseAngle + Mathf.DegToRad(angleOffset);
        var direction = new Vector2(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle));

        var projectile = ProjectileScene.Instantiate<Node2D>();
        _player.GetTree().Root.AddChild(projectile);
        projectile.GlobalPosition = spawnPos;

        if (projectile is SkillEntityBase entity)
        {
            entity.Initialize(_currentCastingSkill, _player, direction);
        }
    }
}
