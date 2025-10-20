# Stat System Implementation

## Overview

The stat system provides character progression through 5 core stats (STR, INT, AGI, VIT, FOR) that scale derived combat stats. It's implemented in StatsManager.cs with formulas loaded from the centralized balance config.

## Core Stats (5 Total)

**Location:** [StatsManager.cs:119](c:\Users\rhick\source\games\zenith-rising\Scripts\Player\Components\StatsManager.cs#L119)

| Stat | Name | Primary Effect | Secondary Effects |
|------|------|----------------|-------------------|
| **STR** | Strength | Melee damage | - |
| **INT** | Intelligence | Spell damage, Cast speed | Cooldown reduction |
| **AGI** | Agility | Attack rate | - |
| **VIT** | Vitality | Max health | - |
| **FOR** | Fortitude | Damage reduction | - |

**Starting values:** 0/0/0/0/0 + 15 points to allocate
**Points per character level:** 1 point

## Derived Stats Formulas

All formulas use config values from `GameBalance.Instance.Config.CharacterProgression`

### Attack Damage

**Melee damage (from STR):**
```csharp
public float GetAttackDamageFromStrength()
{
    float baseDamage = Config.CharacterProgression.BaseAttackDamage;
    float strBonus = Strength * Config.CharacterProgression.StrengthDamagePerPoint;
    return baseDamage + strBonus;
}
```

**Config values:**
- BaseAttackDamage = 10f
- StrengthDamagePerPoint = 2f

**Example:** 20 STR = 10 + (20 × 2) = **50 damage**

**Spell damage (from INT):**
```csharp
public float GetAttackDamageFromIntelligence()
{
    float baseDamage = Config.CharacterProgression.BaseAttackDamage;
    float intBonus = Intelligence * Config.CharacterProgression.IntelligenceDamagePerPoint;
    return baseDamage + intBonus;
}
```

**Config values:**
- BaseAttackDamage = 10f
- IntelligenceDamagePerPoint = 2f

**Example:** 20 INT = 10 + (20 × 2) = **50 damage**

### Cast Speed

**From INT:**
```csharp
public float CastSpeed
{
    get
    {
        float baseCastSpeed = Config.PlayerStats.BaseCastSpeed;
        float intBonus = Intelligence * Config.CharacterProgression.IntelligenceCastSpeedPerPoint;
        float buffBonus = BuffManager?.GetTotalCastSpeedBonus() ?? 0f;
        return baseCastSpeed * (1f + intBonus + buffBonus);
    }
}
```

**Config values:**
- BaseCastSpeed = 1.0f
- IntelligenceCastSpeedPerPoint = 0.01f (1% per point)

**Example:** 20 INT = 1.0 × (1 + 0.20) = **1.20x cast speed** (20% faster)

**With buffs:** 20 INT + Combat Stim (+30%) = 1.0 × (1 + 0.20 + 0.30) = **1.50x cast speed**

### Attack Rate

**From AGI:**
```csharp
public float GetAttackRateFromAgility()
{
    float baseRate = Config.PlayerStats.BaseFireRate;
    float agiBonus = Agility * Config.CharacterProgression.AgilityAttackRatePerPoint;
    return baseRate * (1f + agiBonus);
}

public float CurrentAttackRate
{
    get
    {
        float baseRate = GetAttackRateFromAgility();
        float buffBonus = BuffManager?.GetTotalAttackSpeedBonus() ?? 0f;
        return baseRate * (1f + buffBonus);
    }
}
```

**Config values:**
- BaseFireRate = 1.0f (1 attack per second)
- AgilityAttackRatePerPoint = 0.01f (1% per point)

**Example:** 20 AGI = 1.0 × (1 + 0.20) = **1.20 attacks/sec** (20% faster)

**With buffs:** 20 AGI + Combat Stim (+40%) = 1.20 × (1 + 0.40) = **1.68 attacks/sec**

### Max Health

**From VIT:**
```csharp
public float MaxHealth
{
    get
    {
        float baseHealth = Config.PlayerStats.BaseMaxHealth;
        float vitBonus = Vitality * Config.CharacterProgression.VitalityHealthPerPoint;
        float buffBonus = BuffManager?.GetTotalMaxHealthBonus() ?? 0f;
        return baseHealth + vitBonus + buffBonus;
    }
}
```

**Config values:**
- BaseMaxHealth = 100f
- VitalityHealthPerPoint = 10f

**Example:** 20 VIT = 100 + (20 × 10) = **300 health**

### Damage Reduction

**From FOR:**
```csharp
public float DamageReduction
{
    get
    {
        float baseDR = 0f;
        float forBonus = Fortitude * Config.CharacterProgression.FortitudeDamageReductionPerPoint;
        float buffBonus = BuffManager?.GetTotalDamageReductionBonus() ?? 0f;
        return Mathf.Clamp(baseDR + forBonus + buffBonus, 0f, 0.75f); // 75% cap
    }
}
```

**Config values:**
- FortitudeDamageReductionPerPoint = 0.005f (0.5% per point)

**Example:** 20 FOR = 0 + (20 × 0.005) = **0.10 (10% damage reduction)**

**Cap:** 75% maximum (150 FOR would hit cap)

### Cooldown Reduction

**From INT:**
```csharp
public float CooldownReduction
{
    get
    {
        float baseCDR = 0f;
        float intBonus = Intelligence * Config.CharacterProgression.IntelligenceCooldownReductionPerPoint;
        float buffBonus = BuffManager?.GetTotalCooldownReductionBonus() ?? 0f;
        return Mathf.Clamp(baseCDR + intBonus + buffBonus, 0f, 0.50f); // 50% cap
    }
}
```

**Config values:**
- IntelligenceCooldownReductionPerPoint = 0.01f (1% per point)

**Example:** 20 INT = 0 + (20 × 0.01) = **0.20 (20% CDR)**

**Cap:** 50% maximum (50 INT would hit cap)

**Application:**
```csharp
// SkillManager.cs
float effectiveCooldown = skill.Cooldown * (1f - _statsManager.CooldownReduction);
```

**Example:** 10s cooldown with 20% CDR = 10 × (1 - 0.20) = **8s actual cooldown**

## Movement Speed

**Base speed + buff bonuses:**
```csharp
public float CurrentSpeed
{
    get
    {
        float baseSpeed = Config.PlayerStats.BaseSpeed;
        float buffBonus = BuffManager?.GetTotalMoveSpeedBonus() ?? 0f;
        return baseSpeed * (1f + buffBonus);
    }
}
```

**Config values:**
- BaseSpeed = 200f

**Example:** Combat Stim (+100%) = 200 × (1 + 1.00) = **400 speed** (2x faster)

**Note:** No stat scaling for movement speed currently (could add AGI bonus later)

## Stat Allocation

### Character Leveling

**Two separate level systems:**

| System | Source | Persists? | Awards | Used For |
|--------|--------|-----------|--------|----------|
| **Character Level** | Dungeon run completion | ✅ Yes (saved) | +1 stat point | Permanent progression |
| **Power Level** | Enemy kills during run | ❌ No (resets) | Upgrade choices | Per-run power scaling |

**Character XP formula:**
```csharp
// Dungeon.cs OnFloorCompleted()
int baseXP = 50;
int floorBonus = completedFloor * 20;
int bossBonus = 50;
int totalXP = baseXP + floorBonus + bossBonus;
```

**Example:** Complete Floor 3 = 50 + (3 × 20) + 50 = **160 XP**

**Level-up threshold:**
```csharp
public int RequiredXPForNextLevel => CharacterLevel * 100;
```

**Example:** Level 5 → 6 requires **500 XP** (3-4 full runs)

### Stat Allocation UI

**Panel:** StatAllocationPanel.cs (press C in-game)

**Features:**
- Shows current stat values
- Shows unspent stat points
- +/- buttons to allocate points
- Real-time preview of derived stats
- "Apply Changes" commits allocation

**StatsManager.cs allocation:**
```csharp
public void AllocateStatPoint(StatType stat)
{
    if (UnspentStatPoints <= 0) return;

    switch (stat)
    {
        case StatType.Strength:
            Strength++;
            break;
        // ... other stats
    }

    UnspentStatPoints--;
    RecalculateStats();
}
```

## Stat Recalculation

**Triggered when:**
- Buff applied/removed (BuffManager)
- Stat points allocated (StatAllocationPanel)
- Character level up
- Equipment changed (future)

**RecalculateStats() method:**
```csharp
public void RecalculateStats()
{
    // Derived stats recalculated via properties (no caching)
    // Properties read BuffManager bonuses automatically

    // Emit signal for UI updates
    EmitSignal(SignalName.StatsChanged);
}
```

**All derived stats use property getters (not cached fields):**
- Ensures buffs always apply correctly
- No stale data issues
- Small performance cost (acceptable for ~10 stat calculations)

## Integration with Combat

### Damage Calculation

**CombatSystem.CalculateDamage():**
```csharp
public static float CalculateDamage(
    float baseDamage,
    float damageMultiplier,
    bool isCritical,
    float critDamage,
    StatsManager attackerStats)
{
    // 1. Apply stat-based damage scaling
    float finalDamage = baseDamage;

    // Skill determines which stat scales it (STR for melee, INT for spells)
    // Already added in skill damage calculation via GetAttackDamageFromStrength/Intelligence

    // 2. Apply damage multiplier (from upgrades)
    finalDamage *= damageMultiplier;

    // 3. Apply crit multiplier
    if (isCritical)
    {
        finalDamage *= critDamage;
    }

    // 4. Apply buff bonuses
    if (attackerStats?.BuffManager != null)
    {
        float buffBonus = attackerStats.BuffManager.GetTotalDamageBonus();
        finalDamage *= (1f + buffBonus);
    }

    return finalDamage;
}
```

**Example calculation:**
- Skill base: 30 damage
- STR scaling: +50 (from 25 STR)
- Damage multiplier: 1.20 (from upgrades)
- Crit: 1.50x
- Buff: +30% (Combat Stim)

**Final damage:** (30 + 50) × 1.20 × 1.50 × 1.30 = **187.2 damage**

### Damage Reduction Application

**Enemy.cs TakeDamage():**
```csharp
public virtual void TakeDamage(float damage, Player attacker)
{
    float finalDamage = damage;

    // Apply damage reduction (from FOR stat)
    if (attacker?.StatsManager != null)
    {
        float dr = attacker.StatsManager.DamageReduction;
        finalDamage *= (1f - dr);
    }

    _currentHealth -= finalDamage;
}
```

**Example:** Incoming 100 damage, 10% DR = 100 × (1 - 0.10) = **90 damage taken**

## Known Issues & Future Improvements

### Known Issues (Session 14 Code Review)

1. **GetCooldownReduction() method uses wrong formula:**
   ```csharp
   // WRONG (diminishing returns):
   public float GetCooldownReduction()
   {
       return Intelligence / (Intelligence + 100f);
   }

   // CORRECT (linear, property getter):
   public float CooldownReduction { get { ... } }
   ```
   **Impact:** UI (StatAllocationPanel) shows different CDR than actual gameplay
   **Fix:** Remove GetCooldownReduction() method, use CooldownReduction property

2. **Formula inconsistency:**
   - Some stats use additive bonuses (health, damage)
   - Some use multiplicative bonuses (attack rate, cast speed)
   - Should document reasoning or standardize

### Future Improvements

**Stat soft caps:**
- Diminishing returns after certain thresholds
- Prevents extreme stat stacking
- Example: First 50 STR = 2 damage/point, 51-100 STR = 1 damage/point

**Stat respec:**
- Allow reallocating stat points
- Cost: gold or rare item
- Prevents "bad build" frustration

**Secondary stats:**
- Accuracy/evasion system
- Movement speed scaling from AGI
- Health regeneration from VIT

**Attribute synergies:**
- Bonuses for balanced stats
- Penalties for single-stat stacking
- Encourages build diversity

## Testing Checklist

When modifying stats:
- ✅ Check formula in both property getter AND config values
- ✅ Test with 0 points in stat (should use base value)
- ✅ Test with 100 points (check for overflow/clamping)
- ✅ Test with buffs active (additive stacking)
- ✅ Verify UI displays correct values
- ✅ Check stat allocation persists across save/load
- ✅ Ensure RecalculateStats() called when needed

## Related Systems

- **BuffManager:** Applies temporary stat bonuses
- **SkillManager:** Uses attack rate and CDR for timing
- **CombatSystem:** Uses damage, DR in combat calculations
- **CharacterProgressionConfig:** Stores all scaling formulas
- **SaveManager:** Persists character level and allocated stats

## See Also

- [`buff-system.md`](buff-system.md) - Temporary stat bonuses
- [`skill-mastery-system.md`](skill-mastery-system.md) - Skill-based progression
- [`balance-systems-architecture.md`](balance-systems-architecture.md) - Config system
- [`systems-progression.md`](../01-GAME-DESIGN/systems-progression.md) - Design philosophy
