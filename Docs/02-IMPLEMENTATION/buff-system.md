# Buff System Implementation

## Overview

The buff system provides temporary stat bonuses to the player through skills, items, or other game mechanics. It's implemented as a component architecture with centralized state management and automatic stat recalculation.

## Architecture

### Component Structure

```
Player.cs
└── BuffManager.cs (component)
    ├── Dictionary<string, ActiveBuff> _activeBuffs
    └── StatsManager reference (for recalculation)
```

**Key Design Decisions:**
- Buffs are tracked by unique string IDs (prevents duplicate buffs)
- Duration-based expiration handled in `_Process()`
- Stat recalculation triggered only when buffs change (not every frame)
- Buff bonuses stack additively (multiple buffs = sum of all bonuses)

### BuffManager.cs

**Location:** `Scripts/Player/Components/BuffManager.cs`

**Responsibilities:**
- Track active buffs with remaining duration
- Expire buffs when duration ends
- Apply/remove buff bonuses to StatsManager
- Provide buff state queries (IsActive, GetRemainingDuration)

**Key Methods:**
```csharp
public void ApplyBuff(string buffId, float duration, BuffBonuses bonuses)
public void RemoveBuff(string buffId)
public bool IsBuffActive(string buffId)
public float GetRemainingDuration(string buffId)
```

### ActiveBuff Data Structure

```csharp
private class ActiveBuff
{
    public string Id { get; set; }
    public float RemainingDuration { get; set; }
    public BuffBonuses Bonuses { get; set; }
}
```

### BuffBonuses Data Structure

```csharp
public class BuffBonuses
{
    public float AttackSpeedBonus { get; set; } = 0f;
    public float MoveSpeedBonus { get; set; } = 0f;
    public float CastSpeedBonus { get; set; } = 0f;
    public float DamageBonus { get; set; } = 0f;
    public float CooldownReductionBonus { get; set; } = 0f;
    public float DamageReductionBonus { get; set; } = 0f;
}
```

**All bonuses are percentages (e.g., 0.40 = +40% bonus)**

## How Buffs Work

### 1. Applying a Buff

**From a skill (Player.TryInstantSkill):**
```csharp
if (skill.BuffDuration > 0)
{
    var bonuses = new BuffBonuses
    {
        AttackSpeedBonus = skill.AttackSpeedBonus,
        MoveSpeedBonus = skill.MoveSpeedBonus,
        CastSpeedBonus = skill.CastSpeedBonus,
        DamageBonus = skill.DamageBonus,
        CooldownReductionBonus = skill.CooldownReductionBonus,
        DamageReductionBonus = skill.DamageReductionBonus
    };

    BuffManager.ApplyBuff(skill.SkillId, skill.BuffDuration, bonuses);
}
```

**Behavior:**
- If buff with same ID already active → duration refreshes, bonuses replaced
- Triggers `RecalculateStats()` on StatsManager

### 2. Buff Duration Tracking

**BuffManager._Process():**
```csharp
public override void _Process(double delta)
{
    // Track buffs to expire (can't modify dictionary during iteration)
    List<string> expiredBuffs = new List<string>();

    foreach (var buff in _activeBuffs.Values)
    {
        buff.RemainingDuration -= (float)delta;
        if (buff.RemainingDuration <= 0f)
        {
            expiredBuffs.Add(buff.Id);
        }
    }

    // Remove expired buffs
    foreach (var buffId in expiredBuffs)
    {
        RemoveBuff(buffId);
    }
}
```

### 3. Stat Recalculation

**When buffs change (apply or remove):**
```csharp
private void RecalculateStats()
{
    if (_statsManager != null)
    {
        _statsManager.RecalculateStats();
    }
}
```

**StatsManager applies buff bonuses:**
```csharp
// Example: Attack speed calculation
public float CurrentAttackRate
{
    get
    {
        float baseRate = GetAttackRateFromAgility();
        float buffBonus = BuffManager.GetTotalAttackSpeedBonus();
        return baseRate * (1f + buffBonus);
    }
}
```

### 4. Buff Stacking

**Multiple buffs stack additively:**
```csharp
public float GetTotalAttackSpeedBonus()
{
    float total = 0f;
    foreach (var buff in _activeBuffs.Values)
    {
        total += buff.Bonuses.AttackSpeedBonus;
    }
    return total;
}
```

**Example:**
- Buff A: +40% attack speed
- Buff B: +20% attack speed
- **Total:** +60% attack speed

