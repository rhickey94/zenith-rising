using Godot;

namespace ZenithRising.Scripts.Core.Config;

[GlobalClass]
public partial class PlayerStatsConfig : Resource
{
    [ExportGroup("Base Stats")]
    [Export] public float BaseMaxHealth { get; set; } = 100.0f;
    [Export] public float BaseSpeed { get; set; } = 300.0f;
    [Export] public float BaseFireRate { get; set; } = 0.2f;
    [Export] public float BaseMeleeRate { get; set; } = 0.5f;
    [Export] public float BasePickupRadius { get; set; } = 80f;
    [Export] public float BaseAttackRate { get; set; } = 300.0f;
    [Export] public float BaseCastSpeed { get; set; } = 0.2f;
    [Export] public float BaseDamage { get; set; } = 80f;
}
