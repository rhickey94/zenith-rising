# 🎮 Skill System Architecture Guide

## Overview

The skill system is built on a **type-based, data-driven architecture** that separates skill data, execution logic, and runtime behavior. This design allows you to create 50+ skills with only 7 executor types, making the system highly scalable and maintainable.

**Key Principle:** Executors are **reusable based on skill type**, not specific to individual skills. For example, `ProjectileSkillExecutor` handles *all* projectile-type skills (Fireball, Ice Bolt, Lightning Strike, etc.).

---

## 🏗️ Architecture Components

### 1️⃣ Skill Data (Resources)

**Purpose:** Define what a skill *is* (damage values, cooldowns, behavior parameters)

**Location:** `Scripts/Skills/Data/`

**Pattern:** Godot Resources that inherit from base `Skill` class

**Example:**
```csharp
// Scripts/Skills/Data/ProjectileSkill.cs
[GlobalClass]
public partial class ProjectileSkill : Skill
{
    [Export] public float DirectDamage { get; set; } = 40f;
    [Export] public float ExplosionDamage { get; set; } = 25f;
    [Export] public float ExplosionRadius { get; set; } = 100f;
}
```

**Created in Godot as .tres files:**
- `Resources/Skills/Mage/Fireball.tres` (type: ProjectileSkill)
- `Resources/Skills/Mage/Iceball.tres` (type: ProjectileSkill, different values)

Both skills share the same executor and effect, but have different stats!

---

### 2️⃣ Skill Executors (Type-Based Logic)

**Purpose:** Spawn and initialize effects based on skill *type* (not individual skills)

**Location:** `Scripts/Skills/Executors/`

**Pattern:** Classes implementing `ISkillExecutor` interface

**Example:**
```csharp
// Scripts/Skills/Executors/ProjectileSkillExecutor.cs
// Handles ALL projectile-type skills (Fireball, Ice Bolt, etc.)
public class ProjectileSkillExecutor : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill baseSkill)
    {
        if (baseSkill is not ProjectileSkill skill) return;

        // Calculate direction
        Vector2 direction = (player.GetGlobalMousePosition() - player.GlobalPosition).Normalized();

        // Spawn generic CollisionSkillEffect
        var effect = skill.SkillEffectScene.Instantiate<CollisionSkillEffect>();
        effect.GlobalPosition = player.GlobalPosition;

        // Standardized initialization
        effect.Initialize(skill, player, direction);

        // Add to scene
        player.GetTree().Root.AddChild(effect);
    }
}
```

**Responsibility:** Spawning only—no skill-specific logic. The executor doesn't know if it's spawning a Fireball or Ice Bolt!

---

### 3️⃣ Skill Effects (Runtime Behavior)

**Purpose:** Define how spawned entities *behave*

**Location:** `Scripts/Skills/Effects/`

**Pattern:** Inherit from `SkillEffect` (Node2D) or `CollisionSkillEffect` (Area2D)

**Two-Tier Hierarchy:**
- **SkillEffect** (Node2D) → For instant/non-collision effects (Whirlwind, buffs, teleports)
- **CollisionSkillEffect** (Area2D) → For collision-based effects (projectiles, melee attacks)

**Example:**
```csharp
// Scripts/Skills/Effects/FireballProjectile.cs
public partial class FireballProjectile : CollisionSkillEffect
{
    [Export] public float Speed = 400.0f;
    [Export] public float MaxRange = 500.0f;

    private float _directDamage;
    private float _explosionDamage;
    private float _explosionRadius;
    private Vector2 _startPosition;

    public override void Initialize(Skill sourceSkill, Player caster, Vector2 direction)
    {
        base.Initialize(sourceSkill, caster, direction);

        var skill = sourceSkill as ProjectileSkill;
        _directDamage = skill.DirectDamage;
        _explosionDamage = skill.ExplosionDamage;
        _explosionRadius = skill.ExplosionRadius;
        _startPosition = caster.GlobalPosition;

        // Apply mastery bonuses
        ApplyMasteryBonuses();
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direction * Speed * (float)delta;

        if (GlobalPosition.DistanceTo(_startPosition) >= MaxRange)
            Explode();
    }

    private void Explode()
    {
        // Damage enemies, spawn VFX, destroy self
        // Track kills via OnEnemyKilled(enemy)
        QueueFree();
    }
}
```

