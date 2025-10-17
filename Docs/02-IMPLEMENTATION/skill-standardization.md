# Skill Implementation Standardization (✅ CURRENT IMPLEMENTATION)

> **✅ ACTIVE APPROACH:** This document describes the **animation-driven hitbox pattern** currently being implemented in Phase 3.5.
>
> **Related Documentation:**
> - [`skill-system-architecture.md`](skill-system-architecture.md) - Historical reference (type-based executor pattern)
> - [`../03-CONTENT-DESIGN/class-abilities.md`](../03-CONTENT-DESIGN/class-abilities.md) - Detailed skill specifications (source of truth for mechanics)
> - [`balance-systems-architecture.md`](balance-systems-architecture.md) - How skill balance values are stored and loaded
> - [`../../CLAUDE.md`](../../CLAUDE.md) - Current implementation status

---

> **⚠️ PREREQUISITE:** Before implementing this standardization (Phase B), you MUST complete **Phase A: Balance Systems Foundation** first. See [balance-systems-architecture.md](balance-systems-architecture.md) for details.
>
> Phase A creates the BalanceConfig and SkillBalanceDatabase that this standardization depends on. Skills will load their balance parameters (damage, cooldown, range, etc.) from the database, not from exported fields on .tres files.

## Problem Statement

Before implementing all 18 planned skills across 3 classes, we need a consistent, plug-and-play approach that:
- Prevents rework and inconsistency
- Enables rapid implementation of new skills
- Integrates cleanly with the animation system
- Maintains the existing skill architecture

**The Question:** Should hitboxes/damage be in Player (animation-driven) or in Effects (entity-driven)?

**The Answer:** **Both** - use a hybrid approach based on skill mechanics.

---

## Comprehensive Skill Inventory

All 18 planned skills analyzed from [class-abilities.md](../03-CONTENT-DESIGN/class-abilities.md):

### Marcus (Warrior) - 6 Skills
1. **Fusion Cutter** (Basic) - Melee swing, auto-aims, 5th hit cleaves
2. **Breaching Charge** (Special) - Dash forward, stun/knockback
3. **Crowd Suppression** (Q) - Ground slam 360° AOE
4. **Fortify** (E) - Deploy shield, block damage
5. **Combat Stim** (R) - Self-buff (attack/move speed)
6. **Last Stand** (Ultimate) - Invulnerability buff

### Aria (Ranger) - 6 Skills
1. **Precision Rifle** (Basic) - Hitscan ranged
2. **Charged Shot** (Special) - Hold to charge, piercing
3. **Tactical Grenade** (Q) - Throw grenade, delayed explosion
4. **Evasive Roll** (E) - Dash + spawn decoy
5. **Overwatch** (R) - Precision mode buff
6. **Killzone** (Ultimate) - Spawn turret + self-buff

### Elias (Mage) - 6 Skills
1. **Psionic Pulse** (Basic) - Projectile, stacking debuff
2. **Psionic Wave** (Special) - Cone pushback
3. **Arc Lightning** (Q) - Chain lightning
4. **Void Rift** (E) - Persistent zone (DOT/slow/lifesteal)
5. **Architect's Blessing** (R) - Attack chain buff
6. **Singularity** (Ultimate) - Gravity well + implosion

---

## Standardization Framework

### Core Principle: Two-Axis Classification

Every skill is classified along **two independent axes**:

#### Axis 1: Cast Behavior
- **Instant** - Executes immediately when activated (no animation lock)
- **AnimationDriven** - Requires cast animation, player locked during cast

#### Axis 2: Damage Source
- **PlayerHitbox** - Damage dealt by player-attached Area2D hitboxes
- **EffectCollision** - Damage dealt by spawned Effect's collision system
- **None** - No damage (pure buffs/utility)

### Implementation Matrix (6 Patterns)

