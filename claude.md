# Tower Ascension - Project Documentation

## Project Overview
A sci-fi roguelite action-RPG with idle/incremental mechanics built in Godot 4.x with C#. Players fight through tower floors in active combat sessions while idle systems process materials and generate resources between play sessions.

## Core Design Philosophy
- **Active play = power gains** (levels, gear, combat strength, skill mastery)
- **Idle time = efficiency gains** (material refinement, resource generation, unlock systems)
- **Idle enhances, never replaces** active gameplay
- Players progress meaningfully in both 15-minute sessions and 2-hour grinds
- Every run contributes to permanent progression (no wasted time)

## Current Status
**Phase:** Early Development (MVP - ~30% complete)
**Completed:**
- ✅ Player movement and shooting mechanics
- ✅ Enemy AI (chase and attack)
- ✅ Health/damage system
- ✅ Combat HUD with health, XP, resources display
- ✅ Enemy spawning system
- ✅ XP shard drops and collection
- ✅ Level-up system with upgrade choices
- ✅ 8 basic upgrade types with functional effects

**In Progress:**
- Enemy variety (need more types)
- Upgrade pool expansion (have 8, want 20+)

**Not Started:**
- Active skill system (Q/E/R abilities)
- Upgrade system refactor (currently hardcoded, needs UpgradePoolManager)
- Skill mastery system
- Materials system
- Workshop/Treasury (idle systems)
- Multiple floors (have 1, need 5+)
- Gear/equipment system
- Save system
- Polish (sound, particles, screen shake)
- Endgame infinite floors

## MVP Requirements

### Core Bare Bones: What We Need for MVP

**Combat & Movement Must-Haves:**
- Movement: WASD, fixed movement speed (upgradeable via stats/upgrades)
- Auto-aim basic attack: Targets nearest enemy within range
- Manual aim special attack: Aims toward mouse/right stick direction
- Dash: Quick movement burst with i-frames
- Hit feedback: Screen shake, enemy knockback, damage numbers
- Death state: Character dies, run ends, return to base with some loot lost

**Enemy Basics:**
- 3-4 enemy types per floor minimum
- Simple AI: Chase player, attack on contact or at range
- Clear visual telegraphs for attacks
- Spawn in waves (increases over time in each floor)

**Floor Structure:**
- Duration: 8-10 minutes to boss spawn OR kill count threshold (whichever comes first)
- Boss Fight: Unique boss per floor with distinct mechanics
- Extract Decision: After boss, choose to extract with all loot (safe) or continue to next floor (risk = better rewards but lose some loot if you die)
- Death Penalty: Lose 50% of materials collected in current run, keep all XP/character levels
- 5 floors minimum for MVP
- Each floor: more enemy health, more damage, more spawns
- Visual distinction per floor (color palette, tileset, music)

**Experience & Leveling:**
- In-Run XP: Enemies drop XP shards (visible pickups, like Vampire Survivors)
- XP bar fills, level up, choose 1 of 3 random upgrades
- Max level per run: 20 (prevents infinite scaling)
- Level resets to 1 each new run
- Character Level (Permanent): Separate from in-run leveling
- Gain character XP from all activities (not just boss kills)
- Each character level: +3 stat points to distribute
- Character level cap: 200 (extended from 50 for longer progression)

**Save System:**
- Persists: Character level and stat distribution, all collected gear and materials (in stash), Workshop queue and timers (continues offline), Treasury accumulation and timer, highest floor reached, skill unlocks and mastery progress, Ascension XP and levels
- Resets: In-run level (back to 1), in-run upgrades (chosen fresh each run), floor position (always restart from Floor 1 or highest unlocked)

### Core Loop Validation Checklist
Before launch, players should be able to:
- [ ] Feel powerful growth within a single 10-minute run
- [ ] Return after 8 hours and see meaningful idle progress
- [ ] Make a meaningful choice about their skill loadout
- [ ] Feel excited when rare gear drops
- [ ] Understand what each material is used for
- [ ] Know exactly what they're working toward next
- [ ] Die and not feel like they wasted their time (kept XP and some loot)
- [ ] See clear skill mastery progression from active play
- [ ] Have aspirational content to work toward (infinite floors)