**Responsibility:** All runtime behavior (movement, collision, damage, lifetime management)

---

## 🔄 Execution Flow

```
Player presses Q
    ↓
SkillManager calls Skill.Execute(player)
    ↓
Skill.CreateExecutor() uses C# pattern matching
    ↓
Factory: this switch { Data.ProjectileSkill => new ProjectileSkillExecutor() }
    ↓
Executor.ExecuteSkill(player, skill)
    ↓
Executor spawns effect using skill.SkillEffectScene
    ↓
Effect.Initialize(skill, player, direction) - standardized API
    ↓
Effect handles behavior, tracks kills for mastery
    ↓
Effect destroys itself when done
```

**Visual Diagram:**
```
┌──────────────────┐
│  Fireball.tres   │  (ProjectileSkill Resource)
│  - DirectDmg: 40 │
│  - ExplosionDmg  │
│  - Radius: 100   │
│  - Scene: [ref]  │
└────────┬─────────┘
         │
         ↓ (is type ProjectileSkill)
┌─────────────────────────┐
│ ProjectileSkillExecutor │  (Type-based, reusable)
│ - Reads skill type      │
│ - Spawns SkillEffectScene│
│ - Calls Initialize()    │
└────────┬────────────────┘
         │
         ↓ (creates instance of)
┌─────────────────────────┐
│  FireballProjectile.cs  │  (Specific effect behavior)
│  - Travels forward      │
│  - Explodes on impact   │
│  - Damages enemies      │
│  - Tracks kills         │
└─────────────────────────┘
```

---

## 📊 Skill Mastery System

Every skill tracks **kills** and advances through **mastery tiers** that provide permanent bonuses.

### Mastery Tiers
```csharp
public enum SkillMasteryTier
{
    Bronze,   // 0-99 kills
    Silver,   // 100-499 kills
    Gold,     // 500-1999 kills
    Diamond   // 2000+ kills
}
```

### How It Works

**In Skill.cs (Data):**
```csharp
[Export] public int KillCount { get; set; } = 0;
[Export] public SkillMasteryTier CurrentTier { get; set; } = SkillMasteryTier.Bronze;

private static readonly int[] _masteryThresholds = { 100, 500, 2000, 10000 };

public void RecordKill()
{
    KillCount++;
    CheckMasteryTierUp();
}
```

**In Effects:**
```csharp
// When enemy dies from this skill
if (healthBefore > 0 && enemy.Health <= 0)
{
    OnEnemyKilled(enemy); // Base class tracks the kill
}

// Apply tier bonuses
private void ApplyMasteryBonuses()
{
    switch (_sourceSkill.CurrentTier)
    {
        case SkillMasteryTier.Silver:
            _damage *= 1.5f;
            break;
        case SkillMasteryTier.Gold:
            _damage *= 2.0f;
            _radius *= 1.2f;
            break;
        case SkillMasteryTier.Diamond:
            _damage *= 3.0f;
            _radius *= 1.5f;
            _maxPierce += 2;
            break;
    }
}
```

**Mastery Progress Example:**
```
Whirlwind Mastery: 347/500 kills to Silver ⚔️
├── Bronze (0/100): Base skill unlocked ✓
├── Silver (47/400): +50% damage, +2 sec duration [IN PROGRESS]
├── Gold (0/1500): Pulls enemies, +100% damage
└── Diamond (0/8000): Fire tornado, chains to nearby enemies
```

---

## 🎯 Supported Skill Types

### Current Implementation (7 Types)

| Skill Type | Executor | Effect Base Class | Example Skills |
|------------|----------|-------------------|----------------|
| ProjectileSkill | ProjectileSkillExecutor | CollisionSkillEffect | Fireball, Ice Bolt, Arrow |
| InstantAOESkill | InstantAOESkillExecutor | SkillEffect | Whirlwind, Earthquake |
| MeleeAttackSkill | MeleeSkillExecutor | CollisionSkillEffect | Sword Slash, Hammer Smash |
| DashSkill | DashSkillExecutor | SkillEffect | Disengage, Roll |
| BuffSkill | BuffSkillExecutor | SkillEffect | Battle Cry, Haste |
| TeleportSkill | TeleportSkillExecutor | SkillEffect | Blink, Shadow Step |
| DashMeleeSkill | DashMeleeSkillExecutor | CollisionSkillEffect | Dash Strike, Charge |

