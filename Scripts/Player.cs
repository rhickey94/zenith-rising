using Godot;
using System;

public partial class Player : CharacterBody2D
{

	[Export]
	public float MaxHealth { get; set; } = 100.0f;

	public float Health { get; private set; } = 100.0f;

	[Export]
	public int Level { get; private set; } = 1;

	[Export]
	public int Experience { get; private set; } = 0;

	[Export]
	public int ExperienceToNextLevel { get; private set; } = 100;

	[Export]
	public float Speed = 300.0f;

	[Export]
	public float FireRate = 0.2f;

	[Export]
	public PackedScene ProjectileScene;

	[Export]
	public Hud _hud;
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

	public override void _Ready()
	{
		AddToGroup("player");

		Health = MaxHealth;

		if (_hud == null)
		{
			GD.PrintErr("Player could not find HUD! Make sure HUD is at /root/Root/HUD");
		}
		else
		{
			UpdateHUD();
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

	private void UpdateHUD()
	{
		if (_hud == null) return;

		_hud.UpdateHealth(Health, MaxHealth);
		_hud.UpdateExperience(Experience, ExperienceToNextLevel, Level);
		_hud.UpdateResources(0, 0, 0, 0);
		_hud.UpdateFloorInfo(1, "Initialization");
		_hud.UpdateWaveInfo(1, 0);
	}

	public void TakeDamage(float damage)
	{
		Health -= damage;
		if (Health < 0) Health = 0;

		UpdateHUD();

		if (Health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		GD.Print("Player died!");
		GetTree().ReloadCurrentScene();
	}
}
