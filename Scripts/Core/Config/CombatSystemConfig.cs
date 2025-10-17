using Godot;

namespace ZenithRising.Scripts.Core.Config;

[GlobalClass]
public partial class CombatSystemConfig : Resource
{
    [Export] public float BaseCritDamageMultiplier { get; set; } = 1.5f;
    [Export] public float MaxCritChance { get; set; } = 1.0f;
    [Export] public float AgilityCritChanceCap { get; set; } = 0.5f;
}