### What We're Explicitly NOT Building Yet
**Save for Post-MVP:**
- More than 3 classes
- PvP or multiplayer
- Seasonal content
- Cosmetics system (save for retention updates)
- Advanced enchantments beyond stat boosts

### The MVP Test: "Can You Play It for 10 Hours?"
**Hour 1:** Tutorial, first run, unlock Workshop
**Hour 2-3:** Learning enemy patterns, upgrading first gear pieces
**Hour 4-5:** Experimenting with skill loadouts, reaching Floor 3, seeing skill mastery grow
**Hour 6-7:** Farming for specific gear drops, enhancing to +5, mastering first skill
**Hour 8-9:** Pushing to Floor 5 boss, min-maxing build
**Hour 10:** Beat Floor 5, unlock infinite floors, excited for endgame

**If players are still engaged at Hour 10:** The core loop works. Ship it and expand.
**If players are bored by Hour 5:** The loop is broken. Fix before adding features.

## Tech Stack
- **Engine:** Godot 4.3+ with .NET support
- **Language:** C# (prefer C# over GDScript)
- **Platform:** PC (Steam) for MVP
- **Theme:** Metallic sci-fi (Mass Effect/Star Wars inspired, NOT neon Tron)
- **Art Style:** 2D isometric with placeholder squares (real sprites later)
- **Version Control:** Git + GitHub

## Project Structure
```
TowerAscension/
├── Scenes/
│   ├── Game.tscn (main game scene)
│   ├── Player.tscn
│   ├── Enemy.tscn
│   ├── Projectile.tscn
│   ├── XPShard.tscn
│   ├── HUD.tscn
│   └── LevelUpPanel.tscn
├── Scripts/
│   ├── Player.cs
│   ├── Enemy.cs
│   ├── Projectile.cs
│   ├── XPShard.cs
│   ├── Hud.cs
│   ├── LevelUpPanel.cs
│   ├── Game.cs
│   └── UpgradeResource.cs
└── Assets/
    ├── Sprites/
    ├── Audio/
    └── Fonts/
```

## Core Systems

### Player Stats (5 Core Stats)
All scaling derives from these base stats:
- **Strength:** +2% physical damage, +10 max HP per point
- **Dexterity:** +1% attack speed, +0.5% crit chance per point
- **Intelligence:** +2% magical damage, +2% skill power per point
- **Vitality:** +20 max HP, +0.5 HP/sec regen per point
- **Fortune:** +3% crit damage, +1% rare drop chance per point

**Base Stats by Class:**
- **Warrior:** 15 STR, 8 DEX, 5 INT, 12 VIT, 5 FOR (Total: 45)
- **Mage:** 5 STR, 8 DEX, 15 INT, 8 VIT, 9 FOR (Total: 45)
- **Ranger:** 8 STR, 15 DEX, 7 INT, 8 VIT, 7 FOR (Total: 45)

**Stat Growth:**
- Gain +3 stat points per character level (distribute freely)
- Ascension system provides percentage bonuses to stats
- Gear provides bonus stats

### Combat System
- **Left Click/Spacebar:** Auto-aim basic attack
- **Movement:** WASD with rotation toward movement direction
- **Camera:** Follows player (Camera2D child node)
- **Projectiles:** Spawn at player position, travel toward mouse
- **Collision:** CharacterBody2D for player/enemies, Area2D for projectiles

### Upgrade System (Current)
Uses Godot Resources (`UpgradeResource`) for serialization support.

**Available Upgrades:**
1. Damage Boost (+15% damage)
2. Attack Speed (+20% fire rate)
3. Swift Feet (+15% movement speed)
4. Vitality (+50 max health)
5. Magnet (+30 pickup radius)
6. Piercing Shots (+1 pierce)
7. Critical Hit (+10% crit chance)
8. Regeneration (+2 HP/sec)

**Level-Up Flow:**
1. XP bar fills → Player levels up
2. Game pauses (GetTree().Paused = true)
3. Show 3 random upgrades from pool (prioritizes equipped skill enhancements)
4. Player selects one
5. Upgrade applied immediately
6. Game resumes

### Skill System (Reworked)
**Core Concept:** Skills are mastered through use, not time-gated. Every skill use contributes to permanent progression.

**Core Combat Structure (Always Available):**
- **Left Click:** Basic Attack (auto-aims at nearest enemy)
  - Warrior: Sword swing (melee, cleave)
  - Mage: Magic bolt (ranged, single target)
  - Ranger: Bow shot (ranged, piercing)
  - Scales with stats automatically, no cooldown, no cost

