# Balance Systems Architecture

## Overview

This document describes the centralized balance configuration systems for Zenith Rising. These systems separate balance parameters from code, enabling rapid iteration and sustainable content scaling.

**Two Core Systems:**
1. **BalanceConfig** - Game-wide balance parameters (player stats, enemy scaling, upgrade values)
2. **SkillBalanceDatabase** - Skill-specific content balance (damage, cooldowns, ranges)

**Implementation Status:** 📝 Phase 3.5-A (see [`phase-plan.md`](phase-plan.md))

---

## Why These Systems?

### The Problem We're Solving

**Before Balance Systems:**
```csharp
// StatsManager.cs
public const float STR_DAMAGE_PER_POINT = 0.03f;
public const float VIT_HEALTH_PER_POINT = 25f;

// UpgradeManager.cs  
new Upgrade { Value = 0.15f } // What is this? Damage boost per stack

// WarriorBasicAttack.tres
[Export] public float Damage = 30f;
[Export] public float Cooldown = 0f;
```

**Problems:**
- Magic numbers scattered across dozens of files
- Recompile required for every balance change
- No way to compare similar values (e.g., all projectile speeds)
- Inconsistent formulas between similar skills
- Difficult to tune during playtesting

**After Balance Systems:**
```csharp
// All values in one place, editable in inspector
var damage = GameBalance.Instance.Config.CharacterProgression.StrengthDamagePerPoint;
var upgradeValue = GameBalance.Instance.Config.UpgradeSystem.DamageBoostPerStack;
var skillBalance = GameBalance.Instance.SkillDatabase.GetSkillBalance("warrior_basic_attack");
```

**Benefits:**
- ✅ All balance values in two resource files
- ✅ Edit in Godot inspector, test immediately (no recompile)
- ✅ See all related values side-by-side
- ✅ Consistent formulas guaranteed
- ✅ Easy A/B testing and iteration

---

## System 1: BalanceConfig

### Purpose
Holds **game-wide balance parameters** that define how systems work. These are formulas and multipliers that rarely change but need to be tunable.

### File Locations
- **Script:** `Scripts/Core/BalanceConfig.cs`
- **Singleton:** `Scripts/Core/GameBalance.cs` (autoload)
- **Resource:** `Resources/Balance/balance_config.tres`

### Architecture

```
GameBalance (Singleton Autoload)
    └── Config (BalanceConfig resource)
            ├── PlayerStats
            │   ├── BaseMaxHealth
            │   ├── BaseSpeed
            │   └── BaseDamage
            ├── CharacterProgression  
            │   ├── StrengthDamagePerPoint
            │   ├── VitalityHealthPerPoint
            │   └── (all stat scaling formulas)
            ├── CombatSystem
            │   ├── BaseCritChance
            │   ├── BaseCritDamage
            │   └── DamageTypeMultipliers
            ├── Enemy
            │   ├── HealthScalingPerWave
            │   ├── DamageScalingPerFloor
            │   ├── DefaultAggroRange
            │   └── SpawnRates
            └── UpgradeSystem
                ├── DamageBoostPerStack
                ├── AttackSpeedPerStack
                └── (all upgrade values)
```

### Usage Pattern

**In any script:**
```csharp
// Access singleton
var config = GameBalance.Instance.Config;

// Read from appropriate section
float baseHealth = config.PlayerStats.BaseMaxHealth;
float strScaling = config.CharacterProgression.StrengthDamagePerPoint;
float critDamage = config.CombatSystem.BaseCritDamage;
float aggroRange = config.Enemy.DefaultAggroRange;
float upgradeValue = config.UpgradeSystem.DamageBoostPerStack;
```

### What Goes in BalanceConfig?

**Include:**
- Base values (starting player health, speed, damage)
- Scaling formulas (stat points → stat effects)
- System multipliers (crit damage, enemy scaling per wave)
- Spawn rates and timing
- Upgrade stack values

**Exclude:**
- Skill-specific values (use SkillBalanceDatabase)
- Content data (enemy types, floor definitions)
- UI/visual parameters
- Runtime state (current health, active buffs)

---

## System 2: SkillBalanceDatabase

### Purpose
Holds **skill-specific balance parameters** for all skills. This is content data that defines what each skill does numerically.

