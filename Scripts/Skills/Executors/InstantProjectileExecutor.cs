using Godot;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Base;
using ZenithRising.Scripts.Skills.Entities;

namespace ZenithRising.Scripts.Skills.Executors;

public class InstantProjectileExecutor : ISkillExecutor
{
    public bool ExecuteSkill(Player player, Skill skill)
    {
        if (skill.SkillEffectScene == null)
        {
            GD.PrintErr("ProjectileSkillExecutor: SkillEffectScene not set!");
            return false;
        }

        // Get direction toward mouse
        Vector2 direction = (player.GetGlobalMousePosition() - player.GlobalPosition).Normalized();

        // Spawn projectile effect (generic - works for any projectile type)
        var effect = skill.SkillEffectScene.Instantiate<DamageEntityBase>();
        effect.GlobalPosition = player.GlobalPosition;

        // Standardized initialization
        effect.Initialize(skill, player, direction);

        // Add to scene tree
        player.GetTree().Root.AddChild(effect);

        GD.Print($"{skill.SkillName} launched!");
        return true;
    }
}