- **Right Click:** Special Attack (manual aim, cooldown)
  - Warrior: Shield bash (3 sec cooldown, stuns)
  - Mage: Fireball (4 sec cooldown, AOE)
  - Ranger: Charged shot (5 sec cooldown, high damage)
  - Scales with stats + skill power, core identity move

- **Spacebar:** Dash/Dodge Roll
  - Universal movement ability
  - 2 second cooldown
  - Brief invulnerability frames
  - Mobility and survival tool

**Skill Loadout System (Pre-Run Customization):**
Before each run, equip 3 Active Skills from your unlocked pool:
1. **Q Key** - Offensive Skill
2. **E Key** - Utility/Defensive Skill
3. **R Key** - Ultimate Skill (long cooldown)

**Skill Mastery System (NEW):**
Each skill tracks its own usage and has mastery levels based on enemies killed with that skill:

```
Example: Whirlwind Mastery Progress
├── Bronze (0/100 kills): Base skill unlocked
├── Silver (0/500 kills): +50% damage, +2 sec duration
├── Gold (0/2000 kills): Pulls enemies, +100% damage
└── Diamond (0/10000 kills): Creates fire tornado, chains to nearby enemies

Current: 347/500 kills to Silver ⚔️
```

**How Skills Progress:**
- Every enemy killed with a skill grants mastery XP
- Mastery is permanent and never lost
- Visual feedback in HUD shows progress
- Skills visibly transform at mastery milestones
- In-run upgrades now specifically enhance YOUR equipped skills

**How You Unlock New Skills:**
- Start with 2 basic skills per slot unlocked
- Floor bosses drop skill unlock tokens (guaranteed)
- Research Lab unlocks skills by studying specific monsters
- Rare elite drops can contain skill scrolls

**In-Run Skill Synergies:**
Level-up choices now prioritize YOUR equipped skills:
- "Whirlwind Mastery" - Your Whirlwind gains +2 projectiles (only appears if you have Whirlwind)
- "Skill Combo" - Using Q→E→R in sequence triggers explosion
- "Cooldown Sync" - All skills refresh when you get 10 kills

**Skill Unlock Pace:**
- Early Game (Floors 1-3): 2 skills per slot unlocked (6 total choices)
- Mid Game (Floors 4-7): 4 skills per slot unlocked (12 total)
- Late Game (Floors 8+): 6+ skills per slot (18+ total)

### Enemy System
**Current Implementation:**
- Single enemy type (red square placeholder)
- Chases player using direct pathfinding
- Collision-based damage (hits player on contact)
- Drops 3 XP shards on death (scattered randomly)

**Enemy Stats:**
- Speed: 150-200
- MaxHealth: 100
- Damage: 10
- AttackCooldown: 1.0s

### XP & Progression
- Enemies drop 3 XP shards (scattered in 40px radius)
- Shards auto-move toward player when within 80px
- Each shard worth ~3 XP (10 XP total per enemy)
- Level requirements increase by 20% each level
- Leveling heals to full and grants +20 max HP

### Drop System (Planned)
**Drop Rates:**
- **Energy Cores:** Drop from all enemies (100% chance, 1-3 per enemy)
- **Modification Components:** Drop from elite enemies (20% chance) and bosses (100%)
- **Catalyst Fragments:** Drop from floor bosses only (100%, 1-2 per boss)
- **Gear:** Drop from elites (10% chance) and bosses (100%, higher rarity)
- **Gold:** Drop from all enemies (50% chance, 5-20 gold)
- **Skill Scrolls:** Rare drop from elites (2% chance)

**Visual Clarity:**
- Materials: Small colored particles (blue = Energy Cores, red = Modification Components, gold = Catalyst Fragments)
- Gear: Big glowing item on ground with rarity color (white/green/blue/purple/orange)
- Skill Scrolls: Glowing purple orbs with skill icon
- Auto-pickup radius: Small at start, upgradeable

## Planned Systems

### Material Economy
**Core Philosophy:** Items drop from combat (you never craft gear), but you enhance them using materials. Simple 3-material system with clear, distinct purposes.

