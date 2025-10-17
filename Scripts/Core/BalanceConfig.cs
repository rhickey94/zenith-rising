using Godot;

namespace ZenithRising.Scripts.Core;

[GlobalClass]
public partial class BalanceConfig : Resource
{
    [ExportGroup("Player Stats")]
    [Export] public PlayerStatsConfig PlayerStats { get; set; } = new();

    [ExportGroup("Character Progression")]
    [Export] public CharacterProgressionConfig CharacterProgression { get; set; } = new();

    [ExportGroup("Combat System")]
    [Export] public CombatSystemConfig CombatSystem { get; set; } = new();

    [ExportGroup("Enemy Config")]
    [Export] public EnemyConfig Enemy { get; set; } = new();

    [ExportGroup("Upgrade System")]
    [Export] public UpgradeSystemConfig UpgradeSystem { get; set; } = new();
}


[GlobalClass]
public partial class PlayerStatsConfig : Resource
{
    [ExportGroup("Base Stats")]
    [Export] public float BaseMaxHealth { get; set; } = 100.0f;
    [Export] public float BaseSpeed { get; set; } = 300.0f;
    [Export] public float BaseFireRate { get; set; } = 0.2f;
    [Export] public float BaseMeleeRate { get; set; } = 0.5f;
    [Export] public float BasePickupRadius { get; set; } = 80f;
}


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


[GlobalClass]
public partial class CombatSystemConfig : Resource
{
    [Export] public float BaseCritDamageMultiplier { get; set; } = 1.5f;
    [Export] public float MaxCritChance { get; set; } = 1.0f;
    [Export] public float AgilityCritChanceCap { get; set; } = 0.5f;
}


[GlobalClass]
public partial class EnemyConfig : Resource
{
    [ExportGroup("Spawning")]
    [Export] public float InitialSpawnInterval { get; set; } = 2.0f;
    [Export] public float FinalSpawnInterval { get; set; } = 0.8f;
    [Export] public float SpawnDistance { get; set; } = 400.0f;

    [ExportGroup("Scaling")]
    [Export] public float HealthPerWave { get; set; } = 0.10f;
    [Export] public float DamagePerWave { get; set; } = 0.05f;
    [Export] public float StatsPerFloor { get; set; } = 0.50f;
    [Export] public float BossHealthMultiplier { get; set; } = 5.0f;
    [Export] public float BossDamageMultiplier { get; set; } = 2.0f;

    [ExportGroup("Wave/Floor System")]
    [Export] public float WaveDuration { get; set; } = 30f;
    [Export] public int WavesPerFloor { get; set; } = 10;
    [Export] public int MaxFloors { get; set; } = 5;
    [Export] public float FloorDuration { get; set; } = 30f;
}


[GlobalClass]
public partial class UpgradeSystemConfig : Resource
{
    [Export] public float DamageBoostPerStack { get; set; } = 0.15f;
    [Export] public float AttackSpeedPerStack { get; set; } = 0.20f;
    [Export] public float MovementSpeedPerStack { get; set; } = 0.15f;
    [Export] public float MaxHealthPerStack { get; set; } = 50f;
    [Export] public float PickupRadiusPerStack { get; set; } = 30f;
    [Export] public int ProjectilePiercePerStack { get; set; } = 1;
    [Export] public float CritChancePerStack { get; set; } = 0.10f;
    [Export] public float HealthRegenPerStack { get; set; } = 2f;
    [Export] public float BaseSpeedPerLevel { get; set; } = 10f;
}