| Cast Behavior | Damage Source | Pattern Name | Examples | Implementation |
|---------------|---------------|--------------|----------|----------------|
| AnimationDriven | PlayerHitbox | **Melee Pattern** | Fusion Cutter, Psionic Wave | Player hitbox enabled by animation track |
| AnimationDriven | EffectCollision | **Cast-Spawn Pattern** | Breaching Charge (dash melee) | Animation track spawns effect at key frame |
| AnimationDriven | None | **Cast-Buff Pattern** | Fortify (deployment animation) | Animation track applies buff at key frame |
| Instant | PlayerHitbox | **Instant-Melee Pattern** | (Rare, not used in current design) | Hitbox enabled immediately |
| Instant | EffectCollision | **Projectile Pattern** | Fireball, Psionic Pulse, Grenade | Existing system - executor spawns effect |
| Instant | None | **Instant-Buff Pattern** | Combat Stim, Overwatch | Existing system - BuffExecutor applies stats |

---

## Skill Type Classification

### 1. Melee Pattern (AnimationDriven + PlayerHitbox)
**Skills:** Fusion Cutter, Psionic Wave (cone)

**Flow:**
```
Input → SkillManager.UseSkill()
  → Player.TryBasicAttack() / TryCastSkill()
  → Player state: CastingSkill, play animation
  → Animation frame 0.1s: Call Player.EnableMeleeHitbox()
  → Player.OnHitboxBodyEntered(): Apply damage via CombatSystem
  → Animation frame 0.25s: Call Player.DisableMeleeHitbox()
  → Animation finishes: Return to Idle/Running
```

**Implementation:**
- Hitbox: Child Area2D in player.tscn, disabled by default
- Damage: Player.cs calculates and applies directly
- Visual: Optional - spawn visual effect via animation track
- Effect scene: Purely visual (particles, sprites) - NO collision logic

**Benefits:**
- ✅ Frame-perfect damage timing
- ✅ No spawn delay
- ✅ Simple collision logic in one place

---

### 2. Instant AOE Pattern (AnimationDriven + PlayerHitbox)
**Skills:** Whirlwind, Crowd Suppression

**Flow:**
```
Input → SkillManager.UseSkill()
  → Player.TryCastSkill()
  → Player state: CastingSkill, play animation
  → Animation frame 0.1s: Call Player.EnableAOEHitbox()
  → Player.OnHitboxBodyEntered(): Track hit enemies, apply damage
  → Animation frame 0.15s: Call Player.SpawnVisualEffect() (optional)
  → Animation frame 0.7s: Call Player.DisableAOEHitbox()
  → Animation finishes: Return to Idle/Running
```

**Implementation:**
- Hitbox: Circular Area2D child in player.tscn, disabled by default
- Damage: Player.cs calculates damage, uses HashSet to prevent multi-hit
- Visual: Spawn separate Node2D for spinning/slam visual
- Effect scene: Purely visual (rotation animation, particles) - NO collision logic

**Example Refactor:**
```csharp
// OLD WhirlwindEffect.cs - REMOVE collision logic
private void ApplyDamage()
{
    var enemies = GetTree().GetNodesInGroup("enemies");
    foreach (Node node in enemies)
    {
        if (node is Enemy enemy)
        {
            float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
            if (distance < _radius) { /* Apply damage */ }
        }
    }
}

// NEW WhirlwindVisual.cs - Pure visuals
public partial class WhirlwindVisual : Node2D
{
    [Export] public float Duration = 0.8f;
    [Export] public float RotationSpeed = 10f;

    public override void _Ready()
    {
        GetTree().CreateTimer(Duration).Timeout += QueueFree;
    }

    public override void _Process(double delta)
    {
        Rotation += RotationSpeed * (float)delta; // Visual only
    }
}
```

---

### 3. Projectile Pattern (Instant + EffectCollision)
**Skills:** Fireball, Psionic Pulse, Precision Rifle, Arc Lightning, Tactical Grenade

**Flow:**
```
Input → SkillManager.UseSkill()
  → Skill.Execute() immediately
  → Executor spawns Effect (existing system)
  → Effect handles: movement, collision, damage, lifetime
  → Effect.OnBodyEntered(): Apply damage, track kills
  → Effect.QueueFree() when done
```

**Implementation:**
- Hitbox: Effect's own Area2D collision shape
- Damage: Effect calculates via CalculateDamage() helper
- Visual: Effect controls its own sprite/particles
- **NO changes to existing system** - already works perfectly