**The 3 Materials:**
1. **Energy Cores** (Common)
   - **Dropped by:** All enemies (100% chance, 1-3 per enemy)
   - **Used for:** Power Level upgrades (+10% to all stats per level, max level 10)
   - **Idle Refinement:** Refines into Stabilized Cores (15 min)
     - Raw cores = RNG upgrade chance (60-90% success based on level)
     - Stabilized cores = Guaranteed successful upgrade
   - **Clear purpose:** Make your gear stronger through linear progression

2. **Modification Components** (Uncommon)
   - **Dropped by:** Elite enemies (20% chance) and bosses (100%)
   - **Used for:** Adding or replacing ONE special property per item
   - **Examples:** "Attacks pierce", "20% life steal", "+50% crit damage", "Chain lightning on hit"
   - **Idle Refinement:** Refines into specific mod types (30 min)
     - Raw components = Random mod from any category
     - Refined components = Choose category (Offensive/Defensive/Utility) for targeted mod
   - **Clear purpose:** Customize how your gear behaves

3. **Catalyst Fragments** (Rare)
   - **Dropped by:** Floor bosses only (100%, 1-2 per boss)
   - **Used for:** Adding build-defining synergy bonuses OR corrupting items for risk/reward
   - **Synergy Examples:** "+30% damage if DEX > 50", "+40% effect after using dash", "Double stats if health below 50%"
   - **Corruption:** One-time gamble for massive power with potential downsides
   - **Idle Refinement:** Refines into Perfect Catalysts (60 min)
     - Raw fragments = 50-100% effectiveness on synergies
     - Perfect catalysts = Guaranteed max roll synergies
   - **Clear purpose:** Tie gear into your specific build/playstyle

### Idle Systems

**Workshop (Material Refinement):**
- Queue up to 3 refinement jobs
- Continues processing offline using DateTime
- Refined materials used at Blacksmith for upgrades

**Treasury (Gold Generation):**
- Generates gold per hour based on highest floor cleared
- Formula: (Highest Floor × 10) gold/hour
- Caps at 8 hours (encourages daily check-ins)
- Gold used for: respec, consumables, salvaging, rushing

**Gold's Clear Purposes:**
1. **Respeccing (Player Agency):** Cost 100 gold × character level, completely reset stat points
2. **Skill Reset:** Cost 500 gold, reset all skill mastery to redistribute
3. **Consumables (Active Play Support):** Health Potions (50g), Damage Buffs (100g), XP Boost (150g)
4. **Salvaging (Resource Conversion):** Sell unwanted gear for gold, get 50% of enhancement investment back
5. **Workshop Rushing (Convenience):** 10 gold per minute remaining on refining timer
6. **Base Upgrades (Long-term Sinks):** Unlock facility upgrades (costs scale: 500 → 2000 → 5000 gold)

**Research Lab (Phase 2, Reworked for Mastery):**
- Automatically studies monsters you've killed (tracks kill count)
- Each monster type has research tiers (10 kills, 50 kills, 200 kills)
- Research progresses while idle (1 tier per 2 hours of offline time)

**Research Benefits (Reworked for Mastery, Not Power):**
- Tier 1: Monster weak points glow (easier critical hits)
- Tier 2: See attack telegraphs 0.5 seconds earlier
- Tier 3: Unlock monster-specific skill variant

**Training Ground (Phase 2, Reworked):**
- Generates 1 Quality of Life Token per day (max 7)
- Spend tokens on permanent QoL improvements:
  - **Quick Start:** Begin runs at level 2/3/4 (costs 3/5/7 tokens)
  - **Smart Loot:** Items drop weighted toward your build (5 tokens)
  - **Skill XP Boost:** Next run has 2x skill mastery gain (1 token)
  - **Extended Floors:** Boss timer increased by 30 seconds (3 tokens)

### Gear System
**4 Equipment Slots:**
1. Weapon (primary stat + damage)
2. Armor (defense + secondary stats)
3. Accessory (pure stat bonuses)
4. Relic (unique build-defining properties)

**How Gear Enhancement Works:**
Each piece of equipment has 4 layers:
1. **Base Stats** (fixed on drop, determined by rarity)
2. **Power Level** (0-10, upgraded with Energy Cores)
3. **Modification** (one active special property, added with Modification Components)
4. **Synergy** (one build-defining bonus, added with Catalyst Fragments)

