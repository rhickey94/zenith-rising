using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;
using SpaceTower.Scripts.Skills.SkillTypes;
using SpaceTower.Scripts.Skills.SkillEffects;

namespace SpaceTower.Scripts.Skills.Mage.Offensive;

public class Fireball : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill baseSkill)
    {
        if (baseSkill is not ProjectileSkill skill)
        {
            GD.PrintErr("Fireball: Skill is not of type ProjectileSkill!");
            return;
        }

        if (skill.SkillEffectScene == null)
        {
            GD.PrintErr("Fireball: SkillEffectScene not set! Assign FireballProjectile.tscn to the Skill resource.");
            return;
        }

        // Get direction toward mouse
        Vector2 direction = (player.GetGlobalMousePosition() - player.GlobalPosition).Normalized();

        // Spawn fireball projectile
        var fireball = skill.SkillEffectScene.Instantiate<FireballProjectile>();
        fireball.GlobalPosition = player.GlobalPosition;
        fireball.Initialize(direction, player.GlobalPosition, skill.DirectDamage, skill.ExplosionDamage, skill.ExplosionRadius);

        // Add to scene tree
        player.GetTree().Root.AddChild(fireball);

        GD.Print("Fireball launched!");
    }
}
