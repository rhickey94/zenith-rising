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
**Common (Gray):** 1 primary stat, 0 secondary stats, 0 enchant slots  
**Uncommon (Green):** 1 primary stat, 1 secondary stat, 0 enchant slots  
**Rare (Blue):** 1 primary stat, 2 secondary stats, 0 enchant slots  
**Epic (Purple):** 1 primary stat, 3 secondary stats, 1 enchant slot  
**Legendary (Orange):** 1 primary stat, 3 secondary stats, 2 enchant slots  

### Enhancement Level
**Definition:** Gear upgrade level from 0 to +10  
**Effect:** Each level adds +10% to ALL stats on the item  
**Max Enhancement:** +10 = +100% stats (doubles item stats)  
**Cost:** Refined Energy Cores  
**System:** Added in Phase 3 via Workshop  

**Example:**
```
Iron Sword (Base): +15 STR, +5 VIT, +3 AGI
Iron Sword +5: +22 STR, +7 VIT, +4 AGI (base × 1.5)
Iron Sword +10: +30 STR, +10 VIT, +6 AGI (base × 2.0)
```

### Enchantment Slots
**Definition:** Special effect slots on Epic+ gear  
**Added:** Phase 4 via Workshop  
**Examples:** "+15% Physical Damage", "Restore 2% HP on kill", "10% chance to stun"  
**Slots:** Epic gear has 1 slot, Legendary gear has 2 slots  

### Primary Stat
**Definition:** The main stat bonus on a piece of gear  
**Always Present:** Every gear piece has exactly 1 primary stat  
**Examples:** Iron Sword (+15 STR), Leather Armor (+10 VIT), Amulet (+8 AGI)  

### Secondary Stats
**Definition:** Additional random stat bonuses rolled on gear  
**Count:** 0-3 depending on rarity (see rarity table)  
**Values:** Lower than primary stat values  
**Randomized:** Rolled when item drops  

---

## Materials & Resources

### Energy Cores
**Definition:** The primary material for gear enhancement  
**Drop Source:** All enemies (100% drop rate)  
**States:** Raw Energy Core → Refined Energy Core  
**Refinement:** 30 minutes in Workshop  
**Purpose:** Enhance gear (+10% stats per enhancement level)  
**Phase:** Added in Phase 3  

### Modification Chips
**Definition:** Rare material for adding enchantments  
**Drop Source:** Bosses only (50% chance, guaranteed from Floor 5+)  
**Purpose:** Add enchantments to Epic/Legendary gear  
**Phase:** Added in Phase 4  

### Gold
**Definition:** Universal currency for various purchases  
**Sources:**
- Enemy drops during runs
- Treasury idle generation (based on highest floor cleared)
- Salvaging unwanted gear (50% of enhancement investment)

**Uses:**
- Respec stats (100 gold × character level)
- Buy consumables (health potions, buffs)
- Rush Workshop refinement (10 gold per minute remaining)
- Base upgrades (Workshop/Treasury improvements)

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
- Workshop access (material refinement)
- Treasury collection (idle gold)
- Dungeon selection

### Workshop
**Definition:** Idle system for refining raw materials  
**Function:** Raw Energy Core → Refined Energy Core  
**Time:** 30 minutes per core (Phase 3 MVP baseline)  
**Queue:** 3 simultaneous refinements (expandable to 5 in Phase 4)  
**Offline:** Uses DateTime for progression (works while game is closed)  

**Upgrades (Phase 4):**
- Faster refinement (−5 min per level, minimum 15 min)
- More queue slots (+1 per level, max 5 slots)
- Batch refinement (refine 5 at once)

### Treasury
**Definition:** Idle system for gold generation  
**Generation Rate:** Highest Floor Cleared × 10 gold/hour  
**Cap:** 8 hours maximum accumulation  
**Max Gold:** Rate × 8  

**Example:**
- Floor 5 cleared = 50 gold/hour
- After 8 hours = 400 gold accumulated

**Upgrades (Phase 4):**
- Increase rate (+10 gold/hour per level)
- Increase cap (8h → 10h → 12h)
- Interest system (5% daily on unspent gold, 1000 gold cap)

### Salvage
**Definition:** Converting unwanted gear into gold  
**Returns:** 50% of enhancement investment (or base value for unenhanced)  
**Purpose:** Prevent inventory bloat, give bad drops value  

---

## Game Structure Terms

### Phase
**Definition:** Development milestone with specific feature sets  
**MVP = Phases 1-3:** Core combat, progression, and enhancement systems  

**Phase Overview:**
- **Phase 1:** Combat core + first character
- **Phase 2:** Character stats + equipment
- **Phase 3:** Enhancement + idle systems (Workshop/Treasury)
- **Phase 4:** Skill mastery + enchantments + depth
- **Phase 5:** Endgame content + ascension (post-launch)

### Zenith Station
**Definition:** The first dungeon, used for MVP  
**Floors:** 5 floors  
**Theme:** Sci-fi tower facility  
**Duration:** 25-30 minute runs  

### Ascension (Phase 5)
**Definition:** Post-launch endgame system for long-term progression  
**Function:** Reset progress for permanent bonuses and new challenges  
**Purpose:** Endless content for dedicated players  
**Status:** Planned for Phase 5, not in MVP  

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

_Last updated: 2025-01-15 - Added Power Level vs Character Level distinction_  
_Living document - Update when new terms are introduced_
