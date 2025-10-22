using Godot;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Defines the type of forced movement skill being executed.
/// </summary>
public enum MovementSkillType
{
    Dash,      // Smooth interpolation with ease-out curve
    Leap,      // Parabolic arc trajectory (future)
    Charge,    // Linear movement with collision detection (future)
    Teleport   // Instant position change (future)
}

/// <summary>
/// Handles all forced movement skills that override player input.
/// Supports dash, leap, charge, teleport, and other movement abilities.
/// </summary>
[GlobalClass]
public partial class ForcedMovementController : Node
{
    // ===== INJECTED DEPENDENCIES =====
    private Player _player;

    // ===== STATE =====
    private bool _isActive = false;
    private MovementSkillType _currentMovementType;

    // Dash/Leap state
    private Vector2 _startPos = Vector2.Zero;
    private Vector2 _targetPos = Vector2.Zero;
    private float _elapsed = 0f;
    private float _duration = 0f;

    // Future: Charge state (linear with collision)
    // private Vector2 _chargeDirection;
    // private float _chargeSpeed;

    // ===== PUBLIC API - Initialization =====

    /// <summary>
    /// Initializes the controller with required dependencies.
    /// Called from Player._Ready().
    /// </summary>
    public void Initialize(Player player)
    {
        _player = player;

        if (_player == null)
        {
            GD.PrintErr("ForcedMovementController: Player reference is null!");
        }
    }

    // ===== PUBLIC API - Movement Control =====

    /// <summary>
    /// Returns true if forced movement is currently active.
    /// Used by Player to skip normal movement logic.
    /// </summary>
    public bool IsActive => _isActive;

    /// <summary>
    /// Updates forced movement interpolation.
    /// Called from Player._PhysicsProcess().
    /// Returns true if movement was handled (Player should skip normal movement).
    /// </summary>
    public bool UpdateMovement(double delta)
    {
        if (!_isActive)
        {
            return false;
        }

        switch (_currentMovementType)
        {
            case MovementSkillType.Dash:
                UpdateDash((float)delta);
                break;

            case MovementSkillType.Leap:
                UpdateLeap((float)delta);
                break;

            case MovementSkillType.Charge:
                UpdateCharge((float)delta);
                break;

            case MovementSkillType.Teleport:
                // Instant, no update needed
                break;
        }

        return true; // Movement handled by this controller
    }

    /// <summary>
    /// Forcibly ends any active movement.
    /// Called by SkillEffectController or when interrupted.
    /// </summary>
    public void EndMovement()
    {
        if (_isActive)
        {
            _isActive = false;
            _elapsed = 0f;
        }
    }

    // ===== PUBLIC API - Skill-Specific Starters =====

    /// <summary>
    /// Starts a dash movement with smooth ease-out interpolation.
    /// Called from SkillEffectController.StartDash() animation callback.
    /// </summary>
    public void StartDash(Vector2 direction, float distance, float duration)
    {
        if (_player == null)
        {
            GD.PrintErr("ForcedMovementController: Cannot start dash - Player reference is null!");
            return;
        }

        _isActive = true;
        _currentMovementType = MovementSkillType.Dash;
        _startPos = _player.GlobalPosition;
        _targetPos = _startPos + (direction.Normalized() * distance);
        _duration = duration;
        _elapsed = 0f;
    }

    /// <summary>
    /// Starts a leap movement with parabolic arc trajectory.
    /// Future implementation for Leap Slam skill.
    /// </summary>
    public void StartLeap(Vector2 direction, float distance, float height, float duration)
    {
        if (_player == null)
        {
            GD.PrintErr("ForcedMovementController: Cannot start leap - Player reference is null!");
            return;
        }

        _isActive = true;
        _currentMovementType = MovementSkillType.Leap;
        _startPos = _player.GlobalPosition;
        _targetPos = _startPos + (direction.Normalized() * distance);
        _duration = duration;
        _elapsed = 0f;

        // Future: Store height parameter for arc calculation
    }

