# Systems & Progression

## Overview

Zenith Rising uses a **stat-based power system** where everything scales from 5 core character stats. Gear provides stats, skills scale from stats, and upgrades modify stats or add effects.

**Design Philosophy:**

- Stats are the universal language
- Gear = stats, Enhancement = more stats
- Simple to understand, deep to optimize
- No hidden formulas or complex interactions

---

## Character Stats System ✅ MVP - IMPLEMENTED

### The 5 Core Stats

Every character has 5 base stats that define their combat capabilities:

#### **Strength (STR)** - Physical Power

**Primary Effect:** +3% Physical Damage per point
**Secondary Effect:** +10 Max HP per point

- Scales physical weapon attacks
- Scales physical skills (Whirlwind, melee abilities)
- Warriors prioritize this
- No cap

#### **Intelligence (INT)** - Magical Power

**Primary Effect:** +3% Magical Damage per point
**Secondary Effect:** +2% Skill Cooldown Reduction per point

- Scales magical weapon attacks
- Scales magical skills (Fireball, elemental abilities)
- Mages (Elias) prioritize this
- No cap

#### **Agility (AGI)** - Speed & Precision

**Primary Effect:** +2% Attack Speed per point
**Secondary Effect:** +1% Crit Chance per point

- Faster basic attacks
- Faster skill animations
- More frequent crits
- Rangers (Aria) prioritize this
- Crit Chance capped at 50%

#### **Vitality (VIT)** - Survivability

**Primary Effect:** +25 Max HP per point
**Secondary Effect:** +0.5 HP Regeneration per second per point

- Pure survival stat
- Valuable for all playstyles
- Tank builds prioritize this
- No cap

#### **Fortune (FOR)** - High-Risk Reward

**Primary Effect:** +2% Crit Damage per point
**Secondary Effect:** +1% Rare Drop Rate per point

- Bigger crits (base crit damage is 150%)
- Better loot quality
- Synergizes with AGI (crit chance)
- Crit-focused builds prioritize this
- No cap on crit damage

### Damage Types

**Physical Damage:**

- Scales with STR
- Weapon-based attacks (most weapons)
- Melee skills, physical projectiles
- Examples: Whirlwind, basic sword attacks

**Magical Damage:**

- Scales with INT
- Spell-based attacks (staves, wands)
- Elemental skills, magical projectiles
- Examples: Fireball, lightning skills

**Weapon Determines Damage Type:**

- Basic Attack (Left Click): Uses weapon's damage type
- Special Attack (Right Click): Uses weapon's damage type
- Active Abilities (Q/W/E/R): Individual damage type per skill

### Class Affinities

**Marcus (Warrior):**

- Prioritizes: STR, VIT
- Playstyle: Physical damage, tanky
- Example spread: 8 STR, 5 VIT, 2 AGI

**Aria (Ranger):**

- Prioritizes: AGI, FOR
- Playstyle: Fast attacks, high crits
- Example spread: 6 AGI, 6 FOR, 3 STR

**Elias (Mage):**

- Prioritizes: INT, FOR
- Playstyle: Magical damage, big crits
- Example spread: 8 INT, 5 FOR, 2 VIT

### Starting Stats

**All characters start at 0/0/0/0/0 base stats.**

**Character Creation:**

- Distribute 15 points freely
- No class restrictions (can build Marcus as mage)
- Establish your initial build identity

**Example Distributions:**

- Tank Warrior: 5 STR, 0 INT, 0 AGI, 8 VIT, 2 FOR
- Glass Cannon Mage: 0 STR, 10 INT, 0 AGI, 0 VIT, 5 FOR
- Hybrid Ranger: 3 STR, 3 INT, 6 AGI, 3 VIT, 0 FOR
- Balanced: 3/3/3/3/3

### Stat Growth

**Permanent Progression:**

- Gain +1 stat point per Character Level
- Character Level cap: 100 (for MVP, expandable)
- Total possible: 115 stat points (15 starting + 100 Character Levels)
- Respec available (costs 100 gold × Character Level)

**Why This Works:**

