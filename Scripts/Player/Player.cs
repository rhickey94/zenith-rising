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

	// Skills
	[Export] public Skill PrimarySkill;
	[Export] public Skill SecondarySkill;
	[Export] public Skill UltimateSkill;
	private float _primarySkillCooldownTimer = 0.0f;
	private float _secondarySkillCooldownTimer = 0.0f;
	private float _ultimateSkillCooldownTimer = 0.0f;

	public float Health { get; private set; } = 100.0f;

	// Progression
	[Export] public int Level { get; private set; } = 1;
	[Export] public int Experience { get; private set; } = 0;
	[Export] public int ExperienceToNextLevel { get; private set; } = 100;

	// Scenes
	[Export] public PackedScene ProjectileScene;

	// Dependencies
	[Export] public LevelUpPanel LevelUpPanel;

	// Signals for HUD updates
	[Signal] public delegate void HealthChangedEventHandler(float currentHealth, float maxHealth);
	[Signal] public delegate void ExperienceChangedEventHandler(int currentXP, int requiredXP, int level);
	[Signal] public delegate void ResourcesChangedEventHandler(int gold, int cores, int components, int fragments);
	[Signal] public delegate void FloorInfoChangedEventHandler(int floorNumber, string floorName);
	[Signal] public delegate void WaveInfoChangedEventHandler(int waveNumber, int enemiesRemaining);

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
		// Emit initial state
		EmitHealthUpdate();
		EmitExperienceUpdate();
		EmitResourcesUpdate(0, 0, 0, 0);
		EmitFloorInfoUpdate(1, "Initialization");
		EmitWaveInfoUpdate(1, 0);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed)
		{
			if (eventKey.Keycode == Key.Q && PrimarySkill != null)
			{
				UseSkill(PrimarySkill, ref _primarySkillCooldownTimer);
			}
			else if (eventKey.Keycode == Key.E && SecondarySkill != null)
			{
				UseSkill(SecondarySkill, ref _secondarySkillCooldownTimer);
			}
			else if (eventKey.Keycode == Key.R && UltimateSkill != null)
			{
				UseSkill(UltimateSkill, ref _ultimateSkillCooldownTimer);
			}
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = direction * Speed;

		if (direction != Vector2.Zero)
			Rotation = direction.Angle();

		if (_primarySkillCooldownTimer > 0)
			_primarySkillCooldownTimer -= (float)delta;
		if (_secondarySkillCooldownTimer > 0)
			_secondarySkillCooldownTimer -= (float)delta;
		if (_ultimateSkillCooldownTimer > 0)
			_ultimateSkillCooldownTimer -= (float)delta;

		MoveAndSlide();

		_timeSinceLastShot += (float)delta;

		if (Input.IsActionPressed("ui_left_click") && _timeSinceLastShot > FireRate)
		{
			Shoot();
			_timeSinceLastShot = 0f;
		}
	}

	private void Attack() { }

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

	private void UseSkill(Skill skill, ref float cooldownRemaining)
	{
		if (skill == null)
		{
			GD.Print("No skill equipped!");
			return;
		}

		if (cooldownRemaining > 0)
		{
			GD.Print($"{skill.SkillName} on cooldown: {cooldownRemaining:F1}s remaining");
			return;
		}

		GD.Print($"Using {skill.SkillName}!");

		// Execute skill effect
		ExecuteSkillEffect(skill);

		// Start cooldown
		cooldownRemaining = skill.Cooldown;
	}

	private void ExecuteSkillEffect(Skill skill)
	{
		skill.Execute(this);
	}

	public void TakeDamage(float damage)
	{
		Health -= damage;
		if (Health < 0) Health = 0;

		EmitHealthUpdate();

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

		EmitExperienceUpdate();
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

		EmitHealthUpdate();
		EmitExperienceUpdate();
	}

	private List<Upgrade> GetRandomUpgrades(int count)
	{
		var shuffled = _availableUpgrades.OrderBy(x => GD.Randi()).ToList();
		return [.. shuffled.Take(count)];
	}

	private void EmitHealthUpdate()
	{
		EmitSignal(SignalName.HealthChanged, Health, MaxHealth);
	}

	private void EmitExperienceUpdate()
	{
		EmitSignal(SignalName.ExperienceChanged, Experience, ExperienceToNextLevel, Level);
	}

	private void EmitResourcesUpdate(int gold, int cores, int components, int fragments)
	{
		EmitSignal(SignalName.ResourcesChanged, gold, cores, components, fragments);
	}

	private void EmitFloorInfoUpdate(int floorNumber, string floorName)
	{
		EmitSignal(SignalName.FloorInfoChanged, floorNumber, floorName);
	}

	private void EmitWaveInfoUpdate(int waveNumber, int enemiesRemaining)
	{
		EmitSignal(SignalName.WaveInfoChanged, waveNumber, enemiesRemaining);
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
