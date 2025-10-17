using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Data;

[GlobalClass]
public partial class ProjectileSkill : Skill
{
    [Export] public float DirectDamage { get; set; } = 50f;
    [Export] public float ExplosionDamage { get; set; } = 100f;
    [Export] public float ExplosionRadius { get; set; } = 100f;
}
