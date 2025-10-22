using Godot;
using System.Collections.Generic;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.Skills.Base;
using ZenithRising.Scripts.Skills.Entities;
using ZenithRising.Scripts.Skills.Entities.Visuals;
using ZenithRising.Scripts.Skills.Entities.Zones;

namespace ZenithRising.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class SkillEffectController : Node
{
    // Injected dependencies
    private Player _player;
    private StatsManager _statsManager;
    private BuffManager _buffManager;
    private ForcedMovementController _forcedMovementController;

    // Hitbox references
    private Area2D _meleeHitbox;
    private Area2D _aoeHitbox;

    // State
    private readonly HashSet<Enemy> _hitEnemiesThisCast = [];
    private Skill _currentCastingSkill;
    private Vector2 _currentAttackDirection;

    // Exports
    [Export] public PackedScene ProjectileScene { get; set; }
    [Export] public PackedScene WhirlwindVisualScene { get; set; }
    [Export] public PackedScene ExplosionEffectScene { get; set; }

    public void Initialize(Player player, StatsManager statsManager, BuffManager buffManager, ForcedMovementController forcedMovementController)
    {
        _player = player;
        _statsManager = statsManager;
        _buffManager = buffManager;
        _forcedMovementController = forcedMovementController;
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
            GD.PrintErr("SkillAnimationController: ProjectileScene not assigned!");
        }


        if (WhirlwindVisualScene == null)
        {
            GD.PrintErr("SkillAnimationController: WhirlwindVisualScene not assigned!");
        }

        if (ExplosionEffectScene == null)
        {
            GD.PrintErr("SkillAnimationController: ExplosionEffectScene not assigned!");
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

        // Get attack direction and position hitbox accordingly
        _currentAttackDirection = _player.GetAttackDirection();
        UpdateMeleeHitboxPosition(_currentAttackDirection);

        _meleeHitbox.Monitoring = true;
    }

    public void DisableMeleeHitbox()
    {
        if (_meleeHitbox == null || _currentCastingSkill == null)
        {
            GD.PrintErr("DisableMeleeHitbox: Hitbox or skill not ready!");
            return;
        }

        _meleeHitbox.Monitoring = false;
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
    }

    public void DisableAOEHitbox()
    {
        if (_aoeHitbox == null || _currentCastingSkill == null)
        {
            GD.PrintErr("DisableAOEHitbox: Hitbox or skill not ready!");
            return;
        }

        _aoeHitbox.Monitoring = false;
    }

