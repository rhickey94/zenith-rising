using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
  [Export]
  public float Speed = 200.0f;

  [Export]
  public float Health = 100.0f;

  private Node2D _player;

  public override void _Ready()
  {
    // Get player from group
    var players = GetTree().GetNodesInGroup("player");
    if (players.Count > 0)
    {
      _player = players[0] as Node2D;
    }
    else
    {
      GD.PrintErr("Enemy could not find Player! Make sure Player is in 'player' group.");
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    if (_player == null) return;

    Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
    Velocity = direction * Speed;
    // Rotation = direction.Angle();
    MoveAndSlide();
  }

  public void TakeDamage(float damage)
  {
    Health -= damage;

    if (Health <= 0)
    {
      QueueFree(); // Destroy enemy immediately
      return; // IMPORTANT: Exit before trying to flash
    }

    // Only flash if still alive
    var sprite = GetNode<Sprite2D>("Sprite2D");
    sprite.Modulate = Colors.White;
    GetTree().CreateTimer(0.1).Timeout += () =>
    {
      // Check if sprite still exists before trying to change color
      if (IsInstanceValid(sprite))
      {
        sprite.Modulate = Colors.Red;
      }
    };
  }
}