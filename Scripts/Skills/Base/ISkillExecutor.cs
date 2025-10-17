using ZenithRising.Scripts.PlayerScripts;

namespace ZenithRising.Scripts.Skills.Base;

public interface ISkillExecutor
{
    void ExecuteSkill(Player player, Skill skill);
}
