# Combat & Skills System

## Overview

Tower Ascension uses a **hybrid skill system** combining pre-run customization with in-run evolution. You choose your loadout before each run, then random upgrades modify your abilities as you level up.

**Design Philosophy:**
- Foundation = Your choice (skill loadout)
- Spice = RNG (in-run upgrades)
- Result = Same class, different every run

---

## Core Combat Mechanics

### Always-Available Abilities

Every class has 3 universal controls:

#### **Left Click - Basic Attack**
- Auto-aims at nearest enemy
- No cooldown, no cost
- Scales with character stats
- Class-specific behavior:
  - **Warrior:** Melee sword swing (cleave damage)
  - **Mage:** Magic bolt (ranged projectile)
  - **Ranger:** Bow shot (piercing)

#### **Right Click - Special Attack**
- Manual aim (toward mouse/cursor)
- Short cooldown (3-5 seconds)
- Higher damage than basic attack
- Class identity move:
  - **Warrior:** Shield bash (stun + knockback)
  - **Mage:** Fireball (AOE explosion)
  - **Ranger:** Charged shot (high single-target)

#### **Spacebar - Dash/Dodge**
- Universal movement ability
- 2 second cooldown
- Brief invulnerability frames (0.5s)
- Mobility and survival tool
- Same for all classes

### Class Combat Identities

**Warrior (Breacher):**
- Range: Melee
- Style: Get in, stay in, tank damage
- Strengths: Survivability, crowd control
- Weaknesses: Must close distance, lower mobility

**Mage (Conduit):**
- Range: Medium
- Style: Control battlefield, AOE damage
- Strengths: Multi-target, utility, sustain
- Weaknesses: Positioning-dependent, squishy

**Ranger (Sharpshooter):**
- Range: Long
- Style: Kite, reposition, precision burst
- Strengths: Mobility, single-target damage
- Weaknesses: Fragile, struggles when surrounded

---

## Skill Loadout System

### Pre-Run Customization

Before each run, **equip 3 active skills** from your unlocked pool:

**Skill Slots:**
- **Q Key** - Offensive Skill (8-12s cooldown)
- **E Key** - Utility/Defensive Skill (10-15s cooldown)
- **R Key** - Ultimate Skill (40-60s cooldown)

**Example Warrior Loadout:**
```
Q: Whirlwind (spin attack, AOE damage)
E: Iron Skin (damage reduction buff)
R: Berserker Rage (massive damage boost)
```

**Example Mage Loadout:**
```
Q: Arc Lightning (chain lightning)
E: Void Rift (damage zone + healing)
R: Singularity (gravity well + implosion)
```

**Example Ranger Loadout:**
```
Q: Flashbang (blind + damage boost)
E: Tactical Roll (dash + decoy)
R: Auto-Turret (summon turret)
```

### Skill Unlocking

**Starting Skills:**
- 2 skills per slot (6 total choices)
- Enough for varied gameplay
- Not overwhelming

**Unlock Methods:**
- **Training Ground:** Spend training points (Phase 4)
- **Research Lab:** Study monsters (Phase 4)
- **Boss Drops:** Skill unlock tokens (Phase 3)
- **Character Levels:** Unlock at milestones (Phase 2)

**Unlock Pace:**
- Early Game (Floors 1-3): 2 skills per slot (6 total)
- Mid Game (Floors 4-7): 4 skills per slot (12 total)
- Late Game (Floor 8+): 6+ skills per slot (18+ total)

### Skill Progression

**3 Mastery Tiers (Simplified):**

| Tier | Kills Required | Bonus |
|------|---------------|--------|
| Bronze | 0 (default) | Base skill |
| Silver | 50 kills | +50% effectiveness |
| Gold | 200 kills | +100% effectiveness + special bonus |

**Why Simplified:**
- Achievable in 1-2 runs (Silver)
- Meaningful power spike
- Gold feels special but attainable
- Removed unrealistic Diamond tier (10,000 kills)

**Example - Whirlwind:**
```
Bronze (0 kills):
- 3 second spin
- 30 damage per hit
- 5 meter radius

Silver (50 kills):
- 4.5 second spin
- 45 damage per hit
- 6 meter radius

Gold (200 kills):
- 6 second spin
- 60 damage per hit
- 7.5 meter radius
- Pulls enemies toward center
```

**Mastery Tracking:**
- Kills recorded per skill
- Persists between runs
- Visible in skill selection UI
- Progress bar shows kills to next tier

---

## In-Run Progression (Roguelite Layer)

### Level-Up System

**How It Works:**
1. Kill enemies → Drop XP shards
2. Collect shards → Fill XP bar
3. Level up → Game pauses
4. Choose 1 upgrade from 3 random options
5. Resume combat with upgrade applied

**Max Level Per Run:** 20
- Prevents infinite scaling
- Keeps runs ~30 minutes
- Resets each run

### Upgrade Types (3 Categories)

#### **1. Skill Modifiers (40% chance)**
Modify your equipped skills

**Examples:**
- "Q skill cooldown −2 seconds"
- "E skill radius +30%"
- "R skill now chains to nearby enemies"
- "Q skill applies burning damage"
- "E skill duration +3 seconds"

**Stacking:**
- Can get multiples of same upgrade
- "Q cooldown −2s" can appear 3 times
- Result: Q skill with −6s cooldown

#### **2. Passive Bonuses (40% chance)**
Universal improvements

**Examples:**
- "+15% movement speed"
- "+20% attack speed"
- "+10% critical chance"
- "Projectiles pierce 1 enemy"
- "Heal 2 HP per kill"
- "Attacks apply slow"
- "+1 projectile"

**Synergies:**
- Some combos create powerful builds
- "+1 projectile" + "Pierce" = screen-wide coverage
- "Heal on kill" + "Attack speed" = sustain build

