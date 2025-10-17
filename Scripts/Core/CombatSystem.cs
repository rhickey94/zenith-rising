using Godot;
using ZenithRising.Scripts.PlayerScripts.Components;

namespace ZenithRising.Scripts.Core;

public enum DamageType
{
    Physical,
    Magical
}

public static class CombatSystem
{
    public static float CalculateDamage(
      float baseDamage,
      StatsManager attackerStats,
      DamageType damageType,  // NEW parameter
      bool forceCrit = false)
    {
        if (attackerStats == null)
        {
            GD.PrintErr("CombatSystem.CalculateDamage: attackerStats is null!");
            return baseDamage;
        }

        // Apply damage type multiplier (Physical = STR, Magical = INT)
        float damageTypeMultiplier = damageType == DamageType.Physical
            ? attackerStats.PhysicalDamageMultiplier
            : attackerStats.MagicalDamageMultiplier;

        // Apply general damage multiplier (from upgrades)
        float damage = baseDamage * attackerStats.DamageMultiplier * damageTypeMultiplier;

        // Crit calculation (now uses Fortune stat for crit damage)
        bool isCrit = forceCrit || (GD.Randf() < attackerStats.CritChance);
        if (isCrit)
        {
            damage *= attackerStats.CritDamageMultiplier;
            GD.Print($"CRIT! Damage: {damage:F1} (Multiplier: {attackerStats.CritDamageMultiplier:F2}x)");
        }

        return damage;
    }

    // Helper methods for convenience
    public static float CalculatePhysicalDamage(float baseDamage, StatsManager attackerStats, bool forceCrit = false)
    {
        return CalculateDamage(baseDamage, attackerStats, DamageType.Physical, forceCrit);
    }

    public static float CalculateMagicalDamage(float baseDamage, StatsManager attackerStats, bool forceCrit = false)
    {
        return CalculateDamage(baseDamage, attackerStats, DamageType.Magical, forceCrit);
    }
}
