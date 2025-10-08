# Tower Ascension - Project Documentation

## Project Overview
A sci-fi roguelite action-RPG with idle/incremental mechanics built in Godot 4.x with C#. Players fight through tower floors in active combat sessions while idle systems process materials and generate resources between play sessions.

## Core Design Philosophy
- **Active play = power gains** (levels, gear, combat strength)
- **Idle time = efficiency gains** (material refinement, resource generation, unlock systems)
- **Idle enhances, never replaces** active gameplay
- Players progress meaningfully in both 15-minute sessions and 2-hour grinds

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
- Materials system
- Workshop/Treasury (idle systems)
- Multiple floors (have 1, need 5+)
- Gear/equipment system
- Save system
- Polish (sound, particles, screen shake)

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
- Gain 1 character level per floor boss defeated
- Each character level: +3 stat points to distribute
- Character level cap: 50 (for MVP, expandable later)

**Save System:**
- Persists: Character level and stat distribution, all collected gear and materials (in stash), Workshop queue and timers (continues offline), Treasury accumulation and timer, highest floor reached, skill unlocks
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

### What We're Explicitly NOT Building Yet
**Save for Post-MVP:**
- Skill synergies/combos (great idea, but adds complexity)
- Research Lab (defined but not implemented)
- Training Ground (defined but not implemented)
- More than 3 classes
- More than 5 floors
- PvP or multiplayer
- Prestige/New Game+ systems
- Seasonal content
- Cosmetics system
- Advanced enchantments beyond stat boosts

**Why Wait:**
- Validate the core loop first
- Get player feedback on what they want more of
- Avoid scope creep killing the project
- Ship a tight, polished experience rather than bloated half-baked one

### The MVP Test: "Can You Play It for 10 Hours?"
**Hour 1:** Tutorial, first run, unlock Workshop
**Hour 2-3:** Learning enemy patterns, upgrading first gear pieces
**Hour 4-5:** Experimenting with skill loadouts, reaching Floor 3
**Hour 6-7:** Farming for specific gear drops, enhancing to +5
**Hour 8-9:** Pushing to Floor 5 boss, min-maxing build
**Hour 10:** Beat Floor 5, feel accomplished, excited for more content

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
- Training Ground can provide permanent +1 to specific stats (expensive long-term investment)
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
3. Show 3 random upgrades from pool
4. Player selects one
5. Upgrade applied immediately
6. Game resumes

### Ability System (Planned)
**Hybrid Approach:** Permanent Abilities (Customizable Foundation) + Random Upgrades (Run Variety)

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

**How You Unlock Skills:**
- Start with 2-3 basic skills per slot unlocked
- Research Lab unlocks new skills by studying monsters
- Training Ground can unlock skills with training points
- Floor bosses drop skill unlock tokens

**Example Warrior Skill Pool:**
- **Q Options:** Whirlwind, Charge, Ground Slam, Cleave
- **E Options:** Iron Skin, War Cry, Bloodlust, Parry
- **R Options:** Earthquake, Berserker Rage, Execute, Fortress

**Skill Progression:**
Each skill has 3 levels:
- Level 1: Unlocked (base version)
- Level 2: Enhanced (better numbers, costs 5 training points)
- Level 3: Mastered (adds extra effect, costs 15 training points)

**Example Skill Progression:**
- **Whirlwind (Q)**
  - Lv1: Spin for 3 seconds, dealing damage around you
  - Lv2: Spin for 5 seconds, +50% damage
  - Lv3: Enemies hit are pulled toward you

**In-Run Progression (Level-Up Upgrades):**
Every level-up during a run, choose 1 of 3 random upgrades:

1. **Skill Modifiers** (40% chance)
   - Reduce cooldowns on equipped skills
   - Add effects: "Q skill now burns enemies"
   - Increase damage/duration/area
   - Stack multiple times per run

2. **Passive Bonuses** (40% chance)
   - +15% movement speed
   - +20% attack speed
   - +10% critical chance
   - Gain a damage aura
   - Projectiles pierce enemies
   - Life steal on attacks

3. **Stat Boosts** (20% chance)
   - +5 to a specific stat (temporary for this run)
   - +15% to all stats
   - +50 max HP

**Example Run Progression:**
- Start: Basic Attack, Special (Fireball), Q (Whirlwind), E (Iron Skin), R (Berserker Rage)
- Level 2: Choose "+20% Fireball damage"
- Level 4: Choose "Whirlwind pulls enemies"
- Level 6: Choose "+15% attack speed"
- Level 8: Choose "Fireball chains to nearby enemies"
- Level 10: Choose "+10 Strength"
- End result: Your Fireball is now a chaining nuke, your Whirlwind is a crowd control monster

