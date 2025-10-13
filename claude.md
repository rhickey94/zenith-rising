# Tower Ascension - Project Documentation

## Project Overview
A bullet hell roguelite with idle mechanics. Players fight through tower floors in active combat while idle systems refine materials between sessions.

## Core Design Philosophy
- **Active play = power gains** (character levels, gear, skill mastery)
- **Idle time = efficiency gains** (material refinement, gold generation)
- **Idle enhances, never replaces** active gameplay
- Every run contributes to permanent progression (no wasted time)

## Current Status: Be Honest
**Phase:** Early Prototype (~40% to MVP)

**Actually Working:**
- ✅ Player movement (WASD) and rotation
- ✅ Enemy AI (chase player, contact damage)
- ✅ Basic health/damage system
- ✅ XP shards drop and level-up flow
- ✅ Level-up panel shows 3 upgrade choices
- ✅ Component architecture (StatsManager, SkillManager, UpgradeManager)
- ✅ Q/E/R skill input handling
- ✅ Type-based skill executor pattern
- ✅ 3 working skills: Whirlwind, Fireball, Basic melee/ranged attacks
- ✅ Skill mastery tracking (kills recorded per skill)
- ✅ **Stats consolidated in StatsManager** (Speed/FireRate/MeleeRate calculated from base + upgrades)
- ✅ **Upgrade stacking fixed** (recalculates from base values, no more broken math)
- ✅ **All 8 upgrades work correctly** (MovementSpeed, AttackSpeed, MaxHealth, DamagePercent, PickupRadius, Pierce, Crit, Regen)
- ✅ **CombatSystem.cs** (centralized damage calculation with crit)
- ✅ **3 enemy types** (Basic melee, FastMelee, SlowRanged with kiting + projectiles)
- ✅ **Enemy inheritance system** (virtual Attack/Movement/TryAttack methods)
- ✅ **Enemy scaling system** (Initialize with health/damage multipliers)
- ✅ **Core combat validated as fun** (hypothesis proven)

**In Progress:**
- ⏳ Wave/floor tracking system
- ⏳ Spawn rate escalation (wave-based)
- ⏳ Boss spawning at 5:00 mark
- ⏳ Floor transition UI

**Not Started:**
- Character stat system (STR/VIT/AGI/RES/FOR)
- Gear/equipment system
- Materials or idle systems
- Save/load system
- Hub system

---

## Development Plan: Sequential Risk Reduction

Build in phases where each phase **proves a hypothesis** before investing in the next.

### **Phase 1: Prove Combat is Fun (Week 1-2)**
**Hypothesis:** "Fighting waves with skills and upgrades is engaging for 25+ minutes"

**Tasks:**
1. ✅ Fix all 8 existing upgrades to actually work
2. ✅ Add 2 enemy types:
   - FastMelee (speed 300, HP 50, melee)
   - SlowRanged (speed 100, HP 150, shoots projectiles)
3. ⏳ Implement wave/floor system:
   - Each floor = 5 minutes (300 seconds)
   - 10 waves per floor (30 seconds each)
   - Spawn rate increases per wave (2.0s → 0.8s)
   - Enemy HP/damage scales per wave (+10% HP, +5% damage per wave)
   - Boss spawns at 5:00 mark (5x HP, 2x damage)
4. ⏳ Add floor transition UI:
   - **Continue:** Advance to next floor (+50% base stats)
   - **End Run:** Return to hub with rewards
   - **Save & Exit:** (Phase 2) Serialize run state, resume later
5. 5 floors total (Floor 5 boss = victory condition)

**Run Structure:**
- Floor 1-5: 5 min each = 25 minute complete run
- Each floor escalates: 10 waves → Boss
- Die or beat Floor 5 → Return to hub

**Success Criteria:** You can play 5 floors (~25 min) and it feels engaging. Boss fights feel earned. If boring, nothing else matters.

**Cut for now:**
- New skill types beyond the 3 working ones
- Skill mastery tiers (just track kills)
- Materials/idle systems
- Save system
- Multiple dungeons (Phase 5)

---

