using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	
	public override void _PhysicsProcess(double delta) 
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		Velocity = direction * Speed;
		
		if (direction != Vector2.Zero)
		{
			Rotation = direction.Angle();
		}
		
		MoveAndSlide();
	}
}
