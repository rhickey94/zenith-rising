using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.PlayerScripts.Components;

namespace SpaceTower.Scripts.Core;

public static class CombatSystem
{
    public static float CalculateDamage(float baseDamage, Player player)
    {
        if (player == null)
        {
            GD.PrintErr("Game: Player not assigned!");
        }

        var stats = player.GetNode<StatsManager>("StatsManager");

        if (stats == null)
        {
            GD.PrintErr("CombatSystem: Player StatsManager not found!");
            return baseDamage; // Fallback to base damage
        }

        float damage = baseDamage * stats.DamageMultiplier;
        if (GD.Randf() <= stats.CritChance)
        {
            damage *= 2.0f; // Critical hits deal double damage
        }
        return damage;
    }
}
