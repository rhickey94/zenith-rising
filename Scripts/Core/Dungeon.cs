using Godot;
using ZenithRising.Scripts.Enemies.Base;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.PlayerScripts.Components;
using ZenithRising.Scripts.UI.HUD;
using ZenithRising.Scripts.UI.Panels;

namespace ZenithRising.Scripts.Core;

public partial class Dungeon : Node
{
    // ===== EXPORT FIELDS - Scenes =====
    [Export] public PackedScene[] EnemyScenes;
    [Export] public PackedScene BossScene;

    // ===== EXPORT FIELDS - Dependencies =====
    [Export] public Player Player;
    [Export] public Hud HUD;
    [Export] public FloorTransitionPanel FloorTransitionPanel;
    [Export] private VictoryScreen _victoryScreen;
    [Export] private DeathScreen _deathScreen;
    [Export] private ResultsScreen _resultsScreen;

    // ===== EXPORT FIELDS - Settings =====
    [Export] public string HubScenePath = "res://Scenes/Core/hub.tscn";
    [Export] public float SpawnDistance = 400.0f;
    [Export] public float FloorDuration = 30f;

    // ===== PRIVATE STATE - Floor/Wave Tracking =====
    private int _currentFloor = 1;
    private int _currentWave = 1;
    private float _floorTimeElapsed = 0f;
    private float _timeSinceLastSpawn = 0f;
    private bool _bossSpawned = false;

    // ===== PRIVATE STATE - Enemy Tracking =====
    private int _enemyCount = 0;
    private bool _waitingForBossDefeat = false;

    // ===== PRIVATE STATE - Run Stats =====
    private int _floorsCleared = 0;
    private int _bossesKilled = 0;
    private int _enemiesKilled = 0;
    private float _totalGameTime = 0f;
    private bool _finalBossDefeated = false;
    private int _lastCharacterXPAwarded = 0;

