# Zenith Rising - Game Design Overview

## Core Concept

**Zenith Rising** is a roguelite action-RPG where **active combat grants power**, while **idle time improves efficiency**. Fight your way up the tower in intense combat sessions, then let your base systems refine materials and generate resources while you're away.

**In One Sentence:** A bullet-hell roguelite with meaningful idle mechanics that never replace active play.

---

## Design Philosophy

### Core Pillars

1. **Active Play = Power Gains**

   - Character levels, skill mastery, gear drops
   - Combat is always the fastest way to progress
   - Direct power comes from fighting

2. **Idle Time = Efficiency Gains**

   - Material refinement, gold generation
   - Processes resources you collected actively
   - Makes your active time more effective
   - **Active play 3-5x more rewarding than idle time**

3. **Idle Enhances, Never Replaces**

   - No "wait to play" mechanics
   - No energy systems
   - Idle systems have caps (12-24 hours max)
   - Always better to play than wait

4. **Every Run Matters**

   - Character levels persist
   - Skill mastery accumulates
   - Materials carry over
   - No "wasted" runs

5. **Sequential Risk Reduction**
   - Prove combat is fun before building progression
   - Prove progression works before adding idle
   - Build in phases, validate each hypothesis
   - Ship small, iterate fast

---

## The Core Gameplay Loop

### Active Phase: The Climb

**What You Do:**

1. Enter a tower floor
2. Fight waves of monsters (Vampire Survivors-style + manual aim)
3. Collect Power XP shards → Gain Power Levels → Choose Power Upgrades
4. Gather loot (materials, gear, gold)
5. Defeat floor boss (5 minutes per floor)
6. Choose: **Extract** (safe) or **Continue** (risk/reward)

**What You Gain:**

- Character XP and Character Levels (permanent)
- Skill mastery progress (permanent)
- Equipment drops (persistent)
- Raw materials (for refinement)
- Gold currency

**Run Structure:**

- Dungeons range from 4-9 floors (varying session lengths)
- 10 waves + boss per floor
- ~5 minutes per floor
- Zenith Station (MVP): 5 floors, 25-30 minute runs
- Die or win → Return to hub with rewards
- **See [`dungeon-structure.md`](dungeon-structure.md) for all dungeon details**

### Idle Phase: The Base

**What Happens While Away:**

- **Workshop** refines raw materials → refined components (4-12 hours per batch)
- **Treasury** generates gold based on highest floor cleared (caps at 12 hours)

**What You Get When You Return:**

- Refined materials ready for gear upgrades
- Bonus gold for purchases
- Progress notifications

**Key Design:**

