using Godot;
using SpaceTower.Scripts.Enemies.Base;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.UI.HUD;

namespace SpaceTower.Scripts.Core;

public partial class Game : Node
{
    // Scenes
    [Export] public PackedScene[] EnemyScenes;

    // Dependencies
    [Export] public Player Player;
    [Export] public Hud HUD;

    // Settings
    [Export] public float SpawnDistance = 400.0f;
    [Export] public float SpawnInterval = 2.0f;

    private Timer _spawnTimer;
    private int _enemiesSpawned = 0;

    public override void _Ready()
    {
        _spawnTimer = GetNode<Timer>("SpawnTimer");
        _spawnTimer.WaitTime = SpawnInterval;
        _spawnTimer.Timeout += SpawnEnemy;

        // Validate dependencies
        if (Player == null)
        {
            GD.PrintErr("Game: Player not assigned! Drag Player node into Game's Player field.");
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
        AddChild(enemy);

        _enemiesSpawned++;
    }
}
