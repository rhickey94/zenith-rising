# Tower Ascension: Idle Roguelite Design Document

## Core Concept
A roguelite action-RPG where active combat grants power, while idle time improves your efficiency and unlocks new possibilities. Fight your way up the tower actively, then let your base work for you between sessions.

---

## Core Bare Bones: What We Need for MVP

### 1. Combat & Movement
**Must Have:**
- **Movement:** WASD or analog stick, fixed movement speed (upgradeable via stats/upgrades)
- **Auto-aim basic attack:** Targets nearest enemy within range
- **Manual aim special attack:** Aims toward mouse/right stick direction
- **Dash:** Quick movement burst with i-frames
- **Hit feedback:** Screen shake, enemy knockback, damage numbers
- **Death state:** Character dies, run ends, return to base with some loot lost

**Enemy Basics:**
- 3-4 enemy types per floor minimum
- Simple AI: Chase player, attack on contact or at range
- Clear visual telegraphs for attacks
- Spawn in waves (increases over time in each floor)

### 2. Floor Structure
**Each Floor Contains:**
- **Duration:** 8-10 minutes to boss spawn OR kill count threshold (whichever comes first)
- **Boss Fight:** Unique boss per floor with distinct mechanics
- **Extract Decision:** After boss, choose to:
  - Extract with all loot (safe)
  - Continue to next floor (risk = better rewards but lose some loot if you die)
- **Death Penalty:** Lose 50% of materials collected in current run, keep all XP/character levels

**Floor Progression:**
- 5 floors minimum for MVP
- Each floor: more enemy health, more damage, more spawns
- Visual distinction per floor (color palette, tileset, music)

### 3. Loot & Drops
**Drop System:**
- **Essence Shards:** Drop from all enemies (100% chance, 1-3 per enemy)
- **Monster Parts:** Drop from elite enemies (20% chance) and bosses (100%)
- **Ancient Fragments:** Drop from bosses only (100%, 1-2 per boss)
- **Gear:** Drop from elites (10% chance) and bosses (100%, higher rarity)
- **Gold:** Drop from all enemies (50% chance, 5-20 gold)

**Visual Clarity:**
- Materials: Small colored particles (blue = essence, red = parts, gold = fragments)
- Gear: Big glowing item on ground with rarity color
- Auto-pickup radius: Small at start, upgradeable

### 4. Experience & Leveling (In-Run)
**XP System:**
- Enemies drop XP shards (visible pickups, like Vampire Survivors)
- XP bar fills, level up
- Each level = choose 1 of 3 random upgrades
- Max level per run: 20 (prevents infinite scaling)
- Level resets to 1 each new run

**Character Level (Permanent):**
- Separate from in-run leveling
- Gain 1 character level per floor boss defeated
- Each character level: +3 stat points to distribute
- Character level cap: 50 (for MVP, expandable later)

### 5. Base/Hub Essentials
**Must-Have Facilities:**
- **Portal:** Start a run (floor selection)
- **Blacksmith:** Enhance gear, add enchantments
- **Workshop:** View/collect refined materials, queue new refinements
- **Treasury:** Collect idle gold
- **Character Menu:** Allocate stat points, view stats, equip gear
- **Skill Loadout:** Select 3 active skills before run

**UI Needs:**
- Clear notification system (Workshop done! Treasury full!)
- Material counts always visible
- Simple navigation between facilities

### 6. Progression Gates
**Unlock Flow:**
- Start: Only Workshop and basic Blacksmith available
- Floor 2 cleared: Treasury unlocked
- Floor 3 cleared: Research Lab unlocked (Phase 2 feature)
- Floor 4 cleared: Training Ground unlocked (Phase 2 feature)
- Floor 5 cleared: "You beat MVP!" celebration, more content coming

**This prevents overwhelm:** Players learn systems gradually, not all at once.

### 7. Save System
**What Persists:**
- Character level and stat distribution
- All collected gear and materials (in stash)
- Workshop queue and timers (continues offline)
- Treasury accumulation and timer
- Highest floor reached
- Skill unlocks

**What Resets:**
- In-run level (back to 1)
- In-run upgrades (chosen fresh each run)
- Floor position (always restart from Floor 1 or highest unlocked)

### 8. Core Loop Validation Checklist
Before launch, players should be able to:
- [ ] Feel powerful growth within a single 10-minute run
- [ ] Return after 8 hours and see meaningful idle progress
- [ ] Make a meaningful choice about their skill loadout
- [ ] Feel excited when rare gear drops
- [ ] Understand what each material is used for
- [ ] Know exactly what they're working toward next
- [ ] Die and not feel like they wasted their time (kept XP and some loot)

