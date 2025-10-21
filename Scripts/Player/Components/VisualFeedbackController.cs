using Godot;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Handles visual feedback effects for the player.
/// Currently manages invincibility flashing, can be extended for hit effects, buffs, etc.
/// </summary>
[GlobalClass]
public partial class VisualFeedbackController : Node
{
    // ===== INJECTED DEPENDENCIES =====
    private Sprite2D _sprite;
    private StatsManager _statsManager;

    // ===== STATE =====
    private Tween _invincibilityTween;

    // ===== PUBLIC API - Initialization =====

    /// <summary>
    /// Initializes the controller with required dependencies.
    /// Called from Player._Ready().
    /// </summary>
    public void Initialize(Sprite2D sprite, StatsManager statsManager)
    {
        _sprite = sprite;
        _statsManager = statsManager;

        if (_sprite == null)
        {
            GD.PrintErr("VisualFeedbackController: Sprite2D reference is null!");
        }

        if (_statsManager == null)
        {
            GD.PrintErr("VisualFeedbackController: StatsManager reference is null!");
        }
    }

    // ===== LIFECYCLE =====

    /// <summary>
    /// Updates visual effects each frame.
    /// Watches StatsManager.IsInvincible and manages flashing automatically.
    /// </summary>
    public override void _Process(double delta)
    {
        if (_statsManager == null || _sprite == null)
        {
            return;
        }

        UpdateInvincibilityFlashing();
    }

    // ===== PRIVATE HELPERS - Invincibility Effect =====

    private void UpdateInvincibilityFlashing()
    {
        if (_statsManager.IsInvincible && _invincibilityTween == null)
        {
            StartInvincibilityFlashing();
        }
        else if (!_statsManager.IsInvincible && _invincibilityTween != null)
        {
            StopInvincibilityFlashing();
        }
    }

    private void StartInvincibilityFlashing()
    {
        // Create looping tween that flashes sprite opacity
        _invincibilityTween = CreateTween();
        _invincibilityTween.SetLoops();
        _invincibilityTween.TweenProperty(_sprite, "modulate:a", 0.5, 0.1);
        _invincibilityTween.TweenProperty(_sprite, "modulate:a", 1.0, 0.1);
    }

    private void StopInvincibilityFlashing()
    {
        // Kill tween and reset sprite to full opacity
        _invincibilityTween?.Kill();
        _invincibilityTween = null;

        if (_sprite != null)
        {
            _sprite.Modulate = new Color(1, 1, 1, 1); // Reset to full opacity
        }
    }

    // ===== FUTURE EXPANSION =====
    // These methods can be added later for additional visual feedback:

    // public void ShowHitEffect()
    // {
    //     // Brief red flash when taking damage
    // }

    // public void ShowBuffActivation(Color buffColor)
    // {
    //     // Colored particle burst or glow effect
    // }

    // public void ShowCriticalHit()
    // {
    //     // Screen shake or special effect on crit
    // }

    // public void ShowHealEffect()
    // {
    //     // Green particles or glow when healed
    // }
}
