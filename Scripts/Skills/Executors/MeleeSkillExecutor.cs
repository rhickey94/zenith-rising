using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;
using SpaceTower.Scripts.Skills.Data;
using SpaceTower.Scripts.Skills.Effects;

namespace SpaceTower.Scripts.Skills.Executors;

public class MeleeSkillExecutor : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill baseSkill)
    {
        if (baseSkill is not MeleeAttackSkill skill)
        {
            GD.PrintErr("MeleeSkillExecutor: Skill is not of type MeleeAttackSkill!");
            return;
        }

        if (skill.SkillEffectScene == null)
        {
            GD.PrintErr("MeleeSkillExecutor: SkillEffectScene not set!");
            return;
        }

        // Calculate attack direction toward mouse
        Vector2 direction = (player.GetGlobalMousePosition() - player.GlobalPosition).Normalized();

        // Spawn melee effect (generic - works for any melee type)
        var effect = skill.SkillEffectScene.Instantiate<CollisionSkillEffect>();
        effect.GlobalPosition = player.GlobalPosition;

        // Standardized initialization
        effect.Initialize(skill, player, direction);

        // Add to scene
        player.GetTree().Root.AddChild(effect);

        GD.Print($"{skill.SkillName} executed!");
    }
}