**Enhancement Example:**
```
Plasma Rifle (Epic)
├─ Base: 100 damage, +15 INT, +10 DEX
├─ Power Level: 3 (+30% to all stats)
│  └─ Current: 130 damage, +19 INT, +13 DEX
├─ Modification: "Attacks chain to 2 enemies"
└─ Synergy: "+25% damage when above 80% health"
```

### Ascension System (Post-MVP Phase 2)
**Purpose:** Infinite progression system providing permanent character power independent of gear

**How It Works:**
- **Ascension XP:** Earned from EVERY enemy kill, boss defeat, and floor clear
- **Never Lost:** Accumulates permanently across all runs, even deaths
- **Uncapped:** Infinite levels with exponential XP requirements

**Ascension Levels Grant:**
+1 Ascension Point per level to spend in three trees:

```
Power Tree (Raw Stats):
├── Might: +2% physical damage per point
├── Focus: +2% magical damage per point
├── Vitality: +10 HP per point
└── Agility: +1% attack speed per point

Survival Tree (Defenses):
├── Armor: +2 flat damage reduction per point
├── Resistance: +3% elemental resist per point
├── Recovery: +0.5 HP/sec regen per point
└── Evasion: +1% dodge chance per point (cap 30%)

Utility Tree (Unique Effects):
├── Multishot: +5% chance for projectiles to split
├── Execution: +2% instant-kill chance on enemies below 20% HP
├── Momentum: +1% damage for each enemy killed recently (cap 50%)
└── Resonance: +3% to trigger equipment effects twice
```

### Endgame: Infinite Tower Scaling
**Unlocked after beating Floor 5:**

**Ascension Floors (6+):**
- Each floor is 20% harder than the previous
- No floor cap - push as high as you can
- Ascension becomes necessary to progress
- Special rewards every 5 floors:
  - Floor 10: Exclusive cosmetic set
  - Floor 15: Title: "Tower Climber"
  - Floor 20: Unique modification type
  - Floor 25: Legendary relic blueprint
  - Every 5 floors: Prestige points for leaderboard

**Corruption System (Floors 1-5 Replayability):**
After beating a floor, can replay with Corruption levels 1-10:
- Each Corruption level: +50% enemy stats, +50% rewards
- Corruption modifiers add challenge:
  - Level 1: "Enemies explode on death"
  - Level 3: "No health drops"
  - Level 5: "Elite spawn rate doubled"
  - Level 7: "Skills cost health"
  - Level 10: "One hit kills you"

**Weekly Tower Events:**
- "Speed Week": Clear floors 25% faster for bonus rewards
- "Swarm Week": Double enemies, double XP
- "Elite Week": All enemies are elite tier
- Global leaderboard resets weekly

### Retention Systems

**Daily Missions:**
- "Kill 100 enemies" → 50 Energy Cores
- "Use skills 50 times" → Skill XP boost (2 hours)
- "Clear any floor" → 100 gold

**Weekly Challenges:**
- "Reach Floor 5" → Legendary gear box
- "Master any skill to Silver" → Catalyst Fragment x3
- "Clear Corruption 5+ on any floor" → Exclusive modification

**Collection Goals:**
- Gear Sets: Collect all pieces for set bonuses
- Monster Codex: 100% completion unlocks special title
- Skill Mastery: Master all skills to Diamond for prestige cosmetic

**Leaderboards:**
- Highest Ascension Floor reached
- Fastest Floor 5 clear time
- Total Ascension Level
- Weekly event rankings

### The Hub: Your Base

**Layout (Simple to Start):**
**Central Plaza:** Quick access to all facilities
- **Blacksmith:** Upgrade equipment with refined materials
- **Merchant:** Buy consumables and basic gear with gold
- **Class Selector:** Change class, view stats, see skill mastery
- **Portal:** Enter the tower (start a run)
- **Leaderboard:** Check rankings and weekly events

**Facility Wings:**
- **Workshop** (left): Shows crafting queue, material storage
- **Treasury** (right): Displays gold generation, collection button
- **Research Lab** (back left): Monster codex, active research
- **Training Ground** (back right): Spend QoL tokens, view benefits