**This is the current FireballProjectile pattern - keep as-is!**

---

### 4. Cast-Spawn Pattern (AnimationDriven + EffectCollision)
**Skills:** Breaching Charge (dash + hit)

**Flow:**
```
Input → SkillManager.UseSkill()
  → Player.TryCastSkill()
  → Player state: CastingSkill, play dash animation
  → Animation moves player via position keyframes
  → Animation frame 0.2s: Call Player.ExecuteDashSkill()
  → ExecuteDashSkill() spawns hitbox effect that follows player
  → Effect handles collision/damage during dash
  → Animation finishes: Return to Idle/Running
```

**Implementation:**
- Hitbox: Spawned Effect (Area2D) that follows player during dash
- Damage: Effect calculates and applies
- Visual: Effect includes dash trail, impact effects
- Hybrid: Animation controls player movement, Effect handles collision

---

### 5. Buff Pattern (Instant or AnimationDriven + None)
**Skills:** Combat Stim (instant), Fortify (animated), Overwatch (instant), Architect's Blessing (instant), Last Stand (instant)

**Flow (Instant):**
```
Input → SkillManager.UseSkill()
  → Skill.Execute() immediately
  → BuffExecutor spawns BuffEffect
  → BuffEffect modifies player stats (via StatsManager or direct)
  → BuffEffect persists for duration
  → BuffEffect removes stat changes on destruction
```

**Flow (Animated - e.g., Fortify):**
```
Input → SkillManager.UseSkill()
  → Player.TryCastSkill()
  → Player state: CastingSkill, play deploy animation
  → Animation frame 0.3s: Call Player.ExecuteBuffSkill()
  → ExecuteBuffSkill() spawns BuffEffect (same as instant)
  → Animation finishes: Return to Idle/Running
```

**Implementation:**
- No hitbox needed
- Damage: N/A
- Visual: BuffEffect or shader on player
- Stats: BuffEffect modifies StatsManager properties or UpgradeManager

---

### 6. Persistent Zone Pattern (Instant + EffectCollision)
**Skills:** Void Rift, Singularity, Killzone (turret)

**Flow:**
```
Input → SkillManager.UseSkill()
  → Skill.Execute() immediately at cursor position
  → Executor spawns PersistentZoneEffect at location
  → Effect creates Area2D, monitors overlapping enemies
  → Effect applies continuous damage/effects in _Process()
  → Effect persists for duration or until destroyed
  → Effect.QueueFree() when duration expires
```

**Implementation:**
- Hitbox: Effect's own Area2D with Monitoring enabled
- Damage: Effect applies continuous damage to overlapping bodies
- Visual: Effect manages its own visuals (zone sprite, particles)
- Location: Spawned at cursor/target position, not attached to player

---

## Skill Data Extensions

> **Note:** These enums are added in Phase B. The SkillId and Initialize() pattern are added in Phase A (Balance Systems).

Add two new properties to base **Skill.cs**:

```csharp
public enum CastBehavior
{
    Instant,           // Execute immediately
    AnimationDriven    // Requires animation with timing
}

public enum DamageSource
{
    PlayerHitbox,      // Damage from player-attached hitbox
    EffectCollision,   // Damage from spawned effect
    None              // No damage (buffs/utility)
}

[Export] public CastBehavior CastType { get; set; } = CastBehavior.Instant;
[Export] public DamageSource DamageSourceType { get; set; } = DamageSource.EffectCollision;
[Export] public string AnimationName { get; set; } = "";  // For AnimationDriven skills
```

**Usage in SkillManager.UseSkill():**
```csharp
private void UseSkill(Skill skill, ref float cooldownRemaining)
{
    if (cooldown checks fail) return;

    // Route based on cast behavior
    if (skill.CastType == CastBehavior.AnimationDriven)
    {
        // Request animation from player
        if (_player.TryCastSkill(skill))
        {
            cooldownRemaining = skill.Cooldown;
        }
    }
    else // CastBehavior.Instant
    {
        // Execute immediately (existing system)
        skill.Execute(_player);
        cooldownRemaining = skill.Cooldown;
    }
}
```

---

