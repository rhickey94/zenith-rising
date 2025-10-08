using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export] public float Speed = 600.0f;
	[Export] public float Damage = 25.0f;
	[Export] public float Lifetime = 3.0f;

	private Vector2 _direction;
	
	public void Initialize(Vector2 direction)
	{
		_direction = direction.Normalized();
		Rotation = direction.Angle();
	}

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;

		GetTree().CreateTimer(Lifetime).Timeout += QueueFree;
	}
	
	public override void _PhysicsProcess(double delta) 
	{
		Position += _direction * Speed * (float)delta;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Enemy enemy)
		{
			enemy.TakeDamage(Damage);
			QueueFree();
		}
	}
}