### File Locations
- **Enum:** `Scripts/Skills/Balance/SkillBalanceType.cs`
- **Entry:** `Scripts/Skills/Balance/SkillBalanceEntry.cs`
- **Database:** `Scripts/Skills/Balance/SkillBalanceDatabase.cs`
- **Resource:** `Resources/Balance/skill_balance_database.tres`

### Architecture

```
GameBalance (Singleton Autoload)
    └── SkillDatabase (SkillBalanceDatabase resource)
            └── Skills (Array<SkillBalanceEntry>)
                    ├── Entry: "warrior_basic_attack"
                    │   ├── DisplayName: "Fusion Cutter"
                    │   ├── BaseDamage: 30
                    │   ├── Cooldown: 0
                    │   ├── MeleeRange: 80
                    │   └── (all melee stats)
                    ├── Entry: "whirlwind"
                    │   ├── DisplayName: "Whirlwind"
                    │   ├── BaseDamage: 50
                    │   ├── AOERadius: 150
                    │   └── (all AOE stats)
                    └── (17 more skills...)
```

### SkillBalanceEntry Structure

**Universal Stats** (all skills have these):
- SkillId (string) - unique identifier
- DisplayName (string)
- Description (string)
- SkillType (enum: Projectile, InstantAOE, Melee, etc.)
- AllowedClass (enum: Warrior, Ranger, Mage, All)
- Slot (enum: BasicAttack, Special, Primary, Secondary, Ultimate)
- Cooldown (float)
- BaseDamage (float)
- DamageType (enum: Physical, Magical, True)
- CanCrit (bool)

**Type-Specific Stats** (grouped by skill type):

**Projectile Stats:**
- ProjectileSpeed
- ProjectileMaxRange
- ProjectileSize
- ProjectilePierceCount
- ProjectileExplodes
- ExplosionDamage
- ExplosionRadius

**AOE Stats:**
- AOERadius
- AOEDuration
- AOEHitInterval
- AOEMaxHitsPerEnemy

**Melee Stats:**
- MeleeRange
- MeleeAttackArc
- MeleeWindupTime
- MeleeActiveWindow
- MeleeAnimationDuration
- MeleeKnockbackForce

**Channeled Stats:**
- ChannelDuration
- ChannelDamageInterval
- ChannelInterruptible

**Summon Stats:**
- SummonDuration
- SummonMaxCount
- SummonHealthMultiplier
- SummonDamageMultiplier

**Buff Stats:**
- BuffDuration
- BuffStackable
- BuffMaxStacks
- (Specific buff effects go here)

### Usage Pattern

**Skill initialization:**
```csharp
// In Skill.cs
public void Initialize()
{
    if (_isInitialized) return;
    
    var database = GameBalance.Instance?.SkillDatabase;
    var balance = database.GetSkillBalance(SkillId);
    
    if (balance == null)
    {
        GD.PrintErr($"No balance entry found for {SkillId}");
        return;
    }
    
    // Load universal stats
    SkillName = balance.DisplayName;
    Description = balance.Description;
    Cooldown = balance.Cooldown;
    BaseDamage = balance.BaseDamage;
    DamageType = balance.DamageType;
    CanCrit = balance.CanCrit;
    
    _isInitialized = true;
}
```

**Type-specific loading (in subclass):**
```csharp
// In ProjectileSkill.cs
public new void Initialize()
{
    base.Initialize(); // Load universal stats
    
    var balance = GameBalance.Instance.SkillDatabase.GetSkillBalance(SkillId);
    
    // Load projectile-specific stats
    ProjectileSpeed = balance.ProjectileSpeed;
    ProjectileMaxRange = balance.ProjectileMaxRange;
    ProjectilePierceCount = balance.ProjectilePierceCount;
    // etc.
}
```

### What Goes in SkillBalanceDatabase?

**Include:**
- All numeric skill parameters (damage, cooldown, range, radius, etc.)
- Skill metadata (name, description, allowed class)
- Type-specific parameters for each skill pattern

**Exclude:**
- Visual/audio references (stay in .tres files)
- Execution logic (stays in executor classes)
- Player mastery tracking (stays in Skill.cs)
- Global formulas (use BalanceConfig instead)

---

## Integration with Existing Systems

### StatsManager Refactor

