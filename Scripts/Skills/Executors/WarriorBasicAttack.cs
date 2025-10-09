using Godot;
using SpaceTower.Progression.Upgrades;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Base;
using SpaceTower.Scripts.Skills.Data;
using SpaceTower.Scripts.Skills.Effects;

namespace SpaceTower.Scripts.Skills.Executors.Warrior;

public class WarriorBasicAttack : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill baseSkill)
    {
        // Spawn visual effect
        if (baseSkill is not MeleeAttackSkill skill)
        {
            GD.PrintErr("Basic Attack: Skill is not of type MeleeAttackSkill!");
            return;
        }

        if (skill.SkillEffectScene == null)
        {
            GD.PrintErr("Whirlwind: SkillEffectScene not set! Assign WhirlwindEffect.tscn to the Skill resource.");
            return;
        }

        var meleeAttack = skill.SkillEffectScene.Instantiate<MeleeAttackEffect>();
        meleeAttack.GlobalPosition = player.GlobalPosition;
        // Calculate attack direction toward mouse
        Vector2 attackDirection = (player.GetGlobalMousePosition() - player.GlobalPosition).Normalized();
        var damageBonus = player.GetUpgradeValue(UpgradeType.DamagePercent);
        // Initialize with skill data
        meleeAttack.Initialize(attackDirection, skill.Damage, skill.Lifetime, damageBonus);

        // Add to scene
        player.GetTree().Root.AddChild(meleeAttack);
    }
}