**Same buff ID refreshes duration (doesn't stack with itself):**
- Combat Stim active (5s remaining, +40% attack speed)
- Cast Combat Stim again
- **Result:** 5s remaining, +40% attack speed (duration refreshed, not stacked)

## Integration with Skill System

### BuffData Sub-Resource

**Location:** `Scripts/Skills/Balance/Data/BuffData.cs`

```csharp
[GlobalClass]
public partial class BuffData : Resource
{
    [Export] public float Duration { get; set; } = 5f;
    [Export] public float AttackSpeedBonus { get; set; } = 0f;
    [Export] public float MoveSpeedBonus { get; set; } = 0f;
    [Export] public float CastSpeedBonus { get; set; } = 0f;
    [Export] public float DamageBonus { get; set; } = 0f;
    [Export] public float CooldownReductionBonus { get; set; } = 0f;
    [Export] public float DamageReductionBonus { get; set; } = 0f;
}
```

### SkillBalanceEntry Integration

```csharp
public partial class SkillBalanceEntry : Resource
{
    // ... other properties

    [Export] public BuffData Buff { get; set; }
}
```

### Skill.Initialize() Loading

```csharp
private void LoadBuffData(SkillBalanceEntry entry)
{
    if (entry.Buff != null)
    {
        BuffDuration = entry.Buff.Duration;
        AttackSpeedBonus = entry.Buff.AttackSpeedBonus;
        MoveSpeedBonus = entry.Buff.MoveSpeedBonus;
        // ... load all buff properties
    }
}
```

## Example: Combat Stim Skill

### Database Entry (warrior_combat_stim in skill_balance_database.tres)

```
SkillId: "warrior_combat_stim"
CastBehavior: Instant
DamageSource: None
Buff:
  Duration: 5.0
  AttackSpeedBonus: 0.40  (40%)
  MoveSpeedBonus: 1.00    (100%)
  DamageBonus: 0.30       (30%)
```

### Skill Resource (WarriorCombatStim.tres)

```
SkillId: "warrior_combat_stim"
SkillEffectScene: buff_activation_effect.tscn
```

### Execution Flow

1. Player presses F key
2. SkillManager.UseSkill(SkillSlot.Buff) called
3. skill.Initialize() loads BuffData from database
4. Player.TryInstantSkill() checks `skill.BuffDuration > 0`
5. BuffManager.ApplyBuff() called with bonuses
6. StatsManager.RecalculateStats() triggered
7. Player stats updated:
   - Attack rate: Base × 1.40 (40% faster)
   - Move speed: Base × 2.00 (100% faster = 2x speed)
   - Damage: Base × 1.30 (30% more damage)
8. Visual effect spawned (buff_activation_effect.tscn)
9. After 5 seconds: buff expires, stats return to normal

## Visual Feedback

### Buff Activation Effect

**Scene:** `Scenes/SkillEffects/buff_activation_effect.tscn`

**Structure:**
```
Node2D (root)
├── CPUParticles2D
│   ├── emitting = true
│   ├── one_shot = true
│   ├── amount = 25
│   ├── lifetime = 0.6
│   ├── emission_shape = Sphere (radius 50)
│   └── gravity = (0, 0)
└── Timer
    ├── wait_time = 1.0
    ├── one_shot = true
    ├── autostart = true
    └── timeout → root.queue_free()
```

**Spawning:**
```csharp
if (skill.SkillEffectScene != null)
{
    var effect = skill.SkillEffectScene.Instantiate<Node2D>();
    GetTree().Root.AddChild(effect);
    effect.GlobalPosition = GlobalPosition;
}
```

### Future: Buff Status UI

**Planned features (not yet implemented):**
- Buff icon display
- Remaining duration bar
- Stacking indicators
- Buff tooltips

## Extensibility

### Adding New Buff Types

**To add a new buff bonus:**

1. Add property to BuffBonuses class
2. Add property to BuffData Resource
3. Add GetTotal...Bonus() method to BuffManager
4. Update StatsManager to apply the new bonus
5. Update Skill.Initialize() to load the property

**Example: Adding Critical Chance Bonus**

```csharp
// 1. BuffBonuses.cs
public float CriticalChanceBonus { get; set; } = 0f;

// 2. BuffData.cs
[Export] public float CriticalChanceBonus { get; set; } = 0f;

// 3. BuffManager.cs
public float GetTotalCriticalChanceBonus()
{
    float total = 0f;
    foreach (var buff in _activeBuffs.Values)
    {
        total += buff.Bonuses.CriticalChanceBonus;
    }
    return total;
}

// 4. StatsManager.cs
public float CritChance
{
    get
    {
        float baseChance = // ... base calculation
        float buffBonus = BuffManager.GetTotalCriticalChanceBonus();
        return baseChance + buffBonus; // Additive for crit chance
    }
}

// 5. Skill.cs LoadBuffData()
CriticalChanceBonus = entry.Buff.CriticalChanceBonus;
```

### Adding Debuffs (Future)

**Current system is buff-only. To add debuffs:**

1. Create DebuffManager component (similar to BuffManager)
2. Apply negative bonuses (e.g., -0.30 = 30% slower)
3. Update StatsManager to apply debuff penalties
4. Add enemy/environmental sources for debuffs

**Alternative: Unified BuffDebuffManager**
- Bonuses can be negative (debuffs)
- Single system handles both
- Clearer for UI display (show all active effects)

## Best Practices

**When designing buff skills:**
- Use descriptive buff IDs (e.g., "warrior_combat_stim" not "buff1")
- Balance duration vs. power (short strong buffs vs. long weak buffs)
- Consider cooldown relative to duration (e.g., 5s duration, 10s cooldown = 50% uptime)
- Visual feedback helps players understand when buffs are active
- Data-driven approach: All buff values in SkillBalanceDatabase

**Performance considerations:**
- Buff expiration checked every frame (acceptable for <20 active buffs)
- Stat recalculation only when buffs change (not every frame)
- Dictionary lookup by ID is O(1)

**Testing checklist:**
- ✅ Buff applies correctly
- ✅ Stats update immediately
- ✅ Duration expires correctly
- ✅ Reapplying buff refreshes duration
- ✅ Multiple different buffs stack
- ✅ Buff persists across scene transitions (if needed)
- ✅ Visual effect spawns at player position

## Related Systems

- **StatsManager:** Applies buff bonuses to derived stats
- **SkillManager:** Routes instant skills to buff application
- **SkillBalanceDatabase:** Stores buff parameters
- **Player.TryInstantSkill():** Buff application logic

## See Also

- [`stat-system.md`](stat-system.md) - How stats are calculated with buffs
- [`skill-standardization.md`](skill-standardization.md) - Buff Pattern (Instant + None)
- [`balance-systems-architecture.md`](balance-systems-architecture.md) - Data-driven design philosophy
