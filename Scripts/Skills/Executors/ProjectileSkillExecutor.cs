using Godot;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Base;
using ZenithRising.Scripts.Skills.Data;
using ZenithRising.Scripts.Skills.Effects;

namespace ZenithRising.Scripts.Skills.Executors;

public class ProjectileSkillExecutor : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill baseSkill)
    {
        if (baseSkill is not ProjectileSkill skill)
        {
            GD.PrintErr("ProjectileSkillExecutor: Skill is not of type ProjectileSkill!");
            return;
        }

        if (skill.SkillEffectScene == null)
        {
            GD.PrintErr("ProjectileSkillExecutor: SkillEffectScene not set!");
            return;
        }

        // Get direction toward mouse
        Vector2 direction = (player.GetGlobalMousePosition() - player.GlobalPosition).Normalized();

        // Spawn projectile effect (generic - works for any projectile type)
        var effect = skill.SkillEffectScene.Instantiate<CollisionSkillEffect>();
        effect.GlobalPosition = player.GlobalPosition;

        // Standardized initialization
        effect.Initialize(skill, player, direction);

        // Add to scene tree
        player.GetTree().Root.AddChild(effect);

        GD.Print($"{skill.SkillName} launched!");
    }
}