### **Phase 2: Prove Progression Hook (Week 3-4)**
**Hypothesis:** "Players want to push further because permanent progression feels good"

**Tasks:**
1. **Implement character stats system:**
   - 5 stats: Strength, Vitality, Agility, Resilience, Fortune
   - Simplified scaling (each stat does ONE thing)
   - Gain 1 stat point per character level
   - Distribute freely in stat screen

2. **Save/load system:**
   - Save character level + stat allocation to JSON
   - Save highest floor reached
   - Persist between runs

3. **Basic gear drops:**
   - 3 slots: Weapon, Armor, Accessory
   - 3 rarities: Common, Rare, Epic
   - Just flat stats: +Damage, +HP, +AttackSpeed
   - Drops from bosses (100%) and elites (10%)

4. **5 distinct floors:**
   - Different enemy mix per floor
   - Visual distinction (color palette swap)
   - Boss at end of each floor

**Success Criteria:** After dying, you immediately want to start a new run to test your stat allocation and new gear.

**Cut for now:**
- Gear enhancement (Power Levels, Mods)
- Materials system
- Idle systems
- Complex skill mastery tiers

---

### **Phase 3: Prove Idle Hook (Week 5-6)**
**Hypothesis:** "Players return after being away because idle progress matters"

**Tasks:**
1. **Workshop (MVP version):**
   - ONE material type: Energy Cores
   - Drop from all enemies
   - Refine Raw Cores → Refined Cores (30 min real-time)
   - Used to upgrade gear Power Level (+10% stats per level, max 5)
   - Uses DateTime for offline progression

2. **Treasury (MVP version):**
   - Generates gold based on highest floor cleared
   - Formula: (Floor × 10) gold/hour
   - Caps at 8 hours
   - Gold used for: Respec stats (100g × level)

3. **Simple material drop system:**
   - Cores drop as collectible items (like XP shards)
   - Show count in HUD

**Success Criteria:** Close game, return 2 hours later, collect refined cores, upgrade gear, feel good about passive progress.

**Cut forever:**
- Research Lab (passive power contradicts "active = power")
- Training Ground (QoL tokens are weird currency)
- Multiple material types (start with one)
- Complex gear modifications

---

### **Phase 4: Add Depth (Week 7-8)**

**Tasks:**
1. **Skill mastery (simplified):**
   - 3 tiers: Bronze/Silver/Gold (not 4)
   - Thresholds: 50/200/500 kills (not 100/500/2000/10000)
   - Each tier: +50% effectiveness
   - Bronze (default), Silver (+50%), Gold (+100% + special bonus)

2. **Gear modifications (add 2nd material):**
   - Add Modification Chips (drop from bosses)
   - Add ONE special property per item
   - 10-15 mods: Pierce, Chain, Lifesteal, Crit Damage
   - No synergies layer (too complex)

3. **Expand skill pool:**
   - Add 10 more skills using existing types
   - Don't implement new executors yet
   - Focus on variety and feel

**Success Criteria:** Players experiment with builds (stat allocation + skill choices + gear mods).

---

### **Phase 5: Endgame (Week 9-10)**

**Tasks:**
1. **Multiple Dungeons:**
   - Dungeon 1: Tower Floors (completed in Phase 1)
   - Dungeon 2: Corrupted Sanctum (melee-heavy, tests tank builds)
   - Dungeon 3: Void Rifts (bullet hell chaos, tests dodge builds)
   - Dungeon 4: Endless Ascent (infinite scaling for leaderboards)
   - Each dungeon: 5 floors, unique enemy types, unique boss
   - Dungeons unlock based on highest floor reached

2. **Dungeon-specific rewards:**
   - Different material drop rates per dungeon
   - Targeted farming (need weapon mods? Run Sanctum)
   - Higher difficulty = better drop rates

3. **Ascension system (simple):**
   - Gain ascension XP from all kills/clears
   - Spend points on +% stat bonuses
   - 3 trees: Power, Survival, Utility

4. **Weekly challenges:**
   - Modifiers on dungeons for bonus rewards
   - Simple leaderboard per dungeon