## Player Hitbox Standardization

### Hitbox Nodes in player.tscn

Create standardized hitbox nodes as children of Player:

```
Player (CharacterBody2D)
├── Sprite2D
├── AnimationPlayer
├── CollisionShape2D (player body)
├── MeleeHitbox (Area2D, disabled)
│   └── CollisionShape2D (RectangleShape2D 60x40)
├── AOEHitbox (Area2D, disabled)
│   └── CollisionShape2D (CircleShape2D radius 150)
└── DashHitbox (Area2D, disabled)
    └── CollisionShape2D (CapsuleShape2D for dash trail)
```

### Hitbox Control Methods in Player.cs

```csharp
// Generic hitbox control
private Area2D _meleeHitbox;
private Area2D _aoeHitbox;
private Area2D _dashHitbox;
private HashSet<Enemy> _hitEnemiesThisCast = new();
private Skill _currentCastingSkill;

public void EnableMeleeHitbox()
{
    _hitEnemiesThisCast.Clear();
    UpdateMeleeHitboxPosition(); // Position based on _lastDirection
    _meleeHitbox.Monitoring = true;
}

public void DisableMeleeHitbox()
{
    _meleeHitbox.Monitoring = false;
}

public void EnableAOEHitbox()
{
    _hitEnemiesThisCast.Clear();
    _aoeHitbox.Monitoring = true;
}

public void DisableAOEHitbox()
{
    _aoeHitbox.Monitoring = false;
}

// Collision handlers
private void OnMeleeHitboxBodyEntered(Node2D body)
{
    if (body is not Enemy enemy) return;
    if (_hitEnemiesThisCast.Contains(enemy)) return;

    _hitEnemiesThisCast.Add(enemy);
    ApplyHitboxDamage(enemy);
}

private void OnAOEHitboxBodyEntered(Node2D body)
{
    if (body is not Enemy enemy) return;
    if (_hitEnemiesThisCast.Contains(enemy)) return;

    _hitEnemiesThisCast.Add(enemy);
    ApplyHitboxDamage(enemy);
}

private void ApplyHitboxDamage(Enemy enemy)
{
    if (_currentCastingSkill == null || _statsManager == null) return;

    // Get base damage from skill
    float baseDamage = GetSkillBaseDamage(_currentCastingSkill);

    // Calculate with stats
    float damage = CombatSystem.CalculateDamage(
        baseDamage,
        _statsManager,
        _currentCastingSkill.DamageType
    );

    enemy.TakeDamage(damage);
}

private float GetSkillBaseDamage(Skill skill)
{
    return skill switch
    {
        MeleeAttackSkill melee => melee.Damage,
        InstantAOESkill aoe => aoe.Damage,
        _ => 0f
    };
}
```

---

## Implementation Checklist Template

When implementing any new skill, follow this checklist:

### 1. Classify Skill
- [ ] Determine CastBehavior (Instant or AnimationDriven)
- [ ] Determine DamageSource (PlayerHitbox, EffectCollision, or None)
- [ ] Identify pattern from matrix (Melee, Projectile, Buff, etc.)

### 2. Create Skill Data
- [ ] Choose or create skill data class (MeleeAttackSkill, ProjectileSkill, etc.)
- [ ] Set CastType, DamageSourceType, AnimationName properties
- [ ] Create .tres resource with all properties configured

### 3. Animation (if AnimationDriven)
- [ ] Create animation in AnimationPlayer (warrior_attack_down, etc.)
- [ ] Add region_rect keyframes for sprite animation
- [ ] Add Call Method tracks for timing:
  - EnableHitbox() at damage frame
  - DisableHitbox() at end frame
  - ExecuteCurrentSkillEffect() if spawning effect mid-animation

### 4. Hitbox (if PlayerHitbox)
- [ ] Ensure appropriate hitbox exists in player.tscn (Melee, AOE, Dash)
- [ ] Connect hitbox BodyEntered signal to handler in Player._Ready()
- [ ] Test hitbox positioning and size

