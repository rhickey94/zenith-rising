# Skill Mastery System

> **Status:** ✅ CURRENT  
> **Last Updated:** 2025-10-20  
> **Dependencies:** StatsManager.cs, Skill.cs, kill tracking system

## Overview

The skill mastery system provides skill-specific progression through kill tracking. As players use skills to defeat enemies, they unlock tiered bonuses that enhance that specific skill's power.

## Architecture

### Mastery Tiers (4 Total)

| Tier | Kills Required | Cumulative Kills | Color |
|------|----------------|------------------|-------|
| **Bronze** | 10 | 10 | Bronze |
| **Silver** | 40 | 50 | Silver |
| **Gold** | 150 | 200 | Gold |
| **Diamond** | 300 | 500 | Diamond |

**Location:** [Skill.cs:20](c:\Users\rhick\source\games\zenith-rising\Scripts\Skills\Base\Skill.cs#L20)

```csharp
public enum MasteryTier
{
    None,
    Bronze,
    Silver,
    Gold,
    Diamond
}
```

### Kill Tracking

**Stored in:** StatsManager.cs
**Persisted:** Yes (saved with character data)

```csharp
// Dictionary<skillId, killCount>
private Dictionary<string, int> _skillKills = new Dictionary<string, int>();
private Dictionary<string, MasteryTier> _skillMasteryTiers = new Dictionary<string, int>();
```

## How Mastery Works

### 1. Kill Attribution

**When enemy dies:**
```csharp
// Enemy.cs Die() method
if (_lastDamageSource != null)
{
    Player player = GetTree().Root.GetNode<Player>("Player");
    if (player != null)
    {
        player.StatsManager.IncrementSkillKills(_lastDamageSource);
    }
}
```

**_lastDamageSource tracking:**
```csharp
// Enemy.cs TakeDamage()
public virtual void TakeDamage(float damage, Skill sourceSkill, Player attacker)
{
    _lastDamageSource = sourceSkill?.SkillId;
    // ... apply damage
}
```

**Important:** Last hit attribution (final damage source gets the kill)

### 2. Kill Increment & Tier Checking

**StatsManager.IncrementSkillKills():**
```csharp
public void IncrementSkillKills(string skillId)
{
    if (string.IsNullOrEmpty(skillId)) return;

    // Initialize if first kill with this skill
    if (!_skillKills.ContainsKey(skillId))
    {
        _skillKills[skillId] = 0;
        _skillMasteryTiers[skillId] = MasteryTier.None;
    }

    // Increment kill count
    _skillKills[skillId]++;

    // Check for tier-up
    MasteryTier newTier = GetMasteryTierForKills(_skillKills[skillId]);
    if (newTier > _skillMasteryTiers[skillId])
    {
        _skillMasteryTiers[skillId] = newTier;
        EmitSignal(SignalName.SkillMasteryTierUp, skillId, (int)newTier);
        GD.Print($"Skill {skillId} reached {newTier} mastery!");
    }
}
```

### 3. Tier Threshold Checking

**GetMasteryTierForKills():**
```csharp
public MasteryTier GetMasteryTierForKills(int kills)
{
    if (kills >= 500) return MasteryTier.Diamond;
    if (kills >= 200) return MasteryTier.Gold;
    if (kills >= 50) return MasteryTier.Silver;
    if (kills >= 10) return MasteryTier.Bronze;
    return MasteryTier.None;
}
```

**Note:** Thresholds are currently hardcoded (should load from GameBalance config - see Known Issues)

### 4. Bonus Application

**Skill loading from database:**
```csharp
// Skill.cs Initialize()
CurrentMasteryTier = statsManager.GetSkillMasteryTier(SkillId);

// Apply tier bonuses based on current tier
switch (CurrentMasteryTier)
{
    case MasteryTier.Diamond:
        // Apply Diamond bonuses (cumulative with lower tiers)
        ProjectileSpeed *= (1f + entry.DiamondSpeedBonus);
        ProjectilePierceCount += entry.DiamondPierceBonus;
        goto case MasteryTier.Gold; // Fall through

    case MasteryTier.Gold:
        // Apply Gold bonuses
        Damage *= (1f + entry.GoldDamageBonus);
        goto case MasteryTier.Silver;

    case MasteryTier.Silver:
        // Apply Silver bonuses
        Cooldown *= (1f - entry.SilverCooldownReduction);
        goto case MasteryTier.Bronze;

    case MasteryTier.Bronze:
        // Apply Bronze bonuses
        Damage *= (1f + entry.BronzeDamageBonus);
        break;
}
```

**Important:** Bonuses are **cumulative** (Diamond includes Gold + Silver + Bronze bonuses)

## Mastery Bonuses by Skill Type

### Projectile Skills

**Example: Energy Wave projectiles**

**Database (SkillBalanceEntry.ProjectileData):**
```csharp
[Export] public float BronzeDamageBonus { get; set; } = 0.10f;      // +10%
[Export] public float SilverCooldownReduction { get; set; } = 0.10f; // -10%
[Export] public float GoldDamageBonus { get; set; } = 0.15f;        // +15%
[Export] public float DiamondSpeedBonus { get; set; } = 0.50f;      // +50%
[Export] public int DiamondPierceBonus { get; set; } = 2;           // +2 pierce
```

**Progression example (base: 15 damage, 1.0s cooldown, 300 speed):**
- **None:** 15 damage, 1.0s CD, 300 speed, 0 pierce
- **Bronze (10 kills):** 16.5 damage (+10%), 1.0s CD, 300 speed
- **Silver (50 kills):** 16.5 damage, 0.9s CD (-10%), 300 speed
- **Gold (200 kills):** 19 damage (+10% +15%), 0.9s CD, 300 speed
- **Diamond (500 kills):** 19 damage, 0.9s CD, 450 speed (+50%), 2 pierce

### Melee/AOE Skills

**Example: Whirlwind**

**Database (SkillBalanceEntry.AOEData):**
```csharp
[Export] public int BronzeRotationBonus { get; set; } = 1;          // +1 rotation
[Export] public float SilverCooldownReduction { get; set; } = 0.10f; // -10% CD
[Export] public float GoldDamageBonus { get; set; } = 0.20f;        // +20% damage
[Export] public float DiamondRadiusBonus { get; set; } = 0.25f;     // +25% radius
```

**Progression example (base: 50 damage, 10s CD, 150 radius, 3 rotations):**
- **None:** 50 damage, 10s CD, 150 radius, 3 rotations
- **Bronze (10 kills):** 50 damage, 10s CD, 150 radius, 4 rotations (+1)
- **Silver (50 kills):** 50 damage, 9s CD (-10%), 150 radius, 4 rotations
- **Gold (200 kills):** 60 damage (+20%), 9s CD, 150 radius, 4 rotations
- **Diamond (500 kills):** 60 damage, 9s CD, 187.5 radius (+25%), 4 rotations

### Buff Skills

**Example: Combat Stim**

**Database (SkillBalanceEntry.BuffData):**
```csharp
[Export] public float BronzeDurationBonus { get; set; } = 1.0f;     // +1 second
[Export] public float SilverCooldownReduction { get; set; } = 0.15f; // -15% CD
[Export] public float GoldEffectBonus { get; set; } = 0.10f;        // +10% to all bonuses
[Export] public float DiamondDurationBonus { get; set; } = 2.0f;    // +2 seconds
```

**Progression example (base: 5s duration, 15s CD, +40% attack speed):**
- **None:** 5s duration, 15s CD, +40% attack speed
- **Bronze (10 kills):** 6s duration (+1s), 15s CD, +40% attack speed
- **Silver (50 kills):** 6s duration, 12.75s CD (-15%), +40% attack speed
- **Gold (200 kills):** 6s duration, 12.75s CD, +50% attack speed (+10% bonus to +40%)
- **Diamond (500 kills):** 8s duration (+1s +2s), 12.75s CD, +50% attack speed

## Database Integration

### SkillBalanceEntry Structure

**Each skill type has tier-specific bonuses:**

```csharp
public partial class SkillBalanceEntry : Resource
{
    // Core properties
    [Export] public string SkillId { get; set; }

    // Sub-resources with tier bonuses
    [Export] public ProjectileData Projectile { get; set; }
    [Export] public MeleeData Melee { get; set; }
    [Export] public AOEData AOE { get; set; }
    [Export] public BuffData Buff { get; set; }
    [Export] public ExplosionData Explosion { get; set; }
}
```

### Bonus Naming Convention

**Format:** `{Tier}{BonusType}`

**Examples:**
- BronzeDamageBonus (multiplicative)
- SilverCooldownReduction (multiplicative)
- GoldDamageBonus (multiplicative)
- DiamondSpeedBonus (multiplicative)
- DiamondPierceBonus (additive count)
- BronzeRotationBonus (additive count)

**Consistency:** All percentage bonuses use 0.0-1.0 scale (0.10 = 10%)

## UI Feedback

### Current Implementation

**Console logging:**
```csharp
GD.Print($"Skill {skillId} reached {newTier} mastery!");
```

**StatsChanged signal:**
```csharp
EmitSignal(SignalName.StatsChanged);
```

### Future UI Features (Not Implemented)

**Tier-up notification:**
- Visual effect (particle burst, color flash)
- UI panel showing new bonuses unlocked
- Sound effect

**Skill panel:**
- Display current kills / next tier
- Progress bar (e.g., "15 / 50 kills to Silver")
- Show active bonuses for current tier

**Mastery screen:**
- All skills with mastery progress
- Total kills per skill
- Compare skill usage patterns

## Save/Load Integration

**Mastery persists with character save:**

```csharp
// SaveData.cs
public Dictionary<string, int> SkillKills { get; set; }
public Dictionary<string, int> SkillMasteryTiers { get; set; } // int = (int)MasteryTier

// SaveManager.cs SaveGame()
saveData.SkillKills = _player.StatsManager.GetSkillKills();
saveData.SkillMasteryTiers = _player.StatsManager.GetSkillMasteryTiers();

// SaveManager.cs LoadGame()
_player.StatsManager.LoadSkillMastery(saveData.SkillKills, saveData.SkillMasteryTiers);
```

**Important:** Mastery is **character-based**, not run-based (persists across deaths/victories)

## Design Philosophy

### Why Last-Hit Attribution?

**Pros:**
- Simple implementation
- Encourages finishing blows with preferred skills
- Clear feedback (you know which skill got the kill)

**Cons:**
- Doesn't reward setup/support skills
- AOE skills disadvantaged (overkill splits credit)

**Alternative (not implemented):** Damage contribution tracking
- Track % of total damage dealt by each skill
- Award kill credit proportionally
- More complex, but rewards all skill usage

### Why Cumulative Bonuses?

**Diamond = Bronze + Silver + Gold + Diamond bonuses**

**Reasoning:**
- Avoids "dead tiers" (Silver still matters at Diamond)
- Progressive power increase feels rewarding
- Simpler mental model (higher tier = strictly better)

**Trade-off:** Balancing complexity (must ensure Diamond isn't overpowered)

### Tier Progression Pacing

**Current thresholds:**
- **Bronze (10 kills):** Easy to reach (tutorial/early game)
- **Silver (50 kills):** Moderate investment (mid-game)
- **Gold (200 kills):** Significant commitment (late-game)
- **Diamond (500 kills):** Mastery goal (endgame grind)

**Design intent:**
- Bronze: "I've used this skill a bit"
- Silver: "I like this skill, I use it regularly"
- Gold: "This is my main skill"
- Diamond: "I've mastered this skill completely"

## Known Issues

### 1. Hardcoded Mastery Thresholds

**Current:** Thresholds (10/50/200/500) hardcoded in StatsManager.cs

**Should be:** Loaded from GameBalance config

**Fix:**
```csharp
// CharacterProgressionConfig.cs
[Export] public int BronzeMasteryKills { get; set; } = 10;
[Export] public int SilverMasteryKills { get; set; } = 50;
[Export] public int GoldMasteryKills { get; set; } = 200;
[Export] public int DiamondMasteryKills { get; set; } = 500;

// StatsManager.cs
public MasteryTier GetMasteryTierForKills(int kills)
{
    var config = GameBalance.Instance.Config.CharacterProgression;
    if (kills >= config.DiamondMasteryKills) return MasteryTier.Diamond;
    if (kills >= config.GoldMasteryKills) return MasteryTier.Gold;
    if (kills >= config.SilverMasteryKills) return MasteryTier.Silver;
    if (kills >= config.BronzeMasteryKills) return MasteryTier.Bronze;
    return MasteryTier.None;
}
```

### 2. No Mastery Reset/Respec

**Current:** Once earned, mastery is permanent

**Potential issue:** Players can't experiment without "wasting" kills on skills they don't like

**Possible solutions:**
- Allow mastery reset (costs gold or rare item)
- Separate "session kills" from "total kills" (only session counts for tier)
- Add mastery XP (can be reallocated)

### 3. Last-Hit Bias

**Issue:** Skills that finish enemies get all credit (AOE/DOT disadvantaged)

**Example:** Whirlwind damages 5 enemies to 10% HP, Energy Wave kills all 5
- Energy Wave: +5 kills
- Whirlwind: 0 kills (did 90% of work)

**Potential fix:** Damage contribution tracking (complex)

## Testing Checklist

When modifying mastery:
- ✅ Verify kill attribution works for all skill types
- ✅ Test tier-up triggers at correct thresholds
- ✅ Confirm bonuses apply cumulatively
- ✅ Check mastery persists across save/load
- ✅ Test with 0 kills (tier = None, no bonuses)
- ✅ Verify UI shows correct tier/kills
- ✅ Test edge case: 499 kills → 500 kills (Gold → Diamond)

## Related Systems

- **StatsManager:** Tracks kills and mastery tiers
- **SkillBalanceDatabase:** Stores tier bonus values
- **Skill.Initialize():** Applies tier bonuses
- **SaveManager:** Persists mastery progress
- **Enemy.cs:** Attributes kills to skills

## See Also

- [`skill-standardization.md`](skill-standardization.md) - Skill implementation patterns
- [`balance-systems-architecture.md`](balance-systems-architecture.md) - Database structure
- [`stat-system.md`](stat-system.md) - Character stat progression (separate from mastery)
- [`systems-progression.md`](../01-GAME-DESIGN/systems-progression.md) - Design philosophy