---

## What We're Explicitly NOT Building Yet

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

---

## Technical Considerations

### Performance Targets
- **Framerate:** 60 FPS minimum with 50+ enemies on screen
- **Load Times:** < 2 seconds between base and runs
- **Save/Load:** Instant, no loading bars
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

---

## The MVP Test: "Can You Play It for 10 Hours?"

**Hour 1:** Tutorial, first run, unlock Workshop
**Hour 2-3:** Learning enemy patterns, upgrading first gear pieces
**Hour 4-5:** Experimenting with skill loadouts, reaching Floor 3
**Hour 6-7:** Farming for specific gear drops, enhancing to +5
**Hour 8-9:** Pushing to Floor 5 boss, min-maxing build
**Hour 10:** Beat Floor 5, feel accomplished, excited for more content

**If players are still engaged at Hour 10:** The core loop works. Ship it and expand.

**If players are bored by Hour 5:** The loop is broken. Fix before adding features.

---

## Character Stats & Power System

### Core Stats (Simple Foundation)
Every character has 5 base stats that everything scales from:

1. **Strength** - Increases physical damage and health
2. **Dexterity** - Increases attack speed and critical chance
3. **Intelligence** - Increases magical damage and skill power
4. **Vitality** - Increases maximum health and health regeneration
5. **Fortune** - Increases critical damage and item drop quality

**How Stats Scale:**
- Each point of Strength: +2% physical damage, +10 max HP
- Each point of Dexterity: +1% attack speed, +0.5% crit chance
- Each point of Intelligence: +2% magical damage, +2% skill power
- Each point of Vitality: +20 max HP, +0.5 HP/sec regen
- Each point of Fortune: +3% crit damage, +1% rare drop chance

**Base Stats by Class:**
- **Warrior:** 15 STR, 8 DEX, 5 INT, 12 VIT, 5 FOR (Total: 45)
- **Mage:** 5 STR, 8 DEX, 15 INT, 8 VIT, 9 FOR (Total: 45)
- **Ranger:** 8 STR, 15 DEX, 7 INT, 8 VIT, 7 FOR (Total: 45)

**Stat Growth:**
- Gain +3 stat points per character level (distribute freely)
- Training Ground can provide permanent +1 to specific stats (expensive long-term investment)
- Gear provides bonus stats (explained below)

### Equipment System

**Gear Slots (4 Total):**
1. **Weapon** - Primary stat scaling + damage
2. **Armor** - Defense + secondary stats
3. **Accessory** - Pure stat bonuses
4. **Relic** - Special build-defining properties

**Gear Stat Structure:**
Each piece has:
- **Primary Stat** (always present, scales with rarity)
  - Example: "Iron Sword: +15 Strength"
- **Secondary Stats** (1-3, random on drop)
  - Example: "+8 Vitality, +5 Dexterity"
- **Enhancement Level** (0-10)
  - Each level: +10% to ALL stats on the item
  - Example: Iron Sword +5 = +22 STR (15 × 1.5), +12 VIT, +7 DEX
- **Enchantment Slots** (0-3, can be unlocked)
  - Add special effects (covered in next section)

**Rarity Tiers:**
- Common (white): 1 primary stat
- Uncommon (green): 1 primary + 1 secondary
- Rare (blue): 1 primary + 2 secondary
- Epic (purple): 1 primary + 3 secondary + 1 enchantment slot
- Legendary (orange): 1 primary + 3 secondary + 2 enchantment slots + unique effect

**Enchantment Effects (Added via Workshop):**
Enchantments add percentage-based bonuses, not flat numbers:
- "+15% Physical Damage"
- "+10% Attack Speed"
- "+20% Critical Damage"
- "+8% Movement Speed"
- "+12% Skill Cooldown Reduction"
- "Restore 2% HP on kill"
- "Attacks have 10% chance to stun for 1 second"

**Why This Works:**
- Stats are the universal language (everything scales from them)
- Gear = stats, Enhancement = more stats, Enchantments = stat multipliers
- Simple to understand, deep to optimize
- No confusing "weapon damage" vs "ability damage" systems

---

## Ability System: Hybrid Approach

### The Best of Both Worlds
**Permanent Abilities (Customizable Foundation)** + **Random Upgrades (Run Variety)**

### Core Combat Structure

