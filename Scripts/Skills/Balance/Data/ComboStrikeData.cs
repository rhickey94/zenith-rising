using Godot;

namespace ZenithRising.Scripts.Skills.Balance.Data;

/// <summary>
/// Defines a single strike in a combo attack sequence.
/// Each strike can have its own hitbox, timing, damage, and position.
/// </summary>
[GlobalClass]
public partial class ComboStrikeData : Resource
{
    [ExportGroup("Strike Identity")]
    [Export] public int StrikeIndex { get; set; } = 0; // 0 = first strike, 1 = second, etc.

    [ExportGroup("Hitbox Configuration")]
    [Export] public string HitboxName { get; set; } = "SmallMeleeHitbox"; // References hitbox in player scene
    [Export] public Vector2 PositionOffset { get; set; } = Vector2.Zero; // Offset from player position
    [Export] public float RotationOffset { get; set; } = 0f; // Additional rotation (degrees)

    [ExportGroup("Damage")]
    [Export] public float DamageMultiplier { get; set; } = 1.0f; // Strike 3 might be 1.5x

    [ExportGroup("Visual Effects (Optional)")]
    [Export] public bool TeleportPlayerToOffset { get; set; } = false; // For dash-strike patterns
    [Export] public Vector2 TeleportOffset { get; set; } = Vector2.Zero; // Where to teleport player
}
