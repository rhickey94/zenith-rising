# Glossary

## Overview

This document defines all key terms used throughout Zenith Rising's design documentation. Terms are organized by category for easy reference.

---

## Progression Terms

### Power Level (Temporary)
**Range:** 1-20 per run  
**Persistence:** Resets every run  
**XP Source:** Killing enemies during active combat  
**Rewards:** Choose 1 of 3 random upgrades per level  
**Purpose:** Short-term tactical power scaling within a single run  

**Example:** "Marcus reached Power Level 12 and chose +25% damage upgrade"

### Character Level (Permanent)
**Range:** 1-100 (MVP cap, expandable)  
**Persistence:** Never resets, permanent progression  
**XP Source:** Run performance (floors cleared, bosses killed, bonuses)  
**Rewards:** +1 stat point to allocate per level  
**Purpose:** Long-term strategic character building  
**Tracking:** Separate per character (Marcus level 15, Aria level 8, Elias level 22)  

**Example:** "After completing Floor 5, Marcus gained enough Character XP to reach Level 16"

### Power XP
**Definition:** Experience points earned toward the next Power Level  
**Source:** Killing enemies during a run  
**Persistence:** Temporary - resets at the end of each run  
**Display:** Shown as a bar in the HUD during combat  

### Character XP
**Definition:** Experience points earned toward the next Character Level  
**Source:** Performance metrics at end of run (floors cleared × 100, bosses killed × 150, bonuses)  
**Persistence:** Permanent - accumulates across all runs  
**Display:** Shown on results screen after run ends  

**Example Calculation:**
- Base participation: 50 XP
- Floors cleared (5): 500 XP
- Bosses killed (3): 450 XP
- Final boss defeated: 500 XP bonus
- **Total:** 1,500 Character XP earned

### Power Upgrades
**Definition:** Temporary upgrades chosen when gaining a Power Level  
**Selection:** Choose 1 of 3 random options  
**Examples:** +25% damage, +20% attack speed, restore 50% health  
**Persistence:** Lost at end of run  
**Purpose:** Build variety and run-specific power scaling  

### Stat Points
**Definition:** Permanent points allocated to core stats  
**Source:** +1 per Character Level  
**Total Possible:** 115 (15 from character creation + 100 from levels)  
**Allocation:** Distribute freely across STR/INT/AGI/VIT/FOR  
**Respec Cost:** 100 gold × character level  

---

## Character Stats

### Strength (STR)
**Primary Effect:** +3% Physical Damage per point  
**Secondary Effect:** +10 Max HP per point  
**Scales:** Physical weapon attacks, physical skills  
**Primary Users:** Warriors (Marcus)  

### Intelligence (INT)
**Primary Effect:** +3% Magical Damage per point  
**Secondary Effect:** +2% Skill Cooldown Reduction per point  
**Scales:** Magical weapon attacks, magical skills  
**Primary Users:** Mages (Elias)  

### Agility (AGI)
**Primary Effect:** +2% Attack Speed per point  
**Secondary Effect:** +1% Crit Chance per point  
**Scales:** Attack speed, critical hit chance  
**Primary Users:** Rangers (Aria)  
**Cap:** Crit chance capped at 50%  

### Vitality (VIT)
**Primary Effect:** +25 Max HP per point  
**Secondary Effect:** +0.5 HP Regeneration per second per point  
**Purpose:** Survivability for all playstyles  
**Primary Users:** Tank builds  

### Fortune (FOR)
**Primary Effect:** +2% Crit Damage per point  
**Secondary Effect:** +1% Rare Drop Rate per point  
**Purpose:** High-risk reward scaling  
**Primary Users:** Crit-focused builds  
**Synergy:** Works with AGI (crit chance) for crit builds  

---

## Gear & Equipment Terms

### Gear Rarity Tiers
**Common (Gray):** 50-80 FP, 1-2 affixes (T1-T2)
**Uncommon (Green):** 80-120 FP, 2-3 affixes (T1-T3)
**Rare (Blue):** 120-160 FP, 3-4 affixes (T2-T4)
**Epic (Purple):** 160-200 FP, 4-5 affixes (T3-T5)
**Legendary (Orange):** 180-200 FP, 4-6 affixes (T4-T6, may include T6-T7 drop-only)  

