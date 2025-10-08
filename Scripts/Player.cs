using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// Stats
	[Export] public float MaxHealth { get; set; } = 100.0f;
	[Export] public float Speed = 300.0f;
	[Export] public float FireRate = 0.2f;

	public float Health { get; private set; } = 100.0f;

	// Progression
	[Export] public int Level { get; private set; } = 1;
	[Export] public int Experience { get; private set; } = 0;
	[Export] public int ExperienceToNextLevel { get; private set; } = 100;

	// Scenes
	[Export] public PackedScene ProjectileScene;

	// Dependencies
	[Export] public Hud HUD;

	private float _timeSinceLastShot = 0f;

	public override void _Ready()
	{
		AddToGroup("player");

		Health = MaxHealth;

		if (ProjectileScene == null)
		{
			GD.PrintErr("Player: ProjectileScene not assigned!");
		}
	}

	public void Initialize()
	{
		if (HUD == null)
		{
			GD.PrintErr("Player could not find HUD! Make sure HUD is at /root/Root/HUD");
			return;
		}

		UpdateHUD();
	}

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
		if (ProjectileScene == null) return;

		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 shootDirection = (mousePosition - GlobalPosition).Normalized();

		var projectile = ProjectileScene.Instantiate<Projectile>();
		projectile.GlobalPosition = GlobalPosition;
		projectile.Initialize(shootDirection);

		GetTree().Root.AddChild(projectile);
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

	public void AddExperience(int amount)
	{
		Experience += amount;

		while (Experience >= ExperienceToNextLevel)
		{
			LevelUp();
		}

		UpdateHUD();
	}
	private void LevelUp()
	{
		Experience -= ExperienceToNextLevel;
		Level++;
		ExperienceToNextLevel = (int)(ExperienceToNextLevel * 1.5);

		MaxHealth += 20;
		Health = MaxHealth;
		Speed += 10;

		GD.Print($"Leveled up to {Level}!");
	}

	private void UpdateHUD()
	{
		if (HUD == null) return;

		HUD.UpdateHealth(Health, MaxHealth);
		HUD.UpdateExperience(Experience, ExperienceToNextLevel, Level);
		HUD.UpdateResources(0, 0, 0, 0);
		HUD.UpdateFloorInfo(1, "Initialization");
		HUD.UpdateWaveInfo(1, 0);
	}


	private void Die()
	{
		GD.Print("Player died!");
		GetTree().ReloadCurrentScene();
	}
}
