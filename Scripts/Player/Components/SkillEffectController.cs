using Godot;
using System.Collections.Generic;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.Skills.Base;
using ZenithRising.Scripts.Skills.Entities;
using ZenithRising.Scripts.Skills.Entities.Visuals;
using ZenithRising.Scripts.Skills.Entities.Zones;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Animation callback handler for skill visual effects and spawning.
/// Responsibilities:
/// - Respond to AnimationPlayer Call Method tracks (frame-perfect timing)
/// - Enable/disable hitboxes at precise animation frames
/// - Spawn projectiles, explosions, visual effects
/// - Manage forced movement (dash/leap) initiation and cleanup
/// - Track current casting skill for callback context
/// Pattern: AnimationPlayer calls methods → SkillEffectController spawns/enables effects
/// Initialization order critical: Must Initialize() BEFORE AddChild() for spawned entities.
/// Does NOT handle: Skill execution (SkillManager), buff application (BuffManager)
/// </summary>
[GlobalClass]
public partial class SkillEffectController : Node
{
    // ═══════════════════════════════════════════════════════════════
    // DEPENDENCIES
    // ═══════════════════════════════════════════════════════════════
    private Player _player;
    private StatsManager _statsManager;
    private ForcedMovementController _forcedMovementController;
    private HitboxController _hitboxController;

    // ═══════════════════════════════════════════════════════════════
    // SKILL TRACKING (Set by Player.TryCastSkill)
    // ═══════════════════════════════════════════════════════════════
    private Skill _currentCastingSkill;
    private Vector2 _currentAttackDirection;

    // ═══════════════════════════════════════════════════════════════
    // EFFECT SCENE TEMPLATES (Configured in Godot editor)
    // ═══════════════════════════════════════════════════════════════
    [Export] public PackedScene ProjectileScene { get; set; }
    [Export] public PackedScene WhirlwindVisualScene { get; set; }
    [Export] public PackedScene ExplosionEffectScene { get; set; }

    // ═══════════════════════════════════════════════════════════════
    // LIFECYCLE METHODS
    // ═══════════════════════════════════════════════════════════════
    public void Initialize(Player player, StatsManager statsManager, ForcedMovementController forcedMovementController, HitboxController hitboxController)
    {
        _player = player;
        _statsManager = statsManager;
        _forcedMovementController = forcedMovementController;
        _hitboxController = hitboxController;
    }

    public override void _Ready()
    {
        if (ProjectileScene == null)
        {
            GD.PrintErr("SkillEffectController: ProjectileScene not assigned!");
        }

        if (WhirlwindVisualScene == null)
        {
            GD.PrintErr("SkillEffectController: WhirlwindVisualScene not assigned!");
        }

        if (ExplosionEffectScene == null)
        {
            GD.PrintErr("SkillEffectController: ExplosionEffectScene not assigned!");
        }
    }

    /// <summary>
    /// Sets current casting skill for animation callbacks.
    /// Called by Player.TryCastSkill() when skill begins.
    /// </summary>
    public void SetCurrentSkill(Skill skill)
    {
        _currentCastingSkill = skill;
        _hitboxController?.SetCurrentSkill(skill);
    }

    /// <summary>
    /// Clears current skill reference. Called when animation finishes.
    /// </summary>
    public void ClearCurrentSkill()
    {
        _currentCastingSkill = null;
    }

    // ═══════════════════════════════════════════════════════════════
    // ANIMATION CALLBACKS - HITBOX MANAGEMENT
    // ═══════════════════════════════════════════════════════════════
    public void EnableMeleeHitbox()
    {
        _hitboxController?.EnableHitbox("MeleeHitbox");
    }

    public void DisableMeleeHitbox()
    {
        _hitboxController?.DisableHitbox("MeleeHitbox");
    }

    public void EnableAOEHitbox()
    {
        // Whirlwind follows player
        _hitboxController?.EnableHitbox("AOEHitbox", followPlayer: true);
    }

    public void DisableAOEHitbox()
    {
        _hitboxController?.DisableHitbox("AOEHitbox");
    }

    /// <summary>
    /// Enables combo hitbox for multi-strike basic attacks.
    /// Called from animation Call Method tracks.
    /// </summary>
    public void EnableComboStrike(int strikeIndex)
    {
        _hitboxController?.EnableComboStrike(strikeIndex);
    }