### Forging Potential (FP)
**Definition:** Resource on gear that limits how much it can be crafted
**Range:** 50-200 FP (based on rarity and zone level)
**Consumption:** Each crafting action costs 5-20 FP
**Depletion:** At 0 FP, item cannot be crafted further (natural stopping point)
**Purpose:** Creates optimization decisions and prevents infinite crafting
**System:** Added in Phase 4 via Forge

**Example:**
```
Iron Sword (150 FP):
- Add +STR affix (10 FP) → 140 FP remaining
- Upgrade STR T1→T2 (10 FP) → 130 FP remaining
- Upgrade STR T2→T3 (12 FP) → 118 FP remaining
- Eventually reaches 0 FP (item "complete")
```

### Affixes
**Definition:** Stat modifiers on gear with tier levels
**Tiers:** T1-T7 (T1 weakest, T7 strongest)
**Craftable:** T1-T5 via Forge crafting
**Drop-Only:** T6-T7 (cannot be crafted, only found)
**Examples:** "+15 Strength (T3)", "+12% Crit Chance (T4)"
**System:** Core to Forge system (Phase 4)

**Tier Progression:**
- T1: Base stat value
- T2-T5: Increasing stat values (craftable via Forge)
- T6-T7: Highest values (chase drops from high zones/bosses)

---

## Materials & Resources

### Essence (Common Material)
**Definition:** Universal common material from all enemies
**Drop Source:** All enemies (100% drop rate, 1-3 per enemy)
**States:** Raw Essence → Refined Essence (4 hours in Workshop)
**Purpose:** Basic Forge crafts, workshop fuel, common upgrades
**Usage:** Add T1 affixes (10 Refined Essence)

### Ore (Uncommon Material)
**Definition:** Material from mini-bosses and tough enemies
**Drop Source:** Mini-bosses (100%), elite enemies (30%), zone milestones
**States:** Raw Ore → Ingots (8 hours in Workshop)
**Purpose:** Upgrade affixes to higher tiers
**Usage:** Upgrade T1→T2→T3 (5 Ingots per tier)

### Fragments (Rare Material)
**Definition:** Material from significant zone completion
**Drop Source:** Zone completion rewards, boss chests (50%), challenge runs
**States:** Raw Fragments → Gems (12 hours in Workshop)
**Purpose:** Advanced crafting, reroll numeric values
**Usage:** Reroll affix values (3 Gems)

### Souls (Very Rare Material)
**Definition:** Material from major bosses only
**Drop Source:** Floor bosses (100%), dungeon final bosses (3-5 Souls)
**States:** Raw Souls → Crystallized Souls (12 hours in Workshop)
**Purpose:** Ascension system, high-tier crafts
**Usage:** Ascension tree unlocks, T4-T5 upgrades

### Crystals (Ultra Rare Material)
**Definition:** Endgame material from challenge content
**Drop Source:** Challenge runs with modifiers, achievements, leaderboard rewards
**States:** Pure Crystals (no refinement needed)
**Purpose:** Endgame crafting, rare ascension unlocks
**Usage:** T5 upgrades, special ascension nodes  

### Gold
**Definition:** Universal currency for various purchases
**Sources:**
- Enemy drops during runs (primary source)
- Treasury idle generation based on (Zone² × 10) formula
- Salvaging unwanted gear

**Uses:**
- Forge crafting actions (Add/Upgrade/Reroll affixes)
- Respec stats (100 gold × character level)
- Workshop upgrades (Processing Speed, Extra Slots, Quality improvements)
- Treasury upgrades (Accumulation cap, interest rates)

---

## Combat Terms

### Skill Mastery
**Definition:** Permanent upgrade tiers for skills based on kills  
**Tiers:**
- **Bronze (0 kills):** Base skill
- **Silver (50 kills):** +50% effectiveness
- **Gold (200 kills):** +100% effectiveness + special bonus
- **Diamond (500 kills):** +150% effectiveness + enhanced special bonus (Post-MVP)

