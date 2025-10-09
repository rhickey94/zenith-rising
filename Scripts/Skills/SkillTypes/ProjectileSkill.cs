using Godot;
using SpaceTower.Scripts.Skills.Base;

namespace SpaceTower.Scripts.Skills.SkillTypes;

[GlobalClass]
public partial class ProjectileSkill : Skill
{
    [Export] public float DirectDamage { get; set; } = 50f;
    [Export] public float ExplosionDamage { get; set; } = 100f;
    [Export] public float ExplosionRadius { get; set; } = 100f;
}