- Clear choices (primary + secondary effects)
- Class identity (STR for warriors, INT for mages, AGI for rangers)
- Hybrid builds viable (split points between multiple stats)
- Respec is expensive but not prohibitive
- Long-term goals (100 levels of customization)

---

## Equipment System 📝 Phase 4 - PLANNED

### Gear Acquisition: Drops + Deterministic Crafting

**Philosophy:** Drops provide 80% of needs, Forge crafting provides 100%

**Two-Part System:**
1. **Gear Drops** - Random stats, varying quality
2. **Forge Crafting** - Deterministic improvement via Forging Potential

### Forging Potential (FP) System

**Core Mechanic:**
- All gear drops with 50-200 FP (based on rarity and zone level)
- Each craft action costs 5-20 FP
- At 0 FP, item is "complete" → natural stopping point
- Creates decision: Keep crafting or save FP?

**Crafting Actions:**

**1. Add Affix** (5-10 FP cost)
- Choose which stat to add (Strength, Crit, Attack Speed)
- Adds Tier 1 of that stat
- Cost: 10 Refined Essence + 100 Gold

**2. Upgrade Affix** (8-15 FP per tier)
- Increase tier: T1 → T2 → T3 → T4 → T5
- Each tier significantly stronger
- Cost: 5 Ingots + 200 Gold per tier
- Diminishing returns (T4→T5 costs more than T1→T2)

**3. Reroll Numeric Value** (3-5 FP)
- Keep affix tier, reroll number within range
- Cheap optimization after getting desired affixes
- Cost: 3 Gems + 50 Gold

### Affix Tier System

- **T1-T5:** Craftable at Forge (deterministic)
- **T6-T7:** DROP-ONLY (cannot be crafted)
- **Design Intent:** Rare drops remain exciting even with crafting

**Example:** Finding gear with T6 Life is valuable even if other stats are mediocre—you craft offense around it.

### Why FP Creates Optimization Puzzles

**Decision Points:**
1. **Which gear to invest in?** High FP bad stats vs Low FP good stats
2. **How much to optimize?** "Good enough" vs "Perfect"
3. **Build focus:** Target specific stats for synergies
4. **When to start fresh?** Found new item, current has 10 FP left

**Multiple Valid Strategies:**
- **Safe:** Stop at "good enough" (T3-T4), save FP for future crafts
- **Aggressive:** Push to T5 everything, risk running out of FP
- **Experimental:** Unusual stat combinations for hybrid builds

### Gear Slots & Rarities

**4 Equipment Slots:**
1. **Weapon** - Primary damage + stat bonuses
2. **Armor** - Defense + secondary stats
3. **Accessory** - Pure stat bonuses
4. **Relic** - Build-defining properties (Phase 5+)

**Rarity determines FP range:**
- Common: 50-80 FP
- Uncommon: 70-110 FP
- Rare: 100-150 FP
- Epic: 130-180 FP
- Legendary: 150-200 FP + higher stat rolls

**Drop Rates:**
- Common: 60%
- Uncommon: 25%
- Rare: 10%
- Epic: 4%
- Legendary: 1%

**Boss Drops:** Guaranteed gear drop, 50% chance for Rare+

### Gear Management

**Inventory:**
- Unlimited storage (for MVP)
- Sort by rarity, FP remaining, type
- Quick-equip from inventory
- Highlight high-FP items

**Salvaging:**
- Convert unwanted gear → gold
- Returns materials based on affixes added
- Prevents inventory bloat
- Gives bad drops purpose

**Respec-Friendly:**
- Can freely swap gear
- Try different builds
- No gear binding or soulbound

---

## Material Economy 📝 Phase 4-5 - PLANNED

> **🎯 UPDATED VISION:** Five material types (Essence, Ore, Fragments, Souls, Crystals) enable diverse crafting and upgrade paths while preventing single-resource bottlenecks.

### Design Philosophy