**Before:**
```csharp
public const float STR_DAMAGE_PER_POINT = 0.03f;
public const float STR_HEALTH_PER_POINT = 10f;
// ... 20 more constants

public void RecalculateStats(StatModifiers modifiers)
{
    float characterHealthBonus = (Vitality * VIT_HEALTH_PER_POINT) + (Strength * STR_HEALTH_PER_POINT);
    // ... lots of calculations with constants
}
```

**After:**
```csharp
public void RecalculateStats(StatModifiers modifiers)
{
    // Cache config references for cleaner code
    var playerConfig = GameBalance.Instance.Config.PlayerStats;
    var progressionConfig = GameBalance.Instance.Config.CharacterProgression;
    var combatConfig = GameBalance.Instance.Config.CombatSystem;
    
    // All formulas use config values
    float characterHealthBonus = 
        (Vitality * progressionConfig.VitalityHealthPerPoint) + 
        (Strength * progressionConfig.StrengthHealthPerPoint);
    
    CurrentMaxHealth = (playerConfig.BaseMaxHealth + characterHealthBonus) * 
                       (1 + modifiers.MaxHealthBonus);
    
    // etc.
}
```

### UpgradeManager Refactor

**Before:**
```csharp
private List<Upgrade> _availableUpgrades = new()
{
    new Upgrade { 
        UpgradeName = "Damage Boost", 
        Type = UpgradeType.DamagePercent, 
        Value = 0.15f  // Hardcoded
    },
    // ... more upgrades with hardcoded values
};
```

**After:**
```csharp
private List<Upgrade> _availableUpgrades = new()
{
    new Upgrade { 
        UpgradeName = "Damage Boost", 
        Type = UpgradeType.DamagePercent 
        // No Value - loaded from config when applied
    },
    // ... more upgrades
};

public void ApplyUpgrade(Upgrade upgrade)
{
    var config = GameBalance.Instance.Config.UpgradeSystem;
    
    float value = upgrade.Type switch
    {
        UpgradeType.DamagePercent => config.DamageBoostPerStack,
        UpgradeType.AttackSpeed => config.AttackSpeedPerStack,
        UpgradeType.MovementSpeed => config.MovementSpeedPerStack,
        // etc.
        _ => 0f
    };
    
    // Apply value...
}
```

### Skill Resource Files

**Before (WarriorBasicAttack.tres):**
```
[resource]
script = ExtResource("MeleeAttackSkill")
SkillName = "Fusion Cutter"
Description = "Slash in front of you"
Cooldown = 0.0
BaseDamage = 30.0
DamageType = 0 (Physical)
MeleeRange = 80.0
MeleeAttackArc = 120.0
SkillEffectScene = preload("res://Scenes/Effects/melee_slash.tscn")
```

**After:**
```
[resource]
script = ExtResource("MeleeAttackSkill")
SkillId = "warrior_basic_attack"
SkillEffectScene = preload("res://Scenes/Effects/melee_slash.tscn")
# Everything else loads from database on Initialize()
```

---

## Implementation Checklist

### Phase A.1: Create BalanceConfig (2 hours)

- [ ] Create `Scripts/Core/BalanceConfig.cs`:
  - [ ] Add nested config classes (PlayerStats, CharacterProgression, etc.)
  - [ ] Add [Export] attributes to all fields
  - [ ] Group fields with [ExportGroup] for inspector organization
  - [ ] Add default values that match current game behavior
- [ ] Create `Scripts/Core/GameBalance.cs`:
  - [ ] Singleton pattern with static Instance
  - [ ] [Export] BalanceConfig Config property
  - [ ] [Export] SkillBalanceDatabase SkillDatabase property (null for now)
  - [ ] _Ready() validates both are assigned
- [ ] Create `Resources/Balance/` folder
- [ ] Create `Resources/Balance/balance_config.tres`:
  - [ ] Assign BalanceConfig script
  - [ ] Fill in all default values from current code
- [ ] Add GameBalance to autoload:
  - [ ] Project Settings → Autoload → Add GameBalance.cs
  - [ ] Set as singleton (checkbox enabled)
- [ ] Wire up resource:
  - [ ] Select GameBalance in Autoload list
  - [ ] In inspector, assign balance_config.tres to Config property

### Phase A.2: Create SkillBalanceDatabase (2 hours)

