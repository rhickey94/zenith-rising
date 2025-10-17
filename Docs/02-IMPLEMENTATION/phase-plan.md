# Sequential Development Plan

> **📌 MVP SCOPE:** Phases 1-3 + 3.5 = Core gameplay loop validated
>
> **📝 POST-MVP:** Phases 4-7 = Expansion content and retention features
>
> For current implementation status, see [`../../CLAUDE.md`](../../CLAUDE.md)

---

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
| Phase 3 | Hub enables meta-progression | 1 week | ✅ **COMPLETE** |
| **Phase 3.5** | **Warrior combat validates animation/skill systems** | **2 weeks** | **⏳ IN PROGRESS** |
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

## Phase 3: Hub World & Scene Flow ✅ COMPLETE

**Hypothesis:** "Hub as safe zone enables meta-progression systems and improves game feel"

**Status:** ✅ **COMPLETE** - Hub functional and integrated

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

### Success Criteria ✅ Met
- Scene transitions smooth
- Player movement works in both hub and dungeon
- Save/load works from hub
- Foundation for future systems (vendors, crafting, idle)

### Deferred to Future Phases 📝
- Placeholder NPCs (Phase 4+)
- Hub ambient sounds/music (Phase 7 polish)
- Portal visual effects (Phase 7 polish)

---

## Phase 3.5: Warrior Combat Implementation ⏳ IN PROGRESS

**Focus:** Complete ONE class (Warrior) fully to validate animation and skill standardization systems

**Status:** ⏳ **IN PROGRESS** - Balance systems foundation prioritized before skill implementation

**Why this phase:** Before building 18 skills across 3 classes, establish centralized balance infrastructure and validate systems with 5 warrior skills end-to-end.

### Architecture Completed ✅

**Animation System (Custom FSM + AnimationPlayer)**
- ✅ Phase 1: Foundation (Sprite2D + AnimationPlayer setup)
- ✅ Phase 2: Locomotion (walk/idle animations, 4 directions)
- ✅ Phase 3: State Machine (PlayerState enum, transition logic)
- ✅ Phase 4: Combat Animations (warrior_attack, warrior_whirlwind with Call Method tracks)
- ⏳ Phase 5: Hitboxes (Area2D nodes + collision handlers) - IN PROGRESS

**Skill Standardization Framework**
- ✅ Two-axis classification (CastBehavior × DamageSource)
- ✅ Six implementation patterns identified
- ✅ All 18 skills mapped to patterns
- ✅ Hybrid hitbox approach designed (PlayerHitbox for melee/AOE, EffectCollision for projectiles)
- ✅ Documentation created ([skill-standardization.md](skill-standardization.md), [animation-architecture.md](animation-architecture.md))

---

### Implementation Phases (A-F)

**Phase A: Balance Systems Foundation** (4-6 hours) ✅ **COMPLETE**

**Why this phase came first:**
Setting up centralized balance systems BEFORE implementing remaining skills prevented:
- Hardcoded magic numbers scattered across 18 skill files
- Manual recompilation for every balance tweak
- Inconsistent formulas between similar skills
- Difficulty comparing and tuning related values

**Investment pays off:**
- 4-6 hours setup → saves 10+ hours during skill implementation/tuning
- Makes Phases B-F dramatically faster
- Enables rapid iteration during playtesting
- Creates sustainable architecture for future content

**Tasks:**

1. **Create BalanceConfig System** (2 hours)
   - Create `Scripts/Core/BalanceConfig.cs` with nested config classes:
     - PlayerStatsConfig (base health, speed, damage)
     - CharacterProgressionConfig (stat scaling formulas)
     - CombatSystemConfig (crit damage, damage types)
     - EnemyConfig (scaling, aggro, spawn rates)
     - UpgradeSystemConfig (upgrade values per stack)
   - Create `Scripts/Core/GameBalance.cs` singleton for global access
   - Create `Resources/Balance/balance_config.tres` resource
   - Wire up GameBalance as autoload in Project Settings
   - Assign balance_config.tres to GameBalance.Config export

2. **Create SkillBalanceDatabase** (2 hours)
   - Create `Scripts/Skills/Balance/SkillBalanceType.cs` enum
   - Create `Scripts/Skills/Balance/SkillBalanceEntry.cs` data structure
   - Create `Scripts/Skills/Balance/SkillBalanceDatabase.cs` container
   - Create `Resources/Balance/skill_balance_database.tres` resource
   - Wire up to GameBalance.SkillDatabase export
   - Add entries for existing skills (Whirlwind, Fireball, WarriorBasicAttack)

3. **Refactor Existing Systems** (1-2 hours)
   - **StatsManager.cs**: Replace constants with BalanceConfig reads
     - STR_DAMAGE_PER_POINT → Config.CharacterProgression.StrengthDamagePerPoint
     - VIT_HEALTH_PER_POINT → Config.CharacterProgression.VitalityHealthPerPoint
     - etc. for all stat scaling
   - **UpgradeManager.cs**: Replace hardcoded upgrade values with Config reads
     - DamageBoostPerStack, AttackSpeedPerStack, etc.
   - **Dungeon.cs**: Replace enemy spawn/scaling values with Config reads
   - **Enemy.cs**: Replace aggro/leash ranges with Config reads