**Each type supports unlimited skill variants through resources!**

---

## 📂 File Organization

```
Scripts/Skills/
├── Base/
│   ├── Skill.cs                    # Base class + factory + mastery tracking
│   └── ISkillExecutor.cs           # Executor interface
│
├── Data/                           # Skill type definitions
│   ├── ProjectileSkill.cs
│   ├── InstantAOESkill.cs
│   ├── MeleeAttackSkill.cs
│   ├── DashSkill.cs
│   ├── BuffSkill.cs
│   ├── TeleportSkill.cs
│   └── DashMeleeSkill.cs
│
├── Executors/                      # Type-based executors (reusable!)
│   ├── ProjectileSkillExecutor.cs
│   ├── InstantAOESkillExecutor.cs
│   ├── MeleeSkillExecutor.cs
│   ├── DashSkillExecutor.cs
│   ├── BuffSkillExecutor.cs
│   ├── TeleportSkillExecutor.cs
│   └── DashMeleeSkillExecutor.cs
│
└── Effects/                        # Effect base classes + implementations
    ├── SkillEffect.cs              # Base (Node2D)
    ├── CollisionSkillEffect.cs     # Base (Area2D)
    ├── FireballProjectile.cs
    ├── BasicProjectile.cs
    ├── WhirlwindEffect.cs
    ├── MeleeAttackEffect.cs
    ├── DashEffect.cs
    ├── BuffEffect.cs
    ├── TeleportEffect.cs
    └── DashMeleeEffect.cs

Resources/Skills/                   # Skill .tres files (class-organized)
├── Mage/
│   ├── Fireball.tres               # ProjectileSkill
│   ├── Iceball.tres                # ProjectileSkill (variant!)
│   └── MageBasicAttack.tres
├── Warrior/
│   ├── Whirlwind.tres              # InstantAOESkill
│   └── WarriorBasicAttack.tres     # MeleeAttackSkill
└── Ranger/
    └── RangerBasicAttack.tres

Scenes/SkillEffects/                # Visual scenes for effects
├── fireball_projectile.tscn
├── projectile.tscn
├── whirlwind_effect.tscn
└── melee_attack.tscn
```

---

## 🛠️ Adding a New Skill (Step-by-Step)

### Example: Add "Chain Lightning" (Mage Q skill)

#### Step 1: Choose or Create Skill Type

**Option A:** Use existing type if behavior fits
```csharp
// Chain Lightning is a projectile → Use ProjectileSkill
// No new code needed!
```

**Option B:** Create new type if needed
```csharp
// Scripts/Skills/Data/ChainSkill.cs
[GlobalClass]
public partial class ChainSkill : Skill
{
    [Export] public float Damage { get; set; } = 30f;
    [Export] public int MaxChains { get; set; } = 3;
    [Export] public float ChainRange { get; set; } = 200f;
}
```

#### Step 2: Create Executor (only if new type)

```csharp
// Scripts/Skills/Executors/ChainSkillExecutor.cs
public class ChainSkillExecutor : ISkillExecutor
{
    public void ExecuteSkill(Player player, Skill baseSkill)
    {
        if (baseSkill is not ChainSkill skill) return;

        Vector2 direction = (player.GetGlobalMousePosition() - player.GlobalPosition).Normalized();

        var effect = skill.SkillEffectScene.Instantiate<SkillEffect>();
        effect.GlobalPosition = player.GlobalPosition;
        effect.Initialize(skill, player, direction);

        player.GetTree().Root.AddChild(effect);
    }
}
```

#### Step 3: Create Effect

