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

### Gear Slots

**4 Equipment Slots:**

1. **Weapon** - Primary stat + damage scaling
2. **Armor** - Defense + secondary stats
3. **Accessory** - Pure stat bonuses
4. **Relic** - Special build-defining properties (Phase 4+)

### Rarity Tiers

Equipment drops in 5 rarity tiers:

| Rarity    | Color  | Primary Stat | Secondary Stats | Enchant Slots |
| --------- | ------ | ------------ | --------------- | ------------- |
| Common    | Gray   | 1            | 0               | 0             |
| Uncommon  | Green  | 1            | 1               | 0             |
| Rare      | Blue   | 1            | 2               | 0             |
| Epic      | Purple | 1            | 3               | 1             |
| Legendary | Orange | 1            | 3               | 2             |

**Drop Rates:**

- Common: 60%
- Uncommon: 25%
- Rare: 10%
- Epic: 4%
- Legendary: 1%

**Boss Drops:**

- Guaranteed gear drop
- 50% chance for Rare+
- 10% chance for Epic+
- 2% chance for Legendary

### Gear Stat Structure

Each piece has multiple layers:

#### **1. Primary Stat (Always Present)**

The main stat bonus for this gear type

**Examples:**

- Iron Sword: +15 Strength
- Leather Armor: +10 Vitality
- Amulet: +8 Agility

Scales with item level and rarity.

#### **2. Secondary Stats (0-3 Random)**

Additional stat bonuses rolled on drop

**Examples:**

- +8 Vitality
- +5 Agility
- +3 Resilience

Lower values than primary stat.
Number depends on rarity (see table above).

#### **3. Enhancement Level (0-10)**

**Added via Workshop in Phase 3**

- Each level: +10% to ALL stats on item
- Costs refined materials
- Max +10 = +100% stats (item stats × 2)

**Example:**

```
Iron Sword (Base):
+15 STR, +5 VIT, +3 AGI

Iron Sword +5:
+22 STR, +7 VIT, +4 AGI  (base × 1.5)

Iron Sword +10:
+30 STR, +10 VIT, +6 AGI  (base × 2.0)
```

#### **4. Enchantment Slots (0-3)**

**Added in Phase 4**

Special effects added via Workshop

**Examples:**

- "+15% Physical Damage"
- "Restore 2% HP on kill"
- "Attacks have 10% chance to stun"

Unlock slots with rare materials.
Add enchantments with refined components.

### Gear Acquisition

**Drops Only (No Crafting):**

- Enemies drop gear at random
- Bosses guaranteed drops
- Elites have higher quality drops

**Why No Crafting:**

- Keeps excitement of drops
- Simpler system
- Enhancement provides long-term goals
- Materials used for upgrades, not creation

### Gear Management

**Inventory:**

- Unlimited storage (for MVP)
- Sort by rarity, type, level
- Quick-equip from inventory

**Salvaging:**

- Convert unwanted gear → gold
- Returns 50% of enhancement investment
- Prevents inventory bloat
- Gives bad drops purpose

**Respec-Friendly:**

- Can freely swap gear
- Try different builds
- No gear binding or soulbound

---

## Material Economy (Simplified) 📝 Phase 5 - PLANNED

> ⚠️ **MATERIAL COUNT:** ONE material type (Energy Cores) for MVP. Future phases may add Modification Chips.

### Design Philosophy

**Problem with complex systems:**

- 3 materials × 2 states = 6 item types
- Confusing purposes
- Multiple conversion paths
- Analysis paralysis

**Our solution:**

- **Phase 3:** ONE material type (Energy Cores)
- **Phase 4:** Add second material (Modification Chips)
- **Never:** Add third material

### Phase 3: Energy Cores (MVP)

**Energy Cores** (THE core material)

**Drops from:**

- All enemies (100% drop rate)
- Amount scales with enemy tier
- Basic: 1-2 cores
- Elite: 3-5 cores
- Boss: 10-15 cores

**Raw → Refined:**

- Raw Energy Core → Refined Energy Core
- Takes 30 minutes in Workshop
- Process offline via DateTime
- Queue up to 3 refinements

**Used for:**

- Gear enhancement only (+10% stats per level)
- Clear, single purpose
- Always valuable

**Why This Works:**

- Simple to understand
- One decision: "Enhance which gear?"
- No complex material webs
- Active play = get cores, Idle = refine cores

