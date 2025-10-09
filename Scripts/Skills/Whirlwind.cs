using Godot;
using SpaceTower.Scripts.EnemyScripts;
using SpaceTower.Scripts.PlayerScripts;

namespace SpaceTower.Scripts.Skills;

public class Whirlwind : ISkillExecutor
{
		public void ExecuteSkill(Player player, Skill skill)
		{
				// Spawn visual effect

				if (skill.SkillEffectScene != null)
				{
						var effect = skill.SkillEffectScene.Instantiate<Node2D>();
						effect.GlobalPosition = player.GlobalPosition;
						player.GetTree().Root.AddChild(effect);
				}

				// For now, just damage all enemies in range

				var enemies = player.GetTree().GetNodesInGroup("enemies");
				int hitCount = 0;

				foreach (Node node in enemies)
				{
						if (node is Enemy enemy)
						{
								float distance = player.GlobalPosition.DistanceTo(enemy.GlobalPosition);
								if (distance < skill.Range) // 150 pixel range

								{
										enemy.TakeDamage(skill.EffectValue);
										hitCount++;
								}
						}
				}

				GD.Print($"{skill.SkillName} hit {hitCount} enemies!");
		}
}