**Tracking:** Persists across all runs permanently  
**Purpose:** Rewards skill specialization and build identity  

### Damage Types
**Physical Damage:** Scales with STR, weapon-based attacks, melee skills  
**Magical Damage:** Scales with INT, spell-based attacks, elemental skills  

### Critical Hit
**Crit Chance:** Probability of landing a critical hit (affected by AGI)  
**Crit Damage:** Damage multiplier on critical hits (base 150%, affected by FOR)  
**Example:** 25% crit chance, 200% crit damage means 1 in 4 attacks deals double damage  

---

## Dungeon & Run Terms

### Dungeon
**Definition:** A collection of floors
**Structure:** A varying number of floors
**Completion:** Completing the dungeon unlocks the next dungeon

### Floor
**Definition:** A single stage within a dungeon  
**Structure:** 10 waves of enemies + 1 boss fight  
**Duration:** ~5 minutes per floor  
**Completion:** Defeating the boss unlocks next floor or extraction  

### Wave
**Definition:** A group of enemies that spawn together  
**Count:** 10 waves per floor  
**Scaling:** Enemy count and difficulty increase with each wave  

### Boss
**Definition:** Powerful enemy at the end of each floor  
**Guaranteed Drops:** Gear, materials, gold  
**Increased Rarity:** 50% chance for Rare+, 10% chance for Epic+, 2% chance for Legendary  

### Extract
**Definition:** Option to safely leave the dungeon with all collected rewards  
**Available:** After defeating any floor boss  
**Risk/Reward:** Extract safely or continue to next floor for more rewards  

### Run
**Definition:** A complete dungeon attempt from entry to death or extraction  
**Duration:** 25-30 minutes for full clear (5 floors in Zenith Station MVP)  
**Rewards:** All collected loot, materials, gold, and Character XP based on performance  

---

## Hub & Idle Systems

### Hub
**Definition:** Safe base area between runs
**Functions:**
- Character stat allocation
- Gear management (equip, salvage)
- Forge access (gear crafting)
- Workshop access (material refinement)
- Treasury collection (idle gold)
- Dungeon selection

### Forge
**Definition:** Crafting station for modifying gear using Forging Potential
**Location:** Hub area (Phase 4)
**Requirements:** Gear with remaining FP + refined materials + gold

**Three Crafting Actions:**

**1. Add Affix (5-10 FP)**
- Choose which stat to add (Strength, Crit Chance, etc.)
- Adds Tier 1 of that stat
- Cost: 10 Refined Essence + 100 Gold

**2. Upgrade Affix (8-15 FP per tier)**
- Increase existing affix tier: T1 → T2 → T3 → T4 → T5
- Each tier significantly increases stat value
- Cost: 5 Ingots + 200 Gold per tier (scales up at higher tiers)

**3. Reroll Numeric Value (3-5 FP)**
- Keep affix and tier, reroll the number within range
- Example: +15 STR (T3) → reroll → +18 STR (T3)
- Cost: 3 Gems + 50 Gold

**Design:** Creates optimization puzzles (which gear? how much to invest?)

### Workshop
**Definition:** Idle system for refining raw materials into crafting components
**Function:** Converts 5 material types (Essence→Refined, Ore→Ingots, Fragments→Gems, Souls→Crystallized)
**Processing Times:** 4 hours (Essence), 8 hours (Ore), 12 hours (Fragments/Souls)
**Slots:** 3-5 parallel processing slots (expandable via upgrades)
**Offline:** Uses DateTime for progression (works while game is closed)

**Upgrade Tiers (Phase 5):**
- **Tier 1 (1k-5k gold):** Processing Speed I (1.2x), Extra Slot (+1), Bulk Convert (10x batch)
- **Tier 2 (20k-50k gold):** Processing Speed II (1.5x), Quality Improvement (10% chance 2x output), Auto-Collect
- **Tier 3 (100k+ gold):** Master Craftsman (2x speed), Overflow Slot (5th slot), Legendary Refinement (5% rare upgrade)