    public void SpawnWaveProjectiles()
    {
        if (_currentCastingSkill == null || _player == null)
        {
            GD.PrintErr($"SpawnWaveProjectiles: Missing references! Skill={_currentCastingSkill != null}, Player={_player != null}");
            return;
        }

        if (_currentCastingSkill.ProjectileCount <= 0)
        {
            GD.PrintErr($"SpawnWaveProjectiles: ProjectileCount is {_currentCastingSkill.ProjectileCount}, aborting!");
            return;
        }

        if (ProjectileScene == null)
        {
            GD.PrintErr("SpawnWaveProjectiles: ProjectileScene not assigned!");
            return;
        }

        Vector2 playerDirection = _player.GetAttackDirection();
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

    public void SpawnWhirlwindVisual()
    {
        if (WhirlwindVisualScene == null || _currentCastingSkill == null)
        {
            GD.PrintErr("WhirlwindVisualScene or current skill not assigned!");
            return;
        }

        var visual = WhirlwindVisualScene.Instantiate<WhirlwindVisual>();

        // Initialize BEFORE adding to scene tree (so _Ready has valid data)
        if (visual is WhirlwindVisual whirlwindVisual)
        {
            whirlwindVisual.Initialize(_currentCastingSkill);
        }
        else
        {
            GD.PrintErr($"WhirlwindVisual is wrong type! Type: {visual.GetType().FullName}");
        }

        // NOW add to scene tree (triggers _Ready with initialized values)
        _player.GetParent().AddChild(visual);
        visual.GlobalPosition = _player.GlobalPosition;
    }

    // Called from animation tracks for explosion effects (Leap Slam, Breaching Charge, etc.)
    public void SpawnExplosionEffect()
    {
        if (ExplosionEffectScene == null || _currentCastingSkill == null)
        {
            GD.PrintErr("SpawnExplosionEffect: Scene or skill not ready!");
            return;
        }

        var explosion = ExplosionEffectScene.Instantiate<ExplosionEffect>();

        // Explosions spawn at player position (can be overridden for projectile explosions)
        Vector2 spawnPosition = _player.GlobalPosition;

        // Initialize BEFORE adding to tree
        explosion.Initialize(_currentCastingSkill, _player, Vector2.Zero);

        // Set position BEFORE adding to tree
        explosion.GlobalPosition = spawnPosition;

        // Add to world (not as child of player)
        _player.GetParent().AddChild(explosion);
    }

    // ===== DASH CALLBACKS (Called from AnimationPlayer) =====

    public void StartDash()
    {
        if (_statsManager == null || _forcedMovementController == null)
        {
            GD.PrintErr("StartDash: StatsManager or MovementController not found!");
            return;
        }

        // Calculate smart target position (clamped to range)
        Vector2 targetPosition = CalculateForcedMovementTarget(_currentCastingSkill.Range);
        _forcedMovementController.StartDashToTarget(targetPosition, _currentCastingSkill.Duration);
        _statsManager.SetInvincible(true);
    }

    public void EndDash()
    {
        if (_statsManager == null || _forcedMovementController == null)
        {
            GD.PrintErr("EndDash: StatsManager or MovementController not found!");
            return;
        }

        _forcedMovementController.EndMovement();
        _statsManager.SetInvincible(false);
    }

    /// <summary>
    /// Calculates the target position for a forced movement skill.
    /// If mouse is within range, moves to mouse.
    /// If mouse is outside range, moves max distance toward mouse.
    /// </summary>
    private Vector2 CalculateForcedMovementTarget(float maxRange)
    {
        if (_player == null)
        {
            GD.PrintErr("CalculateForcedMovementTarget: Player is null!");
            return _player.GlobalPosition;
        }

        Vector2 playerPos = _player.GlobalPosition;
        Vector2 mousePos = _player.GetGlobalMousePosition();
        Vector2 directionToMouse = (mousePos - playerPos);
        float distanceToMouse = directionToMouse.Length();

        if (distanceToMouse <= maxRange)
        {
            // Mouse is within range - dash to mouse position
            return mousePos;
        }
        else
        {
            // Mouse is outside range - dash max distance toward mouse
            Vector2 normalizedDirection = directionToMouse.Normalized();
            return playerPos + (normalizedDirection * maxRange);
        }
    }

    // ===== LEAP SLAM CALLBACKS (Called from AnimationPlayer) =====

    public void StartLeapSlam()
    {
        if (_statsManager == null || _forcedMovementController == null)
        {
            GD.PrintErr("StartLeapSlam: StatsManager or MovementController not found!");
            return;
        }

        GD.Print($"[DEBUG] StartLeapSlam called");
        GD.Print($"[DEBUG] Current skill: {_currentCastingSkill?.SkillName}");
        GD.Print($"[DEBUG] Skill range: {_currentCastingSkill?.Range}");
        GD.Print($"[DEBUG] Player position: {_player.GlobalPosition}");

        Vector2 targetPosition = CalculateForcedMovementTarget(_currentCastingSkill.Range);
        GD.Print($"[DEBUG] Calculated target: {targetPosition}");
        GD.Print($"[DEBUG] Distance to target: {(_player.GlobalPosition - targetPosition).Length()}");

        // Calculate smart target position (clamped to Leap Slam's range)
        // Vector2 targetPosition = CalculateForcedMovementTarget(_currentCastingSkill.Range);

        // Use leap-specific method
        _forcedMovementController.StartLeapToTarget(targetPosition, _currentCastingSkill.CastTime);

        // Leap Slam also has invincibility during jump
        _statsManager.SetInvincible(true);
    }

    public void EndLeapSlam()
    {
        if (_statsManager == null || _forcedMovementController == null)
        {
            GD.PrintErr("EndLeapSlam: StatsManager or MovementController not found!");
            return;
        }

        _forcedMovementController.EndMovement();
        _statsManager.SetInvincible(false);

        // Future: Trigger explosion effect on landing
        // if (_currentCastingSkill.Explosion != null)
        // {
        //     SpawnExplosionEffect();
        // }
    }

    // ===== COLLISION HANDLERS =====
    private void UpdateMeleeHitboxPosition(Vector2 direction)
    {
        // Position hitbox in front of player based on attack direction
        float distance = 40f; // How far in front of player
        _meleeHitbox.Position = direction * distance;

        // Rotate hitbox to match direction
        _meleeHitbox.Rotation = direction.Angle();
    }

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
            float step = spreadAngle * 2 / (totalCount - 1);
            angleOffset = -spreadAngle + (step * index);
        }

        float finalAngle = baseAngle + Mathf.DegToRad(angleOffset);
        var direction = new Vector2(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle));

        // var sceneToUse = _currentCastingSkill.SkillEffectScene ?? ProjectileScene;
        var projectile = ProjectileScene.Instantiate<Node2D>();

        // Initialize BEFORE adding to scene tree (so _Ready has valid data)
        if (projectile is DamageEntityBase entity)
        {
            entity.Initialize(_currentCastingSkill, _player, direction);
        }
        else
        {
            GD.PrintErr($">>> Projectile is NOT DamageEntityBase! Type: {projectile.GetType().FullName}");
            return; // Don't add invalid projectiles
        }

        // NOW add to scene tree (triggers _Ready with initialized values)
        _player.GetTree().Root.AddChild(projectile);
        projectile.GlobalPosition = spawnPos;
    }
}
