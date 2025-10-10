using Godot;
using SpaceTower.Scripts.Skills.Base;

namespace SpaceTower.Scripts.Skills.Data;

[GlobalClass]
public partial class TeleportSkill : Skill
{
    [Export] public float MaxDistance { get; set; } = 300f;
}