- [ ] Create `Scripts/Skills/Balance/` folder
- [ ] Create `Scripts/Skills/Balance/SkillBalanceType.cs`:
  - [ ] Add enum: Projectile, InstantAOE, Melee, Channeled, Summon, Buff
- [ ] Create `Scripts/Skills/Balance/SkillBalanceEntry.cs`:
  - [ ] Add all universal stat fields
  - [ ] Add all type-specific stat groups with [ExportGroup]
  - [ ] Set sensible default values
- [ ] Create `Scripts/Skills/Balance/SkillBalanceDatabase.cs`:
  - [ ] [Export] Array<SkillBalanceEntry> Skills
  - [ ] GetSkillBalance(string skillId) lookup method
- [ ] Create `Resources/Balance/skill_balance_database.tres`:
  - [ ] Assign SkillBalanceDatabase script
- [ ] Add entries for existing skills:
  - [ ] warrior_basic_attack (Melee pattern)
  - [ ] whirlwind (InstantAOE pattern)
  - [ ] fireball (Projectile pattern)
- [ ] Wire to GameBalance:
  - [ ] In autoload settings, assign skill_balance_database.tres to SkillDatabase

### Phase A.3: Refactor StatsManager (45 min)

- [ ] Open `StatsManager.cs`
- [ ] Find all constants (STR_DAMAGE_PER_POINT, etc.)
- [ ] In RecalculateStats():
  - [ ] Add config cache: `var progressionConfig = GameBalance.Instance.Config.CharacterProgression;`
  - [ ] Replace each constant reference with config read
- [ ] Delete all constant declarations
- [ ] Test in game - verify stats still work correctly

### Phase A.4: Refactor UpgradeManager (30 min)

- [ ] Open `UpgradeManager.cs`
- [ ] In ApplyUpgrade():
  - [ ] Add switch statement that maps UpgradeType → config value
  - [ ] Read value from `GameBalance.Instance.Config.UpgradeSystem`
- [ ] Remove Value property from Upgrade class
- [ ] Remove Value assignments in _availableUpgrades list
- [ ] Test in game - verify upgrades still work correctly

### Phase A.5: Refactor Dungeon.cs (15 min)

- [ ] Open `Dungeon.cs`
- [ ] Find spawn timing constants
- [ ] Replace with `GameBalance.Instance.Config.Enemy.TimeBetweenSpawns`
- [ ] Find enemy scaling multiplier formulas
- [ ] Replace with config reads
- [ ] Test in game - verify enemy spawning works

### Phase A.6: Refactor Enemy.cs (15 min)

- [ ] Open `Enemy.cs`
- [ ] Find aggro range / leash range constants
- [ ] Replace with `GameBalance.Instance.Config.Enemy.DefaultAggroRange`
- [ ] Test in game - verify enemy AI works

### Phase A.7: Update Skill Loading (1 hour)

- [ ] Open `Skill.cs`:
  - [ ] Change exported properties → runtime properties (remove [Export])
  - [ ] Keep SkillId as only export
  - [ ] Add Initialize() method that loads from database
  - [ ] Add _isInitialized flag (idempotent pattern)
- [ ] Open ProjectileSkill.cs, InstantAOESkill.cs, MeleeAttackSkill.cs:
  - [ ] Add type-specific Initialize() overrides
  - [ ] Load type-specific parameters from balance entry
- [ ] Open `SkillManager.cs`:
  - [ ] In UseSkill(), call skill.Initialize() before execution
- [ ] Update skill .tres files:
  - [ ] WarriorBasicAttack.tres - set SkillId, remove other exports
  - [ ] Whirlwind.tres - set SkillId, remove other exports
  - [ ] Fireball.tres - set SkillId, remove other exports
- [ ] Test in game - verify all skills work correctly

### Phase A.8: Validation Testing (30 min)

- [ ] Launch game, play through one run
- [ ] Verify player stats work correctly
- [ ] Verify enemies spawn and scale correctly
- [ ] Verify all 3 skills work (basic attack, whirlwind, fireball)
- [ ] Verify upgrades apply correctly
- [ ] Check console for errors
- [ ] Tune a value in balance_config.tres, restart, verify change
- [ ] Tune a skill value in skill_balance_database.tres, restart, verify change

### Phase A.9: Documentation (15 min)

