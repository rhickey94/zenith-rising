using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;
using SpaceTower.Scripts.Skills.Data;
using SpaceTower.Scripts.Skills.Effects;

namespace SpaceTower.Scripts.Skills.Executors;

public class InstantAOESkillExecutor : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill baseSkill)
    {
        if (baseSkill is not InstantAOESkill skill)
        {
            GD.PrintErr("InstantAOESkillExecutor: Skill is not of type InstantAOESkill!");
            return;
        }

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
