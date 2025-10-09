using Godot;
using SpaceTower.Scripts.Skills.Base;

namespace SpaceTower.Scripts.Skills.SkillTypes;

[GlobalClass]
public partial class StunSkill : Skill
{
    [Export] public float Range { get; set; } = 10f;
    [Export] public float Damage { get; set; } = 50f;
    [Export] public float Lifetime { get; set; } = 0.3f;
    [Export] public float StunDuration { get; set; } = 2.0f;
}
