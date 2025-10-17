using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Data;

[GlobalClass]
public partial class MeleeAttackSkill : Skill
{
    [Export] public float Range { get; set; } = 10f;
    [Export] public float Damage { get; set; } = 50f;
    [Export] public float Lifetime { get; set; } = 0.3f;
}
