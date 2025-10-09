using SpaceTower.Scripts.PlayerScripts;

namespace SpaceTower.Scripts.Skills.Base;

public interface ISkillExecutor
{
    void ExecuteSkill(Player player, Skill skill);
}