**Always Available (Class-Specific):**
- **Left Click:** Basic Attack (auto-aims at nearest enemy)
  - Warrior: Sword swing (melee, cleave)
  - Mage: Magic bolt (ranged, single target)
  - Ranger: Bow shot (ranged, piercing)
  - Scales with your stats automatically
  - No cooldown, no cost

- **Right Click:** Special Attack (manual aim, cooldown)
  - Warrior: Shield bash (3 sec cooldown, stuns)
  - Mage: Fireball (4 sec cooldown, AOE)
  - Ranger: Charged shot (5 sec cooldown, high damage)
  - Scales with your stats + skill power
  - Core identity move, always useful

- **Spacebar:** Dash/Dodge Roll
  - Universal movement ability
  - 2 second cooldown
  - Brief invulnerability frames
  - Mobility and survival tool

### Skill Loadout System (Pre-Run Customization)

**Before each run, equip 3 Active Skills from your unlocked pool:**

**Skill Slots:**
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

**Example:**
- **Whirlwind (Q)**
  - Lv1: Spin for 3 seconds, dealing damage around you
  - Lv2: Spin for 5 seconds, +50% damage
  - Lv3: Enemies hit are pulled toward you

### In-Run Progression (Roguelite Elements)

**Level-Up System:**
Every time you level up during a run (by collecting XP shards), choose 1 of 3 random upgrades:

**Upgrade Types:**

1. **Skill Modifiers** (40% chance)
   - Reduce cooldowns on your equipped skills
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
- End of run: Your Fireball is now a chaining nuke, your Whirlwind is a crowd control monster

**Why This Hybrid System Works:**

**Customization (Pre-Run):**
- You build your "deck" of abilities
- Experiment with different combinations
- Long-term goals (unlocking and leveling skills)
- Feels like YOUR character

**Variety (In-Run):**
- Every run plays differently based on upgrade luck
- Exciting level-up moments
- Adaptation challenges ("I didn't get cooldown reduction, so I'll focus on attack speed")
- Replayability without starting from zero

**Player Agency:**
- You control the foundation (skill loadout)
- RNG controls the spice (modifiers)
- Bad luck doesn't ruin a run (you still have your core skills)
- Good luck makes you feel OP (stacked upgrades)

### Skill Unlock Pace

**Early Game (Floors 1-3):**
- 2 skills per slot unlocked (6 total choices)
- Focused learning curve
- Clear identity per class

**Mid Game (Floors 4-7):**
- 4 skills per slot unlocked (12 total)
- Build diversity emerges
- Start experimenting with synergies

**Late Game (Floors 8-12):**
- 6+ skills per slot (18+ total)
- Deep customization
- Theory-crafting viable builds
- Meta emerges

