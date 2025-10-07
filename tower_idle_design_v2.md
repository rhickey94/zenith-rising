# Tower Ascension: Idle Roguelite Design Document

## Core Concept
A roguelite action-RPG where active combat grants power, while idle time improves your efficiency and unlocks new possibilities. Fight your way up the tower actively, then let your base work for you between sessions.

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