```csharp
// Scripts/Skills/Effects/ChainLightningEffect.cs
public partial class ChainLightningEffect : SkillEffect
{
    private float _damage;
    private int _maxChains;
    private float _chainRange;
    private List<Enemy> _hitEnemies = new();

    public override void Initialize(Skill sourceSkill, Player caster, Vector2 direction)
    {
        base.Initialize(sourceSkill, caster, direction);

        var skill = sourceSkill as ChainSkill;
        _damage = skill.Damage;
        _maxChains = skill.MaxChains;
        _chainRange = skill.ChainRange;

        ApplyMasteryBonuses();
        ChainToNearestEnemy(caster.GlobalPosition);
    }

    private void ChainToNearestEnemy(Vector2 position)
    {
        // Find nearest unhit enemy within range
        // Deal damage, track kill
        // Recursively chain if chains remaining
        // QueueFree() when done
    }

    private void ApplyMasteryBonuses()
    {
        switch (_sourceSkill.CurrentTier)
        {
            case SkillMasteryTier.Silver:
                _damage *= 1.5f;
                break;
            case SkillMasteryTier.Gold:
                _maxChains += 1;
                _damage *= 2.0f;
                break;
            case SkillMasteryTier.Diamond:
                _maxChains += 2;
                _damage *= 3.0f;
                _chainRange *= 1.5f;
                break;
        }
    }
}
```

#### Step 4: Register in Factory (only if new type)

```csharp
// In Scripts/Skills/Base/Skill.cs
private ISkillExecutor CreateExecutor()
{
    return this switch
    {
        Data.ProjectileSkill => new ProjectileSkillExecutor(),
        Data.InstantAOESkill => new InstantAOESkillExecutor(),
        Data.ChainSkill => new ChainSkillExecutor(), // ADD THIS
        // ... other types
        _ => null
    };
}
```

#### Step 5: Create Effect Scene

1. Create `Scenes/SkillEffects/chain_lightning.tscn`
2. Root node: Node2D (matches SkillEffect base class)
3. Attach `ChainLightningEffect.cs` script
4. Add visual children (Line2D for lightning, particles, etc.)

#### Step 6: Create Resource

1. In Godot: Right-click `Resources/Skills/Mage/` → New Resource
2. Choose type: `ChainSkill`
3. Fill fields:
   - SkillName: "Chain Lightning"
   - AllowedClass: Mage
   - Slot: Primary
   - Cooldown: 5.0
   - Damage: 30
   - MaxChains: 3
   - ChainRange: 200
   - SkillEffectScene: [Drag chain_lightning.tscn]
4. Save as `ChainLightning.tres`

**Done!** The skill is now usable.

---

## 🎨 Key Design Decisions

### ✅ Why Type-Based Executors?

**Old Problem (Name-Based):**
```csharp
// 50 skills = 50 executor files!
"Fireball" => new Fireball(),
"Iceball" => new Iceball(),
"Lightning" => new Lightning(),
// ... doesn't scale
```

**New Solution (Type-Based):**
```csharp
// 50 skills = 7 executor files!
Data.ProjectileSkill => new ProjectileSkillExecutor(),
// Handles Fireball, Iceball, Lightning, etc.
```

**Benefits:**
- Create new skills with zero code (just .tres files)
- Executors are simple and testable
- Scales to 100+ skills effortlessly

### ✅ Why Two-Tier Effect Hierarchy?