**Why Five Materials:**
- Prevents bottlenecks (single resource can't gate everything)
- Creates interesting acquisition strategies (which content to farm?)
- Enables parallel progression systems
- Different rarities for different purposes

**Core Principle:** Materials ONLY from active play (dungeons). Idle systems process materials, never generate them.

### The Five Material Types

**1. Essence (Common)** - Universal material
- **Drops from:** All enemies (100% drop rate)
- **Amount:** 1-3 per enemy, scales with tier
- **Used for:** Basic forge crafts, workshop fuel, common upgrades

**2. Ore (Uncommon)** - Gear enhancement
- **Drops from:** Elite enemies, mini-bosses (50% drop rate)
- **Amount:** 1-2 per drop
- **Used for:** Gear affix upgrades, workshop expansion

**3. Fragments (Rare)** - Advanced crafting
- **Drops from:** Floor completion, zone milestones
- **Amount:** 3-5 per floor clear
- **Used for:** High-tier forge actions, workshop speed upgrades

**4. Souls (Very Rare)** - Prestige currency
- **Drops from:** Bosses only (100% drop rate)
- **Amount:** 1 per boss
- **Used for:** Ascension system, unlock workshop tiers

**5. Crystals (Ultra Rare)** - Endgame optimization
- **Drops from:** Challenge runs, achievements
- **Amount:** 1-3 per challenge
- **Used for:** Endgame forge actions, ascension tree unlocks

### Material Flow

```
Active Dungeon Run → Raw Materials (direct to inventory)
↓
Workshop Processing (3-5 parallel slots, 4-12 hours)
  • Essence → Refined Essence
  • Ore → Ingots
  • Fragments → Gems
↓
Forge Crafting (Refined materials + gold)
  • Add/Upgrade gear affixes
  • Consumes Forging Potential (FP)
↓
Better Gear → Higher Zones → More Materials per Run (1.5-3x)
```

**Why This Works:**
- Active play required (no idle material farming)
- Multiple parallel goals (which material to prioritize?)
- Clear upgrade paths (know what you're farming for)
- Natural bottlenecks prevent instant power spikes

---

## Idle Systems 📝 Phase 5 - PLANNED

### Design Constraints (Preserved)

**Golden Rules:**
1. Only processes what you actively farmed ✅
2. Caps at 12-24 hours maximum (not 8 hours)
3. Never more rewarding than active play (Active = **3-5x better**) ✅
4. No "wait to play" mechanics ✅
5. No FOMO pressure ✅

### Workshop (Material Processing)

**Purpose:** Convert raw materials → refined components for forge crafting

**How It Works:**

```
Raw Material → Refined Material
Processing Time: 4-12 hours depending on tier
Offline Processing: Uses DateTime
Queue: 3-5 slots (upgradeable)
```

**Conversion Options:**
- **Fast** (4 hours): Essence → Refined Essence
- **Medium** (8 hours): Ore → Ingots
- **Slow** (12 hours): Fragments → Gems

**Upgrades (Purchased with Gold):**

**Tier 1** (Early game, 1k-5k gold):
- Processing Speed I: 1.2x faster
- Extra Slot: Unlock 4th slot
- Bulk Convert: Process 10x materials (same time)

**Tier 2** (Mid game, 20k-50k gold):
- Processing Speed II: 1.5x faster
- Quality Improvement: 10% chance for 2x output
- Auto-Collect: Completed conversions auto-deposit

**Tier 3** (Late game, 100k+ gold):
- Master Craftsman: 2x speed
- Overflow Slot: 5th processing slot
- Legendary Refinement: 5% chance for rare upgrade

**Between-Run Interaction:** 2-5 minutes per session

### Treasury (Idle Gold Generation)

**Purpose:** Provide steady income for forge costs and workshop upgrades without forcing gold farming

**Formula:** Gold/hour = (Highest Zone Reached)² × 10
- Zone 20 → 4,000 gold/hour
- Zone 50 → 25,000 gold/hour
- Zone 100 → 100,000 gold/hour

**Accumulation Cap:** 12 hours maximum (prevents weekly check-ins)

**Active Bonus:** Completing dungeons gives **3-5x more gold** than idle time

**Upgrades (Gold Cost):**
- Treasury Expansion: Increase cap (12h → 18h → 24h)
- Interest Rate: +10% generation per tier (5 tiers)
- Instant Collection: Tap for 2 hours worth (1/day)

**Between-Run Interaction:** 30 seconds (collect and see satisfying totals)

**Why This Works:**
- Never blocks gameplay ✅
- Active play significantly better (3-5x multiplier) ✅
- Scales with progression (higher zones = better rate) ✅
- Appointment mechanic (12-hour cap) without punishment ✅

### Gold Uses

1. **Workshop upgrades** (speed, slots, quality)
2. **Treasury upgrades** (expansion, interest rate)
3. **Forge costs** (material + gold for each craft)
4. **Respec stats** (100g × Character Level)
5. **Consumables** (health potions, buffs - Phase 6+)

### Workshop vs Treasury Comparison

| System   | Active Input  | Passive Output    | Purpose              |
| -------- | ------------- | ----------------- | -------------------- |
| Workshop | Raw materials | Refined materials | Enable forge crafting |
| Treasury | Zone clears  | Gold              | Fund upgrades & crafts    |

**Both respect the same rules:**

- ✅ Only rewards past active play
- ✅ Caps prevent FOMO
- ✅ Offline-friendly (DateTime)
- ✅ Never better than active grinding (3-5x multiplier)

---

## Ascension System 📝 Phase 6 - PLANNED

### Purpose

Long-term meta-progression providing goals spanning weeks while maintaining challenge.

### When to Ascend (Player Choice)

**Soft Reset (every 15-25 hours of play):**
- **Keep:** Workshop upgrades, forge gear, treasury level, skill mastery
- **Reset:** Dungeon progress (back to Zone 1), character level, per-run upgrades
- **Gain:** Ascension Points (based on zones cleared + challenges completed)
- **Optimal Timing:** Player discovery through experimentation

### Ascension Tree (Three Branches)

**Combat Branch:**
- +5% damage (1 AP)
- +10% HP (2 AP)
- Start with extra life (5 AP)
- Enemies drop +25% materials (10 AP)

**Economy Branch:**
- Treasury generates 1.2x gold (1 AP)
- Workshop processes 1.2x faster (2 AP)
- Forge costs 10% less materials (5 AP)
- Start runs with gold (10 AP)

**Utility Branch:**
- Unlock 4th workshop slot (3 AP)
- Extended offline processing (5 AP)
- Unlimited treasury collection (8 AP)
- Free forge respec (12 AP)

### Power Budget

- Ascension provides ~5-10% total power
- Focus on **convenience and variety**, not pure stats
- Diminishing returns: First 10 AP huge, next 50 incremental

### Build Diversity

- Multiple viable paths (Combat rush, Economy scaling, Utility QoL)
- Free Respec: Costs 1 AP, encourages experimentation
- Always have next goal (short/mid/long-term AP targets)

---

## Power Budget: Keeping Systems Optional

### Critical Balance Target

**ALL content must be completable with:**
- Base stats from dungeon runs
- Random gear drops (no forge crafting)
- Zero workshop upgrades
- Zero ascension points

**Target:** New player should reach Zone 30-40 with NO idle system engagement.

### Optional Power Breakdown

| System | Power Contribution | Purpose |
|--------|-------------------|----------|
| Core stats from dungeons | 50% | Baseline progression |
| Gear from drops | 30% | RNG excitement |
| Forge crafting | 15-20% | Targeted optimization |
| Ascension tree | 5-10% | Long-term scaling |
| Workshop efficiency | 0% direct | Enables other systems faster |

**Total optional power:** 25-30%

### Why This Target Works

**Research-Validated Sweet Spot:**
- <10% = "Barely noticeable" (not worth pursuing)
- **25-40% = "Worth pursuing"** ← OUR TARGET
- >60% = "Mandatory despite label" (anti-pattern)

**25-30% Power Feels:**
- Worth the time investment
- Not mandatory for progression
- Creates optimization depth
- Respects different playstyles (active grinders vs. idle optimizers)

---

## Progression Loops

### Short-Term Loop (Per Run)

```
Enter Floor
  ↓
Fight Waves → Collect Power XP → Gain Power Levels → Choose Power Upgrades
  ↓
Defeat Boss → Collect Loot (gear, materials, gold)
  ↓
Extract or Continue
  ↓
Return to Hub → Deposit Materials in Workshop
  ↓
Repeat
```

**Duration:** 25-30 minutes per full run
**Rewards:** Immediate temporary power (Power Upgrades), loot drops, Character XP

### Mid-Term Loop (Session)

```
Multiple Runs
  ↓
Gain Character Levels → Allocate Stat Points (permanent)
  ↓
Find Better Gear → Equip Upgrades (persistent)
  ↓
Collect Refined Materials → Enhance Gear (permanent)
  ↓
Feel Permanently Stronger
  ↓
Repeat
```

**Duration:** 1-2 hours per session
**Rewards:** Permanent power (Character stats, enhanced gear)

### Long-Term Loop (Days/Weeks)

```
Daily Check-In
  ↓
Collect Treasury Gold
  ↓
Collect Refined Materials
  ↓
Forge Gear → Spend Gold on Workshop Upgrades
  ↓
Push Further in Tower
  ↓
Unlock Higher Zones → Better Treasury Rate + More Materials
  ↓
(Every 15-25 hours) Consider Ascension
  ↓
Ascend → Gain Ascension Points → Unlock Permanent Bonuses
  ↓
Repeat Climb with Permanent Advantages
```

**Duration:** 1-2 weeks to reach first ascension, ongoing cycles
**Rewards:** Mastery, optimization, permanent progression

---

## Progression Pace

### Phase 2 Targets (MVP)

**Character Levels:**

- Level 1-10: 2-3 hours (early game)
- Level 10-25: 10-15 hours (mid game)
- Level 25-50: 40-60 hours (end game)
- Level 50+: Slow burn (prestige content)

**Gear Progression:**

- First full set: 3-5 hours
- Full Rare set: 10-15 hours
- Full Epic set: 30-40 hours
- Any Legendary: 50+ hours

**Floor Progression:**

- Floor 1: Always accessible
- Floor 3: ~5 hours
- Floor 5: ~15 hours
- Consistent clears: ~30 hours

**Enhancement:**

- +5 gear: 5-10 hours per piece
- +10 gear: 30-40 hours per piece
- Full +10 set: 100+ hours

### Retention Goals

**1 Week:**

- 60% of players still playing
- Average: Floor 3-4 clears
- 2-3 gear pieces +5
- Character level 15-20

**1 Month:**

- 30% of players still playing
- Average: Floor 5 consistent clears
- Most gear +8 or higher
- Character level 35-45

**3 Months:**

- 10% of players still playing
- Full +10 gear
- Character level 50+
- Optimized builds

---

## Monetization (If Applicable)

### Fair Model Philosophy

**Core Principle:** Never sell power

**What We Can Sell:**

- ✅ Cosmetic skins (characters, weapons)
- ✅ Visual effects (death animations, auras)
- ✅ Battle pass (cosmetics + small XP boost)
- ✅ Workshop queue slots (convenience)
- ✅ Inventory tabs (organization)

**What We NEVER Sell:**

- ❌ Stats or damage
- ❌ Gear or materials
- ❌ Character levels
- ❌ Floor unlocks
- ❌ Faster refinement (beyond queue slots)

### Battle Pass Example

**Free Track (All Players):**

- Cosmetic rewards every 5 levels
- Small gold bonuses
- 1-2 free skins

**Premium Track ($10):**

- More cosmetics every level
- +10% XP boost (not power, faster progression)
- Exclusive skins
- Workshop queue slot
- No gameplay advantages

**Season Length:** 3 months

---

## Next Steps

**Related Documentation:**

- Combat details: [`combat-skills.md`](combat-skills.md)
- Narrative context: [`narrative-framework.md`](narrative-framework.md)
- **Idle systems implementation:** [`../02-IMPLEMENTATION/idle-systems-implementation.md`](../02-IMPLEMENTATION/idle-systems-implementation.md) - Detailed technical guide for Forge, Workshop, Treasury, Ascension
- Technical skill system: [`../02-IMPLEMENTATION/skill-system-architecture.md`](../02-IMPLEMENTATION/skill-system-architecture.md)

**Current Implementation Status:**
See [`../../CLAUDE.md`](../../CLAUDE.md) for what's actually built vs planned.

---

_Last updated: Idle systems vision overhaul - 5 materials, Forge/FP system, Ascension, Power Budget_
_Living document - Update when systems change_