**Why Dungeons > Infinite Floors:**
- ✅ Variety (unique themes, enemy types, mechanics)
- ✅ Replayability (choose based on build/goals)
- ✅ Content drops (new update = new dungeon)
- ✅ Avoids infinite scaling math boredom

**Cut forever:**
- Corruption system (redundant with multiple dungeons)
- Gear synergies (mods are enough)

---

## Simplified Systems

### **Stats System (Revised)**

**Current design problem:** Each stat does 2 things (confusing)

**Better design:**
```
Strength: +3% ALL damage per point
Vitality: +25 HP per point
Agility: +2% attack speed per point
Resilience: +1% damage reduction per point (cap 50%)
Fortune: +2% crit chance per point (cap 50%)
```

Each stat does ONE thing. Clear choices.

**Starting stats:** All classes start at 0. Distribute 15 points at character creation.

**Stat growth:** +1 point per character level (cap 100).

---

### **Materials System (Revised)**

**Current design problem:** 3 materials × 2 states = 6 items, complex purposes

**Better design:**

**Phase 3 (MVP):**
- ONE material: Energy Cores
- Drops from all enemies
- Refine in Workshop (30 min)
- Used for: Gear Power Level only

**Phase 4 (Depth):**
- Add SECOND material: Modification Chips
- Drop from bosses
- Used for: Gear mods

**Never add third material.** Two is enough.

---

### **Skill Mastery (Revised)**

**Current design problem:** 4 tiers with insane thresholds (10,000 kills for Diamond)

**Better design:**
```
3 tiers: Bronze/Silver/Gold
Thresholds: 50/200/500 kills

Bronze (0): Base skill
Silver (50): +50% effectiveness (achievable in one 15-min session)
Gold (200): +100% effectiveness + special bonus
```

Faster feedback loop. More achievable goals.

---

### **Gear System (Revised)**

**Current design problem:** 4 layers (Base + Power + Mod + Synergy) = too complex

**Better design:**
```
Phase 2: Base stats only
Phase 3: Add Power Level (0-5)
Phase 4: Add Modifications (one per item)
Never add: Synergies
```

Each layer = ~20 hours dev time. Ship with 2 layers, expand if players demand more.

---

### **Dungeon System (Phase 5 Design)**

**Core Concept:** Multiple 5-floor dungeons instead of infinite scaling

**Dungeon 1: Tower Floors (Phase 1 MVP)**
- Enemy mix: Balanced (melee + ranged)
- Theme: Classic tower ascent
- Difficulty: Beginner-friendly
- Rewards: Balanced material drops

**Future Dungeons (Phase 5):**

**Dungeon 2: Corrupted Sanctum**
- Enemy mix: 80% melee, 20% ranged
- Theme: Close-quarters combat
- Tests: Tank builds, lifesteal, damage reduction
- Rewards: Armor/defensive gear emphasis

**Dungeon 3: Void Rifts**
- Enemy mix: 80% ranged, 20% melee
- Theme: Bullet hell chaos
- Tests: Dodge builds, movement speed, projectile clear
- Rewards: Weapon/offensive gear emphasis

**Dungeon 4: Endless Ascent**
- Infinite floors (no 5-floor cap)
- Scaling: +20% stats per floor
- Purpose: Leaderboards, prestige
- Rewards: Bragging rights

**Unlock System:**
- Dungeon 1: Always available
- Dungeon 2: Unlock by clearing Dungeon 1 Floor 3
- Dungeon 3: Unlock by clearing Dungeon 1 Floor 5
- Dungeon 4: Unlock by clearing both 2 and 3

**Why This Works:**
- Content stays fresh (not just bigger numbers)
- Build diversity matters (some builds excel in certain dungeons)
- Farming loops (target specific dungeons for specific gear)
- Expandable (new dungeon = major content update)

---

## Architecture Improvements

### **Problem 1: Skill.cs mixes data and behavior**

**Current:**
```csharp
public partial class Skill : Resource  // Data
{
    public void Execute(Player player)  // Behavior - WRONG
    {
        _executor ??= CreateExecutor();
        _executor?.ExecuteSkill(player, this);
    }
}
```