**Progression Gates:**
- Start: Only Workshop and basic Blacksmith available
- Floor 2 cleared: Treasury unlocked
- Floor 3 cleared: Research Lab unlocked
- Floor 4 cleared: Training Ground unlocked
- Floor 5 cleared: Infinite Ascension Floors unlocked
- Floor 10 reached: Prestige shop unlocked

### Progression Flow Example

**Day 1 - New Player:**
1. Tutorial run on Floor 1, learn combat basics
2. Extract with basic loot and materials
3. Unlock Workshop and Treasury
4. Deposit materials in Workshop (start first refinement)
5. Close game

**Day 2 - Return:**
1. Collect refined materials and 200 gold from idle earnings
2. Use materials to upgrade weapon at Blacksmith (+10% damage)
3. Run Floor 1 again, feels stronger, push to Floor 2
4. Defeat Floor 2 boss, extract with better loot
5. Notice Whirlwind skill progress: 23/100 kills
6. Close game

**Day 3-7 - Early Loop:**
- Daily check-ins to collect idle progress
- Use refined materials to steadily upgrade gear
- Push to higher floors (unlock new facilities)
- See skills gradually mastering through use
- Complete daily missions for extra resources

**Week 2+ - Established Loop:**
- Character level approaching 50+
- Multiple skills at Silver mastery tier
- Farming Floor 5 for Catalyst Fragments
- Workshop constantly refining materials
- Starting to eye Corruption levels for extra challenge
- Can choose to grind actively for hours OR check in daily

**Month 2+ - Endgame:**
- Character level 150+, Ascension level 30+
- Pushing Floor 15+ in Ascension Tower
- Several skills at Diamond mastery
- Competing on weekly leaderboards
- Perfect gear with synergies tailored to build
- Still finding new skill combinations to master

## Godot-Specific Patterns

### Node Finding Best Practices
- **Own children:** Use `GetNode<T>("NodeName")`
- **Singletons (Player, HUD):** Use Groups + `GetFirstNodeInGroup("player")`
- **Collections:** Use Groups + `GetNodesInGroup("enemies")`
- **Complex UI:** Use `[Export]` references (drag in editor)
- **Frequently reorganized:** Use `%UniqueName` syntax

### Animation Approach
- **Tween:** Simple property changes (fade, move, scale) - use for effects
- **AnimationPlayer:** Complex multi-node choreography - use for cutscenes/attacks
- **AnimatedSprite2D:** Frame-by-frame sprite sheets - use when art available
- Currently using Tweens for effects (XP shard bobbing, death animations)

### Physics & Deferred Calls
**Critical Rule:** Never modify physics tree during collision processing!
- ❌ `AddChild()` in collision callback → ERROR
- ✅ `CallDeferred(MethodName.AddChild, node)` → Works
- Use `CallDeferred` when spawning/removing nodes in physics callbacks
- Use `SetDeferred` for collision shape properties

### Signal Limitations
Godot C# signals can only pass:
- ✅ Built-in types: int, float, string, bool
- ✅ Godot types: Vector2, Node, Resource
- ❌ Custom C# classes

**Solution:** Use Godot Resources for custom data classes
```csharp
// Wrong - won't compile
[Signal] public delegate void MySignalEventHandler(CustomClass data);

// Right - Resource can be passed
[Signal] public delegate void MySignalEventHandler(CustomResource data);
```

### Scene Instancing Pattern
```csharp
[Export] public PackedScene EnemyScene; // Drag in editor

var enemy = EnemyScene.Instantiate<Enemy>();
enemy.GlobalPosition = spawnPos;
AddChild(enemy); // Or GetTree().Root.AddChild(enemy) for persistence
```

### Architecture Patterns