- [ ] Update CLAUDE.md Session Progress Log with completion
- [ ] Mark Phase A as ✅ COMPLETE in Current Phase Focus
- [ ] Commit changes with clear message: "Phase A complete: Balance systems foundation"

---

## Testing Workflow

### During Development
1. Change value in balance_config.tres or skill_balance_database.tres
2. Save resource file
3. Hit F5 to run game
4. Test change immediately (no recompile!)

### Validation Checklist
- [ ] Player health matches expected base + stat bonuses
- [ ] Player damage scales with STR/INT correctly
- [ ] Enemy health/damage scales per wave/floor correctly
- [ ] Upgrades apply correct stack values
- [ ] Skills deal expected damage
- [ ] Skill cooldowns work as configured
- [ ] No console errors on skill use
- [ ] Changing config values takes effect on next run

---

## Future Extensions

### CSV Export/Import (Phase 7+)
Once you have many skills and want spreadsheet-based tuning:
1. Create editor script that exports SkillBalanceDatabase to CSV
2. Edit in Excel/Google Sheets
3. Import CSV back to update skill_balance_database.tres
4. Useful for bulk operations and designer workflows

### Multiple Balance Profiles (Phase 7+)
For playtesting different tuning approaches:
1. Create balance_config_aggressive.tres
2. Create balance_config_defensive.tres
3. Swap which is assigned to GameBalance
4. Compare player feedback

### Balance Change Logging (Phase 6+)
Track what changes were made when:
1. Add BalanceChangeLog.cs autoload
2. Log when balance values are read
3. Export to file for analysis
4. Helps understand what values matter most

---

## Common Patterns

### Caching Config References
**Good practice for methods that read many values:**
```csharp
public void CalculateSomething()
{
    // Cache once at method start
    var config = GameBalance.Instance.Config.PlayerStats;
    
    // Use cached reference multiple times
    float value1 = config.BaseMaxHealth;
    float value2 = config.BaseSpeed;
    float value3 = config.BaseDamage;
}
```

### Null-Safety
**Always check before access:**
```csharp
if (GameBalance.Instance?.Config?.PlayerStats == null)
{
    GD.PrintErr("BalanceConfig not loaded!");
    return defaultValue;
}

float health = GameBalance.Instance.Config.PlayerStats.BaseMaxHealth;
```

### Default Values
**Config should have sensible defaults:**
```csharp
[Export] public float BaseMaxHealth { get; set; } = 100f;
```
This prevents null reference errors and makes the config work "out of the box".

---

## Design Decisions

### Why Nested Classes in BalanceConfig?
**Reason:** Inspector organization. Without nesting, you'd have 100+ flat fields making it hard to find anything. Nested classes group related values.

### Why Two Systems (Config + Database)?
**Reason:** Different access patterns. Config is "formulas that define how the game works." Database is "content values that define specific things." Mixing them would create confusion.

### Why Singleton Pattern?
**Reason:** Balance values are truly global configuration. Every system needs access. Dependency injection would create coupling nightmares. The singleton makes the global nature explicit and convenient.

### Why Initialize() Instead of _Ready()?
**Reason:** Skills are resources, not nodes, so they have no _Ready(). Calling Initialize() explicitly gives control over when loading happens and makes it idempotent (safe to call multiple times).

---

## Troubleshooting

### "GameBalance.Instance is null"
- GameBalance not added to autoload
- Check Project Settings → Autoload → ensure GameBalance is listed

### "Config is null"
- balance_config.tres not assigned to GameBalance
- In autoload settings, select GameBalance, assign config in inspector

### "GetSkillBalance returns null"
- SkillId doesn't match any entry in database
- Check spelling, check that entry exists in skill_balance_database.tres

### "Skills still have old damage values"
- Initialize() not being called
- Add call in SkillManager.UseSkill() before execution

### "Changes to config don't take effect"
- You must restart the game for resource changes to load
- No hot-reload for resource files (Godot limitation)

---

## Related Documentation

- **Implementation Plan:** [phase-plan.md](phase-plan.md) - Phase 3.5-A details
- **Skill Patterns:** [skill-standardization.md](skill-standardization.md) - How skills use database
- **Current Status:** [../../CLAUDE.md](../../CLAUDE.md) - Daily progress tracking

---

*This architecture creates a sustainable foundation for 18+ skills and enables rapid iteration during playtesting.*
