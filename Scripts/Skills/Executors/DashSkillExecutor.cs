using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;
using SpaceTower.Scripts.Skills.Data;
using SpaceTower.Scripts.Skills.Effects;
using System;

namespace SpaceTower.Scripts.Skills.Executors;

public partial class DashSkillExecutor : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill baseSkill)
    {
        if (baseSkill is not DashSkill skill)
        {
            GD.PrintErr("DashSkillExecutor: Skill is not of type DashSkill!");
            return;
        }

        if (skill.SkillEffectScene == null)
        {
            GD.PrintErr("DashSkillExecutor: SkillEffectScene not set!");
            return;
        }

        // Calculate dash direction toward mouse
        Vector2 direction = (player.GetGlobalMousePosition() - player.GlobalPosition).Normalized();

        // Spawn dash effect
        var effect = skill.SkillEffectScene.Instantiate<SkillEffect>();
        effect.GlobalPosition = player.GlobalPosition;

        effect.Initialize(skill, player, direction);

        // Add to scene
        player.GetTree().Root.AddChild(effect);

        GD.Print($"{skill.SkillName} executed!");
    }


}