    /// <summary>
    /// Disables combo hitbox after strike window ends.
    /// </summary>
    public void DisableComboStrike(int strikeIndex)
    {
        _hitboxController?.DisableComboStrike(strikeIndex);
    }

    // ═══════════════════════════════════════════════════════════════
    // ANIMATION CALLBACKS - PROJECTILE SPAWNING
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Spawns multiple projectiles in a spread pattern (Energy Wave skill).
    /// Called from animation Call Method track at frame-perfect timing.
    /// </summary>
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

    // ═══════════════════════════════════════════════════════════════
    // ANIMATION CALLBACKS - VISUAL EFFECTS
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Spawns Whirlwind visual effect (spinning ring).
    /// Critical: Initialize() BEFORE AddChild() to pass skill data to visual.
    /// </summary>
    public void SpawnWhirlwindVisual()
    {
        if (WhirlwindVisualScene == null || _currentCastingSkill == null)
        {
            GD.PrintErr("WhirlwindVisualScene or current skill not assigned!");
            return;
        }

        var visual = WhirlwindVisualScene.Instantiate<WhirlwindVisual>();

        if (visual is WhirlwindVisual whirlwindVisual)
        {
            whirlwindVisual.Initialize(_currentCastingSkill);
        }
        else
        {
            GD.PrintErr($"WhirlwindVisual is wrong type! Type: {visual.GetType().FullName}");
        }

        _player.GetParent().AddChild(visual);
        visual.GlobalPosition = _player.GlobalPosition;
    }

    /// <summary>
    /// Spawns explosion effect (Leap Slam, Breaching Charge).
    /// Critical: Initialize() BEFORE AddChild() for skill data.
    /// </summary>
    public void SpawnExplosionEffect()
    {
        if (ExplosionEffectScene == null || _currentCastingSkill == null)
        {
            GD.PrintErr("SpawnExplosionEffect: Scene or skill not ready!");
            return;
        }

        var explosion = ExplosionEffectScene.Instantiate<ExplosionEffect>();
        Vector2 spawnPosition = _player.GlobalPosition;

        explosion.Initialize(_currentCastingSkill, _player, Vector2.Zero);
        explosion.GlobalPosition = spawnPosition;
        _player.GetParent().AddChild(explosion);
    }

    // ═══════════════════════════════════════════════════════════════
    // ANIMATION CALLBACKS - FORCED MOVEMENT (Dash, Leap Slam)
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Initiates dash movement with invincibility.
    /// Called from Dash skill animation Call Method track.
    /// </summary>
    public void StartDash()
    {
        if (_statsManager == null || _forcedMovementController == null)
        {
            GD.PrintErr("StartDash: StatsManager or MovementController not found!");
            return;
        }

        Vector2 targetPosition = CalculateForcedMovementTarget(_currentCastingSkill.Range);
        _forcedMovementController.StartDashToTarget(targetPosition, _currentCastingSkill.Duration);
        _statsManager.SetInvincible(true);
    }

    /// <summary>
    /// Ends dash movement and removes invincibility.
    /// Called from Dash skill animation Call Method track.
    /// </summary>
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
    /// Initiates leap movement with invincibility (Leap Slam skill).
    /// Uses arc trajectory instead of linear dash.
    /// </summary>
    public void StartLeapSlam()
    {
        if (_statsManager == null || _forcedMovementController == null)
        {
            GD.PrintErr("StartLeapSlam: StatsManager or MovementController not found!");
            return;
        }

        Vector2 targetPosition = CalculateForcedMovementTarget(_currentCastingSkill.Range);
        _forcedMovementController.StartLeapToTarget(targetPosition, _currentCastingSkill.CastTime);
        _statsManager.SetInvincible(true);
    }

    /// <summary>
    /// Ends leap movement and removes invincibility.
    /// </summary>
    public void EndLeapSlam()
    {
        if (_statsManager == null || _forcedMovementController == null)
        {
            GD.PrintErr("EndLeapSlam: StatsManager or MovementController not found!");
            return;
        }

        _forcedMovementController.EndMovement();
        _statsManager.SetInvincible(false);
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Calculates dash/leap target position toward mouse, clamped to skill range.
    /// If mouse within range: Move to exact mouse position.
    /// If mouse outside range: Move max distance toward mouse.
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

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Spawns a single projectile with spread angle calculation.
    /// Used by SpawnWaveProjectiles() to create projectile spreads.
    /// Calculates angle offset based on index position in spread pattern.
    /// </summary>
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