4. **Update Skill Loading Pattern** (1 hour)
   - Add `skill.Initialize()` method that loads from SkillBalanceDatabase
   - Update Skill.cs base class with runtime properties (not exports)
   - Only SkillId remains as export - all other values load from database
   - Update SkillManager to call skill.Initialize() before first use
   - Simplify skill .tres files (only SkillId + visual references)

**Testing & Validation:**
- Verify StatsManager reads player stats from config correctly
- Change a balance value in inspector, run game, confirm change takes effect
- Verify skills load damage/cooldown from database
- All existing skills (Whirlwind, Fireball) still work correctly

**Documentation:**
- Create `Docs/02-IMPLEMENTATION/balance-systems-architecture.md`
- Update this file with "Phase A Complete" when done

**Success Criteria:**
- ✅ GameBalance singleton accessible globally
- ✅ balance_config.tres holds all game-wide tuning values
- ✅ skill_balance_database.tres holds all skill-specific values
- ✅ StatsManager/UpgradeManager/Dungeon read from config (no constants)
- ✅ Skills load from database (no exported damage/cooldown values)
- ✅ Can tune balance in inspector without recompiling code

---

**Phase B: Skill System Standardization** (2-3 hours) 📝 NEXT AFTER PHASE A

**Tasks:**
- Add CastBehavior and DamageSource enums to Skill.cs
- Update SkillManager.UseSkill() to route based on CastBehavior
- Add hitbox infrastructure to Player.cs (EnableMeleeHitbox, EnableAOEHitbox, etc.)
- Create BasicAttackHitbox and WhirlwindHitbox Area2D nodes in player.tscn
- Wire collision signals to Player.cs handlers
- Update Player state machine to handle CastingSkill state

**Why after Phase A:** 
Skills will load balance from database, so database must exist first.

---

**Phase C: Fusion Cutter (Basic Attack)** (1-2 hours) 📝 PLANNED
- Prove Melee Pattern works end-to-end
- Configure WarriorBasicAttack.tres (load from database)
- Add Call Method tracks to warrior_attack animations
- Implement OnMeleeHitboxBodyEntered() damage application
- Test: left-click → animation → hitbox → damage → kill tracking
- Tune damage/range in skill_balance_database.tres until it feels good

**Why after Phases A+B:** 
Database must exist (Phase A), hitbox infrastructure must exist (Phase B).

---

**Phase D: Whirlwind** (1-2 hours) 📝 PLANNED
- Prove Instant AOE Pattern works end-to-end
- Configure Whirlwind.tres (load from database)
- Refactor WhirlwindEffect.cs → WhirlwindVisual.cs (remove collision logic)
- Add Call Method tracks to warrior_whirlwind animation
- Implement OnAOEHitboxBodyEntered() damage application
- Test: Q key → animation → AOE hitbox → damage → kill tracking
- Tune radius/damage/duration in skill_balance_database.tres

**Why after Phase C:** 
Validates second hitbox pattern (AOE vs Melee).

---

**Phase E: Remaining Warrior Skills** (3-4 hours) 📝 PLANNED
- **Crowd Suppression** (Q alternative) - reuse Instant AOE pattern
- **Combat Stim** (R) - prove Buff pattern works
- **Breaching Charge** (E) - prove Cast-Spawn pattern works (dash + collision)
- Each skill gets database entry for easy tuning
- Each skill follows pattern from skill-standardization.md
- Validate all 6 patterns work end-to-end

---

**Phase F: Testing & Polish** (1-2 hours) 📝 PLANNED
- Playtest all 5 warrior skills in full run
- Adjust timings in AnimationPlayer
- Tune damage/cooldown/radius in skill_balance_database.tres
- Add visual effects polish
- Verify skill mastery tracking
- Validate upgrade interactions
- Bug fixing

---

### Estimated Duration
**Total: 12-19 hours** (2-3 work sessions)

**Phase A (Balance): 4-6 hours**
**Phases B-F (Skills): 8-13 hours**

### Success Criteria
- All 5 warrior skills functional and feeling good
- Animation-driven skills properly synced (hitboxes, timing, state transitions)
- Instant skills work without animation locks
- Hybrid hitbox approach validated (no rework needed for other classes)
- Skill standardization patterns proven extensible
- **Balance systems enable rapid iteration** (critical for success)

### Deferred to Post-Warrior
- Ranger class (5 skills)
- Mage class (8 skills)
- Additional enemy types
- Boss abilities

---

## Phase 4: Gear & Loot System 📝 NEXT

**Hypothesis:** "Loot drops add excitement and build variety"

### Planned Tasks

1. **Basic Gear Drops**
   - 3 slots: Weapon, Armor, Accessory
   - 3 rarities: Common, Rare, Epic
   - Flat stat bonuses (read from BalanceConfig)
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

**Phase 3.5 - Phases B-E (Remaining Warrior Skills)**

Phase A (Balance Systems) is complete! Now implementing the remaining 3 warrior skills using the established patterns and centralized balance database.

**Current Status:**
- ✅ Phase A Complete: Balance infrastructure ready
- ✅ Basic Attack (Fusion Cutter) functional
- ✅ Whirlwind functional
- 📝 Crowd Suppression - planned
- 📝 Combat Stim - planned
- 📝 Breaching Charge - planned

**See [`../../CLAUDE.md`](../../CLAUDE.md) for current session progress and next tasks.**

---

*This plan is flexible - we pivot based on validation results.*
