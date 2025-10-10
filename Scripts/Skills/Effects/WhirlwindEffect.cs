using Godot;
using SpaceTower.Scripts.Enemies.Base;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;
using SpaceTower.Scripts.Skills.Data;

namespace SpaceTower.Scripts.Skills.Effects;

public partial class WhirlwindEffect : SkillEffect
{
	[Export] public float Duration = 0.5f;  // Visual animation duration
	[Export] public float RotationSpeed = 10f;  // Rotation speed (radians/sec)

	private float _damage;
	private float _radius;

	public override void Initialize(Skill sourceSkill, Player caster, Vector2 direction)
	{
		base.Initialize(sourceSkill, caster, direction);

		var skill = sourceSkill as InstantAOESkill;
		if (skill == null)
		{
			GD.PrintErr("WhirlwindEffect: sourceSkill is not InstantAOESkill!");
			return;
		}

		_damage = skill.Damage;
		_radius = skill.Radius;

		// Apply mastery bonuses
		ApplyMasteryBonuses();
	}

	private void ApplyMasteryBonuses()
	{
		switch (_sourceSkill.CurrentTier)
		{
			case SkillMasteryTier.Silver:
				_damage *= 1.5f;
				Duration += 1.0f; // Lasts longer
				break;
			case SkillMasteryTier.Gold:
				_damage *= 2.0f;
				_radius *= 1.3f; // Larger radius
				Duration += 2.0f;
				break;
			case SkillMasteryTier.Diamond:
				_damage *= 3.0f;
				_radius *= 1.5f;
				Duration += 3.0f;
				RotationSpeed *= 1.5f; // Spins faster
				break;
		}
	}

	public override void _Ready()
	{
		// Apply damage immediately on spawn
		ApplyDamage();

		// Destroy after visual animation duration
		GetTree().CreateTimer(Duration).Timeout += QueueFree;
	}

	public override void _Process(double delta)
	{
		// Rotate for visual effect
		Rotation += RotationSpeed * (float)delta;
	}

	private void ApplyDamage()
	{
		var enemies = GetTree().GetNodesInGroup("enemies");
		int hitCount = 0;

		foreach (Node node in enemies)
		{
			if (node is Enemy enemy)
			{
				float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
				if (distance < _radius)
				{
					float healthBefore = enemy.Health;
					enemy.TakeDamage(_damage);
					hitCount++;

					// Track kill if enemy died
					if (healthBefore > 0 && enemy.Health <= 0)
					{
						OnEnemyKilled(enemy);
					}
				}
			}
		}

		GD.Print($"Whirlwind hit {hitCount} enemies!");
	}
}