**Skill Unlock Pace:**
- Early Game (Floors 1-3): 2 skills per slot unlocked (6 total choices)
- Mid Game (Floors 4-7): 4 skills per slot unlocked (12 total)
- Late Game (Floors 8-12): 6+ skills per slot (18+ total)

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

**Visual Clarity:**
- Materials: Small colored particles (blue = Energy Cores, red = Modification Components, gold = Catalyst Fragments)
- Gear: Big glowing item on ground with rarity color (white/green/blue/purple/orange)
- Auto-pickup radius: Small at start, upgradeable

## Planned Systems (Not Yet Implemented)

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

**Why This Works:**
- Active players: Use raw materials immediately with RNG outcomes
- Idle players: Refine materials while away for guaranteed/better outcomes
- Simple to understand: Only 3 materials, each with one clear purpose
- Deep optimization: Choosing mods and synergies creates build variety
- Idle enhances active: Refined materials are better, not different
- No bloat: No confusing web of 15+ crafting materials

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
1. **Respeccing (Player Agency):** Cost 100 gold × character level, completely reset skill choices
2. **Consumables (Active Play Support):** Health Potions (50g), Damage Buffs (100g), XP Boost (150g)
3. **Salvaging (Resource Conversion):** Sell unwanted gear for gold, get 50% of enhancement investment back
4. **Workshop Rushing (Convenience):** 10 gold per minute remaining on refining timer
5. **Base Upgrades (Long-term Sinks):** Unlock facility upgrades (costs scale: 500 → 2000 → 5000 gold)

**Treasury Upgrades:**
- Increase generation rate (+20% per level)
- Increase cap duration (8h → 12h → 16h)
- Unlock "interest" mechanic (unspent gold gains 5% per day, max 1000)

**Research Lab (Phase 2):**
- Automatically studies monsters you've killed (tracks kill count)
- Each monster type has research tiers (10 kills, 50 kills, 200 kills)
- Research progresses while idle (1 tier per 2 hours of offline time)
- Can only research monsters you've personally killed

**Research Benefits:**
- Tier 1: +5% drop rate from that monster
- Tier 2: Unlock a new skill related to that monster type
- Tier 3: Bonus damage vs that monster type

**Research Lab Upgrades:**
- Research multiple monsters simultaneously
- Faster research speed
- Unlock "monster essence" drops for special crafting

**Training Ground (Phase 2):**
- Accumulates 1 Training Point per hour (max 24 points, then stops)
- Spend Training Points on permanent efficiency bonuses
- Doesn't increase combat stats directly, makes active play MORE rewarding

**Training Options:**
- **Loot Luck:** +2% rare drop chance per point (max 10 points)
- **Experience Boost:** +5% XP gain per point (max 10 points)
- **Material Yield:** +10% extra materials from drops per point (max 5 points)
- **Starting Power:** Begin runs at level 2/3/4 (costs 5/10/15 points)
- **Skill Leveling:** Upgrade skills to Level 2 (5 points) or Level 3 (15 points)

### Gear System (Planned)
**4 Equipment Slots:**
1. Weapon (primary stat + damage)
2. Armor (defense + secondary stats)
3. Accessory (pure stat bonuses)
4. Relic (unique build-defining properties)

**Gear Properties:**
- **Rarity:** Common → Uncommon → Rare → Epic → Legendary (determines base stats)
- **Power Level:** 0-10 (each level adds +10% to all base stats)
- **Modification Slot:** One active special property (can be replaced)
- **Synergy Slot:** One build-defining bonus (requires rare materials)

**How Gear Enhancement Works:**
Each piece of equipment has 4 layers:
1. **Base Stats** (fixed on drop, determined by rarity)
2. **Power Level** (0-10, upgraded with Energy Cores)
   - Each level adds +10% to all base stats
   - Max level 10 = +100% stats (doubles item power)
3. **Modification** (one active special property, added with Modification Components)
   - Examples: "Attacks pierce", "20% life steal", "+50% crit damage"
   - Can be replaced/rerolled
4. **Synergy** (one build-defining bonus, added with Catalyst Fragments)
   - Examples: "+30% damage if DEX > 50", "+40% effect after using dash"
   - Ties gear into specific build/playstyle