**Skill Leveling Investment:**
- Level 2 upgrades: 5 training points (affordable, everyone does this)
- Level 3 upgrades: 15 training points (commitment, only for favorites)
- Forces meaningful choices (can't max everything)

---

## The Central Gameplay Loop

### Active Phase: The Climb
**What You Do:**
- Enter a floor of the tower
- Fight waves of monsters (Vampire Survivors style auto-combat + manual aiming)
- Collect XP shards to level up and choose skill upgrades during the run
- Gather loot drops (equipment, crafting materials, gold)
- Defeat the floor boss
- Extract with your rewards OR push to the next floor (risk/reward)

**What You Gain:**
- Character experience and levels (permanent)
- Equipment and gear for your character
- Raw crafting materials (ore, essence shards, monster parts)
- Gold currency
- Floor completion unlocks

### Idle Phase: The Base
**What Happens:**
- The **Workshop** slowly converts raw materials into refined components
- The **Treasury** generates a small trickle of gold (based on your highest floor reached)
- The **Research Lab** studies monsters you've defeated, unlocking bonuses
- The **Training Ground** accumulates training points

**What You Get When You Return:**
- Refined materials ready to use for crafting/upgrading
- Bonus gold to spend
- Research completion notifications (new abilities unlocked, drop rate bonuses)
- Training points to spend on efficiency upgrades

---

## Core Systems (Simplified Start)

### 1. Combat & Runs
**The Basics:**
- Choose a class (Warrior, Mage, Ranger to start)
- Enter a floor with your equipped gear
- Survive waves, level up, collect loot
- Boss appears after X minutes or X kills
- Choose to extract (keep all loot) or continue (risk losing some loot but better rewards)

**Progression:**
- Each floor gets harder but drops better loot
- Character levels carry over (permanent power)
- Gear persists between runs (equip your best loadout)

### 2. The Workshop (Idle Crafting & Enhancement)
**Purpose:** Converts raw materials into gear enhancements and consumables

**Core Material Types:**
1. **Essence Shards** (magical energy)
   - Dropped by: All monsters (common)
   - Used for: Adding enchantments to gear, crafting consumables
   - Refines into: Concentrated Essence (takes 15 min idle)

2. **Monster Parts** (organic materials)
   - Dropped by: Elite enemies and bosses (uncommon)
   - Used for: Enhancing armor, crafting potions
   - Refines into: Purified Extracts (takes 30 min idle)

3. **Ancient Fragments** (tower relics)
   - Dropped by: Floor bosses (rare)
   - Used for: Upgrading legendary gear, unlocking new enchantment slots
   - Refines into: Restored Relics (takes 60 min idle)

**How Enhancement Works:**
- Gear DROPS from combat (you don't craft base items)
- Each gear piece has:
  - Base stats (fixed, determined by rarity)
  - Enhancement level (0-10, increases base stats)
  - Enchantment slots (0-3, adds special effects)

**Workshop Process:**
1. **Refining** (Idle): Raw materials → Refined materials
2. **Enhancement** (Active): At Blacksmith, spend refined materials to:
   - Upgrade enhancement level (+10% stats per level, costs Concentrated Essence)
   - Add enchantments to open slots (costs Purified Extracts)
   - Unlock additional enchantment slots (costs Restored Relics)

**Example Enhancement Path:**
- Find: Iron Sword (10 damage, 0 enchantments)
- Enhance to +5: (15 damage, costs 50 Concentrated Essence)
- Add enchantment: "Fire Strike" (costs 10 Purified Extracts)
- Unlock 2nd slot: (costs 1 Restored Relic)
- Add enchantment: "Life Steal" (costs 15 Purified Extracts)

**Why This Works:**
- Drops give you the base excitement ("I got a legendary!")
- Enhancement gives you long-term goals for each piece
- Idle time processes the materials you farmed actively
- Clear material → upgrade path (not a confusing web)

**Workshop Upgrades:**
- Increase refining speed (30% faster per level)
- Add queue slots (process multiple materials at once)
- Reduce material costs (10% discount per level)
- Unlock higher tier refinements (for legendary gear)

### 3. The Treasury (Idle Gold)
**Purpose:** Provides flexible currency for player agency and convenience

**How It Works:**
- Generates gold per hour based on highest floor cleared
- Caps after 8 hours (encourages daily check-ins, not week-long absences)
- Formula: (Highest Floor × 10) gold per hour
- Example: Floor 5 cleared = 50 gold/hour, max 400 gold when you return

**Gold's Clear Purposes:**

**1. Respeccing (Player Agency)**
- Cost: 100 gold × character level
- Lets you completely reset skill choices
- Encourages experimentation without being punished

**2. Consumables (Active Play Support)**
- Health Potions: 50 gold (instant heal in run)
- Damage Buffs: 100 gold (15 min damage boost in run)
- XP Boost: 150 gold (30 min +50% XP in run)
- Allows underprepared players to push higher floors

**3. Salvaging (Resource Conversion)**
- Sell unwanted gear for gold
- Mechanic: "Salvage" gear for 50% of its enhancement investment back
- Prevents inventory bloat, gives bad drops purpose

**4. Workshop Rushing (Convenience, Not Power)**
- Cost: 10 gold per minute remaining on refining timer
- For impatient players, but never required
- Example: 30 min refinement = 300 gold to instant-complete

**5. Base Upgrades (Long-term Sinks)**
- Unlock facility upgrades (Workshop queue slots, Treasury cap increase)
- Costs scale significantly (500 → 2000 → 5000 gold)
- Always something to save toward

**Why This Works:**
- Every gold source has a clear decision attached ("Do I respec? Buy potions? Upgrade Treasury?")
- Never feels like "just a number going up"
- Casual players get steady income, active players earn it faster through salvage
- No dead-end currencies or materials

**Treasury Upgrades:**
- Increase generation rate (+20% per level)
- Increase cap duration (8h → 12h → 16h)
- Unlock "interest" mechanic (unspent gold gains 5% per day, max 1000)

### 4. Research Lab (Idle Monster Study)
**Purpose:** Unlocks combat bonuses and new options

**How It Works:**
- Automatically studies monsters you've killed (tracks kill count)
- Each monster type has research tiers (10 kills, 50 kills, 200 kills)
- Research progresses while idle (1 tier per 2 hours of offline time)
- Can only research monsters you've personally killed

**Active Benefits:**
- Tier 1: +5% drop rate from that monster
- Tier 2: Unlock a new skill related to that monster type
- Tier 3: Bonus damage vs that monster type

**Upgrades:**
- Research multiple monsters simultaneously
- Faster research speed
- Unlock "monster essence" drops for special crafting

### 5. Training Ground (Idle Efficiency Points)
**Purpose:** Long-term meta-progression separate from character power

**How It Works:**
- Accumulates 1 Training Point per hour (max 24 points, then stops)
- Spend Training Points on permanent efficiency bonuses

**Training Options:**
- **Loot Luck:** +2% rare drop chance per point (max 10 points)
- **Experience Boost:** +5% XP gain per point (max 10 points)
- **Material Yield:** +10% extra materials from drops per point (max 5 points)
- **Starting Power:** Begin runs at level 2/3/4 (costs 5/10/15 points)

**Why This Works:**
- Doesn't increase combat stats directly
- Makes active play MORE rewarding
- Gives something meaningful to spend on even without playing

---

## The Hub: Your Base

### Layout (Simple to Start)
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

### Visual Feedback
- Facilities glow when they have something ready to collect
- Progress bars show idle timers
- Notifications appear when you open the game: "Workshop completed 3 items!" "Treasury full: 1,450 gold!"

---

## Progression Flow Example

### Day 1 - New Player
1. Tutorial run on Floor 1, learn combat basics
2. Extract with basic loot and materials
3. Unlock Workshop and Treasury
4. Deposit materials in Workshop (start first refinement)
5. Close game

### Day 2 - Return
1. Collect refined gem and 200 gold from idle earnings
2. Use gem to upgrade weapon at Blacksmith (+10% damage)
3. Run Floor 1 again, feels stronger, push to Floor 2
4. Defeat Floor 2 boss, extract with better loot
5. Unlock Research Lab
6. Set Lab to study skeletons, deposit new materials in Workshop
7. Close game

### Day 3-7 - Early Loop
- Daily check-ins to collect idle progress
- Use refined materials to steadily upgrade gear
- Push to higher floors (unlock Treasury bonuses)
- Complete monster research (unlock new skills)
- Accumulate training points (invest in Loot Luck)

### Week 2+ - Established Loop
- Strong enough to farm Floor 5-7 efficiently
- Workshop constantly refining materials for next upgrades
- Research Lab cycling through monster types
- Training Ground bonuses making runs more efficient
- Can choose to grind actively for hours OR check in daily
- Both playstyles progress meaningfully

---

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

---

## Why This Design Works

### For Active Players:
- Fighting is always the fastest way to progress
- Idle systems give you something to do with your loot (refining, research)
- No waiting—you can always jump in and play
- Grinding is rewarded with direct power gains

### For Casual Players:
- Check in once a day for meaningful progress
- Idle systems catch you up slowly
- Never feel pressured to play for hours
- Training Points provide long-term goals

### For Both:
- Clear feedback loop: fight → collect → refine → upgrade → fight stronger
- Multiple progression paths (gear, research, training, floors)
- Complexity increases gradually (start with 2 systems, unlock more)
- Respects player time (idle has caps, active is efficient)

---

## Launch Scope (MVP)

**Must Have:**
- 3 classes
- 5 floors with bosses
- Workshop (1 refinement type to start)
- Treasury (basic gold generation)
- Basic combat with 10-15 skills per class
- Equipment system with 4 slots (weapon, armor, accessory, relic)

**Phase 2 (Post-Launch):**
- Research Lab system
- Training Ground
- More floors (up to 12)
- More classes
- Expanded refinement types
- Prestige/New Game+ system

**Phase 3 (Content Updates):**
- Seasonal events
- New monster types
- Legendary equipment tiers
- Base customization
- Challenge modes

---

## Key Design Principles

1. **Idle enhances, never replaces** - Active play is always most rewarding
2. **Clear feedback** - Players always know what idle progress earned them
3. **No punishment** - Missing a day doesn't ruin progress (caps prevent FOMO)
4. **Respect time** - Both 15-minute sessions and 2-hour grinds feel productive
5. **Gradual complexity** - Start simple, layer systems as players advance
6. **Fair monetization** - Never pay-to-win, always pay-for-convenience or cosmetics

---

## Next Steps

1. **Prototype combat** - Get the feel of fighting right first
2. **Build Workshop system** - Test idle crafting loop
3. **Create 3 floors** - Establish difficulty curve
4. **Add Treasury** - Verify idle income feels balanced
5. **Playtest loop** - Run, refine, upgrade, repeat
6. **Iterate** - Based on what feels fun vs tedious