#### **3. Stat Boosts (20% chance)**
Temporary stat increases

**Examples:**
- "+5 Strength" (temporary for run)
- "+15% to all stats"
- "+50 max HP"
- "+3 Fortune"

**Less Exciting:**
- Direct stat increases
- No unique gameplay changes
- Lower spawn rate to favor modifiers

### Upgrade Rarity System

**Rarity Distribution:**
- Common (70%): Small, safe bonuses
- Uncommon (20%): Medium bonuses
- Rare (8%): Large bonuses or special effects
- Epic (2%): Build-defining changes

**Rarity Visual Cues:**
- Common: Gray border
- Uncommon: Green border
- Rare: Blue border + subtle glow
- Epic: Purple border + pulse animation

**Epic Examples:**
- "All attacks chain to 2 nearby enemies"
- "Abilities cost HP instead of cooldown"
- "Gain +1% damage per 1% missing HP"
- "Movement slows time by 20%"

---

## Skill System Philosophy

### Why This Hybrid Works

**Pre-Run Loadout (Foundation):**
- YOU control your build direction
- Long-term goals (unlock/master skills)
- Feels like YOUR character
- Consistent class identity

**In-Run Upgrades (Spice):**
- Every run plays differently
- Exciting level-up moments
- Adaptation challenges
- Replayability

**Player Agency Balance:**
- Control foundation, RNG adds variety
- Bad RNG doesn't ruin run (still have core skills)
- Good RNG makes you feel OP
- Can pivot build based on upgrades offered

### Build Diversity Examples

**Warrior - Tank Build:**
- Q: Shockwave (crowd control)
- E: Iron Skin (damage reduction)
- R: Last Stand (survive death)
- Upgrades: +Max HP, +Regen, Lifesteal on hit

**Warrior - Berserker Build:**
- Q: Whirlwind (AOE damage)
- E: Combat Stim (attack speed)
- R: Berserker Rage (damage boost)
- Upgrades: +Attack Speed, +Damage, Crit chance

**Mage - Chain Lightning Build:**
- Q: Arc Lightning
- E: Void Rift (sustain)
- R: Architect's Blessing (cooldown reduction)
- Upgrades: +Chain targets, −Cooldowns, Chain damage

**Ranger - Sniper Build:**
- Q: Charged Shot
- E: Tactical Roll (positioning)
- R: Deadeye (crit boost)
- Upgrades: +Crit damage, Pierce, Slow enemies

---

## Combat Flow

### Wave Combat Loop

```
Wave Starts
  ↓
Enemies Spawn (increasing density)
  ↓
Position & Kite
  ↓
Use Basic Attack (Left Click - constant damage)
  ↓
Use Special Attack (Right Click - burst on cooldown)
  ↓
Use Q Skill (when enemies group)
  ↓
Use E Skill (when threatened or opportunity)
  ↓
Collect XP Shards
  ↓
Level Up → Choose Upgrade
  ↓
Adapt Strategy Based on Upgrade
  ↓
Use R Skill (Ultimate - clutch moments)
  ↓
Wave Complete → Next Wave
```

**Duration:** ~30 seconds per wave
**Intensity:** Ramps up over 10 waves
**Boss:** Appears at 5:00 mark

### Skill Usage Strategy

**Q Skill (Offensive):**
- Use on cooldown
- Aim for grouped enemies
- Primary damage source

**E Skill (Utility):**
- Save for key moments
- Defensive: Use when threatened
- Offensive: Use when opportunity

**R Skill (Ultimate):**
- Long cooldown (40-60s)
- Save for:
  - Boss fights
  - Overwhelming situations
  - Guaranteed value moments

**Example Strategy - Mage:**
```
Normal Wave:
- Basic attack for chip damage
- Q (Arc Lightning) on groups
- E (Void Rift) for healing
- Dash to reposition

Boss:
- Kite with basic attacks
- Q on cooldown
- E for sustained healing
- R (Singularity) at 50% HP for burst
- Dash to avoid telegraphed attacks
```

---

## Skill Implementation Status

### ✅ Currently Implemented (Phase 1)

**Working Skills:**
- Basic Attack (all 3 classes)
- Special Attack (placeholders)
- Whirlwind (Warrior Q) - full implementation
- Fireball (Mage Q) - full implementation
- Melee Attack (Warrior basic) - full implementation

**Working Upgrades:**
- 8 upgrade types functional
- Random selection from pool
- Level-up panel UI
- Upgrade application

### ⏳ Planned (Phase 2-3)

**Skills to Add:**
- E and R skills per class (9 skills total)
- Alternative Q skills (2 more per class)
- Total: 15 skills for MVP

**Upgrades to Add:**
- Expand pool to 40+ upgrades
- Class-specific upgrades
- Epic-tier unique upgrades

### 📝 Future (Phase 4-5)

**Deep Skill Pool:**
- 6+ skills per slot per class (18+ per class)
- Skill unlock progression
- Skill-specific mastery bonuses
- Cross-class skills (unlock via achievements)

---

## Related Documentation

**For detailed class designs:**
- See [`../03-CONTENT-DESIGN/class-abilities.md`](../03-CONTENT-DESIGN/class-abilities.md)

**For upgrade pool:**
- See [`../03-CONTENT-DESIGN/upgrade-pool.md`](../03-CONTENT-DESIGN/upgrade-pool.md)

**For technical implementation:**
- See [`../02-IMPLEMENTATION/skill-system-architecture.md`](../02-IMPLEMENTATION/skill-system-architecture.md)

**For current status:**
- See [`CLAUDE.md`](../../CLAUDE.md)

---

*Last updated: Full documentation reorganization*
*Living document - Update as combat evolves*