**Fix:** Create SkillSystem manager (Autoload singleton)
```csharp
public partial class SkillSystem : Node
{
    private Dictionary<Type, ISkillExecutor> _executors = new();

    public void ExecuteSkill(Skill skill, Player player)
    {
        var executor = GetOrCreateExecutor(skill);
        executor.ExecuteSkill(player, skill);
    }
}
```

Resources stay pure data. System handles execution.

---

### **Problem 2: No central game state**

**Current:** Player emits signals for floor/resources but no one owns this state

**Fix:** Create GameState manager (Autoload singleton)
```csharp
public partial class GameState : Node
{
    // Run state
    public int CurrentFloor { get; private set; } = 1;
    public int CurrentWave { get; private set; } = 1;

    // Resources
    public int Gold { get; private set; } = 0;
    public int EnergyCores { get; private set; } = 0;

    [Signal] public delegate void FloorChangedEventHandler(int floor);
    [Signal] public delegate void ResourcesChangedEventHandler(int gold, int cores);

    public void AdvanceFloor() { ... }
    public void AddGold(int amount) { ... }
}
```

Now everything references one source of truth.

---

### **Problem 3: UpgradeManager hardcodes upgrades**

**Current:** Hardcoded List in UpgradeManager.cs

**Fix:** Load from Resources/Upgrades/ folder
```csharp
public override void _Ready()
{
    _availableUpgrades = LoadUpgradesFromResources("res://Resources/Upgrades/");
}

private List<Upgrade> LoadUpgradesFromResources(string path)
{
    var upgrades = new List<Upgrade>();
    var dir = DirAccess.Open(path);

    foreach (var file in dir.GetFiles())
    {
        if (file.EndsWith(".tres"))
        {
            upgrades.Add(ResourceLoader.Load<Upgrade>($"{path}{file}"));
        }
    }
    return upgrades;
}
```

Now adding upgrades = create .tres file. No code changes.

---

## Project Structure
```
SpaceTower/
├── Scenes/
│   ├── Core/
│   │   └── game.tscn
│   ├── Player/
│   │   └── player.tscn
│   ├── Enemies/
│   │   ├── enemy.tscn
│   │   ├── fast_melee_enemy.tscn
│   │   └── slow_ranged_enemy.tscn
│   ├── Items/
│   │   ├── experience_shard.tscn
│   │   └── energy_core.tscn
│   ├── SkillEffects/
│   │   ├── fireball_projectile.tscn
│   │   ├── whirlwind_effect.tscn
│   │   └── melee_attack.tscn
│   └── UI/
│       ├── hud.tscn
│       └── level_up_panel.tscn
├── Scripts/
│   ├── Core/
│   │   ├── Game.cs
│   │   └── GameState.cs (Autoload)
│   ├── PlayerScripts/
│   │   ├── Player.cs
│   │   └── Components/
│   │       ├── StatsManager.cs
│   │       ├── SkillManager.cs
│   │       └── UpgradeManager.cs
│   ├── Skills/
│   │   ├── Base/
│   │   │   ├── Skill.cs
│   │   │   ├── ISkillExecutor.cs
│   │   │   └── SkillSystem.cs (Autoload)
│   │   ├── Data/
│   │   │   ├── ProjectileSkill.cs
│   │   │   ├── InstantAOESkill.cs
│   │   │   └── MeleeAttackSkill.cs
│   │   ├── Executors/
│   │   │   ├── ProjectileSkillExecutor.cs
│   │   │   ├── InstantAOESkillExecutor.cs
│   │   │   └── MeleeSkillExecutor.cs
│   │   └── Effects/
│   │       ├── SkillEffect.cs
│   │       ├── CollisionSkillEffect.cs
│   │       ├── FireballProjectile.cs
│   │       └── WhirlwindEffect.cs
│   ├── Enemies/
│   │   └── Base/
│   │       └── Enemy.cs
│   └── UI/
│       ├── Hud.cs
│       └── LevelUpPanel.cs
├── Resources/
│   ├── Skills/
│   │   ├── Mage/
│   │   └── Warrior/
│   └── Upgrades/
│       ├── DamageBoost.tres
│       ├── AttackSpeed.tres
│       └── ...
└── Assets/
    ├── Sprites/
    ├── Audio/
    └── Fonts/
```