**Enhancement Example:**
```
Plasma Rifle (Epic)
├─ Base: 100 damage, +15 INT, +10 DEX
├─ Power Level: 3 (+30% to all stats)
│  └─ Current: 130 damage, +19 INT, +13 DEX
├─ Modification: "Attacks chain to 2 enemies"
└─ Synergy: "+25% damage when above 80% health"
```

**Key Design Principles:**
- Gear DROPS from combat (never crafted)
- Materials ENHANCE dropped gear
- Power Level = linear strength progression
- Modifications = behavioral customization
- Synergies = build identity

### The Hub: Your Base (Planned)

**Layout (Simple to Start):**
**Central Plaza:** Quick access to all facilities
- **Blacksmith:** Upgrade equipment with refined materials
- **Merchant:** Buy consumables and basic gear with gold
- **Class Selector:** Change class, view stats
- **Portal:** Enter the tower (start a run)

**Facility Wings:**
- **Workshop** (left): Shows crafting queue, material storage
- **Treasury** (right): Displays gold generation, collection button
- **Research Lab** (back left): Monster codex, active research
- **Training Ground** (back right): Spend training points, view efficiency stats

**Progression Gates:**
- Start: Only Workshop and basic Blacksmith available
- Floor 2 cleared: Treasury unlocked
- Floor 3 cleared: Research Lab unlocked (Phase 2 feature)
- Floor 4 cleared: Training Ground unlocked (Phase 2 feature)
- Floor 5 cleared: "You beat MVP!" celebration

**Visual Feedback:**
- Facilities glow when they have something ready to collect
- Progress bars show idle timers
- Notifications appear when you open the game: "Workshop completed 3 items!" "Treasury full: 1,450 gold!"

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
5. Unlock Research Lab
6. Set Lab to study skeletons, deposit new materials in Workshop
7. Close game

**Day 3-7 - Early Loop:**
- Daily check-ins to collect idle progress
- Use refined materials to steadily upgrade gear
- Push to higher floors (unlock Treasury bonuses)
- Complete monster research (unlock new skills)
- Accumulate training points (invest in Loot Luck)

**Week 2+ - Established Loop:**
- Strong enough to farm Floor 5-7 efficiently
- Workshop constantly refining materials for next upgrades
- Research Lab cycling through monster types
- Training Ground bonuses making runs more efficient
- Can choose to grind actively for hours OR check in daily
- Both playstyles progress meaningfully

Ascension System (Post-MVP Phase 2)
Purpose: Infinite progression system providing permanent character power independent of gear
How It Works:

Ascension XP: Earned from EVERY enemy kill, boss defeat, and floor clear (separate from character XP)
Never Lost: Accumulates permanently across all runs, even deaths
Uncapped: Infinite levels with exponential XP requirements

Ascension Levels Grant:

+1 Ascension Point per level to spend in three trees:

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
Why This Complements Your Systems:

Bad Luck Protection: Even with poor gear drops, you're always getting stronger
Long-term Goal: Something to work toward after hitting level 50 cap
Synergizes with Gear: Ascension % bonuses multiply with gear stats
Respects Time: Every enemy killed matters, even in failed runs
Clear from Idle: Ascension is ONLY from active play (combat), while idle helps with gear

How It Ties Into Your Game Loop
Example Progression Path:
Week 1:

Focus on Character Levels (1-50)
Start accumulating Ascension XP passively
Maybe gain 5-10 Ascension Levels just from playing

Week 2-4:

Hit Character Level cap (50)
Ascension becomes primary progression
Use Ascension Points to complement your build
"I'm a Ranger, so I'll invest in Agility and Multishot"

Month 2+:

Ascension Level 50+ with specialized build
Research Lab unlocking monster bonuses
Training Ground providing QoL improvements
Perfect gear with max Power Levels
Still gaining Ascension XP for that next point

Implementation Priority
Based on your current documentation, I'd suggest:

Keep Ascension in Phase 2 (Post-MVP) along with Research Lab and Training Ground
For MVP: Character Levels + Gear is enough progression
Add Ascension when: Players start hitting level cap in testing

This way you can launch with a tight experience and add Ascension as the "endgame update" that keeps players engaged long-term.
The Complete Power Ecosystem
With Ascension added, your power systems would be:

Gear (RNG/Crafting) - Your equipment and how you enhance it
Character Level (Limited) - Your base stat foundation
Ascension (Infinite) - Your permanent account-wide progression
In-Run Upgrades (Temporary) - Run variety and excitement
Research Lab (Knowledge) - Make you better at playing
Training Ground (Efficiency) - Make your time more valuable

