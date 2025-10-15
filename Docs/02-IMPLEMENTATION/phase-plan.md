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
- Power XP/Power Level flow (1-20 per run)
- 8 working Power Upgrades (stacking fixed)
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
- Power Upgrades matter
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
   - +1 point per Character Level (permanent progression)
   - Distribute freely in stat screen UI

2. **Character XP & Leveling**
   - Track Character XP separately from Power XP
   - Award Character XP at end of run based on performance
   - Character Level increases between runs only
   - Results screen shows Character XP earned

3. **Save/Load System**
   - Save Character Level + stat allocation to JSON
   - Save Character XP progress
   - Save highest floor reached
   - Persist between runs

4. **Basic Gear Drops**
   - 3 slots: Weapon, Armor, Accessory
   - 3 rarities: Common, Rare, Epic
   - Flat stats only
   - Drops from bosses (100%) and elites (10%)

5. **5 Distinct Floors**
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

**Hypothesis:** "Multiple dungeons with varying lengths sustain long-term interest and respect player time"

### Tasks

1. **Multiple Dungeons (6 total)** - Varying lengths for different session times
   - **Research Station Kepler** (4 floors, 20-25 min) - Quick challenge dungeon
   - **Military Blacksite Omega** (7 floors, 35-40 min) - Extended commitment dungeon
   - **Megacity New Babel** (8 floors, 40-45 min) - Epic run dungeon
   - **Frost Grave** (6 floors, 30-35 min) - Mind-bender dungeon
   - **Verdant Tomb** (9 floors, 45-50 min) - Ultimate challenge dungeon
   - Note: Zenith Station (5 floors, 25-30 min) is the MVP dungeon built in Phases 1-3

2. **Ascension System**
   - Unlocks at Character Level 50+
   - Reset to Character Level 1, keep gear and skill mastery
   - Gain +5% permanent damage per ascension
   - Unlock new Power Upgrade tiers and cosmetics

3. **Difficulty Tiers**
   - Normal dungeons (current)
   - True difficulty (higher enemy scaling, 2x loot)
   - Nightmare difficulty (extreme scaling, 5x loot)

4. **Weekly Challenges** (stretch goal)
   - Special modifiers on dungeons
   - Leaderboards
   - Exclusive rewards

### Design Philosophy
**Respect player time through content variety:**
- Players with 20 minutes? Run Research Station (4 floors)
- Players with 30 minutes? Run Zenith or Frost Grave (5-6 floors)
- Players with 45+ minutes? Run Verdant Tomb (9 floors)
- All session lengths provide meaningful progression

### Success Criteria
- 20% 3-month retention
- Players engage with multiple dungeons (not just farming one)
- Average 5+ different dungeon completions per week
- Positive feedback on "respecting time" in reviews

### Implementation Priority
**Ship dungeons sequentially, not all at once:**
1. Research Station + Military Blacksite (update 1)
2. Megacity + Frost Grave (update 2)
3. Verdant Tomb + Ascension system (update 3)

**Each release = content update = renewed player engagement**

**See [`../01-GAME-DESIGN/dungeon-structure.md`](../01-GAME-DESIGN/dungeon-structure.md) for complete dungeon designs**

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
