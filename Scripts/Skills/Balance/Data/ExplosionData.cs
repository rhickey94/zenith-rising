using Godot;

namespace ZenithRising.Scripts.Skills.Balance.Data;

[GlobalClass]
public partial class ExplosionData : Resource
{
    [ExportGroup("Explosion Properties")]
    [Export] public float ExplosionDamage { get; set; } = 0f;
    [Export] public float ExplosionRadius { get; set; } = 0f;
    [Export] public float ExplosionKnockback { get; set; } = 0f;
}
