using Godot;

namespace ZenithRising.Scripts.Skills.Balance.Data;

[GlobalClass]
public partial class MeleeData : Resource
{
    [ExportGroup("Melee Properties")]
    [Export] public float Range { get; set; } = 0f; // For melee/projectile
    [Export] public float Width { get; set; } = 0f; // For melee arcs
    [Export] public float CastTime { get; set; } = 0f; // Animation duration
}
