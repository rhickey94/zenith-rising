using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;
using SpaceTower.Scripts.Skills.SkillEffects;
using SpaceTower.Scripts.Skills.SkillTypes;

namespace SpaceTower.Scripts.Skills.Warrior.Offensive;

public class Whirlwind : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill baseSkill)
    {
        // Spawn visual effect
        if (baseSkill is not InstantAOESkill skill)
        {
            GD.PrintErr("Whirlwind: Skill is not of type InstantAOESkill!");
            return;
        }

        if (skill.SkillEffectScene == null)
        {
            GD.PrintErr("Whirlwind: SkillEffectScene not set! Assign WhirlwindEffect.tscn to the Skill resource.");
            return;
        }

        var effect = skill.SkillEffectScene.Instantiate<WhirlwindEffect>();
        effect.GlobalPosition = player.GlobalPosition;
        effect.Initialize(skill.Damage, skill.Radius); // 0.5s lifetime
        player.GetTree().Root.AddChild(effect);
    }
}
