using Godot;
using System.Collections.Generic;
using ZenithRising.Scripts.Core;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class HitboxController : Node
{
    // === DEPENDENCIES ===
    private Player _player;
    private StatsManager _statsManager;

    // === HITBOX POOL ===
    private Dictionary<string, Area2D> _hitboxPool = new();

    // === HIT TRACKING ===
    private HashSet<Enemy> _hitEnemiesThisStrike = new();
    private Skill _currentSkill;
    private int _currentStrikeIndex = 0;
    private float _currentStrikeDamageMultiplier = 1.0f;

    // === FOLLOW-PLAYER LOGIC ===
    private Area2D _followingHitbox = null; // Currently following hitbox (Whirlwind)

    // === INITIALIZATION ===
    public void Initialize(Player player, StatsManager statsManager)
    {
        _player = player;
        _statsManager = statsManager;
    }

    public override void _Ready()
    {
        // Build hitbox pool from HitboxContainer in player scene
        var hitboxContainer = GetParent().GetNodeOrNull<Node2D>("HitboxContainer");

        if (hitboxContainer == null)
        {
            GD.PrintErr("HitboxController: HitboxContainer node not found under Player!");
            GD.Print($"HitboxController: Parent is {GetParent()?.Name}");

            // List all children of parent for debugging
            if (GetParent() != null)
            {
                GD.Print("HitboxController: Available nodes under Player:");
                foreach (Node child in GetParent().GetChildren())
                {
                    GD.Print($"  - {child.Name} ({child.GetType().Name})");
                }
            }
            return;
        }

        GD.Print($"HitboxController: Found HitboxContainer with {hitboxContainer.GetChildCount()} children");

        foreach (Node child in hitboxContainer.GetChildren())
        {
            if (child is Area2D hitbox)
            {
                _hitboxPool[child.Name] = hitbox;
                hitbox.BodyEntered += OnHitboxBodyEntered;
                hitbox.Monitoring = false; // Disabled by default
                GD.Print($"HitboxController: Registered hitbox '{child.Name}'");
            }
            else
            {
                GD.Print($"HitboxController: Skipped non-Area2D child '{child.Name}' ({child.GetType().Name})");
            }
        }

        GD.Print($"HitboxController: Initialized with {_hitboxPool.Count} hitboxes");
    }

    // === PUBLIC API (Called by SkillEffectController) ===

    public void SetCurrentSkill(Skill skill)
    {
        _currentSkill = skill;
    }

    /// <summary>
    /// Enables a named hitbox (for simple skills like Whirlwind).
    /// </summary>
    public void EnableHitbox(string hitboxName, bool followPlayer = false)
    {
        if (!_hitboxPool.TryGetValue(hitboxName, out Area2D hitbox))
        {
            GD.PrintErr($"HitboxController: Hitbox '{hitboxName}' not found!");
            return;
        }

        _hitEnemiesThisStrike.Clear();
        hitbox.Monitoring = true;

        if (followPlayer)
        {
            _followingHitbox = hitbox;
        }
    }

    /// <summary>
    /// Disables a named hitbox.
    /// </summary>
    public void DisableHitbox(string hitboxName)
    {
        if (_hitboxPool.TryGetValue(hitboxName, out Area2D hitbox))
        {
            hitbox.Monitoring = false;

            if (_followingHitbox == hitbox)
            {
                _followingHitbox = null;
            }
        }
    }

    /// <summary>
    /// Enables a combo strike hitbox with configured settings.
    /// Called from animation tracks for multi-strike combos.
    /// </summary>
    public void EnableComboStrike(int strikeIndex)
    {
        if (_currentSkill == null)
        {
            GD.PrintErr("HitboxController: No current skill set!");
            return;
        }

        // Get strike configuration from skill data
        var strikeConfig = _currentSkill.GetStrikeConfig(strikeIndex);
        if (strikeConfig == null)
        {
            GD.PrintErr($"HitboxController: No config for strike {strikeIndex}!");
            return;
        }

        // Get hitbox from pool
        if (!_hitboxPool.TryGetValue(strikeConfig.HitboxName, out Area2D hitbox))
        {
            GD.PrintErr($"HitboxController: Hitbox '{strikeConfig.HitboxName}' not found!");
            return;
        }

        // Reset hit tracking (enemies can be hit once PER strike)
        _hitEnemiesThisStrike.Clear();

        // Store strike info for damage calculation
        _currentStrikeIndex = strikeIndex;
        _currentStrikeDamageMultiplier = strikeConfig.DamageMultiplier;

        // Position hitbox based on attack direction
        Vector2 attackDir = _player.GetAttackDirection();
        float baseAngle = attackDir.Angle();

        // Apply configured offset and rotation
        Vector2 offsetPosition = strikeConfig.PositionOffset.Rotated(baseAngle);
        hitbox.Position = offsetPosition;
        hitbox.Rotation = baseAngle + Mathf.DegToRad(strikeConfig.RotationOffset);

        // Enable monitoring
        hitbox.Monitoring = true;
    }

    /// <summary>
    /// Disables a combo strike hitbox.
    /// </summary>
    public void DisableComboStrike(int strikeIndex)
    {
        if (_currentSkill == null) return;

        var strikeConfig = _currentSkill.GetStrikeConfig(strikeIndex);
        if (strikeConfig == null) return;

        if (_hitboxPool.TryGetValue(strikeConfig.HitboxName, out Area2D hitbox))
        {
            hitbox.Monitoring = false;
        }
    }

    /// <summary>
    /// Updates position of following hitboxes (called from Player._PhysicsProcess).
    /// Used for skills like Whirlwind that follow the player.
    /// </summary>
    public void UpdateFollowingHitboxes()
    {
        if (_followingHitbox != null && _player != null)
        {
            _followingHitbox.GlobalPosition = _player.GlobalPosition;
        }
    }

    // === COLLISION HANDLING ===

    private void OnHitboxBodyEntered(Node2D body)
    {
        if (body is not Enemy enemy)
        {
            return;
        }

        // Check if already hit this strike
        if (_hitEnemiesThisStrike.Contains(enemy))
        {
            return;
        }

        _hitEnemiesThisStrike.Add(enemy);
        ApplyDamage(enemy);
    }

    private void ApplyDamage(Enemy enemy)
    {
        if (_currentSkill == null || _statsManager == null)
        {
            GD.PrintErr("HitboxController: No skill or stats for damage calculation!");
            return;
        }

        // Calculate damage with strike multiplier
        float baseDamage = _currentSkill.BaseDamage * _currentStrikeDamageMultiplier;
        float damage = CombatSystem.CalculateDamage(
            baseDamage,
            _statsManager,
            _currentSkill.DamageType
        );

        // Apply damage
        float healthBefore = enemy.Health;
        enemy.TakeDamage(damage);

        // Record kill for mastery
        if (healthBefore > 0 && enemy.Health <= 0)
        {
            _currentSkill.RecordKill();
        }
    }
}
