using Godot;
using SpaceTower.Scripts.Enemies.Base;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.UI.HUD;

namespace SpaceTower.Scripts.Core;

public partial class Game : Node
{
    // Scenes
    [Export] public PackedScene[] EnemyScenes;
    [Export] public PackedScene BossScene; // NEW: Boss enemy scene

    // Dependencies
    [Export] public Player Player;
    [Export] public Hud HUD;

    // Settings
    [Export] public float SpawnDistance = 400.0f;

    // Wave/Floor Constants
    private const float FloorDuration = 60f; // 5 minutes per floor
    private const float WaveDuration = 30f;   // 30 seconds per wave
    private const int WavesPerFloor = 10;
    private const int MaxFloors = 5;

    // Spawn rate progression (2.0s → 0.8s over 10 waves)
    private const float InitialSpawnInterval = 2.0f;
    private const float FinalSpawnInterval = 0.8f;

    // Scaling constants
    private const float HealthPerWave = 0.10f;    // +10% HP per wave
    private const float DamagePerWave = 0.05f;    // +5% damage per wave
    private const float StatsPerFloor = 0.50f;    // +50% per floor

    // State tracking
    private int _currentFloor = 1;
    private int _currentWave = 1;
    private float _floorTimeElapsed = 0f;
    private float _timeSinceLastSpawn = 0f;
    private bool _bossSpawned = false;

    public override void _Ready()
    {
        // Validate dependencies
        if (Player == null)
        {
            GD.PrintErr("Game: Player not assigned!");
            return;
        }

        if (HUD == null)
        {
            GD.PrintErr("Game: HUD not assigned!");
            return;
        }

        if (EnemyScenes == null || EnemyScenes.Length == 0)
        {
            GD.PrintErr("Game: EnemyScenes not assigned!");
        }

        Player.Initialize();
        UpdateHUD();
    }

    public override void _PhysicsProcess(double delta)
    {
        _floorTimeElapsed += (float)delta;
        _timeSinceLastSpawn += (float)delta;

        // Update wave progression
        UpdateWaveProgression();

        // Boss spawning at 5:00 mark
        if (_floorTimeElapsed >= FloorDuration && !_bossSpawned)
        {
            SpawnBoss();
            _bossSpawned = true;
            return; // Stop regular spawning
        }

        // Regular enemy spawning (before boss)
        if (!_bossSpawned && _timeSinceLastSpawn >= GetCurrentSpawnInterval())
        {
            SpawnEnemy();
            _timeSinceLastSpawn = 0f;
        }
    }

    private void UpdateWaveProgression()
    {
        int newWave = Mathf.Min((int)(_floorTimeElapsed / WaveDuration) + 1, WavesPerFloor);

        if (newWave != _currentWave)
        {
            _currentWave = newWave;
            GD.Print($"Wave {_currentWave} started!");
            UpdateHUD();
        }
    }

    private float GetCurrentSpawnInterval()
    {
        // Linear interpolation from 2.0s to 0.8s over 10 waves
        float t = (_currentWave - 1) / (float)(WavesPerFloor - 1);
        return Mathf.Lerp(InitialSpawnInterval, FinalSpawnInterval, t);
    }

    private void SpawnEnemy()
    {
        if (Player == null || EnemyScenes == null || EnemyScenes.Length == 0)
        {
            return;
        }


        var angle = GD.Randf() * Mathf.Tau;
        var spawnPosition = Player.Position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * SpawnDistance;

        var enemyScene = EnemyScenes[GD.Randi() % EnemyScenes.Length];
        var enemy = enemyScene.Instantiate<Enemy>();
        enemy.Position = spawnPosition;

        // Calculate scaling multipliers
        float healthMult = CalculateHealthMultiplier();
        float damageMult = CalculateDamageMultiplier();

        enemy.Initialize(healthMult, damageMult);

        AddChild(enemy);
    }

    private void SpawnBoss()
    {
        if (Player == null || BossScene == null)
        {
            GD.PrintErr("Cannot spawn boss - missing Player or BossScene");
            return;
        }

        GD.Print($"BOSS SPAWNED - Floor {_currentFloor}!");

        var angle = GD.Randf() * Mathf.Tau;
        var spawnPosition = Player.Position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * SpawnDistance;

        var boss = BossScene.Instantiate<Enemy>();
        boss.Position = spawnPosition;

        // Boss scaling: 5x HP, 2x damage (plus floor/wave scaling)
        float healthMult = CalculateHealthMultiplier() * 5.0f;
        float damageMult = CalculateDamageMultiplier() * 2.0f;

        boss.Initialize(healthMult, damageMult);

        AddChild(boss);
        UpdateHUD();
    }

    private float CalculateHealthMultiplier()
    {
        // Base = 1.0
        // Floor scaling: +50% per floor beyond 1
        float floorMult = 1.0f + (_currentFloor - 1) * StatsPerFloor;

        // Wave scaling: +10% per wave beyond 1
        float waveMult = 1.0f + (_currentWave - 1) * HealthPerWave;

        return floorMult * waveMult;
    }

    private float CalculateDamageMultiplier()
    {
        // Base = 1.0
        // Floor scaling: +50% per floor beyond 1
        float floorMult = 1.0f + (_currentFloor - 1) * StatsPerFloor;

        // Wave scaling: +5% per wave beyond 1
        float waveMult = 1.0f + (_currentWave - 1) * DamagePerWave;

        return floorMult * waveMult;
    }

    private void UpdateHUD()
    {
        if (HUD == null)
        {
            return;
        }


        string status = _bossSpawned ? "BOSS FIGHT" : $"Wave {_currentWave}/{WavesPerFloor}";
        // You'd add a method to HUD.cs to display this:
        // HUD.UpdateFloorWaveDisplay(_currentFloor, status);
    }

    // Called when boss is defeated - show floor transition UI
    private void OnBossDefeated()
    {
        if (_currentFloor >= MaxFloors)
        {
            // Victory! Beat all 5 floors
            ShowVictoryScreen();
        }
        else
        {
            // Show Continue/End Run choice
            ShowFloorTransitionUI();
        }
    }

    private void AdvanceToNextFloor()
    {
        _currentFloor++;
        _currentWave = 1;
        _floorTimeElapsed = 0f;
        _timeSinceLastSpawn = 0f;
        _bossSpawned = false;

        UpdateHUD();
        GD.Print($"Advanced to Floor {_currentFloor}!");
    }

    private void ShowFloorTransitionUI()
    {
        // TODO: Implement UI panel with Continue/End Run buttons
        GD.Print("Floor cleared! Show transition UI here.");
    }

    private void ShowVictoryScreen()
    {
        // TODO: Implement victory screen
        GD.Print("VICTORY! All 5 floors cleared!");
    }
}
