using SpaceTower.Scripts.PlayerScripts;

namespace SpaceTower.Scripts.Skills;

public interface ISkillExecutor
{
  void ExecuteSkill(Player player, Skill skill);
}
