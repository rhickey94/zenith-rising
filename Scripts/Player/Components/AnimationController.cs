using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class AnimationController : Node
{
    private AnimationPlayer _animationPlayer;
    private Player _player;
    private Vector2 _lastDirection = Vector2.Down;

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

    // Modify PlaySkillAnimation to accept strike number:
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

    // NEW: Get animation duration for recovery window tracking
    public float GetSkillAnimationDuration(Skill skill, int strikeNumber)
    {
        string animName = GetStrikeAnimationName(skill, strikeNumber);

        if (_animationPlayer.HasAnimation(animName))
        {
            return (float)_animationPlayer.GetAnimation(animName).Length;
        }

        return 0.5f; // Default fallback
    }

    // ===== PUBLIC API - Direction Queries =====

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

    // ===== PRIVATE HELPERS - Animation Playback =====

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

    // ===== PRIVATE HELPERS - Animation Name Building =====

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

    private string GetStrikeAnimationName(Skill skill, int strikeNumber)
    {
        // For combo attacks, animations are named: warrior_attack_down_1, warrior_attack_down_2, etc.
        if (skill.Slot == SkillSlot.BasicAttack && strikeNumber > 1)
        {
            string direction = GetDirectionSuffix();
            return $"{skill.AnimationBaseName}_{direction}_{strikeNumber}";
        }

        // Non-combo skills use standard naming
        return GetSkillAnimationName(skill);
    }

    private string GetDirectionSuffix()
    {
        // Returns "down", "up", "left", or "right" based on facing direction
        // This method should already exist in your AnimationController
        // If not, you need to implement it based on GetFacingDirection()

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