**Singleton Pattern (Game Managers)**
```csharp
public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    
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
Add to Project Settings → Autoload for true singletons.

**Component Pattern (Player)**
Player has separate component scripts for different concerns:
- Movement logic
- Combat logic
- Inventory management
- Stats calculation
Helps keep code modular and testable.

**Factory Pattern (Spawning)**
Game.cs acts as spawn manager:
```csharp
public Enemy SpawnEnemy(Vector2 position)
{
    var enemy = EnemyScene.Instantiate<Enemy>();
    enemy.GlobalPosition = position;
    AddChild(enemy);
    return enemy;
}
```

### UI Design Guidelines

**Color Palette (Metallic Sci-Fi)**
- Primary: Steel Blue (#4a90e2)
- Secondary: Muted Purple (#9b59b6)
- Backgrounds: Slate grays with depth
- Accents: Subtle glows, no harsh neon
- Fonts: Clean sans-serif (Segoe UI style)

**HUD Structure**
- Canvas Layer (layer 100): Keeps UI on screen
- Follow Viewport: Must be FALSE for screen-space UI
- Full Rect anchors: UI Control at (0,0)-(1,1) with offset 0
- TopLeft: Health/XP bars, level badge, primary stats
- TopRight: Resource counters (gold, materials)
- TopCenter: Floor info, wave counter
- Bottom: Skills bar with mastery progress

**UI Elements**
- Semi-transparent dark panels (rgba with alpha ~0.95)
- Border: 2px steel blue (#4a90e2)
- Corner radius: 8px for panels, 4px for bars
- Progress bars: Custom StyleBoxFlat with gradient fills

### Development Workflow

**Daily Session Structure (2 hours)**
- Hour 1: Build one specific feature
  - 50 min: Code implementation
  - 10 min: Test
- Hour 2: Iterate and polish
  - 30 min: Fix bugs from yesterday
  - 20 min: Playtest current build
  - 10 min: Plan tomorrow's task

**Git Commit Pattern**
Commit after each working feature:
- "Add XP shard pickup system"
- "Implement level-up UI with upgrades"
- "Fix physics collision error in enemy death"

**Testing Checklist**
Before ending session:
- [ ] Game runs without errors
- [ ] Core loop works (kill → loot → level → upgrade)
- [ ] No red errors in Output panel
- [ ] Git committed with clear message

### Common Gotchas & Solutions

**Problem:** HUD doesn't follow camera
- **Cause:** CanvasLayer has follow_viewport_enabled = true
- **Fix:** Set to false (default). CanvasLayer should be screen-space.

**Problem:** Node not found errors
- **Cause:** Using FindChild in _Ready() before scene fully loaded
- **Fix:** Use Groups + cache reference, or use CallDeferred for initialization

**Problem:** Physics error when spawning
- **Cause:** Trying to AddChild during collision processing
- **Fix:** Use CallDeferred(MethodName.SpawnMethod)

**Problem:** Signal parameter error (GD0202)
- **Cause:** Trying to pass custom C# class through signal
- **Fix:** Use int index or convert class to Godot Resource

**Problem:** Upgrade not applying
- **Cause:** Not calling ApplyUpgrade() after selecting
- **Fix:** Ensure OnUpgradeSelected() calls ApplyUpgrade(upgrade.Type)

## Technical Considerations

### Performance Targets
- **FPS:** 60 FPS minimum with 50+ enemies on screen
- **Load Times:** <2 seconds between base and runs
- **Save/Load:** Instant, no loading bars
- **Spawning:** Can spawn 10+ entities per second without lag
- **Idle Processing:** Background timer system, not simulation

### Platform Priorities
**MVP Target:** PC (Steam)
- Mouse + keyboard primary
- Controller support secondary
- Mobile: Consider for post-launch (UI needs redesign)

### Data Structure Considerations
**Keep it simple:**
- Gear stats: Use flat modifiers, not complex formulas
- Enemy scaling: Linear increases per floor (HP +30%, Damage +20% per floor)
- Material counts: No decimals, whole numbers only
- Timer system: Server-side validation if online, local storage if offline

### Future Considerations

**When to Add Object Pooling**
- Spawning 50+ enemies total: Consider pooling
- Shooting 20+ projectiles/second: Definitely pool
- 100+ XP shards on screen: Pool them

**When to Optimize**
- After core loop is fun (don't optimize too early)
- When hitting performance targets becomes difficult
- Before adding more content that would compound issues

## Launch Scope & Expansion Path

### MVP Launch Scope
**Must Have:**
- 3 classes with distinct playstyles
- 5 floors with unique bosses
- Skill mastery system (use-based progression)
- Workshop and Treasury
- Basic gear system with enhancements
- Character leveling to 200
- In-run upgrade system that synergizes with equipped skills
- Save/load system

### Post-MVP Phase 1 (First Month)
- Infinite Ascension Floors (6+)
- Ascension system
- Corruption levels for floors 1-5
- Weekly events and leaderboards
- Daily missions

### Post-MVP Phase 2 (Months 2-3)
- Research Lab implementation
- Training Ground with QoL tokens
- 10 more skills per class
- Gear set bonuses
- Monster codex completion rewards

### Future Expansions
- New classes
- Prestige system at Floor 50+
- Seasonal events with unique rewards
- Co-op mode for 2 players
- Custom challenge mode creator

## Why This Design Works

### For Active Players:
- Fighting is always the fastest way to progress
- Skill mastery rewards active use, not waiting
- Infinite floors provide endless challenge
- Grinding is rewarded with direct power gains
- Multiple progression paths (gear, skills, ascension, floors)

### For Casual Players:
- Check in once a day for meaningful idle progress
- Can focus on mastering one skill at a time
- Corruption levels let them replay at comfortable difficulty
- Clear daily missions in 15 minutes
- Never feel "left behind" due to personal progression

### For Both:
- Clear feedback loop: fight → collect → refine → upgrade → master skills → climb higher
- Always something to work toward (next skill tier, next floor, next ascension level)
- No wasted runs - everything contributes to permanent progression
- Complexity increases gradually
- Respects player time with multiple progression speeds

## Key Design Principles

1. **Active play drives progression** - Idle only enhances, never replaces
2. **Every action matters** - All kills contribute to skill/ascension progress
3. **Clear purpose for power** - Infinite floors give reason to get stronger
4. **No time-gating on core progression** - Skills and power come from play
5. **Multiple valid goals** - Chase floors, perfect builds, or complete collections
6. **Respect player investment** - Permanent progression never lost
7. **Gradual complexity** - Start simple, layer systems as players advance

## Known Issues
- Enemy spawning doesn't increase difficulty over time (planned)
- No sound effects or music
- Placeholder art (colored squares)
- No death particles or screen shake
- Single enemy type (need variety)
- No save system (progress lost on quit)

## Next Priority Tasks
1. **Implement Q/E/R active skill system** (foundation for everything else)
   - Add cooldown-based skills (1-2 per slot for one class)
   - Manual/auto aim depending on skill
   - Visual feedback (cooldown UI in HUD)
   - Keep it simple: just make skills work and feel good

2. **Refactor upgrade system to use UpgradePoolManager** (AFTER skills work)
   - Extend `Upgrade.cs` with: Category, RequiredSkill, Weight, CanStack fields
   - Create UpgradePoolManager singleton (autoload)
   - Load upgrades from Resources/Upgrades/ directory
   - Implement weighted selection (40% skill modifiers, 40% passives, 20% stats)
   - Add skill-specific filtering (only show if skill equipped)
   - Remove hardcoded upgrade list from Player.cs

3. **Implement skill mastery tracking** (builds on working skills)
   - Track kills per skill (Bronze/Silver/Gold/Diamond tiers)
   - Visual progress in HUD
   - Permanent progression across runs

4. Add 2-3 enemy types (fast/tank/ranged)
5. Implement basic material drop system
6. Create floor transition system
7. Add boss encounter for Floor 1

## Development Notes
- Keep sessions focused on ONE feature
- Playtest after every change
- Skills should feel impactful immediately
- Every run should progress something permanent
- Don't add complexity until current systems are fun
- Commit working code daily

### Technical Debt / Pending Refactors
- **Upgrade System:** Currently hardcoded in Player.cs. Needs UpgradePoolManager refactor (documented in Next Priority Tasks #2) - do AFTER skills are implemented

## For Claude Code

**Your Role: Mentor, Not Implementer**
- **DO NOT implement features unless explicitly asked**
- Your role is to guide, advise, and explain - not to write code proactively
- When asked "what's next" or "where are we" - provide analysis and recommendations, not implementation
- Wait for explicit requests like "implement X" or "write the code for Y" before coding
- Offer guidance on approach, architecture, and Godot patterns first

**When Providing Implementation (only when explicitly requested):**
- Assume C# knowledge but Godot beginner
- Explain Godot-specific concepts clearly
- Provide complete, copy-paste ready code
- Point out common Godot pitfalls
- Reference this document's patterns and architecture
- Remember skill mastery is use-based, not time-based
- Prioritize working code over perfect code
- Keep explanations practical and example-driven

---

This documentation represents the complete game design with all systems interconnected for maximum retention and player satisfaction. The core loop of fight → collect → enhance → master → climb creates endless meaningful progression.