---

## Tech Stack
- **Engine:** Godot 4.3+ with .NET support
- **Language:** C# (prefer C# over GDScript)
- **Platform:** PC (Steam) for MVP
- **Art Style:** 2D top-down with placeholder sprites
- **Version Control:** Git + GitHub

---

## Combat System (Current)
- **Left Click:** Basic Attack (auto-aim nearest enemy)
- **Right Click:** Special Attack (manual aim, cooldown)
- **Q/E/R:** Active skills (pre-run loadout)
- **WASD:** Movement with rotation toward direction

---

## Skill System Architecture

**3-Layer System:**
1. **Data (Resources):** Define skill stats in .tres files
2. **Executors (Type-based):** Spawn effects based on skill type
3. **Effects (Runtime):** Handle behavior, damage, movement

**Skill Types:**
| Type | Example | Effect Base |
|------|---------|-------------|
| ProjectileSkill | Fireball | CollisionSkillEffect (Area2D) |
| InstantAOESkill | Whirlwind | SkillEffect (Node2D) |
| MeleeAttackSkill | Slash | CollisionSkillEffect (Area2D) |

**Factory Pattern:**
```csharp
private ISkillExecutor CreateExecutor()
{
    return this switch
    {
        Data.ProjectileSkill => new ProjectileSkillExecutor(),
        Data.InstantAOESkill => new InstantAOESkillExecutor(),
        Data.MeleeAttackSkill => new MeleeSkillExecutor(),
        _ => null
    };
}
```

**Effect Pattern:**
```csharp
public override void Initialize(Skill sourceSkill, Player caster, Vector2 direction)
{
    base.Initialize(sourceSkill, caster, direction);
    // Extract skill-specific data
    // Apply mastery bonuses
}

private void OnBodyEntered(Node2D body)
{
    if (body is Enemy enemy)
    {
        float healthBefore = enemy.Health;
        enemy.TakeDamage(_damage);

        // Track kill for mastery
        if (healthBefore > 0 && enemy.Health <= 0)
        {
            OnEnemyKilled(enemy);
        }
    }
}
```

---

## Godot Patterns

### Physics & Deferred Calls
Never modify physics tree during collision processing:
- ❌ `AddChild()` in collision callback → ERROR
- ✅ `CallDeferred(MethodName.AddChild, node)` → Works

### Scene Instancing
```csharp
[Export] public PackedScene EnemyScene;

var enemy = EnemyScene.Instantiate<Enemy>();
enemy.GlobalPosition = spawnPos;
AddChild(enemy);
```

### Singleton Pattern (Autoload)
```csharp
public partial class GameState : Node
{
    public static GameState Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree();
            return;
        }
        Instance = this;
    }
}
```

Add to Project Settings → Autoload.

---

## Current Session Progress (Session 3)

**Completed (Session 2):**
- ✅ Stats consolidation refactor (Speed/FireRate/MeleeRate now in StatsManager)
- ✅ Fixed upgrade stacking math (recalculates from base + all upgrades)
- ✅ Fixed MaxHealth level-up bug (now persists correctly)
- ✅ Architecture discussion: Decided to keep components for Phase 1, add singletons in Phase 2
- ✅ Core combat validated as fun (no need to test hypothesis)

**Completed (Session 3):**
- ✅ Added cached upgrade stats to StatsManager (DamageMultiplier, CritChance, PickupRadius, etc.)
- ✅ Created CombatSystem.cs for centralized damage calculation
- ✅ Fixed all 5 broken upgrades (DamagePercent, CritChance, PickupRadius, Pierce, HealthRegen)
- ✅ Implemented StatModifiers struct to avoid parameter explosion
- ✅ Updated all skill effects to use CombatSystem.CalculateDamage
- ✅ Added FastMeleeEnemy (speed variant, stat-based)
- ✅ Added SlowRangedEnemy (kiting behavior + projectile attacks)
- ✅ Created EnemyProjectile.cs for enemy ranged attacks
- ✅ Implemented virtual Attack/Movement/TryAttack in Enemy.cs
- ✅ Added Enemy.Initialize(healthMult, damageMult) for scaling
- ✅ Fixed enemy sprite color system (removed Sprite2D modulate conflict)
- ✅ Design decisions: 5-floor dungeons, 10 waves per floor, multiple dungeons in Phase 5

