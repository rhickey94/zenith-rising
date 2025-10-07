using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	
	[Export]
	public float FireRate = 0.2f;
	
	[Export]
	public PackedScene ProjectileScene;
	
	private float _timeSinceLastShot = 0f;
	
	public override void _PhysicsProcess(double delta) 
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = direction * Speed;
		if (direction != Vector2.Zero)
		{
			Rotation = direction.Angle();
		}
		MoveAndSlide();
		
		_timeSinceLastShot += (float)delta;
		
		if (Input.IsActionPressed("ui_left_click") && _timeSinceLastShot > FireRate) 
		{
			Shoot();
			_timeSinceLastShot = 0f;
		}
	}
	
	private void Shoot() 
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 shootDirection = (mousePosition - GlobalPosition).Normalized();
		
		var projectile = ProjectileScene.Instantiate<Projectile>();
		projectile.GlobalPosition = GlobalPosition;
		projectile.Initialize(shootDirection);
		
		GetTree().Root.AddChild(projectile);
		
	}
}
