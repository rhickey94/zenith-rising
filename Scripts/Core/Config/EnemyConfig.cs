using Godot;

namespace ZenithRising.Scripts.Core.Config;

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
