using Godot;
using SpaceTower.Scripts.Enemies.Base;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.PlayerScripts.Components;
using SpaceTower.Scripts.UI.HUD;
using SpaceTower.Scripts.UI.Panels;

namespace SpaceTower.Scripts.Core;

public partial class Game : Node
{
    // Scenes
    [Export] public PackedScene[] EnemyScenes;
    [Export] public PackedScene BossScene; // NEW: Boss enemy scene

    // Dependencies
    [Export] public Player Player;
    [Export] public Hud HUD;
    [Export] public FloorTransitionPanel FloorTransitionPanel;
    [Export] private Control _victoryScreen;
    [Export] private Control _deathScreen;

    [Export] public string MainMenuScenePath = "res://Scenes/UI/Menus/main_menu.tscn";

    // Settings
    [Export] public float SpawnDistance = 400.0f;

    // Wave/Floor Constants
    [Export] public float FloorDuration = 30f; // 5 minutes per floor
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
    private int _enemyCount = 0;
    private bool _waitingForBossDefeat = false;
    private float _totalGameTime = 0f;
    private int _enemiesKilled = 0;

    public override void _Ready()
    {
        // Validate dependencies
        if (!ValidateDependencies())
        {
            GD.PrintErr("Game: Missing dependencies - cannot start!");
            return;
        }

        Player.Initialize();
        UpdateHUD();
        SetupFloorTransitionPanel();
    }

    public override void _PhysicsProcess(double delta)
    {
        _floorTimeElapsed += (float)delta;
        _timeSinceLastSpawn += (float)delta;
        _totalGameTime += (float)delta;

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

    public void OnPlayerDeath()
    {
        var deathScreen = _deathScreen as DeathScreen;
        int playerLevel = Player?.GetNode<StatsManager>("StatsManager")?.RunLevel ?? 1;
        deathScreen?.ShowScreen(_totalGameTime, _enemiesKilled, playerLevel, _currentFloor);
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

        enemy.TreeExited += OnEnemyDestroyed;
        _enemyCount++;

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

        boss.TreeExited += OnEnemyDestroyed;
        _enemyCount++;
        _waitingForBossDefeat = true;

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

        string floorName = $"Floor {_currentFloor}";
        HUD.UpdateFloorInfo(_currentFloor, floorName);

        int enemiesRemaining = _enemyCount; // or track separately
        HUD.UpdateWaveInfo(_currentWave, enemiesRemaining);
    }

    private void AdvanceToNextFloor()
    {
        _currentFloor++;
        _currentWave = 1;
        _floorTimeElapsed = 0f;
        _timeSinceLastSpawn = 0f;
        _bossSpawned = false;
        _enemyCount = 0;
        _waitingForBossDefeat = false;

        UpdateHUD();
        GD.Print($"Advanced to Floor {_currentFloor}!");
    }

    private void SetupFloorTransitionPanel()
    {
        // Just connect signals - panel already exists in scene
        FloorTransitionPanel.ContinueButtonPressed += OnContinueToNextFloor;
        FloorTransitionPanel.EndRunButtonPressed += OnEndRun;

        GD.Print("Floor transition panel setup complete");
    }

    private void ShowFloorTransitionUI()
    {
        FloorTransitionPanel.ShowPanel(_currentFloor, _currentFloor + 1);
    }

    private void ShowVictoryScreen()
    {
        var victoryScreen = _victoryScreen as VictoryScreen;
        var playerLevel = Player?.GetNode<StatsManager>("StatsManager")?.RunLevel ?? 1;

        victoryScreen?.ShowScreen(_totalGameTime, _enemiesKilled, playerLevel, _currentFloor);
        GD.Print("Victory! Player has completed all floors.");
    }

    private void OnContinueToNextFloor()
    {
        GD.Print("Player chose to continue to next floor");
        AdvanceToNextFloor();
    }

    private void OnEndRun()
    {
        GD.Print("Player chose to end run - returning to main menu");

        // TODO: Phase 2 - Show rewards summary first

        // Return to main menu
        GetTree().Paused = false; // Ensure game is unpaused before scene change
        GetTree().ChangeSceneToFile(MainMenuScenePath);
    }

    private void OnEnemyDestroyed()
    {
        _enemyCount--;
        _enemiesKilled++;

        // Check if boss was defeated (all enemies dead after boss spawned)
        if (_waitingForBossDefeat && _enemyCount <= 0)
        {
            _waitingForBossDefeat = false;
            GD.Print($"Boss defeated! Enemy count: {_enemyCount}");
            OnBossDefeated();
        }
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

    private bool ValidateDependencies()
    {
        bool valid = true;

        if (Player == null)
        {
            GD.PrintErr("Game: Player not assigned!");
            valid = false;
        }

        if (HUD == null)
        {
            GD.PrintErr("Game: HUD not assigned!");
            valid = false;
        }

        if (EnemyScenes == null || EnemyScenes.Length == 0)
        {
            GD.PrintErr("Game: EnemyScenes not assigned!");
            valid = false;
        }

        if (BossScene == null)
        {
            GD.PrintErr("Game: BossScene not assigned!");
            valid = false;
        }

        if (FloorTransitionPanel == null)
        {
            GD.PrintErr("Game: FloorTransitionPanel not assigned!");
            valid = false;
        }

        if (_victoryScreen == null)
        {
            GD.PrintErr("Game: VictoryScreen not assigned!");
            valid = false;
        }

        if (_deathScreen == null)
        {
            GD.PrintErr("Game: DeathScreen not assigned!");
            valid = false;
        }

        return valid;
    }
}
