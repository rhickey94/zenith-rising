# Sequential Development Plan

## Development Philosophy

**Core Principle:** Build in phases where **each phase proves a hypothesis** before investing in the next.

**Why Sequential:**
- Validates assumptions early
- Prevents wasted work on features nobody wants
- Allows pivoting based on feedback
- Ships faster (MVP in weeks, not months)

---

## Phase Overview

| Phase | Hypothesis | Duration | Status |
|-------|-----------|----------|--------|
| Phase 1 | Combat is fun | 2 weeks | ✅ **PROVEN** |
| Phase 2 | Progression hooks players | 2 weeks | ⏳ Next |
| Phase 3 | Idle adds value | 2 weeks | 📝 Planned |
| Phase 4 | Depth increases retention | 2 weeks | 📝 Planned |
| Phase 5 | Endgame sustains interest | 2+ weeks | 📝 Planned |

---

## Phase 1: Prove Combat is Fun ✅ PROVEN

**Hypothesis:** "Fighting waves with skills and upgrades is engaging for 25+ minutes"

**Status:** ✅ **SUCCESS** - Combat validated as fun through playtesting

### Completed ✅
- Player movement and shooting mechanics
- 3 enemy types (Basic, FastMelee, SlowRanged)
- Enemy AI with virtual methods (Attack/Movement/TryAttack)
- Health/damage system with crit
- XP/level-up flow
- 8 working upgrades (stacking fixed)
- 3 functional skills (Whirlwind, Fireball, basic attacks)
- Wave/floor system (10 waves + boss per floor)
- Enemy scaling (HP/damage mult per wave/floor)
- Boss spawning at 5:00 mark
- Main menu

### In Progress ⏳
- Floor transition UI (Continue/End Run buttons)
- Victory screen
- Boss defeat detection wiring
- HUD integration with Game.cs

### Success Criteria ✅ Met
- Combat feels engaging
- Upgrades matter
- Enemy variety works
- Want to play "one more run"

### Cut from Phase 1 ✂️
- Character stat system (Phase 2)
- Gear drops (Phase 2)
- Materials/idle (Phase 3)
- Save system (Phase 2)
- Hub world (Phase 2)

---

## Phase 2: Prove Progression Hook ⏳ NEXT

**Hypothesis:** "Players want to push further because permanent progression feels good"

**Goal:** After dying, you immediately want to start a new run to test your stat allocation and new gear.

### Tasks

1. **Character Stats System**
   - 5 stats: STR/VIT/AGI/RES/FOR
   - Each stat does ONE thing (simplified)
   - +1 point per character level
   - Distribute freely in stat screen UI

2. **Save/Load System**
   - Save character level + stat allocation to JSON
   - Save highest floor reached
   - Persist between runs

3. **Basic Gear Drops**
   - 3 slots: Weapon, Armor, Accessory
   - 3 rarities: Common, Rare, Epic
   - Flat stats only
   - Drops from bosses (100%) and elites (10%)

4. **5 Distinct Floors**
   - Different enemy mix per floor
   - Visual distinction
   - Boss at end of each floor

### Success Criteria
- 60% 1-week retention
- Average 3+ runs per session
- Players reach Floor 3 within 5 hours

---

## Phase 3: Prove Idle Hook 📝 PLANNED

**Hypothesis:** "Players return after being away because idle progress matters"

### Tasks

1. **Workshop (MVP)** - ONE material type: Energy Cores
2. **Treasury (MVP)** - Gold generation
3. **Material Drop System**
4. **Gear Power Level** (+1 to +10 enhancement)

### Success Criteria
- 70% 1-week retention
- Players check in after 2+ hours
- Treasury gold gets spent

---

## Phase 4: Add Depth 📝 PLANNED

**Hypothesis:** "Build diversity increases engagement"

### Tasks
1. Skill mastery (3 tiers)
2. Gear mods (add 2nd material)
3. Expand skill pool (10+ more skills)

---

## Phase 5: Endgame 📝 PLANNED

**Hypothesis:** "Multiple dungeons sustain long-term interest"

### Tasks
1. **Multiple Dungeons** (4 total)
2. **Ascension system**
3. **Weekly challenges**

---

## What We're NOT Building

**Cut Forever:**
- Research Lab (passive power gain)
- Training Ground (weird currency)
- 3+ material types
- Complex gear synergies
- PvP/multiplayer (for MVP)

**Why Cut:**
- Doesn't serve core loop
- Adds complexity without value
- Can add later if players demand

---

## Current Focus

**See [`../../CLAUDE.md`](../../CLAUDE.md) for current session progress and next tasks.**

---

*This plan is flexible - we pivot based on validation results.*
