using Godot;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Base;
using ZenithRising.Scripts.Skills.Effects;

namespace ZenithRising.Scripts.Skills.Executors;

public class InstantAOESkillExecutor : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill skill)
    {
        if (skill.SkillEffectScene == null)
        {
            GD.PrintErr("InstantAOESkillExecutor: SkillEffectScene not set!");
            return;
        }

        // Spawn AOE effect (generic - works for any instant AOE type)
        var effect = skill.SkillEffectScene.Instantiate<SkillEffect>();
        effect.GlobalPosition = player.GlobalPosition;

        // Standardized initialization (direction not needed for AOE)
        effect.Initialize(skill, player, Vector2.Zero);

        player.GetTree().Root.AddChild(effect);

        GD.Print($"{skill.SkillName} activated!");
    }
}
