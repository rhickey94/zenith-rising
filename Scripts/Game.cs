using Godot;
using System;

public partial class Game : Node
{
  [Export]
  public PackedScene EnemyScene;

  [Export]
  public Node2D Player;

  [Export]
  public float SpawnDistance = 400.0f;

  private Timer _spawnTimer;

  public override void _Ready()
  {
    _spawnTimer = GetNode<Timer>("SpawnTimer");
    _spawnTimer.Timeout += SpawnEnemy;
  }

  private void SpawnEnemy()
  {
    if (Player == null) return;

    var angle = GD.Randf() * Mathf.Tau;
    var spawnPosition = Player.Position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * SpawnDistance;

    var enemy = EnemyScene.Instantiate<Enemy>();
    enemy.Position = spawnPosition;
    AddChild(enemy);
  }
}
