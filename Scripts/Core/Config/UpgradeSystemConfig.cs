using Godot;

namespace ZenithRising.Scripts.Core.Config;

[GlobalClass]
public partial class UpgradeSystemConfig : Resource
{
    [Export] public float DamageBoostPerStack { get; set; } = 0.15f;
    [Export] public float AttackSpeedPerStack { get; set; } = 0.20f;
    [Export] public float MovementSpeedPerStack { get; set; } = 0.15f;
    [Export] public float MaxHealthPerStack { get; set; } = 50f;
    [Export] public float PickupRadiusPerStack { get; set; } = 30f;
    [Export] public int ProjectilePiercePerStack { get; set; } = 1;
    [Export] public float CritChancePerStack { get; set; } = 0.10f;
    [Export] public float HealthRegenPerStack { get; set; } = 2f;
    [Export] public float BaseSpeedPerLevel { get; set; } = 10f;
}
