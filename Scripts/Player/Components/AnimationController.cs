using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Animation playback controller for player character.
/// Responsibilities:
/// - Locomotion animation (walk/idle in 4 directions)
/// - Skill animation playback (with combo support for multi-strike attacks)
/// - Mouse-aimed directional animation selection
/// - Attack speed/cast speed scaling via SpeedScale
/// - Animation duration queries for recovery window calculation
/// Pattern: Mouse cursor determines facing direction (twin-stick controls)
/// Does NOT handle: Animation callbacks (SkillEffectController), FSM state management (Player)
/// </summary>
[GlobalClass]
public partial class AnimationController : Node
{
    // ═══════════════════════════════════════════════════════════════
    // DEPENDENCIES
    // ═══════════════════════════════════════════════════════════════
    private AnimationPlayer _animationPlayer;
    private Player _player;
    private Vector2 _lastDirection = Vector2.Down;

    // ═══════════════════════════════════════════════════════════════
    // LIFECYCLE METHODS
    // ═══════════════════════════════════════════════════════════════
    public void Initialize(Player player, AnimationPlayer animationPlayer)
    {
        _player = player;
        _animationPlayer = animationPlayer;

        if (_player == null)
        {
            GD.PrintErr("AnimationController: Player reference is null!");
        }

        if (_animationPlayer == null)
        {
            GD.PrintErr("AnimationController: AnimationPlayer reference is null!");
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - ANIMATION PLAYBACK
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Plays locomotion animations (walk/idle) based on movement input.
    /// Uses mouse direction for facing, not movement direction (twin-stick controls).
    /// </summary>
    public void PlayLocomotion(PlayerState state, Vector2 movementInput)
    {
        if (_animationPlayer == null)
        {
            return;
        }

        // Determine facing direction (mouse for aiming)
        Vector2 facingDirection = GetMouseDirection();

        if (movementInput != Vector2.Zero)
        {
            _lastDirection = movementInput; // Track last movement for fallback
            PlayWalkAnimation(facingDirection);
        }
        else
        {
            PlayIdleAnimation(facingDirection);
        }
    }

    /// <summary>
    /// Plays skill animation with speed scaling and combo support.
    /// speedScale: Multiplier from attack rate (attacks) or cast speed (spells).
    /// strikeNumber: For combo attacks (1 = first strike, 2 = second strike, etc.).
    /// </summary>
    public void PlaySkillAnimation(Skill skill, int strikeNumber, float speedScale)
    {
        if (_animationPlayer == null || skill == null)
        {
            GD.PrintErr("PlaySkillAnimation: Missing AnimationPlayer or Skill!");
            return;
        }

        string animationName = GetStrikeAnimationName(skill, strikeNumber);

        if (string.IsNullOrEmpty(animationName))
        {
            GD.PrintErr($"PlaySkillAnimation: No animation found for {skill.SkillName} strike {strikeNumber}");
            return;
        }

        _animationPlayer.SpeedScale = speedScale;
        _animationPlayer.Play(animationName);
    }

    /// <summary>
    /// Gets animation duration in seconds for recovery window calculation.
    /// Used by Player to determine when input buffering begins (last 15% of animation).
    /// </summary>
    public float GetSkillAnimationDuration(Skill skill, int strikeNumber)
    {
        string animName = GetStrikeAnimationName(skill, strikeNumber);

        if (_animationPlayer.HasAnimation(animName))
        {
            return (float)_animationPlayer.GetAnimation(animName).Length;
        }

        return 0.5f; // Default fallback
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - DIRECTION QUERIES
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Gets the direction the player is facing (mouse direction).
    /// Used for skill execution and animation selection.
    /// </summary>
    public Vector2 GetFacingDirection()
    {
        return GetMouseDirection();
    }

    /// <summary>
    /// Gets the direction from player to mouse cursor.
    /// Falls back to last movement direction if mouse is on player.
    /// </summary>
    public Vector2 GetMouseDirection()
    {
        if (_player == null)
        {
            return _lastDirection;
        }

        Vector2 mousePosition = _player.GetGlobalMousePosition();
        Vector2 direction = mousePosition - _player.GlobalPosition;

        // Guard against mouse exactly on player
        if (direction.LengthSquared() < 0.01f)
        {
            return _lastDirection; // Fallback to last known direction
        }

        return direction.Normalized();
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - ANIMATION PLAYBACK
    // ═══════════════════════════════════════════════════════════════
    private void PlayWalkAnimation(Vector2 direction)
    {
        string animName = GetDirectionalAnimationName("walk", direction);

        if (_animationPlayer.CurrentAnimation != animName)
        {
            _animationPlayer.Play(animName);
        }
    }

    private void PlayIdleAnimation(Vector2 direction)
    {
        string animName = GetDirectionalAnimationName("idle", direction);

        if (_animationPlayer.CurrentAnimation != animName)
        {
            _animationPlayer.Play(animName);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - ANIMATION NAME BUILDING
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Builds directional animation name (e.g., "walk_down", "idle_left").
    /// </summary>
    private string GetDirectionalAnimationName(string baseAnimation, Vector2 direction)
    {
        string directionSuffix;

        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            directionSuffix = direction.X > 0 ? "right" : "left";
        }
        else
        {
            directionSuffix = direction.Y > 0 ? "down" : "up";
        }

        return $"{baseAnimation}_{directionSuffix}";
    }

    /// <summary>
    /// Gets the animation name for a skill.
    /// Handles both directional and non-directional skill animations.
    /// </summary>
    private string GetSkillAnimationName(Skill skill)
    {
        if (string.IsNullOrEmpty(skill.AnimationBaseName))
        {
            GD.PrintErr($"AnimationController: No AnimationBaseName for skill: {skill.SkillId}");
            return "idle_down"; // Safe fallback
        }

        return skill.UsesDirectionalAnimation
            ? GetDirectionalAnimationName(skill.AnimationBaseName, GetMouseDirection())
            : skill.AnimationBaseName;
    }

    /// <summary>
    /// Gets animation name for combo attacks with strike number support.
    /// Example: warrior_attack_down_1, warrior_attack_down_2, etc.
    /// </summary>
    private string GetStrikeAnimationName(Skill skill, int strikeNumber)
    {
        // Combo attacks append strike number to directional animation name
        if (skill.Slot == SkillSlot.BasicAttack && strikeNumber > 1)
        {
            string direction = GetDirectionSuffix();
            return $"{skill.AnimationBaseName}_{direction}_{strikeNumber}";
        }

        // Non-combo skills use standard naming
        return GetSkillAnimationName(skill);
    }

    /// <summary>
    /// Returns directional suffix string ("down", "up", "left", "right") based on facing direction.
    /// Used for combo attack animation naming.
    /// </summary>
    private string GetDirectionSuffix()
    {
        Vector2 facing = GetFacingDirection();

        if (Mathf.Abs(facing.Y) > Mathf.Abs(facing.X))
        {
            return facing.Y > 0 ? "down" : "up";
        }
        else
        {
            return facing.X > 0 ? "right" : "left";
        }
    }
}
