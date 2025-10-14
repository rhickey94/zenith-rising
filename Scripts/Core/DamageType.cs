/// <summary>
/// Defines the type of damage dealt by attacks and skills.
/// Physical damage scales with STR, Magical damage scales with INT.
/// </summary>
public enum DamageType
{
    /// <summary>
    /// Physical damage - scales with Strength (STR).
    /// Used by: Melee weapons, physical projectiles, physical skills.
    /// </summary>
    Physical,
    
    /// <summary>
    /// Magical damage - scales with Intelligence (INT).
    /// Used by: Staves, wands, elemental skills, magical projectiles.
    /// </summary>
    Magical
}
