using Godot;
using SpaceTower.Scripts.Enemies.Base;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.UI.HUD;

namespace SpaceTower.Scripts.Core;

public partial class Game : Node
{
    // Scenes
    [Export] public PackedScene EnemyScene;

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

        if (EnemyScene == null)
        {
            GD.PrintErr("Game: EnemyScene not assigned!");
        }

        Player.Initialize();
    }

    private void SpawnEnemy()
    {
        if (Player == null || EnemyScene == null)
        {
            return;
        }


        var angle = GD.Randf() * Mathf.Tau;
        var spawnPosition = Player.Position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * SpawnDistance;

        var enemy = EnemyScene.Instantiate<Enemy>();
        enemy.Position = spawnPosition;
        AddChild(enemy);

        _enemiesSpawned++;
    }
}