### Treasury
**Definition:** Idle system for passive gold generation
**Generation Formula:** (Highest Zone Reached)² × 10 gold/hour
**Accumulation Cap:** 12 hours maximum
**Active Bonus:** Dungeon runs give 3-5x more gold than idle time

**Examples:**
- Zone 20 cleared = (20² × 10) = 4,000 gold/hour → 48,000 gold max (12h)
- Zone 50 cleared = (50² × 10) = 25,000 gold/hour → 300,000 gold max (12h)
- Zone 100 cleared = (100² × 10) = 100,000 gold/hour → 1,200,000 gold max (12h)

**Upgrade Tiers (Phase 5-6):**
- **Expansion Upgrades:** Increase cap (12h → 18h → 24h)
- **Interest Rate Upgrades:** Compound bonus (1.1x → 1.3x → 1.5x)
- **Instant Collection:** Collect 2 hours early (1/day)

### Salvage
**Definition:** Converting unwanted gear into gold
**Returns:** Base gold value (scales with rarity and item level)
**Purpose:** Prevent inventory bloat, give all drops value
**Note:** Cannot recover materials spent on Forge crafting (sunk cost)  

---

## Game Structure Terms

### Phase
**Definition:** Development milestone with specific feature sets
**MVP = Phases 1-3:** Core combat, progression, hub world, and balance systems

**Phase Overview:**
- **Phase 1:** Combat core (proven fun) ✅
- **Phase 2:** Character stats + save system ✅
- **Phase 3:** Hub world + scene flow ✅
- **Phase 3.5:** Warrior skills + balance systems ✅ (in progress)
- **Phase 4:** Gear & Forge system (FP, affixes, crafting)
- **Phase 5:** Idle systems (Workshop, Treasury)
- **Phase 6:** Depth & Ascension (prestige progression)
- **Phase 7:** Endgame (multiple dungeons, challenges)

### Zenith Station
**Definition:** The first dungeon, used for MVP  
**Floors:** 5 floors  
**Theme:** Sci-fi tower facility  
**Duration:** 25-30 minute runs  

### Ascension (Phase 6)
**Definition:** Prestige system for long-term meta-progression
**Timing:** Soft reset every 15-25 hours of play (player choice)
**Mechanics:** Reset dungeon progress (back to Zone 1), keep gear/workshop/treasury
**Rewards:** Ascension Points (AP) based on zones cleared + challenges completed

**Three Ascension Branches:**
- **Combat Branch:** Damage, HP, extra lives, material drop bonuses
- **Economy Branch:** Treasury rates, workshop speed, forge cost reduction
- **Utility Branch:** Extra slots, extended caps, free respec

**Purpose:** Provides 50-100+ hour goals for optimizers while remaining optional
**Status:** Planned for Phase 6  

---

## Terminology Notes

### When to Use Power vs Character

**Power Level:**
- Always refers to temporary in-run progression (1-20)
- "You reached Power Level 12"
- "Power XP bar filled"
- "Choose a Power Upgrade"

**Character Level:**
- Always refers to permanent cross-run progression (1-100)
- "Marcus is Character Level 15"
- "You earned 850 Character XP"
- "You gained a stat point from leveling up"

### Leveling Happens At Different Times

**Power Leveling:**
- Happens during active combat
- Triggered by killing enemies
- Pauses game for upgrade choice
- 20 times per run

**Character Leveling:**
- Happens between runs only
- Triggered by run completion
- Shows on results screen
- Happens when you've accumulated enough Character XP

---

## Related Documentation

- **Full progression details:** [`systems-progression.md`](systems-progression.md)
- **Combat mechanics:** [`combat-skills.md`](combat-skills.md)
- **Dungeon layouts:** [`dungeon-structure.md`](dungeon-structure.md)
- **Overall design:** [`design-overview.md`](design-overview.md)

---

_Last updated: 2025-10-17 - Complete overhaul for updated idle systems vision (5 materials, Forge/FP system, updated Workshop/Treasury)_
_Living document - Update when new terms are introduced_
