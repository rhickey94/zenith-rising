using Godot;
using SpaceTower.Scripts.Skills.Base;

namespace SpaceTower.Scripts.Skills.Data;

[GlobalClass]
public partial class DashSkill : Skill
{
    [Export] public float Distance { get; set; } = 300.0f;
    [Export] public float Duration { get; set; } = 0.2f;
    [Export] public bool HasInvulnerability { get; set; } = true;
}