### Phase 4: Modification Chips (Depth)

**Modification Chips** (optional complexity)

**Drops from:**

- Bosses only (50% chance)
- Guaranteed from Floor 5 boss

**Used for:**

- Adding enchantments to gear
- One enchantment per item
- Choose from pool of 10-15 effects

**Why Add This:**

- Build customization
- Chase mechanic (rare drops)
- Doesn't confuse core loop
- Optional optimization

### Never Adding

**❌ Third Material Type**

- Two materials is enough
- More = complexity creep
- Diminishing returns on engagement

**❌ Material Conversions**

- No core → chip conversion
- No chip → core conversion
- Keeps each material valuable

**❌ Material Types Per Slot**

- No "weapon materials" vs "armor materials"
- Same materials for all slots
- Reduces inventory bloat

---

## Idle Systems 📝 Phase 5 - PLANNED

### Design Constraints

**Golden Rules:**

1. Only processes what you actively farmed
2. Caps at 8 hours maximum
3. Never more rewarding than active play
4. No "wait to play" mechanics
5. No FOMO pressure

### Workshop (Material Refinement)

**Purpose:** Convert raw materials → refined components

**How It Works:**

```
Raw Energy Core → Refined Energy Core
Time: 30 minutes per core
Offline: Uses DateTime for progression
Queue: Up to 3 refinements simultaneously
```

**Process:**

1. Finish combat run, collect Raw Cores
2. Open Workshop, add cores to queue
3. Close game
4. Return 30+ minutes later
5. Collect Refined Cores
6. Use at Blacksmith to enhance gear

**Upgrades (Phase 4):**

- Faster refinement (−5 min per level, min 15 min)
- More queue slots (+1 slot per level, max 5)
- Batch refinement (refine 5 at once)

**Why This Works:**

- Clear input → output
- Respects active grinding (only refines what you earned)
- Has cap (queue fills, then stops)
- Offline-friendly (DateTime calculation)

### Treasury (Idle Gold Generation)

**Purpose:** Provide steady income for flexible spending

**How It Works:**

```
Generation Rate = Highest Floor Cleared × 10 gold/hour
Cap Duration: 8 hours
Max Accumulation: Rate × 8
```

**Examples:**

- Floor 3 cleared = 30 gold/hour, max 240
- Floor 5 cleared = 50 gold/hour, max 400
- Floor 10 cleared = 100 gold/hour, max 800

**Collection:**

- Open Treasury UI
- See accumulated gold
- Click "Collect" button
- Gold added to total

**Why 8-Hour Cap:**

- Encourages daily check-ins (not week-long AFK)
- Prevents runaway passive income
- Respects player time (don't miss much if busy)
- Active play still better (combat drops gold too)

**Gold Uses:**

1. **Respec stats** (100g × Character Level)
2. **Buy consumables** (health potions, buffs)
3. **Salvage gear** (convert to gold)
4. **Rush Workshop** (10g per minute remaining)
5. **Base upgrades** (Workshop/Treasury improvements)

**Upgrades (Phase 4):**

- Increase rate (+10 gold/hour per level)
- Increase cap (8h → 10h → 12h)
- Interest system (unspent gold gains 5% daily, cap 1000)

### Workshop vs Treasury Comparison

| System   | Active Input  | Passive Output    | Purpose              |
| -------- | ------------- | ----------------- | -------------------- |
| Workshop | Raw materials | Refined materials | Enable gear upgrades |
| Treasury | Floor clears  | Gold              | Flexible spending    |

**Both respect the same rules:**

- ✅ Only rewards past active play
- ✅ Caps prevent FOMO
- ✅ Offline-friendly (DateTime)
- ✅ Never better than active grinding

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
Enhance Gear → Respec Stats → Buy Consumables
  ↓
Push Further in Tower
  ↓
Unlock Higher Floors → Better Treasury Rate
  ↓
Repeat
```

**Duration:** 1-2 weeks to max MVP content
**Rewards:** Mastery, optimization, completion

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
- Technical implementation: [`../02-IMPLEMENTATION/skill-system-architecture.md`](../02-IMPLEMENTATION/skill-system-architecture.md)

**Current Implementation Status:**
See [`CLAUDE.md`](../../CLAUDE.md) for what's actually built vs planned.

---

_Last updated: Full documentation reorganization_
_Living document - Update when systems change_
