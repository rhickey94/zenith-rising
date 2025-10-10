using Godot;
using SpaceTower.Scripts.Skills.Base;

namespace SpaceTower.Scripts.Skills.Data;

[GlobalClass]
public partial class InstantAOESkill : Skill
{
    [Export] public float Radius { get; set; } = 100f;
    [Export] public float Damage { get; set; } = 50f;
}
