using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.PlayerScripts.Components;

namespace SpaceTower.Scripts.Core;

public static class CombatSystem
{
    public static float CalculateDamage(float baseDamage, Player player)
    {
        var stats = player.GetNode<StatsManager>("StatsManager");
        float damage = baseDamage * stats.DamageMultiplier;
        if (GD.Randf() <= stats.CritChance)
        {
            damage *= 2.0f; // Critical hits deal double damage
        }
        return damage;
    }
}