**Current Tasks (Wave/Floor System):**
1. ⏳ Add wave and floor tracking to Game.cs
2. Implement 30-second wave timer system
3. Add spawn rate escalation per wave (2.0s → 0.8s)
4. Wire up Enemy.Initialize() with wave/floor multipliers
5. Display current floor/wave in HUD
6. Create basic boss enemy (tanky variant)
7. Add boss spawn at 5:00 mark
8. Implement floor transition choice UI (Continue/End Run)

**Next Session (Phase 1 Completion):**
- Complete wave/floor system
- Playtest full 5-floor run (~25 min)
- Balance spawn rates and scaling
- Move to Phase 2: Character stats + gear drops

---

## Known Issues
- No wave/floor tracking system yet (in progress)
- No boss enemy type
- No floor transition UI
- No save system
- Skill.Execute() should be in SkillSystem, not Resource (deferred to Phase 2)
- No hub system
- No character stat system (STR/VIT/AGI/RES/FOR)

---

## Development Principles

1. **Be honest about progress** - No aspirational completion percentages
2. **Prove before building** - Each phase validates a hypothesis
3. **Simplify ruthlessly** - Cut features that don't directly support the core loop
4. **Playtest constantly** - Record yourself playing, watch it back
5. **Ship small** - 2-week MVP beats 6-month vaporware
6. **Don't refactor working architecture until you hit pain points** - Current components work fine for single-class Phase 1

## Architecture Decisions (Session 2)

### **Components vs Singletons**

**Decision:** Hybrid approach
- **Keep as Player components:** StatsManager, SkillManager, UpgradeManager (run-specific state)
- **Add as singletons in Phase 2:** PlayerProgression (persistent), GameState (run state), CombatSystem (stateless utility)

**Reasoning:**
- Components work well for single-class Phase 1
- Class switching (Warrior/Mage/Ranger) not needed until Phase 2
- Don't refactor working code before stress-testing it
- Singletons make sense for: persistence (PlayerProgression), global state (GameState), and stateless utilities (CombatSystem)

### **Upgrade System Architecture**

**Problem identified:** No centralized damage calculation leads to duplicated upgrade logic across all skills

**Solution:** CombatSystem static class
```csharp
public static class CombatSystem
{
    public static float CalculateDamage(float baseDamage, Player attacker)
    {
        var stats = attacker.GetNode<StatsManager>("StatsManager");
        float damage = baseDamage * stats.DamageMultiplier;

        if (GD.Randf() < stats.CritChance)
            damage *= 2.0f;

        return damage;
    }
}
```

**Benefits:**
- One place for all damage logic
- Easy to extend in Phase 2 (armor, resistances)
- Skills become simpler: `enemy.TakeDamage(CombatSystem.CalculateDamage(_damage, _caster))`

### **Cached Stats Pattern**

**Upgrade values cached in StatsManager** instead of dictionary lookups every frame:
```csharp
// Recalculated when upgrades change
public float DamageMultiplier { get; private set; } = 1.0f;
public float CritChance { get; private set; } = 0f;
public float PickupRadius { get; private set; } = 80f;
```

**Why:** Performance (no dictionary lookups) + StatsManager becomes the "character sheet"

---

## For Claude Code

When the user says "implement X":
1. Check if X is in current phase - if not, remind them of sequential plan
2. Provide complete, working code
3. Test that it actually works (run the game if possible)
4. Update this doc with realistic completion status

When the user asks "what's next":
1. Show current phase progress
2. List remaining tasks in current phase
3. Don't jump ahead to future phases

**Core rule:** Don't let the user (or yourself) add features outside the current phase. The sequential plan exists to prevent scope creep.
