using Godot;
using System;

namespace ZenithRising.Scripts.Core.Config;

[GlobalClass]
public partial class CharacterProgressionConfig : Resource
{
    [ExportGroup("Stat Scaling Formulas")]
    [Export] public float StrengthDamagePerPoint { get; set; } = 0.03f;
    [Export] public float StrengthHealthPerPoint { get; set; } = 10f;
    [Export] public float IntelligenceDamagePerPoint { get; set; } = 0.03f;
    [Export] public float IntelligenceCDRPerPoint { get; set; } = 0.02f;
    [Export] public float AgilityAttackSpeedPerPoint { get; set; } = 0.02f;
    [Export] public float AgilityCritPerPoint { get; set; } = 0.01f;
    [Export] public float VitalityHealthPerPoint { get; set; } = 25f;
    [Export] public float VitalityRegenPerPoint { get; set; } = 0.5f;
    [Export] public float FortuneCritDamagePerPoint { get; set; } = 0.02f;
    [Export] public float FortuneDropRatePerPoint { get; set; } = 0.01f;

    [ExportGroup("Character Leveling")]
    [Export] public int CharacterXPBase { get; set; } = 500;
    [Export] public float CharacterXPGrowth { get; set; } = 1.1f;
    [Export] public float PowerLevelHealthBonus { get; set; } = 20f;
}
