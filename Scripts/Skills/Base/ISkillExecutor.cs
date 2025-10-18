using ZenithRising.Scripts.PlayerScripts;

namespace ZenithRising.Scripts.Skills.Base;

public interface ISkillExecutor
{
    bool ExecuteSkill(Player player, Skill skill);
}
