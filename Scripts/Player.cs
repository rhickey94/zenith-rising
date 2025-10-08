using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
	[Export] public LevelUpPanel LevelUpPanel;

	private Dictionary<UpgradeType, float> _activeUpgrades = [];
	// Available upgrades pool
	private List<Upgrade> _availableUpgrades =
	[
			new Upgrade { UpgradeName = "Damage Boost", Description = "+15% Damage", Type = UpgradeType.DamagePercent, Value = 0.15f },
				new Upgrade { UpgradeName = "Attack Speed", Description = "+20% Fire Rate", Type = UpgradeType.AttackSpeed, Value = 0.20f },
				new Upgrade { UpgradeName = "Swift Feet", Description = "+15% Movement Speed", Type = UpgradeType.MovementSpeed, Value = 0.15f },
				new Upgrade { UpgradeName = "Vitality", Description = "+50 Max Health", Type = UpgradeType.MaxHealth, Value = 50f },
				new Upgrade { UpgradeName = "Magnet", Description = "+30 Pickup Radius", Type = UpgradeType.PickupRadius, Value = 30f },
				new Upgrade { UpgradeName = "Piercing Shots", Description = "Projectiles Pierce +1", Type = UpgradeType.ProjectilePierce, Value = 1f },
				new Upgrade { UpgradeName = "Critical Hit", Description = "+10% Crit Chance", Type = UpgradeType.CritChance, Value = 0.10f },
				new Upgrade { UpgradeName = "Regeneration", Description = "+2 HP/sec", Type = UpgradeType.HealthRegen, Value = 2f }
	];
	private float _timeSinceLastShot = 0f;

	public override void _Ready()
	{
		AddToGroup("player");

		Health = MaxHealth;

		if (ProjectileScene == null)
		{
			GD.PrintErr("Player: ProjectileScene not assigned!");
		}

		if (LevelUpPanel != null)
		{
			LevelUpPanel.UpgradeSelected += OnUpgradeSelected;
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

		var damageBonus = GetUpgradeValue(UpgradeType.DamagePercent);
		var pierceCount = (int)GetUpgradeValue(UpgradeType.ProjectilePierce);
		projectile.Initialize(shootDirection, damageBonus, pierceCount);

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

		if (LevelUpPanel != null)
		{
			var upgradeOptions = GetRandomUpgrades(3);
			LevelUpPanel.ShowUpgrades(upgradeOptions);
		}

		UpdateHUD();
	}

	private List<Upgrade> GetRandomUpgrades(int count)
	{
		var shuffled = _availableUpgrades.OrderBy(x => GD.Randi()).ToList();
		return [.. shuffled.Take(count)];
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

	private void OnUpgradeSelected(Upgrade upgrade)
	{
		// Track upgrade
		if (_activeUpgrades.ContainsKey(upgrade.Type))
		{
			_activeUpgrades[upgrade.Type] += upgrade.Value;
		}
		else
		{
			_activeUpgrades[upgrade.Type] = upgrade.Value;
		}

		// Apply upgrade immediately
		ApplyUpgrade(upgrade.Type);

		GD.Print($"Selected: {upgrade.UpgradeName}");
		GD.Print($"New Value: {_activeUpgrades[upgrade.Type]}");
	}


	private void ApplyUpgrade(UpgradeType upgradeType)
	{
		switch (upgradeType)
		{
			case UpgradeType.DamagePercent:
				// Apply damage percent upgrade logic
				break;
			case UpgradeType.AttackSpeed:
				FireRate *= 1 - _activeUpgrades[upgradeType];
				break;
			case UpgradeType.MovementSpeed:
				Speed *= 1 + _activeUpgrades[upgradeType];
				break;
			case UpgradeType.MaxHealth:
				MaxHealth += _activeUpgrades[upgradeType];
				Health = MaxHealth; // Restore health on upgrade
				break;
			case UpgradeType.PickupRadius:
				// Apply pickup radius upgrade logic
				break;
			case UpgradeType.ProjectilePierce:
				// Apply projectile pierce upgrade logic
				break;
			case UpgradeType.CritChance:
				// Apply crit chance upgrade logic
				break;
			case UpgradeType.HealthRegen:
				// Apply health regen upgrade logic
				break;
		}
	}

	public float GetUpgradeValue(UpgradeType type)
	{
		return _activeUpgrades.GetValueOrDefault(type, 0f);
	}
}