**Problem:** Godot scenes have strict type requirements
- `whirlwind_effect.tscn` has Node2D root (doesn't need collision)
- `fireball_projectile.tscn` has Area2D root (needs collision)

**Solution:**
- **SkillEffect** (Node2D) → For instant/buff/teleport effects
- **CollisionSkillEffect** (Area2D) → For projectile/melee effects
- Both provide identical `Initialize()` and `OnEnemyKilled()` APIs

### ✅ Why Standardized Initialize()?

**Old Problem:**
```csharp
fireball.Initialize(dir, pos, dmg, expDmg, radius);    // 5 params
whirlwind.Initialize(pos, duration);                   // 2 params
// Every effect has different signature!
```

**New Solution:**
```csharp
effect.Initialize(skill, player, direction);  // Always 3 params
// Effect reads what it needs from skill via casting
```

**Benefits:**
- Executors don't know skill-specific details
- Effects get full skill data for flexibility
- Easy to add upgrade bonuses via `player.GetUpgradeValue()`

---

## 🧪 Testing a Skill

1. Set player class in `Player.tscn` (CurrentClass = Mage)
2. Equip skill in `SkillManager` (PrimarySkill = Fireball.tres)
3. Run game (F5)
4. Press Q to activate
5. Check Output panel for debug prints
6. Kill enemies to see mastery tracking

---

## 🔮 Skill Variants (Already Supported!)

**The Power of Type-Based Design:**

Create multiple skill variants with **zero additional code**:

```
Fireball.tres     (ProjectileSkill: 40 damage, orange sprite)
Iceball.tres      (ProjectileSkill: 30 damage, blue sprite, larger radius)
PoisonBall.tres   (ProjectileSkill: 20 damage, green sprite, DOT effect)
```

All three use:
- Same executor: `ProjectileSkillExecutor`
- Same effect script: `FireballProjectile.cs` (or create variant)
- Different resources: `.tres` files with different values

**Want a skill that behaves differently?**
→ Create a new effect script (e.g., `IceballProjectile.cs` that slows enemies)
→ Assign it to `Iceball.tres` SkillEffectScene
→ Still uses same executor!

---

## 📝 Common Patterns

### Pattern 1: Instant Effect (Whirlwind)
```
Executor → Spawn effect → Effect damages immediately → Plays animation → Destroy
```

### Pattern 2: Projectile (Fireball)
```
Executor → Spawn projectile → Travels → Collision → Damage → Explode → Destroy
```

### Pattern 3: Buff (Battle Cry)
```
Executor → Spawn buff → Apply stat changes → Follow player → Remove buffs → Destroy
```

### Pattern 4: Combo (Dash Strike)
```
Executor → Spawn effect → Dash player → Hit during dash → Attack at end → Destroy
```

---

## 🐛 Troubleshooting

### Skill doesn't cast
- ✓ Check factory in `Skill.CreateExecutor()` has the skill type
- ✓ Verify `AllowedClass` matches player class
- ✓ Ensure `SkillEffectScene` is assigned in .tres
- ✓ Check cooldown isn't active

### Effect doesn't spawn
- ✓ Verify executor calls `skill.SkillEffectScene.Instantiate<T>()`
- ✓ Check scene has script attached
- ✓ Ensure correct base class (SkillEffect vs CollisionSkillEffect)
- ✓ Verify `AddChild()` or `GetTree().Root.AddChild()`

### InvalidCastException
- ✓ Executor instantiates as `CollisionSkillEffect` for Area2D scenes
- ✓ Executor instantiates as `SkillEffect` for Node2D scenes
- ✓ Scene root type matches script base class

### Mastery not tracking
- ✓ Effect calls `OnEnemyKilled(enemy)` when enemy dies
- ✓ Check `healthBefore > 0 && enemy.Health <= 0` condition
- ✓ Verify enemy has Health property

### Damage not working
- ✓ Check `Initialize()` reads skill values correctly
- ✓ Verify enemies are in "enemies" group
- ✓ Check collision layers/masks on Area2D
- ✓ Ensure `ApplyMasteryBonuses()` is called

---

## 📚 Key Files Reference

| File | Purpose |
|------|---------|
| `Skill.cs` | Base class, factory, mastery tracking |
| `ISkillExecutor.cs` | Interface all executors implement |
| `SkillEffect.cs` | Base class for Node2D effects |
| `CollisionSkillEffect.cs` | Base class for Area2D effects |
| `{Type}Skill.cs` | Data definition for skill type |
| `{Type}SkillExecutor.cs` | Spawning logic for skill type |
| `{Name}Effect.cs` | Specific effect behavior |
| `SkillManager.cs` | Handles input, cooldowns, execution |

---

## 🚀 Architecture Benefits

### For Development:
- **Add skills in minutes** (just resources, no code)
- **Executors are 20 lines** (minimal complexity)
- **Effects are isolated** (easy to debug)
- **Type-safe data** (compile-time validation)

### For Balancing:
- **Edit .tres files** (no code changes needed)
- **Mastery tiers configurable** (per-skill scaling)
- **Test variants quickly** (duplicate .tres, tweak values)

### For Scalability:
- **7 executors → 100+ skills** (proven pattern)
- **Reusable effects** (Ice Bolt uses Fireball effect with different sprite)
- **Data-driven** (import from CSV/JSON if needed)

---

That's the refactored skill system! The type-based architecture is designed to scale from 6 skills to 100+ while keeping code organized, testable, and maintainable. 🎯