### 5. Effect (if EffectCollision or visual-only)
- [ ] Create Effect script (inherits SkillEffect or CollisionSkillEffect)
- [ ] Implement Initialize() to read skill data
- [ ] Implement behavior (movement, collision, damage, lifetime)
- [ ] Create effect scene with visuals
- [ ] Assign scene to skill .tres

### 6. Executor (usually reuse existing)
- [ ] Verify appropriate executor exists for skill type
- [ ] If new type needed, create executor following ProjectileSkillExecutor pattern
- [ ] Register in Skill.CreateExecutor() factory

### 7. Testing
- [ ] Skill casts correctly (animation plays or instant)
- [ ] Damage applied at correct frame/moment
- [ ] Cooldown works
- [ ] Visual effects spawn and look correct
- [ ] Mastery tracking increments

---

## Skill Type Mapping (All 18 Skills)

| Skill | Class | Pattern | CastBehavior | DamageSource | AnimationName | Notes |
|-------|-------|---------|--------------|--------------|---------------|-------|
| **Fusion Cutter** | Warrior | Melee | AnimationDriven | PlayerHitbox | warrior_attack_[dir] | 5th hit cleave = track hit count |
| **Whirlwind** | Warrior | Instant AOE | AnimationDriven | PlayerHitbox | warrior_whirlwind | Circular hitbox |
| **Breaching Charge** | Warrior | Cast-Spawn | AnimationDriven | EffectCollision | warrior_dash | Dash with collision trail |
| **Crowd Suppression** | Warrior | Instant AOE | AnimationDriven | PlayerHitbox | warrior_slam | Ground slam animation |
| **Fortify** | Warrior | Cast-Buff | AnimationDriven | None | warrior_fortify | Shield deployment |
| **Combat Stim** | Warrior | Instant-Buff | Instant | None | - | Immediate stat boost |
| **Last Stand** | Warrior | Instant-Buff | Instant | None | - | Invulnerability buff |
| **Precision Rifle** | Ranger | Projectile | Instant | EffectCollision | - | Hitscan projectile |
| **Charged Shot** | Ranger | Projectile | AnimationDriven | EffectCollision | ranger_charge | Hold animation, release spawns |
| **Tactical Grenade** | Ranger | Projectile | Instant | EffectCollision | - | Grenade with delayed explosion |
| **Evasive Roll** | Ranger | Cast-Spawn | AnimationDriven | EffectCollision | ranger_roll | Dash + spawn decoy |
| **Overwatch** | Ranger | Instant-Buff | Instant | None | - | Precision mode buff |
| **Killzone** | Ranger | Projectile | Instant | EffectCollision | - | Spawn turret entity |
| **Psionic Pulse** | Mage | Projectile | Instant | EffectCollision | - | Stacking projectile |
| **Psionic Wave** | Mage | Melee | AnimationDriven | PlayerHitbox | mage_wave | Cone-shaped hitbox |
| **Arc Lightning** | Mage | Projectile | Instant | EffectCollision | - | Chain effect |
| **Void Rift** | Mage | Persistent Zone | Instant | EffectCollision | - | Zone at cursor |
| **Architect's Blessing** | Mage | Instant-Buff | Instant | None | - | Attack chain buff |
| **Singularity** | Mage | Persistent Zone | Instant | EffectCollision | - | Gravity well |

---

## Benefits of This Standardization

### For Development
- ✅ **Clear pattern selection** - Look up skill in table, follow checklist
- ✅ **Minimal code duplication** - Reuse hitboxes, executors, patterns
- ✅ **Predictable debugging** - Each pattern has defined flow
- ✅ **Fast iteration** - Most skills reuse existing types

### For Architecture
- ✅ **Skill system unchanged** - Executors/effects remain data-driven
- ✅ **Animation system clean** - FSM only manages state, not gameplay logic
- ✅ **Separation of concerns** - Collision source is explicit per skill
- ✅ **No breaking changes** - Existing projectile skills work as-is

### For Future Skills
- ✅ **Plug-and-play** - New skills just pick pattern from matrix
- ✅ **Extensible** - Can add new patterns (e.g., Channeled skills)
- ✅ **Documented** - This guide serves as implementation reference

---

## Extensibility

The framework is designed to handle new skill concepts by:

