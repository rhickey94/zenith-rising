using Godot;
using SpaceTower.Scripts.Effects;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;

namespace SpaceTower.Scripts.Skills.Mage.Offensive;

public class Fireball : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill skill)
    {
        if (skill.SkillEffectScene == null)
        {
            GD.PrintErr("Fireball: SkillEffectScene not set! Assign FireballProjectile.tscn to the Skill resource.");
            return;
        }

        // Get direction toward mouse
        Vector2 mousePosition = player.GetGlobalMousePosition();
        Vector2 direction = (mousePosition - player.GlobalPosition).Normalized();

        // Spawn fireball projectile
        var fireball = skill.SkillEffectScene.Instantiate<FireballProjectile>();
        fireball.GlobalPosition = player.GlobalPosition;
        fireball.Initialize(direction, player.GlobalPosition);

        // Add to scene tree
        player.GetTree().Root.AddChild(fireball);

        GD.Print("Fireball launched!");
    }
}