Each system has a clear role and they all complement rather than compete with each other!

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
Scene Instancing Pattern
csharp[Export] public PackedScene EnemyScene; // Drag in editor

var enemy = EnemyScene.Instantiate<Enemy>();
enemy.GlobalPosition = spawnPos;
AddChild(enemy); // Or GetTree().Root.AddChild(enemy) for persistence
Architecture Patterns
Singleton Pattern (Game Managers)
csharppublic partial class GameManager : Node
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
Add to Project Settings → Autoload for true singletons.
Component Pattern (Player)
Player has separate component scripts for different concerns:

Movement logic
Combat logic
Inventory management
Stats calculation
Helps keep code modular and testable.

Factory Pattern (Spawning)
Game.cs acts as spawn manager:
csharppublic Enemy SpawnEnemy(Vector2 position)
{
    var enemy = EnemyScene.Instantiate<Enemy>();
    enemy.GlobalPosition = position;
    AddChild(enemy);
    return enemy;
}
UI Design Guidelines
Color Palette (Metallic Sci-Fi)

Primary: Steel Blue (#4a90e2)
Secondary: Muted Purple (#9b59b6)
Backgrounds: Slate grays with depth
Accents: Subtle glows, no harsh neon
Fonts: Clean sans-serif (Segoe UI style)

HUD Structure

Canvas Layer (layer 100): Keeps UI on screen
Follow Viewport: Must be FALSE for screen-space UI
Full Rect anchors: UI Control at (0,0)-(1,1) with offset 0
TopLeft: Health/XP bars, level badge, primary stats
TopRight: Resource counters (gold, materials)
TopCenter: Floor info, wave counter
Bottom: Skills bar (not yet implemented)

UI Elements

Semi-transparent dark panels (rgba with alpha ~0.95)
Border: 2px steel blue (#4a90e2)
Corner radius: 8px for panels, 4px for bars
Progress bars: Custom StyleBoxFlat with gradient fills

Development Workflow
Daily Session Structure (2 hours)

Hour 1: Build one specific feature

50 min: Code implementation
10 min: Test


Hour 2: Iterate and polish

30 min: Fix bugs from yesterday
20 min: Playtest current build
10 min: Plan tomorrow's task



Git Commit Pattern
Commit after each working feature:

"Add XP shard pickup system"
"Implement level-up UI with upgrades"
"Fix physics collision error in enemy death"

Testing Checklist
Before ending session:

 Game runs without errors
 Core loop works (kill → loot → level → upgrade)
 No red errors in Output panel
 Git committed with clear message

Common Gotchas & Solutions
Problem: HUD doesn't follow camera
Cause: CanvasLayer has follow_viewport_enabled = true
Fix: Set to false (default). CanvasLayer should be screen-space.
Problem: Node not found errors
Cause: Using FindChild in _Ready() before scene fully loaded
Fix: Use Groups + cache reference, or use CallDeferred for initialization
Problem: Physics error when spawning
Cause: Trying to AddChild during collision processing
Fix: Use CallDeferred(MethodName.SpawnMethod)
Problem: Signal parameter error (GD0202)
Cause: Trying to pass custom C# class through signal
Fix: Use int index or convert class to Godot Resource
Problem: Upgrade not applying
Cause: Not calling ApplyUpgrade() after selecting
Fix: Ensure OnUpgradeSelected() calls ApplyUpgrade(upgrade.Type)
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

Future Considerations
When to Add Object Pooling

Spawning 50+ enemies total: Consider pooling
Shooting 20+ projectiles/second: Definitely pool
100+ XP shards on screen: Pool them

When to Optimize

After core loop is fun (don't optimize too early)
When hitting performance targets becomes difficult
Before adding more content that would compound issues

## Launch Scope & Expansion Path

### MVP Launch Scope
**Must Have:**
- 3 classes (Warrior, Mage, Ranger)
- 5 floors with unique bosses
- Workshop (material refinement)
- Treasury (basic gold generation)
- Basic combat with 10-15 skills per class
- Equipment system with 4 slots (weapon, armor, accessory, relic)
- Character leveling and stat allocation
- In-run upgrade system
- Save/load system

**Phase 1: Core Combat Loop (CURRENT)**
- Player movement and shooting
- Enemy AI and spawning
- XP and leveling system
- Basic upgrade pool
- Combat HUD

**Phase 2: Materials & Idle Systems**
- Material drop system (Energy Cores, Modification Components, Catalyst Fragments)
- Workshop implementation
- Treasury implementation
- Gear enhancement system
- Base/hub structure

**Phase 3: Multiple Floors & Enemies**
- 5 floors with visual distinction
- 3-4 enemy types per floor
- Boss encounters
- Extract/continue decision system
- Death penalty mechanics

**Phase 4: Gear/Equipment System**
- Gear drops with rarity tiers
- Power Level upgrades
- Modification system
- Synergy system
- Inventory management

**Phase 5: Polish & Juice**
- Screen shake, particles, visual effects
- Sound effects and music
- Damage numbers, hit feedback
- Enemy knockback
- Death animations

**Phase 6: Save System & Meta-Progression**
- Persistent character levels
- Gear and material storage
- Workshop queue persistence
- Treasury offline accumulation
- Skill unlocks

**Phase 7: MVP Launch**
- Final balance pass
- Bug fixes
- Performance optimization
- Early Access on Steam

### Post-MVP Content (Phase 8+)
**Phase 8: Research Lab & Training Ground**
- Research Lab system
- Training Ground
- More skill unlocks
- Monster codex

**Phase 9: Expansion**
- More floors (up to 12)
- More classes
- Expanded refinement types
- More enemy variety

**Phase 10: Meta Systems**
- Prestige/New Game+ system
- Challenge modes
- Seasonal events
- Legendary equipment tiers
- Base customization
## Why This Design Works

### For Active Players:
- Fighting is always the fastest way to progress
- Idle systems give you something to do with your loot (refining, research)
- No waiting—you can always jump in and play
- Grinding is rewarded with direct power gains
- Multiple progression paths (gear, research, training, floors)

### For Casual Players:
- Check in once a day for meaningful progress
- Idle systems catch you up slowly
- Never feel pressured to play for hours
- Training Points provide long-term goals
- Clear feedback on idle progress earned

### For Both:
- Clear feedback loop: fight → collect → refine → upgrade → fight stronger
- Complexity increases gradually (start with 2 systems, unlock more)
- Respects player time (idle has caps, active is efficient)
- Multiple viable playstyles
- Always something to work toward

## Key Design Principles

1. **Idle enhances, never replaces** - Active play is always most rewarding
2. **Clear feedback** - Players always know what idle progress earned them
3. **No punishment** - Missing a day doesn't ruin progress (caps prevent FOMO)
4. **Respect time** - Both 15-minute sessions and 2-hour grinds feel productive
5. **Gradual complexity** - Start simple, layer systems as players advance
6. **Player agency** - Meaningful choices about skills, stats, and gear
7. **Fair monetization** - Never pay-to-win, always pay-for-convenience or cosmetics (if applicable)

## Monetization Considerations (If Applicable)

**Fair Model:**
- **Free:** All core gameplay, no power locked behind payments
- **Premium Currency Uses:**
  - Cosmetic skins for characters/weapons
  - Workshop queue speedups (NOT bypassing gameplay)
  - Extra storage slots (convenience, not power)
  - Battle pass with cosmetics + small boosts

**Never:**
- Direct power purchases
- Loot boxes with gameplay items
- Energy systems that block active play
- Ads required for core features

## Known Issues

Enemy spawning doesn't increase difficulty over time (planned)
No sound effects or music
Placeholder art (colored squares)
No death particles or screen shake
Single enemy type (need variety)
No save system (progress lost on quit)

Next Priority Tasks

Add 2-3 enemy types (fast/tank/ranged)
Expand upgrade pool to 15-20 options
Add basic juice (screen shake, particles)
Implement wave system with difficulty scaling
Start material drop system

Development Notes

Keep sessions focused on ONE feature
Playtest after every change
Don't add complexity until current systems are fun
Commit working code daily
Ask questions when stuck - don't waste time debugging alone

Reference Links

Godot C# Docs: https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/
Design Document: [See full design doc in project artifacts]
Asset Sources: itch.io, OpenGameArt.org, Kenney.nl (when ready for real art)


For Claude Code
When helping with this project:

Assume C# knowledge but Godot beginner
Explain Godot-specific concepts clearly
Provide complete, copy-paste ready code
Point out common Godot pitfalls
Reference this document's patterns and architecture
Prioritize working code over perfect code
Keep explanations practical and example-driven


---

Save this as `claude.md` in your project root! This gives Claude Code full context about:
- What you've built
- How systems work
- Godot-specific patterns you're using
- Common issues and solutions
- Your design philosophy
- Next steps

When you start a new Claude Code session, you can say: "Read claude.md for project context" and it'll know everything! 🎉