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
| Phase 2 | Progression hooks players | 2 weeks | ✅ **COMPLETE** |
| Phase 3 | Hub enables meta-progression | 1 week | ⏳ **IN PROGRESS** |
| Phase 4 | Gear & loot add variety | 2 weeks | 📝 Next |
| Phase 5 | Idle adds value | 2 weeks | 📝 Planned |
| Phase 6 | Depth increases retention | 2 weeks | 📝 Planned |
| Phase 7 | Endgame sustains interest | 2+ weeks | 📝 Planned |

---

## Phase 1: Prove Combat is Fun ✅ PROVEN

**Hypothesis:** "Fighting waves with skills and upgrades is engaging for 25+ minutes"

**Status:** ✅ **SUCCESS** - Combat validated as fun through playtesting

### Completed ✅
- Player movement and shooting mechanics
- 3 enemy types (Basic, FastMelee, SlowRanged)
- Enemy AI with virtual methods (Attack/Movement/TryAttack)
- Health/damage system with crit
- Power XP/Power Level flow (per-run progression)
- 8 working Power Upgrades (stacking fixed)
- 3 functional skills (Whirlwind, Fireball, basic attacks)
- Wave/floor system (10 waves + boss per floor, 5 floors total)
- Enemy scaling (HP/damage mult per wave/floor)
- Boss spawning at 5:00 mark
- Main menu with styled buttons
- Floor transition UI (Continue/End Run buttons)
- Victory screen (Floor 5 completion)
- Death screen with stats
- Results screen with stat allocation
- Boss defeat detection via TreeExited signals
- HUD integration with Dungeon.cs

### Success Criteria ✅ Met
- Combat feels engaging
- Power Upgrades matter
- Enemy variety works
- Want to play "one more run"
- Complete game loop: Menu → Combat → Death/Victory → Menu

### Cut from Phase 1 ✂️
- Character stat system (Phase 2)
- Gear drops (Phase 4)
- Materials/idle (Phase 5)
- Save system (Phase 2)
- Hub world (Phase 3)

---

## Phase 2: Progression & Persistence ✅ COMPLETE

**Hypothesis:** "Players want to push further because permanent progression feels good"

**Status:** ✅ **COMPLETE** - Character stat system and save/load working

### Completed ✅

1. **Character Stats System**
   - 5 stats: STR/INT/AGI/VIT/FOR
   - Each stat provides specific bonuses (damage, health, speed, regen, crit)
   - +1 point per Character Level (permanent progression)
   - Stat allocation panel (press C in-game)
   - Stats properly scale with formulas

2. **Character XP & Leveling**
   - Character XP tracked separately from Power XP
   - Character XP awarded at end of run based on performance
   - XP formula: 50 base + 100 per floor + 150 per boss + 500 victory bonus
   - Character Level persists between runs
   - Results screen shows Character XP earned

3. **Save/Load System**
   - JSON-based save file (SaveManager.cs + SaveData.cs)
   - Saves Character Level, stats, XP, highest floor
   - Saves Power Level and active upgrades (run state)
   - Persists between sessions
   - Continue Game button on main menu
   - Checkpoint system: saves on floor completion

### Deferred to Phase 4 📝
- Gear drops (requires inventory UI)
- Distinct floor visuals (cosmetic)

### Success Criteria ✅ Met
- Permanent progression working
- Save/load reliable
- Stat allocation feels impactful
- Players motivated to earn character XP

---

## Phase 3: Hub World & Scene Flow ⏳ IN PROGRESS

**Hypothesis:** "Hub as safe zone enables meta-progression systems and improves game feel"

**Status:** ⏳ **IN PROGRESS** - Core hub working, needs polish

### Completed ✅

1. **Hub Scene**
   - hub.tscn with player spawn at (0, 0)
   - Hub.cs manages player initialization
   - CallDeferred used to ensure proper component initialization
   - Background placeholder (dark blue/purple)

2. **Dungeon Portal Interaction**
   - DungeonPortal.cs with Area2D detection
   - "[E] Enter Dungeon" prompt when in range
   - 'interact' input action (E key)
   - Smooth transition to dungeon.tscn

3. **Scene Architecture**
   - Renamed game.tscn → dungeon.tscn for clarity
   - Renamed Game.cs → Dungeon.cs
   - All transitions return to hub (not main menu)
   - Death/Victory/End Run all return to hub
   - Main Menu → Hub → Dungeon → Hub flow

4. **Player Initialization Fix**
   - Fixed Player.Initialize() to handle new game case
   - Previously only loaded saves, now initializes fresh stats
   - Bug prevented movement on new games (CurrentSpeed = 0)
   - StatsManager.Initialize() now called for fresh starts

### In Progress ⏳
- Complete flow testing (hub → dungeon → hub)
- Visual polish (background, lighting, atmosphere)

### Planned 📝
- Placeholder NPCs (Vendor, Upgrade Master, etc.)
- Hub ambient sounds/music
- Portal visual effects (particles, glow)

### Success Criteria
- Scene transitions smooth
- Player movement works in both hub and dungeon
- Save/load works from hub
- Foundation for future systems (vendors, crafting, idle)

---

## Phase 4: Gear & Loot System 📝 NEXT

**Hypothesis:** "Loot drops add excitement and build variety"

### Planned Tasks

1. **Basic Gear Drops**
   - 3 slots: Weapon, Armor, Accessory
   - 3 rarities: Common, Rare, Epic
   - Flat stat bonuses
   - Drops from bosses (100%) and enemies (5-10%)

2. **Inventory UI**
   - Equipment screen
   - Gear comparison tooltips
   - Equip/unequip functionality

3. **Gear Save/Load**
   - Persist equipped gear
   - Integrate with save system

### Success Criteria
- Players excited by drops
- Gear choices matter
- Build variety emerges

---

## Phase 5: Prove Idle Hook 📝 PLANNED

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

## Phase 6: Add Depth 📝 PLANNED

**Hypothesis:** "Build diversity increases engagement"

### Tasks
1. Skill mastery (3 tiers)
2. Gear mods (add 2nd material)
3. Expand skill pool (10+ more skills)

---

## Phase 7: Endgame 📝 PLANNED

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