    // ===== LIFECYCLE METHODS =====
    public override void _Ready()
    {
        // Validate dependencies
        if (!ValidateDependencies())
        {
            GD.PrintErr("Dungeon: Missing dependencies - cannot start!");
            return;
        }

        ResetRunTracking();
        Player.Initialize();
        UpdateHUD();
        SetupFloorTransitionPanel();
        SetupResultsScreen();
        SetupVictoryDeathScreens();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (GameBalance.Instance == null || GameBalance.Instance.Config == null)
        {
            return; // GameBalance not ready yet, skip this frame
        }

        _floorTimeElapsed += (float)delta;
        _timeSinceLastSpawn += (float)delta;
        _totalGameTime += (float)delta;
        var config = GameBalance.Instance.Config.Enemy;

        // Update wave progression
        UpdateWaveProgression();

        // Boss spawning at 5:00 mark
        if (_floorTimeElapsed >= config.FloorDuration && !_bossSpawned)
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

    // ===== PUBLIC API =====
    public void OnPlayerDeath()
    {
        // Calculate and award character XP
        int characterXP = CalculateCharacterXP();
        _lastCharacterXPAwarded = characterXP;

        var statsManager = Player?.GetNode<StatsManager>("StatsManager");
        if (statsManager != null)
        {
            statsManager.AddCharacterExperience(_lastCharacterXPAwarded);

            // Reset run state BEFORE saving (death ends run)
            var upgradeManager = Player?.GetNode<UpgradeManager>("UpgradeManager");
            upgradeManager?.ClearUpgrades();

            // Reset power level
            statsManager.PowerLevel = 1;
            statsManager.PowerExperience = 0;

            // NOW save with cleared run state
            SaveCharacterProgress();
        }

        int playerLevel = statsManager?.PowerLevel ?? 1;
        _deathScreen?.ShowScreen(_totalGameTime, _enemiesKilled, playerLevel, _currentFloor);
    }

    // ===== SCREEN MANAGEMENT =====
    private void ShowVictoryScreen()
    {
        int characterXP = CalculateCharacterXP();
        _lastCharacterXPAwarded = characterXP;

        var statsManager = Player?.GetNode<StatsManager>("StatsManager");
        if (statsManager != null)
        {
            statsManager.AddCharacterExperience(_lastCharacterXPAwarded);

            // Reset run state BEFORE saving (victory ends run)
            var upgradeManager = Player?.GetNode<UpgradeManager>("UpgradeManager");
            upgradeManager?.ClearUpgrades();

            statsManager.PowerLevel = 1;
            statsManager.PowerExperience = 0;

            // NOW save with cleared run state
            SaveCharacterProgress();
        }

        var playerLevel = statsManager?.PowerLevel ?? 1;
        _victoryScreen?.ShowScreen(_totalGameTime, _enemiesKilled, playerLevel, _currentFloor);
    }

    private void ShowResultsScreen()
    {
        var statsManager = Player?.GetNode<StatsManager>("StatsManager");
        if (statsManager == null || _resultsScreen == null)
        {
            GD.PrintErr("Cannot show results screen - missing dependencies");
            return;
        }

        _resultsScreen.ShowScreen(
            _totalGameTime,
            _enemiesKilled,
            _floorsCleared,
            _bossesKilled,
            _lastCharacterXPAwarded,
            statsManager
        );
    }

    private void ShowFloorTransitionUI()
    {
        FloorTransitionPanel.ShowPanel(_currentFloor, _currentFloor + 1);
    }

    // ===== GAME LOOP - Spawning =====
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

    private void OnEnemyDestroyed()
    {
        // If we're being destroyed or scene is changing, ignore this signal
        if (!IsInsideTree() || IsQueuedForDeletion())
        {
            return;
        }

        _enemyCount--;
        _enemiesKilled++;

        // Check if boss was defeated (all enemies dead after boss spawned)
        if (_waitingForBossDefeat && _enemyCount <= 0)
        {
            _waitingForBossDefeat = false;
            OnBossDefeated();
        }
    }

    // ===== GAME LOOP - Wave/Floor Progression =====
    private void UpdateWaveProgression()
    {
        var config = GameBalance.Instance.Config.Enemy;
        int newWave = Mathf.Min((int)(_floorTimeElapsed / config.WaveDuration) + 1, config.WavesPerFloor);

        if (newWave != _currentWave)
        {
            _currentWave = newWave;
            UpdateHUD();
        }
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
        _floorsCleared = _currentFloor - 1;

        UpdateHUD();
    }

    private void OnBossDefeated()
    {
        var config = GameBalance.Instance.Config.Enemy;
        _bossesKilled++;

        // Update highest floor (checkpoint at floor COMPLETION)
        var statsManager = Player?.GetNode<StatsManager>("StatsManager");
        if (statsManager != null)
        {
            statsManager.UpdateHighestFloor(_currentFloor); // Current floor completed
        }

        // SAVE CHARACTER PROGRESS (floor completed)
        SaveCharacterProgress();

        if (_currentFloor >= config.MaxFloors)
        {
            _finalBossDefeated = true;
            ShowVictoryScreen();
        }
        else
        {
            ShowFloorTransitionUI();
        }
    }

    // ===== SIGNAL HANDLERS - Floor Transition =====
    private void OnContinueToNextFloor()
    {
        AdvanceToNextFloor();
    }

    private void OnEndRun()
    {
        SaveCharacterProgress();

        // Return to hub
        GetTree().Paused = false; // Ensure game is unpaused before scene change
        GetTree().ChangeSceneToFile(HubScenePath);
    }

    // ===== SIGNAL HANDLERS - Results Screen =====
    private void OnAllocateStatsRequested()
    {
        // Open stat allocation panel
        Player?.StatAllocationPanel?.ShowPanel(
            Player.GetNode<StatsManager>("StatsManager"),
            Player.GetNode<UpgradeManager>("UpgradeManager")
        );
    }

    private void OnReturnToMenuFromResults()
    {
        // Return to hub
        GetTree().ChangeSceneToFile(HubScenePath);
    }

    // ===== CALCULATION HELPERS =====
    private float GetCurrentSpawnInterval()
    {
        var config = GameBalance.Instance.Config.Enemy;
        float t = (_currentWave - 1) / (float)(config.WavesPerFloor - 1);
        return Mathf.Lerp(config.InitialSpawnInterval, config.FinalSpawnInterval, t);
    }

    private float CalculateHealthMultiplier()
    {
        var config = GameBalance.Instance.Config.Enemy;
        float floorMult = 1.0f + (_currentFloor - 1) * config.StatsPerFloor;
        float waveMult = 1.0f + (_currentWave - 1) * config.HealthPerWave;
        return floorMult * waveMult;
    }

    private float CalculateDamageMultiplier()
    {
        var config = GameBalance.Instance.Config.Enemy;
        float floorMult = 1.0f + (_currentFloor - 1) * config.StatsPerFloor;
        float waveMult = 1.0f + (_currentWave - 1) * config.DamagePerWave;
        return floorMult * waveMult;
    }

    private int CalculateCharacterXP()
    {
        int xp = 50; // Base participation
        xp += _floorsCleared * 100; // Per floor cleared
        xp += _bossesKilled * 150; // Per boss killed

        if (_finalBossDefeated)
        {
            xp += 500; // Victory bonus
        }

        // TODO Phase 3: Optional performance bonuses
        // if (_totalGameTime < 900f) xp += 100; // Speed clear bonus (< 15 min)
        // if (player never died) xp += 150; // No death bonus

        return xp;
    }

    // ===== INITIALIZATION & SETUP =====
    private void SetupFloorTransitionPanel()
    {
        // Just connect signals - panel already exists in scene
        FloorTransitionPanel.ContinueButtonPressed += OnContinueToNextFloor;
        FloorTransitionPanel.EndRunButtonPressed += OnEndRun;
    }

    private void SetupResultsScreen()
    {
        _resultsScreen.AllocateStatsRequested += OnAllocateStatsRequested;
        _resultsScreen.ReturnToMenuRequested += OnReturnToMenuFromResults;
    }

    private void SetupVictoryDeathScreens()
    {
        _victoryScreen.ContinueButtonPressed += ShowResultsScreen;
        _deathScreen.ContinueButtonPressed += ShowResultsScreen;
    }

    private void ResetRunTracking()
    {
        _floorsCleared = 0;
        _bossesKilled = 0;
        _finalBossDefeated = false;
        _enemiesKilled = 0;
        _totalGameTime = 0f;
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

    private bool ValidateDependencies()
    {
        bool valid = true;

        if (Player == null)
        {
            GD.PrintErr("Dungeon: Player not assigned!");
            valid = false;
        }

        if (HUD == null)
        {
            GD.PrintErr("Dungeon: HUD not assigned!");
            valid = false;
        }

        if (EnemyScenes == null || EnemyScenes.Length == 0)
        {
            GD.PrintErr("Dungeon: EnemyScenes not assigned!");
            valid = false;
        }

        if (BossScene == null)
        {
            GD.PrintErr("Dungeon: BossScene not assigned!");
            valid = false;
        }

        if (FloorTransitionPanel == null)
        {
            GD.PrintErr("Dungeon: FloorTransitionPanel not assigned!");
            valid = false;
        }

        if (_victoryScreen == null)
        {
            GD.PrintErr("Dungeon: VictoryScreen not assigned!");
            valid = false;
        }

        if (_deathScreen == null)
        {
            GD.PrintErr("Dungeon: DeathScreen not assigned!");
            valid = false;
        }

        if (_resultsScreen == null)
        {
            GD.PrintErr("Dungeon: ResultsScreen not assigned!");
            valid = false;
        }

        return valid;
    }

    // ===== PRIVATE HELPERS - Save System =====
    private void SaveCharacterProgress()
    {
        var statsManager = Player?.GetNode<StatsManager>("StatsManager");
        var upgradeManager = Player?.GetNode<UpgradeManager>("UpgradeManager");

        if (statsManager == null)
        {
            GD.PrintErr("Cannot save - StatsManager not found!");
            return;
        }

        // Get base data from StatsManager
        SaveData saveData = statsManager.GetSaveData();

        // Add run state from Game + UpgradeManager
        saveData.CurrentFloor = _currentFloor;
        saveData.ActiveUpgrades = upgradeManager?.GetActiveUpgrades() ?? [];
        saveData.HasActiveRun = saveData.PowerLevel > 1 || saveData.ActiveUpgrades.Count > 0;

        SaveManager.Instance?.SaveGame(saveData);
    }
}