    /// <summary>
    /// Starts a leap movement to an absolute target position.
    /// Similar to dash but uses Leap movement type (future: add parabolic arc).
    /// </summary>
    public void StartLeapToTarget(Vector2 targetPosition, float duration)
    {
        if (_player == null)
        {
            GD.PrintErr("ForcedMovementController: Cannot start leap - Player reference is null!");
            return;
        }

        _isActive = true;
        _currentMovementType = MovementSkillType.Leap;  // Uses Leap, not Dash
        _startPos = _player.GlobalPosition;
        _targetPos = targetPosition;
        _duration = duration;
        _elapsed = 0f;
    }

    /// <summary>
    /// Starts a charge movement with linear motion and collision detection.
    /// Future implementation for charging skills.
    /// </summary>
    public void StartCharge(Vector2 direction, float distance, float speed)
    {
        if (_player == null)
        {
            GD.PrintErr("ForcedMovementController: Cannot start charge - Player reference is null!");
            return;
        }

        _isActive = true;
        _currentMovementType = MovementSkillType.Charge;
        _startPos = _player.GlobalPosition;
        _targetPos = _startPos + (direction.Normalized() * distance);
        _elapsed = 0f;

        // Future: Store speed and collision parameters
    }

    /// <summary>
    /// Instantly teleports player to target position.
    /// Future implementation for teleport/blink skills.
    /// </summary>
    public void StartTeleport(Vector2 targetPosition)
    {
        if (_player == null)
        {
            GD.PrintErr("ForcedMovementController: Cannot teleport - Player reference is null!");
            return;
        }

        _isActive = true;
        _currentMovementType = MovementSkillType.Teleport;
        _player.GlobalPosition = targetPosition;
        _isActive = false; // Instant, no update needed
    }

    /// <summary>
    /// Starts a dash movement to an absolute target position.
    /// Used for mouse-targeted dashes where target is pre-calculated.
    /// </summary>
    public void StartDashToTarget(Vector2 targetPosition, float duration)
    {
        if (_player == null)
        {
            GD.PrintErr("ForcedMovementController: Cannot start dash - Player reference is null!");
            return;
        }

        _isActive = true;
        _currentMovementType = MovementSkillType.Dash;
        _startPos = _player.GlobalPosition;
        _targetPos = targetPosition;  // Use absolute position, not calculated offset
        _duration = duration;
        _elapsed = 0f;
    }

    // ===== PRIVATE HELPERS - Movement Types =====

    private void UpdateDash(float delta)
    {
        if (_player == null || _duration <= 0)
        {
            _isActive = false;
            return;
        }

        _elapsed += delta;
        float t = Mathf.Clamp(_elapsed / _duration, 0f, 1f);

        // Ease-out curve for smooth deceleration
        t = (float)Mathf.Ease(t, -2.0);

        // Interpolate position
        _player.GlobalPosition = _startPos.Lerp(_targetPos, t);

        // Check if dash completed
        if (t >= 1.0f)
        {
            _isActive = false;
        }
    }

    private void UpdateLeap(float delta)
    {
        if (_player == null || _duration <= 0)
        {
            _isActive = false;
            return;
        }

        _elapsed += delta;
        float t = Mathf.Clamp(_elapsed / _duration, 0f, 1f);

        // ✅ LINEAR interpolation (no easing) - travels full distance
        _player.GlobalPosition = _startPos.Lerp(_targetPos, t);

        // Check if leap completed
        if (t >= 1.0f)
        {
            _isActive = false;
        }

        // Future: Add parabolic arc
        // float heightOffset = CalculateParabolicHeight(t, maxHeight);
        // _player.GlobalPosition += new Vector2(0, -heightOffset);
    }

    private void UpdateCharge(float delta)
    {
        // Future implementation: Linear movement with collision detection
        // For now, use same logic as dash
        UpdateDash(delta);

        // Future: Detect collisions during charge
        // if (HitWall()) { _isActive = false; TriggerChargeImpact(); }
    }

    // Future helper methods:
    // private float CalculateParabolicHeight(float t, float maxHeight);
    // private bool HitWall();
    // private void TriggerChargeImpact();
}
