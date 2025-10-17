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

## Phase 4: Gear & Forge System 📝 NEXT

**Hypothesis:** "Forge crafting creates optimization puzzles without mandatory grind"

### Planned Tasks

**1. Material Drop System**
   - 5 material types (Essence, Ore, Fragments, Souls, Crystals)
   - Drop rates tied to enemy types and zone milestones
   - Integrate with loot tables

**2. Basic Forge (FP System MVP)**
   - Forging Potential mechanics
   - Add Affix action (choose stat, T1 only)
   - Upgrade Affix action (T1→T2→T3)
   - FP consumption and depletion
   - Cost: Refined materials + gold

**3. Inventory UI**
   - Equipment screen showing 4 slots
   - Forge interface (FP bar, affix list, cost preview)
   - Material inventory view
   - Gear comparison tooltips

**4. Gear Save/Load**
   - Persist equipped gear + inventory
   - Track FP remaining per item
   - Integrate with existing save system

### Success Criteria
- Players excited by finding high-FP items
- Forge feels impactful (T1→T3 noticeable power gain)
- FP creates "good enough" vs "perfect" decisions
- 70% of players engage with forge at least once
- Optional (can complete content with drops alone)

### Deferred to Phase 5
- Workshop (material processing)
- Treasury (gold generation)
- Advanced forge (T4-T5 affixes, reroll action)

**Estimated Duration:** 2-3 weeks

---

## Phase 5: Idle Systems 📝 PLANNED

**Hypothesis:** "Players return after being away because idle progress feels rewarding (not mandatory)"

### Tasks

**1. Workshop System (MVP)**
   - 3 processing slots
   - Basic conversions (Essence, Ore, Fragments → Refined)
   - Processing times (4-8-12 hours)
   - DateTime-based offline progression
   - Simple UI (queue view, collect button)

**2. Treasury System (MVP)**
   - Formula: (Zone² × 10) gold/hour
   - 12-hour accumulation cap
   - Collect UI with satisfying animations
   - Active bonus (dungeon runs give 3-5x gold)

**3. Workshop Upgrades (Tier 1)**
   - Processing Speed I (1.2x faster, 1k gold)
   - Extra Slot (+1 slot, 3k gold)
   - Bulk Convert (10x batch, 5k gold)

**4. Treasury Upgrades (Basic)**
   - Expansion I (12h → 18h cap, 5k gold)
   - Interest Rate I (+10% generation, 3k gold)

### Success Criteria
- 70% 1-week retention (up from 60%)
- Players check in after 4+ hours
- Treasury/Workshop gold gets spent
- Players DON'T feel forced to engage
- Active play still feels better (3-5x validation)

### Deferred to Phase 6
- Advanced workshop upgrades (Tiers 2-3)
- Advanced treasury upgrades
- Ascension system

**Estimated Duration:** 2-3 weeks

---

## Phase 6: Depth & Ascension 📝 PLANNED

**Hypothesis:** "Build diversity and long-term goals increase retention without overwhelming casuals"

### Tasks

**1. Ascension System (MVP)**
   - Basic ascension tree (2 branches: Combat + Economy)
   - 15-20 unlock nodes per branch
   - Soft reset mechanics (keep gear, reset zones)
   - Ascension Point calculation (zones + challenges)
   - UI for tree view and unlock confirmation

**2. Workshop Upgrades (Tiers 2-3)**
   - Tier 2: Processing Speed II, Quality Improvement, Auto-Collect (20k-50k gold)
   - Tier 3: Master Craftsman, 5th Slot, Legendary Refinement (100k+ gold)

**3. Treasury Upgrades (Advanced)**
   - Expansion II-III (18h → 24h cap)
   - Interest Rate II-V (compound to 1.5x)
   - Instant Collection (2 hours, 1/day)

**4. Forge Expansion**
   - Add T4-T5 affix tiers (higher costs, FP)
   - Add Reroll action (3-5 FP, cheap optimization)
   - T6-T7 affixes (drop-only, cannot craft)

### Success Criteria
- Optimizers have 50+ hour goals (complete ascension trees)
- Casuals not overwhelmed (can ignore ascension, still progress)
- 30% 1-month retention
- Players experiment with ascension builds
- "Good enough" vs "perfect" forge decisions validated

### Deferred to Phase 7
- Utility ascension branch (3rd branch)
- Challenge runs (modify dungeon rules for Crystals)
- Skill mastery system
- Additional dungeons

**Estimated Duration:** 2-3 weeks

---

## Phase 7: Endgame 📝 PLANNED

**Hypothesis:** "Multiple dungeons with varying lengths sustain long-term interest and respect player time"

### Tasks

**1. Ascension System Completion**
   - Add Utility branch (3rd branch)
   - Expand to 30+ total nodes per branch
   - Challenge-based AP bonuses
   - Ascension cosmetic rewards

**2. Multiple Dungeons (6 total)** - Varying lengths for different session times
   - Research Station Kepler (4 floors, 20-25 min)
   - Military Blacksite Omega (7 floors, 35-40 min)
   - Megacity New Babel (8 floors, 40-45 min)
   - Frost Grave (6 floors, 30-35 min)
   - Verdant Tomb (9 floors, 45-50 min)
   - Note: Zenith Station (5 floors, 25-30 min) is MVP dungeon

**3. Challenge Runs**
   - Modifiers (increased enemy HP, no healing, timer)
   - Reward: Crystals (ultra-rare material)
   - Leaderboards (optional)

**4. Difficulty Tiers** (Stretch Goal)
   - Normal (current)
   - Hard (higher scaling, 2x loot)
   - Nightmare (extreme scaling, 5x loot)

**5. Skill Mastery System** (Stretch Goal)
   - 3 mastery tiers (Bronze/Silver/Gold)
   - Permanent skill improvements
   - Track kills per skill

### Design Philosophy
**Respect player time through content variety:**
- Players with 20 minutes? Run Research Station (4 floors)
- Players with 30 minutes? Run Zenith or Frost Grave (5-6 floors)
- Players with 45+ minutes? Run Verdant Tomb (9 floors)
- All session lengths provide meaningful progression

### Success Criteria
- 20% 3-month retention
- Multiple dungeon engagement
- Ascension provides long-term goals (100+ hours)
- Positive "respects time" feedback

**Estimated Duration:** 2-3 weeks per update (ship sequentially)

**See [`../01-GAME-DESIGN/dungeon-structure.md`](../01-GAME-DESIGN/dungeon-structure.md) for complete dungeon designs (if exists)**

---

## What We're NOT Building

**Cut Forever:**
- Research Lab (passive power gain)
- Training Ground (weird currency)
- 6+ material types (5 is the limit)
- Gear crafting from scratch (Forge modifies drops, doesn't create)
- PvP/multiplayer (for MVP)

**Why Cut:**
- Doesn't serve core loop
- Adds complexity without value
- Can add later if players demand
- 5 materials already provides enough complexity

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