### Adding New CastBehavior Values
Example: `Channeled` for skills that maintain while held
- Add `Channeled` to enum
- Implement `TryChannelSkill()` in Player.cs
- Create channel loop in animation

### Adding New DamageSource Values
Example: `PersistentHitbox` for continuous damage zones
- Add to enum
- Implement zone management in Player.cs
- Wire to animation tracks

### Adding New Patterns
New patterns emerge by combining existing axes or extending enums:
- Channeled + PlayerHitbox = "Flamethrower Pattern"
- Instant + PersistentHitbox = "Trap Pattern"
- Toggle + EffectCollision = "Drone Pattern"

**The framework doesn't limit you** - it provides structure for common cases while allowing flexibility for unique mechanics.

---

## Implementation Order Recommendation

> **UPDATED:** Phase names have been adjusted to reflect the Balance Systems implementation priority.

### Phase A: Balance Systems Foundation (Do First - 4-6 hours)
**Status:** ⏳ CURRENT PRIORITY

**This phase MUST be completed before Phase B.** See [balance-systems-architecture.md](balance-systems-architecture.md) for full implementation guide.

1. Create BalanceConfig system (game-wide parameters)
2. Create SkillBalanceDatabase system (skill-specific parameters)
3. Refactor StatsManager to use BalanceConfig
4. Refactor UpgradeManager to use BalanceConfig
5. Add Initialize() pattern to Skill.cs that loads from database
6. Update existing skills to use database

**Why this comes first:** Skills in Phases C-F will immediately load from the database. Creating skills with exported values that need refactoring later wastes time.

### Phase B: Skill System Standardization (After Phase A - 2-3 hours)
**Status:** 📝 NEXT

1. Add CastBehavior and DamageSource enums to Skill.cs
2. Update SkillManager.UseSkill() to route based on CastBehavior
3. Add hitbox control methods to Player.cs
4. Create hitbox nodes in player.tscn

### Phase C: Prove Pattern (Validate Approach - 1-2 hours)
1. Implement Fusion Cutter (Melee Pattern) - prove PlayerHitbox works
2. Implement Whirlwind (Instant AOE Pattern) - prove animation-driven AOE works
3. Test both extensively - ensure damage timing, animation flow correct

### Phase D: Whirlwind (AOE Pattern - 1-2 hours)
1. Implement Whirlwind using Instant AOE Pattern
2. Skill loads balance from database (damage, radius, duration)
3. Refactor WhirlwindEffect → WhirlwindVisual (remove collision)
4. Test animation-driven AOE hitbox
5. Tune values in skill_balance_database.tres

### Phase E: Complete Warrior (Build Out - 3-4 hours)
1. Breaching Charge (Cast-Spawn Pattern)
2. Crowd Suppression (reuse Instant AOE Pattern)
3. Combat Stim (Instant-Buff Pattern)
4. Fortify and Last Stand later (Phase 4+)

### Phase F: Polish (1-2 hours)
1. Tune all warrior skills in inspector
2. Visual effects polish
3. Animation timing adjustments

---

## Post-Warrior: Ranger & Mage (Phase 4+)
1. Use established patterns from matrix
2. Most are Instant + EffectCollision (existing system works)
3. Only Psionic Wave needs new animation (Melee Pattern)

---

## Success Criteria

- ✅ All 6 patterns from matrix are proven to work
- ✅ Fusion Cutter and Whirlwind work with animation-driven hitboxes
- ✅ Existing Fireball still works (Projectile Pattern unchanged)
- ✅ New skills can be implemented by following checklist
- ✅ No confusion about "which approach to use" - table provides answer
- ✅ Code is maintainable and documented

---

## Related Documentation

- **Animation Integration:** [animation-architecture.md](animation-architecture.md)
- **Core Skill System:** [skill-system-architecture.md](skill-system-architecture.md)
- **Skill Designs:** [../03-CONTENT-DESIGN/class-abilities.md](../03-CONTENT-DESIGN/class-abilities.md)
- **Implementation Plan:** [phase-plan.md](phase-plan.md)

---

*This standardization provides a comprehensive, coherent framework for implementing all planned skills with clear rules, minimal code duplication, and predictable behavior.*