- Only processes materials you farmed actively
- Caps prevent FOMO (missing a day doesn't ruin progress)
- No "wait to play" mechanics

---

## Target Audience

### Primary Players

**Active Grinders (40%)**

- Love 1-2 hour combat sessions
- Min-max builds and optimize strategies
- Compete on leaderboards
- Want immediate power gains

**Casual Progressors (40%)**

- Play 15-30 minutes daily
- Check in to collect idle rewards
- Enjoy steady progression
- Don't want to fall behind active players

**Hybrid Players (20%)**

- Mix long and short sessions
- Weekend warriors + weekday check-ins
- Value flexibility
- Want both playstyles rewarded

### Why This Game Appeals

**For Active Players:**

- Combat is always the fastest progression
- No artificial waiting
- Skill expression matters
- Grinding is rewarded directly

**For Casual Players:**

- Meaningful progress from daily check-ins
- Never fall hopelessly behind
- Idle systems feel like "free" rewards
- Short sessions still productive (4-floor dungeons in 20 minutes)
- Long sessions also rewarding (9-floor dungeons in 45 minutes)

**For Both:**

- Respectful of time (no forced grind OR forced waiting)
- Clear goals at all play durations
- Multiple progression paths
- Constant sense of growth

---

## What Makes This Unique

### The Hybrid Hook

**Most idle games:** Idle = power, active play optional
**Most roguelites:** No progression between runs, or minimal meta

**Zenith Rising:**

- ✅ Deep roguelite combat (Vampire Survivors meets Hades)
- ✅ Meaningful meta-progression (character levels, gear, mastery)
- ✅ Idle systems that enhance active play (not replace it)
- ✅ Respect for player time (no energy, no daily quests)

### Key Differentiators

1. **Hybrid Skill System**

   - Pre-run customization (choose 3 skills from unlocked pool)
   - In-run evolution (random upgrades modify your skills)
   - Permanent mastery (skills get stronger as you use them)
   - Result: Same class plays differently every run, but has consistent identity

2. **Honest Idle Mechanics**

   - Only refines materials you actively farmed
   - Caps at 12-24 hours (no week-long AFK gains)
   - Gold generation is bonus income, not primary
   - Active play always beats waiting (3-5x multiplier)

3. **Sequential Phases**

   - Each development phase proves a hypothesis
   - Ship early, expand based on validation
   - Prevents scope creep and vaporware

4. **Stat-Based Everything**
   - 5 core stats (STR/INT/AGI/VIT/FOR)
   - Everything scales from stats (no hidden formulas)
   - Gear adds stats, skills scale from stats
   - Simple to understand, deep to optimize

---

## MVP Scope (Phase 1-2)

### Must Have for Launch

**Combat (Phase 1):** ✅ PROVEN FUN

- 3 classes with distinct playstyles
- 10+ skills per class
- 20+ Power Upgrade types (temporary, in-run)
- 3 enemy types + boss
- 5 floors
- Wave/boss system

**Progression (Phase 2):** 🔄 IN PROGRESS

- Character stat system (5 stats)
- Gear drops (3 slots: weapon, armor, accessory)
- Save/load system
- 5 distinct floors

**Idle Systems (Phase 5):**

- Workshop (5 material types: Essence, Ore, Fragments, Souls, Crystals)
- Treasury (gold generation)
- Material drop and refinement

**Core Loop Validated:**

- Fight → Collect → Refine → Upgrade → Fight Stronger
- 15-minute sessions feel productive
- 2-hour grinds feel rewarding
- Daily check-ins show meaningful progress

### Explicitly Cut for Launch

**Not Building (Yet):**

- Research Lab (passive power gain contradicts design)
- Training Ground (QoL tokens are weird currency)
- Complex gear synergies
- Multiple dungeons (just Tower for MVP)
- PvP or multiplayer
- Seasonal content

**Why Cut:**

- Validate core loop first
- Each system = 2+ weeks dev time
- Ship tight MVP, expand based on player feedback
- Avoid scope creep

---

## Post-Launch Expansion (Phase 4-7)

### Phase 4: Gear & Forge System

- Gear drops with Forging Potential (FP)
- Forge crafting (add/upgrade/reroll affixes)
- Material acquisition system (5 types)
- Inventory and Forge UI

### Phase 5: Idle Systems

- Workshop processing (3-5 slots, 4-12 hour conversions)
- Treasury gold generation
- Workshop upgrades (3 tiers)

### Phase 6: Depth & Ascension

- Ascension system (soft resets, prestige progression)
- Ascension tree (Combat/Economy/Utility branches)
- Advanced workshop/treasury upgrades
- Forge expansion (T4-T5 affixes, reroll action)

### Phase 7: Endgame

- **Multiple dungeons (6 total)** with varying lengths to respect different session times:
  - **Zenith Station** (5 floors, 25-30 min) - MVP dungeon
  - **Research Station Kepler** (4 floors, 20-25 min) - Quick challenge
  - **Military Blacksite Omega** (7 floors, 35-40 min) - Extended commitment
  - **Megacity New Babel** (8 floors, 40-45 min) - Epic run
  - **Frost Grave** (6 floors, 30-35 min) - Mind-bender
  - **Verdant Tomb** (9 floors, 45-50 min) - Ultimate challenge
- Skill mastery system (Bronze/Silver/Gold tiers)
- Challenge runs with modifiers
- **See [`dungeon-structure.md`](dungeon-structure.md) for complete dungeon design**

### Future Considerations

- New classes
- Seasonal events
- Prestige systems
- Co-op mode (if players demand it)

---

## Success Metrics

### Phase 1 (Combat Validation)

**Goal:** Prove combat is engaging for 25+ minutes

- ✅ **PROVEN** - Multiple successful playtests
- Players want "one more run"
- Power Upgrade choices feel meaningful
- Enemy variety creates interesting challenges

### Phase 2 (Progression Hook)

**Goal:** Players want to push further

- Return after death to test new stats/gear
- 1 week retention > 40%
- Average session > 20 minutes
- Players reach Floor 5 within first 10 runs

### Phase 3 (Idle Validation)

**Goal:** Players return after being away

- Check in after 2+ hours
- Collect idle rewards
- Feel good about passive progress
- 1 week retention > 60%

### Long-Term Success

- 1 month retention > 30%
- Average session duration: 25-40 minutes
- Daily active users grow steadily
- Positive reviews mention "respectful of time"

---

## Design Anti-Patterns We Avoid

### ❌ What We DON'T Do

**Predatory Idle Mechanics:**

- ❌ Week-long timers that pressure check-ins
- ❌ Pay to speed up timers
- ❌ Artificial scarcity (energy systems)
- ❌ FOMO-driven daily quests

**Disrespectful Progression:**

- ❌ Runs that feel wasted (no permanent progress)
- ❌ Grind walls that force idle waiting
- ❌ RNG that can soft-lock progression
- ❌ Power creep that invalidates builds

**Scope Creep Traps:**

- ❌ Building features before core loop validation
- ❌ Adding complexity without purpose
- ❌ Chasing every player suggestion
- ❌ Feature parity with 5-year-old games

### ✅ What We DO Instead

**Player-Friendly Monetization (If Any):**

- ✅ Cosmetics only (skins, effects)
- ✅ Battle pass with free track
- ✅ Convenience (not power) purchases
- ✅ All gameplay free

**Respectful Design:**

- ✅ Every run grants permanent progress
- ✅ Active play always beats waiting
- ✅ Clear goals at all time investments
- ✅ No punish for playing "wrong"

**Focused Development:**

- ✅ Prove hypotheses before building
- ✅ Ship small, iterate based on data
- ✅ Cut ruthlessly, expand thoughtfully
- ✅ Quality over quantity

---

## Vision Statement

**Zenith Rising respects your time.**

Whether you have 15 minutes or 2 hours, you'll make meaningful progress. Active play rewards you immediately. Idle time works for you passively. Neither is wasted.

We're building a roguelite that you can play intensely when you want, and progress casually when you can't—without feeling like you're "playing it wrong" either way.

**Core Promise:**

> "Every session matters. Every choice matters. Your time matters."

---

## Next Steps

**Ready to dive deeper?**

- **Systems Details:** See [`systems-progression.md`](systems-progression.md)
- **Combat Mechanics:** See [`combat-skills.md`](combat-skills.md)
- **Narrative:** See [`narrative-framework.md`](narrative-framework.md)
- **Implementation:** See [`../02-IMPLEMENTATION/phase-plan.md`](../02-IMPLEMENTATION/phase-plan.md)

**Want current status?** Check [`CLAUDE.md`](../../CLAUDE.md) in project root.

---

_This document represents the stable design vision. For day-to-day changes, see CLAUDE.md._
