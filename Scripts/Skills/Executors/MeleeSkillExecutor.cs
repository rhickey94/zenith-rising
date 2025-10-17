using Godot;
using ZenithRising.Scripts.PlayerScripts;
using ZenithRising.Scripts.Skills.Base;
using ZenithRising.Scripts.Skills.Effects;

namespace ZenithRising.Scripts.Skills.Executors;

public class MeleeSkillExecutor : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill skill)
    {
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
