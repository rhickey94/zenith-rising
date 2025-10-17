using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Data;

[GlobalClass]
public partial class InstantAOESkill : Skill
{
    [Export] public float Radius { get; set; } = 100f;
    [Export] public float Damage { get; set; } = 50f;
}
