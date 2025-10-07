using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export]
	public float Speed = 600.0f;

	[Export]
	public float Damage = 25.0f;
	
	private Vector2 _direction;
	
	public void Initialize(Vector2 direction)
	{
		_direction = direction.Normalized();
		Rotation = direction.Angle();
	}
	
	public override void _PhysicsProcess(double delta) 
	{
		Position += _direction * Speed * (float)delta;
	}

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		BodyEntered += OnBodyEntered;

		GetTree().CreateTimer(3.0).Timeout += QueueFree;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Enemy enemy)
		{
			enemy.TakeDamage(Damage);
			QueueFree();
		}
	}
	
	private void OnAreaEntered(Area2D area)
	{
		if (area.GetParent() is Enemy enemy)
		{
			enemy.TakeDamage(Damage);
			QueueFree();
		}
	}
}
