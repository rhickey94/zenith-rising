using Godot;
using System;

namespace SpaceTower.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class StatsManager : Node
{
		[Export] public float MaxHealth { get; set; } = 100.0f;
		public float Health { get; private set; } = 100.0f;

		// Progression

		[Export] public int Level { get; private set; } = 1;
		[Export] public int Experience { get; private set; } = 0;
		[Export] public int ExperienceToNextLevel { get; private set; } = 100;

		// Event for level up (Player will listen)

		public event Action LeveledUp;

		private Player _player;

		public override void _Ready()
		{
				_player = GetParent<Player>();
				if (_player == null)
				{
						GD.PrintErr("StatsManager: Could not find Player parent!");
				}
				Health = MaxHealth;
		}

		public void Initialize()
		{
				EmitHealthUpdate();
				EmitExperienceUpdate();
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

		public void IncreaseMaxHealth(float amount)
		{
				MaxHealth += amount;
				Health += amount; // Heal player by the same amount

				EmitHealthUpdate();
		}

		private void LevelUp()
		{
				Experience -= ExperienceToNextLevel;
				Level++;
				ExperienceToNextLevel = (int)(ExperienceToNextLevel * 1.5);

				MaxHealth += 20;
				Health = MaxHealth;
				_player.Speed += 10;

				GD.Print($"Leveled up to {Level}!");

				EmitHealthUpdate();
				EmitExperienceUpdate();

				// Notify Player that level up occurred

				LeveledUp?.Invoke();
		}

		private void Die()
		{
				GD.Print("Player died!");
				GetTree().ReloadCurrentScene();
		}

		private void EmitHealthUpdate()
		{
				_player.EmitSignal(Player.SignalName.HealthChanged, Health, MaxHealth);
		}

		private void EmitExperienceUpdate()
		{
				_player.EmitSignal(Player.SignalName.ExperienceChanged, Experience, ExperienceToNextLevel, Level);
		}
}
