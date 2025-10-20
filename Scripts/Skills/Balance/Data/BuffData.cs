using Godot;

namespace ZenithRising.Scripts.Skills.Balance.Data;

[GlobalClass]
public partial class BuffData : Resource
{
    [ExportGroup("Buff Properties")]
    [Export] public float Duration { get; set; } = 5f;
    [Export] public float AttackSpeedBonus { get; set; } = 0f;
    [Export] public float MoveSpeedBonus { get; set; } = 0f;
    [Export] public float CastSpeedBonus { get; set; } = 0f;
    [Export] public float DamageBonus { get; set; } = 0f;
    [Export] public float CooldownReductionBonus { get; set; } = 0f;
    [Export] public float DamageReductionBonus { get; set; } = 0f;
}
