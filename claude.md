# Zenith Rising - Project Documentation

## 📚 Documentation Structure

**This file (CLAUDE.md):** Day-to-day progress tracking (updated every session)

**Docs/ folder:** Stable reference documentation (organized by topic)

### Quick Links to Documentation

**👉 Start Here:** [`Docs/00-START-HERE.md`](Docs/00-START-HERE.md) - Navigation hub for all documentation

**Game Design (What we're building):**

- [`design-overview.md`](Docs/01-GAME-DESIGN/design-overview.md) - Core concept & philosophy
- [`systems-progression.md`](Docs/01-GAME-DESIGN/systems-progression.md) - Stats, gear, idle systems
- [`combat-skills.md`](Docs/01-GAME-DESIGN/combat-skills.md) - Combat mechanics
- [`narrative-framework.md`](Docs/01-GAME-DESIGN/narrative-framework.md) - Story & characters

**Implementation (How we're building it):**

- [`phase-plan.md`](Docs/02-IMPLEMENTATION/phase-plan.md) - **Development phases & current focus**
- [`skill-system-architecture.md`](Docs/02-IMPLEMENTATION/skill-system-architecture.md) - Technical skill system
- [`idle-systems-implementation.md`](Docs/02-IMPLEMENTATION/idle-systems-implementation.md) - Technical idle/meta-progression systems
- [`project-structure.md`](Docs/02-IMPLEMENTATION/project-structure.md) - File organization
- [`godot-patterns.md`](Docs/02-IMPLEMENTATION/godot-patterns.md) - Best practices & gotchas

**Content Design:**

- [`class-abilities.md`](Docs/03-CONTENT-DESIGN/class-abilities.md) - Full class skill designs
- [`upgrade-pool.md`](Docs/03-CONTENT-DESIGN/upgrade-pool.md) - All upgrade definitions
- [`enemy-design.md`](Docs/03-CONTENT-DESIGN/enemy-design.md) - Enemy types & behaviors

**Visual & Audio:**

- [`visual-style-guide.md`](Docs/04-VISUAL-AUDIO/visual-style-guide.md) - UI, colors, mockups
- [`asset-requirements.md`](Docs/04-VISUAL-AUDIO/asset-requirements.md) - Asset needs & sources

---

## Project Overview

A bullet hell roguelite with idle mechanics. Players fight through tower floors in active combat while idle systems refine materials between sessions.

**Core Philosophy:**

- Active play = power gains (character levels, gear, skill mastery)
- Idle time = efficiency gains (material refinement, gold generation)
- Idle enhances, never replaces active gameplay

---

## Current Status: Be Honest

**Phase:** Warrior Skill Implementation (Phase 3.5-B+) - ⏳ **IN PROGRESS**

**🎉 PHASES 1, 2, 3, & 3.5-A COMPLETE! 🎉**

**Phase 1 - Combat:** Proven fun and engaging through multiple playtests
**Phase 2 - Progression:** Character stats, save/load, stat allocation working
**Phase 3 - Hub World:** Scene flow and player initialization working
**Phase 3.5-A - Balance Systems:** Centralized config infrastructure complete

### ✅ Actually Working

**Core Combat (Phase 1):**
- Player movement (WASD) and rotation
- Enemy AI (chase player, contact damage)
- Health/damage system with crits
- XP shards drop and level-up flow
- Level-up panel with 3 upgrade choices
- Component architecture (StatsManager, SkillManager, UpgradeManager)
- Type-based skill executor pattern
- **3 working skills:** Whirlwind, Fireball, Basic attacks
- **8 working upgrades:** All functional with proper stacking
- **CombatSystem.cs** - Centralized damage calculation with null safety
- **3 enemy types:** Basic melee, FastMelee, SlowRanged (with projectiles)
- **Enemy scaling system** - HP/damage multipliers per wave/floor
- **Wave/floor system** - 10 waves + boss per floor, 5 floors total
- **Boss spawning** - At 5:00 mark with 5x HP, 2x damage
- **Boss defeat detection** - TreeExited signal tracking, enemy count management
- **Main menu** - Functional with styled buttons, scene transitions
- **Floor transition UI** - Continue/End Run panel with hover effects, pause/unpause
- **HUD integration** - Floor/wave display wired to Dungeon.cs
- **Victory screen** - Full stats display, return to hub when Floor 5 cleared
- **Death screen** - Full stats display, return to hub on player death

**Character Progression (Phase 2):**
- **Character stat system** - STR/INT/AGI/VIT/FOR with proper scaling
- **Stat allocation panel** - UI for spending stat points (press C in-game)
- **Save/load system** - JSON-based save with character + run state
- **Character leveling** - XP awards from runs, persistent character progression
- **Highest floor tracking** - Checkpoint system for floor completion

**Hub World (Phase 3):**
- **Hub scene** - Safe zone with player spawn
- **Dungeon portal** - Interact with E key to enter dungeon
- **Scene transitions** - Main Menu → Hub → Dungeon → Hub
- **Player initialization** - Proper setup for new and saved games
- **Save integration** - Hub correctly loads/saves character state

**Animation System (Warrior):**
- **Sprite2D + AnimationPlayer** - Custom FSM pattern chosen
- **Locomotion animations** - Walk/idle in all 4 directions complete
- **PlayerState enum** - FSM with transition validation working
- **Combat animations** - warrior_attack_[dir], warrior_whirlwind created
- **Phases 1-4 complete** - Foundation, locomotion, FSM, combat animations

**Balance Systems (Phase 3.5-A):**
- **GameBalance singleton** - Autoload with centralized config access
- **5 config Resources** - Player stats, progression, combat, enemies, upgrades (separate files)
- **Inspector-based tuning** - Edit balance values without recompilation
- **Hybrid architecture** - Individual .tres files + C# wrapper for clean access
- **Lazy initialization** - Null guards prevent timing issues during startup
- **Warrior combat working** - Basic attack and Whirlwind deal damage via hitboxes
- **SkillBalanceDatabase** - Centralized skill parameter storage

### ⏳ In Progress

**Warrior Skills Implementation:**
- ✅ Basic Attack (Fusion Cutter) - Melee Pattern (functional)
- ✅ Whirlwind - Instant AOE Pattern (functional + visual effect)
- ✅ Energy Wave - Hybrid Pattern (melee + 3 projectiles, functional)
- 📝 Leap Slam - Database entry added (not implemented)
- 📝 Combat Stim - Planned (Buff Pattern)
- 📝 Breaching Charge - Planned (Cast-Spawn Pattern)

**Combat Polish:**
- 📝 Animation timing refinement
- 📝 Hitbox frame tuning for better feel
- 📝 Visual effects for attacks

### 📝 Not Started (Phase 4+)

- Gear/equipment drops
- Materials or idle systems
- NPC vendors/systems
- More hub features

---

## Current Phase Focus

**Phase 3.5: Warrior Combat Implementation** ⏳ **IN PROGRESS**
**Sub-Phase A: Balance Systems Foundation** ⏳ **CURRENT PRIORITY**

### Phase A: Balance Systems Foundation (4-6 hours)

**Why This Phase Comes First:**
Before implementing 5 warrior skills (and eventually 18 total skills), we need centralized balance infrastructure. This prevents:
- Hardcoded magic numbers scattered across skill files
- Manual recompilation for every balance tweak  
- Inconsistent formulas between similar skills
- Difficulty comparing and tuning related values

**Investment Pays Off:**
- 4-6 hours setup → saves 10+ hours during Phases B-F
- Enables rapid iteration during playtesting
- Creates sustainable architecture for 18+ skills

**Current Tasks (Step-by-Step Guide):**

**STEP 1: Create BalanceConfig System (2 hours)**
1. Create `Scripts/Core/BalanceConfig.cs` with nested config classes:
   - PlayerStatsConfig (BaseMaxHealth, BaseSpeed, BaseDamage)
   - CharacterProgressionConfig (stat scaling formulas)
   - CombatSystemConfig (crit damage, damage type multipliers)
   - EnemyConfig (scaling formulas, aggro ranges, spawn rates)
   - UpgradeSystemConfig (upgrade values per stack)
2. Create `Scripts/Core/GameBalance.cs` singleton autoload
3. Create folder `Resources/Balance/`
4. Create resource `Resources/Balance/balance_config.tres`
5. Add GameBalance to Project Settings → Autoload
6. Assign balance_config.tres to GameBalance.Config export in editor

**STEP 2: Create SkillBalanceDatabase (2 hours)**
1. Create `Scripts/Skills/Balance/` folder
2. Create `SkillBalanceType.cs` enum (Projectile, InstantAOE, Melee, etc.)
3. Create `SkillBalanceEntry.cs` data structure (all skill parameters)
4. Create `SkillBalanceDatabase.cs` container with GetSkillBalance() lookup
5. Create resource `Resources/Balance/skill_balance_database.tres`
6. Assign to GameBalance.SkillDatabase export in editor
7. Add entries for existing skills:
   - warrior_basic_attack
   - whirlwind  
   - fireball

**STEP 3: Refactor Existing Systems (1-2 hours)**
1. **StatsManager.cs**: Replace ALL constants with Config reads:
   - STR_DAMAGE_PER_POINT → GameBalance.Instance.Config.CharacterProgression.StrengthDamagePerPoint
   - VIT_HEALTH_PER_POINT → GameBalance.Instance.Config.CharacterProgression.VitalityHealthPerPoint
   - (Do this for ALL stat scaling constants)
2. **UpgradeManager.cs**: Replace hardcoded upgrade values:
   - DamageBoostPerStack → GameBalance.Instance.Config.UpgradeSystem.DamageBoostPerStack
   - (Do this for ALL upgrade types)
3. **Dungeon.cs**: Replace enemy spawn/scaling hardcoded values
4. **Enemy.cs**: Replace aggro/leash range constants

**STEP 4: Update Skill Loading Pattern (1 hour)**  
1. Add `Initialize()` method to Skill.cs that loads from database
2. Change Skill.cs to use runtime properties (not exports) for balance values
3. Only SkillId remains as export on skill resources
4. Update SkillManager to call skill.Initialize() before first use
5. Simplify existing .tres files (WarriorBasicAttack, Whirlwind, Fireball)

**Testing & Validation:**
- Run game, verify player stats still work correctly
- Change BaseMaxHealth in balance_config.tres, verify change takes effect
- Verify Whirlwind/Fireball load damage from database
- No errors in console

**Success Criteria for Phase A:**
- ✅ GameBalance singleton accessible via GameBalance.Instance
- ✅ balance_config.tres holds all game-wide tuning values
- ✅ skill_balance_database.tres holds all skill-specific values
- ✅ StatsManager has ZERO hardcoded constants (all read from config)
- ✅ UpgradeManager reads from config
- ✅ Skills load damage/cooldown from database
- ✅ Can tune balance in Godot inspector without recompiling

**Documentation:**
- Create `Docs/02-IMPLEMENTATION/balance-systems-architecture.md` when complete
- Update CLAUDE.md Session Progress Log
- Mark Phase A as ✅ COMPLETE in this section

---

### Phases B-F: Skill Implementation (AFTER Phase A)

**Phase B: Skill System Standardization** (2-3 hours) 📝 NEXT
- Add CastBehavior and DamageSource enums to Skill.cs
- Update SkillManager.UseSkill() to route based on CastBehavior  
- Add hitbox control methods to Player.cs
- Create hitbox Area2D nodes in player.tscn

**Phase C: Fusion Cutter** (1-2 hours) 📝 PLANNED
- Prove Melee Pattern works
- Skill loads from database
- Animation-driven hitbox damage

**Phase D: Whirlwind** (1-2 hours) 📝 PLANNED  
- Prove Instant AOE Pattern works
- Refactor WhirlwindEffect → WhirlwindVisual
- AOE hitbox damage

**Phase E: Remaining Warrior Skills** (3-4 hours) 📝 PLANNED
- Crowd Suppression, Combat Stim, Breaching Charge
- Each loads from database

**Phase F: Polish** (1-2 hours) 📝 PLANNED
- Tune all values in inspector
- Visual effects polish

**Architecture completed:**
- ✅ Custom FSM + AnimationPlayer pattern chosen
- ✅ Sprite2D with atlas-based animation
- ✅ PlayerState enum with transition validation
- ✅ Locomotion animations (walk/idle, all directions)
- ✅ Combat animations created (attack, whirlwind)
- ✅ 6 skill implementation patterns designed
- ✅ All 18 skills mapped to patterns

**See:** [`Docs/02-IMPLEMENTATION/animation-architecture.md`](Docs/02-IMPLEMENTATION/animation-architecture.md) and [`Docs/02-IMPLEMENTATION/skill-standardization.md`](Docs/02-IMPLEMENTATION/skill-standardization.md)

---

**After Warrior Complete: Return to Phase 4 (Gear & Loot)**

**Implementation Strategy:**
Phase A (Balance Systems) is prioritized BEFORE Phase B (Skill Standardization) because:
1. Skills in Phases C-F will immediately use the database for their parameters
2. Prevents creating skills with hardcoded values that need refactoring later  
3. StatsManager refactor validates the pattern before applying to skills
4. Creates inspector-based workflow that accelerates all remaining warrior work

**The 4-6 hour investment in Phase A saves 10+ hours in Phases B-F.**

**See [`Docs/02-IMPLEMENTATION/phase-plan.md`](Docs/02-IMPLEMENTATION/phase-plan.md) for full phase details.**

---

## Session Progress Log

### Session 9 - Hub World & Dungeon Separation 🏠

**Completed:**

- ✅ Renamed game.tscn → dungeon.tscn and Game.cs → Dungeon.cs for architectural clarity
- ✅ Updated all scene transitions to return to hub instead of main menu
- ✅ Created Hub.cs script with proper player initialization using CallDeferred
- ✅ Created hub.tscn scene with player spawn and dungeon portal
- ✅ Created DungeonPortal.cs with Area2D interaction system ("[E] Enter Dungeon")
- ✅ Added 'interact' input action (E key) to project settings
- ✅ **Fixed critical bug in Player.Initialize():**
  - Previously only handled "load save" case
  - Added else clause to call StatsManager.Initialize() for new games
  - Bug prevented player movement (CurrentSpeed = 0) on fresh starts
- ✅ Updated MainMenu.cs to load hub scene for both New Game and Continue
- ✅ Updated StatsManager.cs death flow to find Dungeon node (not Game)
- ✅ Tested player movement in hub successfully

**Achievements:**

- 🎉 **Hub/Dungeon separation complete!** Architectural clarity achieved
- 🎉 **Scene flow working:** Main Menu → Hub → Dungeon → Hub
- 🎉 **Player initialization fixed** for both new and saved games
- 🏗️ **Foundation laid** for meta-progression systems (NPCs, vendors, idle mechanics)

**Lessons Learned:**

- Godot parent _Ready() runs before children, requiring CallDeferred for initialization
- Player.Initialize() needs to handle BOTH save loading AND fresh initialization
- Scene renaming (game → dungeon) improved code clarity significantly
- The bug existed in Phase 1 but wasn't caught due to always having save data during testing
- No architecture rework needed - hub/dungeon separation was clean

**Debugging Process:**

- Initial symptom: Player couldn't move in hub (CurrentSpeed = 0)
- Root cause: Player.Initialize() missing else clause for new game case
- Investigated initialization order, scene structure, StatsManager flow
- Used sequential thinking to trace through code paths
- Simple one-line fix resolved complex-seeming issue

**Next Session:**

- Complete hub → dungeon → hub flow testing
- Polish hub visuals (background, lighting, atmosphere)
- Consider adding placeholder NPCs or hub features

### Session 10 - Animation Architecture & Skill Standardization 🎨

**Context:**
After completing Phase 3 (Hub World), began work on warrior character animations and combat. Realized existing skill system needed standardization before scaling to 18 total skills across 3 classes.

**Research & Planning Completed:**

**Animation Architecture Decision:**
- ✅ Researched AnimationTree vs Custom FSM approaches
- ✅ **Decided on Custom FSM + AnimationPlayer pattern**
  - Reason: Unifies game logic with animation control (single source of truth)
  - AnimationTree would create two separate state machines requiring constant sync
  - Custom FSM simpler for sprite-based ability combat
- ✅ Replacing AnimatedSprite2D with Sprite2D + AnimationPlayer
  - AnimationPlayer keyframes `region_rect` property to show atlas frames
  - All animations (locomotion + combat) controlled by single AnimationPlayer
- ✅ Call Method tracks enable frame-perfect hitbox timing
- ✅ FSM not extracted as component - each entity gets own state machine

**Animation Implementation Progress:**
- ✅ **Phases 1-4 COMPLETED:**
  - Sprite2D setup with warrior.png atlas
  - Locomotion animations created (walk_down/up/left/right, idle_down/up/left/right)
  - PlayerState enum implemented (Idle, Running, BasicAttacking, CastingSkill, Hurt, Dead)
  - State transition logic with CanTransitionTo() validation
  - Combat animations created (warrior_attack_[dir], warrior_whirlwind)
- ⏳ **Phase 5 IN PROGRESS:** Creating hitbox nodes and wiring collision

**Skill System Standardization:**
- ✅ Analyzed all 18 planned skills across 3 classes
- ✅ Created **6 implementation patterns** based on two-axis classification:
  - **Axis 1:** CastBehavior (Instant vs AnimationDriven)
  - **Axis 2:** DamageSource (PlayerHitbox, EffectCollision, None)
- ✅ **Pattern Matrix Created:**
  1. **Melee Pattern** (AnimationDriven + PlayerHitbox) - Fusion Cutter, Psionic Wave
  2. **Instant AOE Pattern** (AnimationDriven + PlayerHitbox) - Whirlwind, Crowd Suppression
  3. **Projectile Pattern** (Instant + EffectCollision) - Fireball, Arc Lightning, Grenade
  4. **Cast-Spawn Pattern** (AnimationDriven + EffectCollision) - Breaching Charge, Dash skills
  5. **Buff Pattern** (Instant/Animated + None) - Combat Stim, Fortify, Overwatch
  6. **Persistent Zone Pattern** (Instant + EffectCollision) - Void Rift, Singularity
- ✅ All 18 skills mapped to patterns with implementation checklist
- ✅ Design validated as extensible (can add new patterns by extending axes)

**Hybrid Hitbox Approach:**
- **PlayerHitbox for melee/AOE:** Animation-driven timing, Player applies damage
- **EffectCollision for projectiles/zones:** Independent entity collision, Effect applies damage
- WhirlwindEffect.cs to be refactored → WhirlwindVisual.cs (remove collision, purely visual)

**Documentation Created:**
- ✅ Committed animation architecture to memory
- ✅ Committed skill standardization patterns to memory
- ✅ Created implementation checklist template
- ✅ Created skill type mapping table for all 18 skills
- ✅ Created [`animation-architecture.md`](Docs/02-IMPLEMENTATION/animation-architecture.md)
- ✅ Created [`skill-standardization.md`](Docs/02-IMPLEMENTATION/skill-standardization.md)

**Current Status:**
- **Phase 3 (Hub) COMPLETE** ✅
- **Animation System:** Foundation complete, hitboxes in progress
- **Skill Standardization:** Fully planned, NOT yet implemented

**Path Forward - Warrior Focus:**
1. **Phase A:** Implement skill standardization (add enums, hitbox infrastructure)
2. **Phase B:** Get Fusion Cutter (basic attack) working - prove Melee Pattern
3. **Phase C:** Get Whirlwind working - prove Instant AOE Pattern
4. **Phase D:** Implement remaining warrior skills (Crowd Suppression, Combat Stim, Breaching Charge)
5. **Phase E:** Polish warrior animations and combat feel

**Design Decisions:**
- Focus on completing ONE class (Warrior) fully to validate system
- Standardization enables rapid implementation of remaining 13 skills
- Animation + skill work happens together (not separate phases)
- Return to Phase 4 (Gear & Loot) after warrior completion

**Lessons Learned:**
- Taking time to standardize before scaling = smart investment
- Planning detour was valuable - clear, extensible system designed
- Breaking overwhelming work into phases (A-E) makes it manageable
- Warrior best starting point (hardest patterns - melee/AOE/dash)

**Next Session:**
- Begin Phase A: Skill system standardization implementation
- Add CastBehavior and DamageSource enums to Skill.cs
- Create hitbox nodes in player.tscn
- Update SkillManager.UseSkill() to route based on CastBehavior

### Session 11 - Balance Systems Foundation (Phase 3.5-A) COMPLETE! 🎯

**Context:**
After Session 10 planning work, began implementation of centralized balance systems infrastructure. Session focused on creating Godot-friendly Resource architecture and fixing initialization timing issues.

**Completed:**

**Balance System Architecture (Phase 3.5-A):**
- ✅ Created 5 separate config Resource classes in `Scripts/Core/Config/`:
  - PlayerStatsConfig.cs (BaseMaxHealth, BaseSpeed, BaseFireRate, BaseMeleeRate, BasePickupRadius)
  - CharacterProgressionConfig.cs (stat scaling formulas, XP curves)
  - CombatSystemConfig.cs (crit multipliers, damage caps)
  - EnemyConfig.cs (spawn rates, scaling formulas, wave/floor system)
  - UpgradeSystemConfig.cs (upgrade values per stack)
- ✅ Created BalanceConfig.cs wrapper class (plain C#, not a Resource)
- ✅ Modified GameBalance.cs to use hybrid approach:
  - Exports individual .tres resources (PlayerStatsResource, etc.)
  - Assembles BalanceConfig wrapper in _Ready() for clean code access
  - Added validation for all Resource properties before creating Config
- ✅ Created 5 .tres resource files in `Resources/Balance/`:
  - player_stats_config.tres
  - character_progression_config.tres
  - combat_system_config.tres
  - enemy_config.tres
  - upgrade_system_config.tres
- ✅ Updated game_balance.tscn to reference all 5 individual .tres files
- ✅ Fixed project.godot autoload to point to game_balance.tscn (not .cs file)

**Initialization Timing Fixes:**
- ✅ Added null guards to prevent NullReferenceException during startup:
  - StatsManager.cs: Field initializers use default values, config loads in _Ready()
  - UpgradeManager.cs: Added lazy initialization with InitializeUpgradeList() method
  - Dungeon.cs: Added null check in _PhysicsProcess() to skip frame if GameBalance not ready
  - StatAllocationPanel.cs: Added null guards to all formatting methods
- ✅ Fixed Godot Resource initialization order issues (autoload vs scene nodes)

**Combat System Integration:**
- ✅ Warrior basic attack (Fusion Cutter) now functional with hitbox damage
- ✅ Whirlwind AOE attack working
- ✅ Animation-driven combat with Call Method tracks for hitboxes
- ✅ Fixed animation loop mode (animations now finish properly)
- ✅ Player state machine returns to locomotion after attacks
- ✅ Damage calculation working through centralized CombatSystem.cs

**Skill System Updates:**
- ✅ SkillManager routes skills by CastBehavior (Instant vs AnimationDriven)
- ✅ Player.TryBasicAttack() sets _currentCastingSkill for hitbox damage calculation
- ✅ Skill.Initialize() loads balance data from SkillBalanceDatabase
- ✅ Skills use runtime properties loaded from database (not hardcoded exports)

**Challenges Solved:**

1. **Nested Resources Problem:**
   - **Issue:** Godot's C# system struggles with Resources nested in .tres files
   - **Root Cause:** Multiple [GlobalClass] attributes in one file, corrupted sub-resource references
   - **Solution:** Split config classes into separate files, use hybrid architecture (individual .tres + wrapper)

2. **GameBalance Initialization Timing:**
   - **Issue:** Scene nodes' _Ready() sometimes called before autoload _Ready()
   - **Root Cause:** Godot initialization order not guaranteed for autoload exports
   - **Solution:** Lazy initialization pattern with null guards in all consuming systems

3. **Animation Looping Infinitely:**
   - **Issue:** Attack animations looped forever, player stuck in attack state
   - **Root Cause:** Animation loop_mode = 1, AnimationFinished never fired
   - **Solution:** Changed loop_mode to 0 (no loop) in Godot editor

4. **No Damage Being Dealt:**
   - **Issue:** Attacks played but enemies took no damage
   - **Root Cause:** Hitboxes never enabled (no Call Method tracks in animations)
   - **Solution:** Added Call Method tracks to animations for EnableMeleeHitbox/DisableMeleeHitbox

**Architecture Insights:**

- **Godot Resource Best Practices:**
  - Each [GlobalClass] Resource should be in its own file
  - Avoid deep nesting of Resources in .tres files
  - Use plain C# classes as wrappers for code convenience
  - Individual .tres files are easier to edit and maintain

- **Autoload Initialization:**
  - Autoloads initialize in order defined in project.godot
  - Scene nodes may _Ready() before autoload _Ready() completes
  - Always add null guards when accessing autoload exports
  - Lazy initialization pattern handles timing uncertainty

**Testing Results:**
- ✅ Game loads without errors
- ✅ Player moves normally in hub and dungeon
- ✅ Basic attacks (left click) deal damage and finish properly
- ✅ Whirlwind (right click) deals AOE damage
- ✅ Player returns to locomotion state after attacks
- ✅ Balance values can be edited in Godot inspector without recompiling
- ✅ All GameBalance.Instance.Config references work correctly

**Achievements:**

- 🎉 **Phase 3.5-A COMPLETE!** Balance Systems Foundation fully implemented
- 🎯 **Centralized balance infrastructure** ready for rapid iteration
- 🏗️ **Architecture validated** through Godot C# Resource system challenges
- ⚔️ **Warrior combat functional** with animation-driven hitbox damage
- 📊 **Inspector-based workflow** enables tuning without recompilation

**Lessons Learned:**

- Godot's Resource system requires careful architecture for C# classes
- Split complex nested structures into flat individual files for Godot
- Hybrid approach (individual exports + wrapper) balances editor and code convenience
- Sequential thinking tool invaluable for tracing initialization order bugs
- Taking time to build infrastructure properly pays off for rapid iteration

**Next Session:**

- Polish animation timing and hitbox frames for better combat feel
- Implement remaining warrior skills (Crowd Suppression, Combat Stim, Breaching Charge)
- Test balance values by tweaking in inspector during playtesting
- Consider adding visual effects for attacks

### Session 12 - Code Quality & Cleanup - A+ ACHIEVEMENT! ✨

**Context:**
After completing Phase 3.5-A (Balance Systems), performed comprehensive code review and cleanup to achieve production-ready code quality across all warrior combat systems.

**Completed:**

**Dead Code Removal (~53 lines):**
- ✅ Removed TryBasicAttack() from Player.cs (unified to TryCastSkill)
- ✅ Removed OnMeleeHitboxBodyEntered() from Player.cs (moved to SkillAnimationController)
- ✅ Removed ApplyMeleeHitboxDamage() from Player.cs (moved to SkillAnimationController)
- ✅ Removed EnableMeleeHitbox() from Player.cs (moved to SkillAnimationController)
- ✅ Removed DisableMeleeHitbox() from Player.cs (moved to SkillAnimationController)
- ✅ Removed BasicAttacking state from PlayerState enum
- ✅ Updated CanTransitionTo() logic to remove BasicAttacking references
- ✅ Cleaned up orphaned using statements and comments

**Critical Bug Fixes:**
1. **ProjectileLifetime Never Loaded:**
   - Issue: Database had value but Skill.cs never read it
   - Fix: Added `ProjectileLifetime = entry.ProjectileLifetime;` to Skill.Initialize()

2. **Validation Bugs in SkillManager:**
   - Issue: PrimarySkill validated twice, BasicAttackSkill never validated
   - Fix: Corrected ValidateSkill calls in _Ready()

3. **Mastery Bonuses Hardcoded:**
   - Issue: Diamond tier bonuses (+50% speed, +2 pierce) hardcoded in projectile
   - Fix: Loaded from SkillBalanceEntry (DiamondSpeedBonus, DiamondPierceBonus)
   - Database updated with values for all mastery tiers

**FSM Simplification:**
- ✅ **6 states → 5 states** by removing BasicAttacking
- ✅ Unified skill casting through single TryCastSkill() method
- ✅ All skills (basic attack, whirlwind, future skills) use same code path
- ✅ Cleaner state machine with fewer transitions to validate

**Code Quality Achievements:**
- ✅ **Player.cs: A+ Grade** - Clean FSM, no dead code, proper delegation
- ✅ **SkillManager.cs: A+ Grade** - Correct validation, unified casting
- ✅ **SkillAnimationController.cs: A+ Grade** - All hitbox logic centralized
- ✅ **EnergyProjectile.cs: A+ Grade** - Data-driven mastery bonuses
- ✅ **Skill.cs: A+ Grade** - Complete database loading
- ✅ **SkillBalanceEntry.cs: A+ Grade** - All mastery fields defined

**Architecture Improvements:**
- **Separation of Concerns:** Player.cs handles FSM, SkillAnimationController handles combat
- **Data-Driven Design:** ALL skill parameters from database (zero hardcoding)
- **Unified Patterns:** Single code path for all skill execution
- **Extensibility:** Adding new skills requires NO Player.cs changes

**Testing Results:**
- ✅ Basic attack works (melee hitbox damage)
- ✅ Whirlwind works (AOE hitbox damage)
- ✅ Projectile skills load all parameters correctly
- ✅ Mastery bonuses apply dynamically from database
- ✅ No console errors or warnings

**Achievements:**

- 🎉 **A+ Code Quality Across All Combat Systems!**
- 🏗️ **Production-Ready Architecture** - Clean, maintainable, extensible
- 🔥 **~53 Lines of Dead Code Removed** - Leaner, clearer codebase
- 🐛 **3 Critical Bugs Fixed** - ProjectileLifetime, validation, mastery
- 🎯 **Data-Driven Mastery System** - All tiers load from database
- ⚙️ **FSM Simplified** - 6 states → 5 states, unified casting

**Lessons Learned:**

- Code reviews after major features prevent technical debt accumulation
- Removing dead code improves readability more than adding comments
- Unified code paths reduce bug surface area significantly
- Data-driven design requires discipline but pays massive dividends
- FSM simplification (fewer states) = fewer edge cases to test
- Taking time for cleanup between phases maintains code quality

**Ready for Next Phase:**
With A+ code quality achieved, the codebase is ready for:
- Energy Wave implementation (hybrid melee + projectile pattern)
- Remaining warrior skills (Crowd Suppression, Combat Stim, Breaching Charge)
- Rapid iteration without fighting technical debt

**Next Session:**

- Update documentation to reflect Session 12 achievements
- Implement Energy Wave (validate hybrid pattern)
- Continue warrior skill implementation

### Session 13 - Energy Wave & Mouse-Aimed Combat - Hybrid Pattern Validated! ⚔️

**Context:**
After Session 12 cleanup, implemented Energy Wave (hybrid melee + projectile skill), mouse-aimed combat system, hitbox refinement, and Whirlwind visual effects.

**Completed:**

**Energy Wave Implementation (Hybrid Pattern):**
- ✅ Created warrior_energy_wave animations (4 directional variants)
- ✅ Added Call Method tracks: EnableMeleeHitbox, DisableMeleeHitbox, SpawnWaveProjectiles
- ✅ Created WarriorEnergyWave.tres skill resource
- ✅ Wired to E key (SecondarySkill slot in SkillManager)
- ✅ Fixed critical bug: DamageEntityBase type checking (was checking for SkillEntityBase)
- ✅ Fixed initialization timing: Initialize() must be called BEFORE AddChild()
- ✅ Melee swing deals 30 damage + spawns 3 projectiles (15 damage each)
- ✅ Projectiles spread at 25° angle in attack direction

**Mouse-Aimed Combat System:**
- ✅ Attacks now aim toward mouse cursor (independent of movement direction)
- ✅ GetMouseDirection() added to Player.cs
- ✅ GetSkillAnimationName() uses mouse direction for attack animations
- ✅ SkillAnimationController.SpawnWaveProjectiles() uses GetAttackDirection()
- ✅ Character sprite faces mouse for both locomotion AND attacks (twin-stick controls)
- ✅ All 4 directional animations (up/down/left/right) work with mouse aiming

**Hitbox Refinement:**
- ✅ Added UpdateMeleeHitboxPosition() to SkillAnimationController
- ✅ Melee hitbox dynamically positioned based on attack direction
- ✅ Hitbox rotation matches attack angle (0°/90°/180°/-90°)
- ✅ Hitbox sizes tuned to match animation visual range
- ✅ Fixed: Melee hitbox no longer stuck facing right

**Whirlwind Visual Effect:**
- ✅ Created WhirlwindVisual.cs with procedural _Draw() rendering
- ✅ Spinning ring visual shows actual AOE hitbox radius (150 pixels)
- ✅ Upgrade-compatible: Rotation speed scales with skill parameters
- ✅ Added SpawnWhirlwindVisual() to SkillAnimationController
- ✅ Visual fades out over last 20% of duration
- ✅ Pulse effect scales with rotation count

**Critical Bug Fixes:**
- ✅ Projectiles spawning with 0 speed/direction (Initialize called after AddChild)
- ✅ Type mismatch: Changed SkillEntityBase check to DamageEntityBase
- ✅ DrawCircle parameter order corrected
- ✅ Added null guards throughout SkillAnimationController

**Achievements:**

- 🎉 **Hybrid Pattern Validated!** Energy Wave proves melee + projectile combo works
- 🎯 **Mouse-Aimed Combat!** Twin-stick controls feel responsive and tactical
- 🎨 **Visual Feedback Complete!** Players can see Whirlwind AOE range clearly
- 🔧 **Hitbox System Refined!** Attacks hit where they visually appear to hit
- 🏗️ **Architecture Improved!** Initialization patterns and upgrade compatibility established

**Skills Status:**
- ✅ Fusion Cutter (Basic Attack) - Melee Pattern
- ✅ Whirlwind (Special Attack) - Instant AOE Pattern + visual effect
- ✅ Energy Wave (Secondary) - Hybrid Pattern (melee + projectiles)
- 📝 Leap Slam - Database entry added
- 📝 Combat Stim - Planned
- 📝 Breaching Charge - Planned

**Lessons Learned:**

- Initialize() must be called BEFORE AddChild() for projectiles/effects
- DamageEntityBase is the correct base class for collision effects (not SkillEntityBase)
- Mouse-aimed attacks create twin-stick shooter feel (movement + aim independent)
- Procedural drawing (_Draw) requires QueueRedraw() every frame for animated effects
- Upgrade-compatible visuals should read skill parameters, not hardcode values
- Visual feedback dramatically improves player understanding of skill ranges

**Architecture Insights:**

- **Hybrid Pattern Works:** Single skill can use both PlayerHitbox AND EffectCollision
- **Twin-Stick Controls:** WASD movement + mouse aiming is natural and tactical
- **Visual Effect System:** Initialize(skill) pattern allows upgrade scaling
- **Hitbox Positioning:** Dynamic calculation based on attack direction required for mouse-aiming

**Next Session:**

- Implement remaining warrior skills (Leap Slam, Combat Stim, Breaching Charge)
- Polish existing skills (timing, VFX, balance tuning)
- Consider energy/resource system for skills

### Session 8 - Victory & Death Screens - PHASE 1 COMPLETE! 🎉

**Completed:**

- ✅ VictoryScreen.cs + victory_screen.tscn implementation
- ✅ DeathScreen.cs + death_screen.tscn implementation
- ✅ Both screens display run statistics:
  - Time survived (MM:SS format)
  - Enemies killed
  - Final player level
  - Floors reached/completed
- ✅ Game.cs integration:
  - Added \_totalGameTime and \_enemiesKilled tracking
  - ShowVictoryScreen() implementation
  - OnPlayerDeath() public method
  - ValidateDependencies() updated for both screens
- ✅ StatsManager.cs death flow updated to call Game.OnPlayerDeath()
- ✅ Fixed UI input issues:
  - ColorRect mouse_filter set to Ignore
  - process_mode = 3 (Always) on root nodes for pause compatibility
  - Button signal connections wired correctly
- ✅ Both screens properly pause game and unpause on menu return
- ✅ Comprehensive code reviews throughout implementation

**Achievements:**

- 🎉 **PHASE 1 COMPLETE!** All combat loop features implemented and working
- 🎉 **Full game cycle tested:** Start → Combat → Victory/Death → Menu
- 🎉 **Production-ready game loop:** No stub methods, all features functional

**Lessons Learned:**

- Godot pause system requires process_mode = Always on UI that needs interaction when paused
- ColorRect overlays need mouse_filter = Ignore to allow clicks through
- Signals must be connected either in editor or code - no automatic connections
- Scene tree paths for GetNode need careful consideration (autoload vs scene hierarchy)

**Next Session:**

- Begin Phase 2: Character stat system (STR/VIT/AGI/RES/FOR)

### Session 7 - Floor Transition UI & Code Quality

**Completed:**

- ✅ Floor Transition Panel full implementation (FloorTransitionPanel.cs + scene)
- ✅ Boss defeat detection via TreeExited signal and enemy count tracking
- ✅ Floor advancement system (AdvanceToNextFloor with proper state reset)
- ✅ Comprehensive project code review (Game.cs, Player.cs, components, enemies, UI)
- ✅ Fixed critical bugs:
  - Tween memory leak in button hover animations
  - Enemy count not resetting between floors
  - CombatSystem null reference risks
  - Missing ExperienceShardScene validation
  - HUD not displaying floor/wave info
- ✅ Code improvements:
  - Exported NodePaths for flexibility
  - Extracted constants (hover scale, animation duration)
  - Parameter validation in ShowPanel()
  - Centralized dependency validation
- ✅ Namespace consistency fix (Scripts.Progression.Upgrades)
- ✅ Validated project architecture (component pattern, signals, virtual methods)

**Achievements:**

- 🎉 **Complete game loop now works:** Main Menu → Game → Floor Transitions → Main Menu
- 🎉 **Boss fights functional:** Spawn at 5:00, defeat triggers floor transition
- 🎉 **UI flow polished:** Pause/unpause, screen-space rendering, hover effects

**Remaining for Phase 1:**

- Victory screen (Floor 5 completion)
- Death screen improvement

**Next Session:**

- Complete victory/death screens → Phase 1 DONE
- Begin Phase 2: Character stats + save system

### Session 6 - Documentation Reorganization

**Completed:**

- ✅ Created organized `Docs/` folder structure (4 categories)
- ✅ Consolidated 4 versions of tower_idle_design → 3 focused files
- ✅ Moved existing docs to organized folders
- ✅ Created new docs: phase-plan, godot-patterns, asset-requirements, enemy-design
- ✅ Added 00-START-HERE.md navigation hub
- ✅ Cross-referenced all documentation
- ✅ Updated CLAUDE.md with doc links
- ✅ Trimmed CLAUDE.md to focus on daily tracking

### Session 5 - Main Menu & Combat Validation

- ✅ Main menu scene with styled buttons
- ✅ Scene transitions working
- 🎉 **Phase 1 hypothesis PROVEN** - Combat is fun!

### Session 4 - Wave/Floor System

- ✅ Wave/floor tracking (10 waves per floor)
- ✅ 30-second wave timer with auto-progression
- ✅ Spawn rate escalation (2.0s → 0.8s)
- ✅ Enemy scaling multipliers (+10% HP, +5% damage per wave)
- ✅ Boss spawning at 5:00 mark
- ✅ AdvanceToNextFloor() method

### Session 3 - Enemy Variety & Combat Polish

- ✅ CombatSystem.cs for centralized damage
- ✅ Fixed all 5 broken upgrades
- ✅ FastMeleeEnemy (speed variant)
- ✅ SlowRangedEnemy (kiting + projectiles)
- ✅ Enemy inheritance with virtual methods
- ✅ Enemy scaling system

### Session 2 - Stats Consolidation

- ✅ Stats consolidation refactor
- ✅ Fixed upgrade stacking math
- ✅ Fixed MaxHealth level-up bug
- ✅ Architecture decisions: Components for Phase 1

### Session 1 - Core Combat MVP

- ✅ Player movement and basic attacks
- ✅ Enemy AI and health system
- ✅ XP/level-up system
- ✅ 8 upgrade types (initially broken)

---

## Known Issues

- Main menu needs background texture (polish, non-critical)
- Signal connection patterns inconsistent across project (editor vs code)

---

## Tech Stack

- **Engine:** Godot 4.3+ with .NET
- **Language:** C# (prefer over GDScript)
- **Platform:** PC (Steam) for MVP
- **Art:** 2D top-down, placeholder sprites
- **Version Control:** Git + GitHub

---

## Quick Reference

### Current File Structure

```
SpaceTower/
├── Scenes/
│   ├── Core/
│   │   ├── hub.tscn (Safe zone with portal)
│   │   └── dungeon.tscn (Combat zone, formerly game.tscn)
│   ├── Player/player.tscn
│   ├── Enemies/ (enemy.tscn, fast_melee_enemy.tscn, slow_ranged_enemy.tscn, boss.tscn)
│   ├── Items/experience_shard.tscn
│   ├── SkillEffects/ (fireball_projectile.tscn, whirlwind_effect.tscn)
│   └── UI/
│       ├── Menus/ (main_menu.tscn, floor_transition_panel.tscn)
│       ├── Panels/ (level_up_panel.tscn, victory_screen.tscn, death_screen.tscn, stat_allocation_panel.tscn, results_screen.tscn)
│       └── hud.tscn
├── Scripts/
│   ├── Core/
│   │   ├── Hub.cs (Safe zone management)
│   │   ├── Dungeon.cs (Combat/spawning, formerly Game.cs)
│   │   ├── DungeonPortal.cs (Interaction system)
│   │   ├── CombatSystem.cs
│   │   ├── SaveManager.cs
│   │   └── SaveData.cs
│   ├── Player/
│   │   ├── Player.cs
│   │   └── Components/ (StatsManager.cs, SkillManager.cs, UpgradeManager.cs)
│   ├── Skills/ (Skill.cs, executors, effects)
│   ├── UI/
│   │   ├── Panels/ (VictoryScreen.cs, DeathScreen.cs, FloorTransitionPanel.cs, LevelUpPanel.cs, StatAllocationPanel.cs, ResultsScreen.cs)
│   │   └── HUD/ (Hud.cs)
│   └── Enemies/Base/Enemy.cs
└── Resources/Skills/ (Fireball.tres, Whirlwind.tres, MageBasicAttack.tres)
```

### Enemy Scaling Formula

```csharp
float healthMult = (1 + currentWave * 0.1f) * (1 + currentFloor * 0.5f);
float damageMult = (1 + currentWave * 0.05f) * (1 + currentFloor * 0.5f);
```

### Character Stat System (Implemented - Phase 2)

**5 Core Stats:** STR, INT, AGI, VIT, FOR

For complete stat formulas and progression details, see [`Docs/01-GAME-DESIGN/systems-progression.md`](Docs/01-GAME-DESIGN/systems-progression.md#the-5-core-stats)

**Quick Reference:**
- Starting stats: 0/0/0/0/0 + 15 points to distribute
- Character Level: Permanent, awards 1 stat point per level
- Character XP: Earned from dungeon runs (50 base + per floor/boss)
- Power Level: Per-run progression, resets to 1 after death/victory
- Power XP: Earned from enemies during run

---

## Development Principles

1. **Be honest about progress** - No aspirational percentages
2. **Prove before building** - Validate each phase hypothesis
3. **Simplify ruthlessly** - Cut features that don't serve core loop
4. **Playtest constantly** - Feel before features
5. **Ship small** - 2-week MVP beats 6-month vaporware
6. **Document as you go** - Update CLAUDE.md daily, Docs/ when stable
7. **Stay in phase** - Don't jump ahead, finish current phase first

---

## For Claude Code

**When helping with this project:**

1. **Check current phase** - See "Current Phase Focus" section above
2. **Reference docs** - Point to relevant files in Docs/ for context
3. **Update progress** - Add completed work to Session Progress Log
4. **Don't jump phases** - Remind me if I try to add Phase 2+ features

**Documentation guidelines:**

- **CLAUDE.md** = Daily tracking (this file)
- **Docs/** = Stable reference (organized by topic)
- Update CLAUDE.md after every session
- Update Docs/ when features stabilize

**Quick doc lookups:**

- Design questions → [`Docs/01-GAME-DESIGN/`](Docs/01-GAME-DESIGN/)
- Technical questions → [`Docs/02-IMPLEMENTATION/`](Docs/02-IMPLEMENTATION/)
- Content specs → [`Docs/03-CONTENT-DESIGN/`](Docs/03-CONTENT-DESIGN/)
- Visual/UI → [`Docs/04-VISUAL-AUDIO/`](Docs/04-VISUAL-AUDIO/)

**Core rule:** The sequential plan prevents scope creep. Stay focused on current phase.

---

### Session 14 - Planned: Composition Refactor + Leap Slam 📋

**Status:** Ready for next session

**Part 1: Composition Pattern Refactor (1-2 hours)**
- Create sub-Resource classes: ProjectileData, MeleeData, AOEData, ExplosionData
- Refactor SkillBalanceEntry to use composition (remove skill-type-specific properties)
- Update Skill.Initialize() with helper methods for loading sub-Resources
- Update existing .tres files (warrior_basic_attack, warrior_whirlwind, warrior_energy_wave)
- Validate all 3 existing warrior skills still work

**Part 2: Leap Slam Implementation (1-2 hours)**
- Create leap_slam database entry using new composition architecture
- Create warrior_leap_slam_[dir] animations with jump sprite frames
- Add Call Method tracks for takeoff/apex/landing hitbox
- Create Resources/Skills/Warrior/LeapSlam.tres
- Wire to Q key, test in all 4 directions

**Why Refactor Now:**
- SkillBalanceEntry already showing bloat (whirlwind-specific rotation properties)
- Composition pattern scales to 30+ skills without class bloat
- Inspector will only show relevant properties per skill type
- Memory efficient (skills only load properties they use)

**Architecture Decision:**
- Composition + Strategy Pattern recommended (deferred Strategy to later)
- Sub-Resources map to 6 skill patterns (Projectile, Melee, AOE, Explosion, Buff, Zone)
- Data-driven approach maintains current philosophy

**See Session 13 conversation for full scalability analysis and technique comparison.**

---

_Last updated: Session 13 - Energy Wave & Mouse-Aimed Combat complete, Session 14 planned!_
_🎉 PHASES 1, 2, 3, & 3.5-A COMPLETE - 3 warrior skills working, hybrid pattern validated! 🎉_

temp storage
Phase 1: Complete Executor Pattern Architecture - Detailed Implementation Plan
🎯 Strategic Goal
Validate the skill system architecture by implementing the complete executor pattern foundation, enabling rapid addition of 18+ planned skills with minimal code changes per skill.
📋 Memory & Cleanup Complete
✅ Committed to memory:
Executor Pattern Architecture Decision (complete pattern, not remove)
Skill System Three-Layer Architecture (Executor → Effect → Systems)
18 Planned Skills Validation (all feasible with extensions)
✅ Memory pruned:
Removed outdated "executor pattern half-implemented" issue (now superseded by decision to complete it)
🏗️ Implementation Phases
Phase 1A: Foundation & Refactoring (2-3 hours)
Task 1.1: Update CastBehavior Enum
File: Scripts/Skills/Base/Skill.cs Add new CastBehavior types:
public enum CastBehavior
{
    Instant,           // Non-interrupting (can use while doing other actions)
    AnimationDriven,   // Interrupting (locks FSM to CastingSkill state)
    Channeled,         // Hold button for continuous effect
    Charged,           // Hold to power up, release to fire
    Combo,             // Input sequence with timing windows
    Toggle             // On/off state (Overwatch precision mode)
}
Validation: Enum compiles, existing skills still work
Task 1.2: Create AnimationDrivenExecutor
New File: Scripts/Skills/Executors/AnimationDrivenExecutor.cs Extract AnimationDriven logic from Player.TryCastSkill():
public class AnimationDrivenExecutor : ISkillExecutor
{
    public bool ExecuteSkill(Player player, Skill skill)
    {
        // Set current skill for animation callbacks
        player.SkillAnimationController.SetCurrentSkill(skill);
        
        // Get directional animation name
        string animName = player.GetSkillAnimationName(skill);
        
        // Play animation (triggers Call Method tracks)
        player.AnimationPlayer.Play(animName);
        
        // Transition FSM to CastingSkill state
        player.SetState(PlayerState.CastingSkill);
        
        return true;
    }
}
Requirement: Player.cs must expose:
AnimationPlayer property (public getter)
SetState(PlayerState) public method
GetSkillAnimationName(Skill) public method
SkillAnimationController property (already public)
Validation: Whirlwind, Energy Wave, Basic Attack still work via executor
Task 1.3: Refactor InstantProjectileExecutor → InstantExecutor
File: Scripts/Skills/Executors/InstantProjectileExecutor.cs (rename to InstantExecutor.cs) Generalize for ANY instant effect (not just projectiles):
public class InstantExecutor : ISkillExecutor
{
    public bool ExecuteSkill(Player player, Skill skill)
    {
        // Set current skill (for tracking, even though instant)
        player.SkillAnimationController.SetCurrentSkill(skill);
        
        // Spawn effect immediately if skill has one
        if (skill.SkillEffectScene != null)
        {
            Vector2 direction = player.GetAttackDirection();
            var effect = skill.SkillEffectScene.Instantiate<DamageEntityBase>();
            
            // Initialize BEFORE adding to tree
            effect.Initialize(skill, player, direction);
            
            // Add to world
            player.GetTree().Root.AddChild(effect);
            effect.GlobalPosition = player.GlobalPosition;
        }
        
        // Optional: Play cosmetic animation (doesn't control timing)
        if (skill.HasPlayerAnimation)
        {
            string animName = player.GetSkillAnimationName(skill);
            player.AnimationPlayer.Play(animName);
        }
        
        // DO NOT change FSM state (non-interrupting behavior)
        
        return true;
    }
}
Validation: Skills can be used without interrupting current action
Task 1.4: Refactor Player.TryCastSkill()
File: Scripts/Player/Player.cs Replace inline logic with executor routing:
public bool TryCastSkill(Skill skill)
{
    if (skill == null) return false;
    
    // Route to appropriate executor based on CastBehavior
    ISkillExecutor executor = skill.CastBehavior switch
    {
        CastBehavior.Instant => new InstantExecutor(),
        CastBehavior.AnimationDriven => new AnimationDrivenExecutor(),
        CastBehavior.Channeled => new ChanneledExecutor(),
        CastBehavior.Charged => new ChargedExecutor(),
        CastBehavior.Combo => new ComboExecutor(),
        CastBehavior.Toggle => new ToggleExecutor(),
        _ => null
    };
    
    if (executor == null)
    {
        GD.PrintErr($"No executor for CastBehavior: {skill.CastBehavior}");
        return false;
    }
    
    return executor.ExecuteSkill(this, skill);
}
Validation:
Existing 3 skills (Basic Attack, Whirlwind, Energy Wave) work
Dash skill works
No console errors
Task 1.5: Expose Required Player Properties
File: Scripts/Player/Player.cs Add public accessors needed by executors:
// Already exists: public SkillAnimationController SkillAnimationController { get; private set; }

public AnimationPlayer AnimationPlayer => _animationPlayer;
public SkillManager SkillManager => _skillManager;

public void SetState(PlayerState newState)
{
    if (!CanTransitionTo(newState)) return;
    _state = newState;
}
Validation: Executors can access needed Player internals
Phase 1B: Channeled Skills (3-4 hours)
Task 2.1: Create ChannelState Class
New File: Scripts/Skills/States/ChannelState.cs
public class ChannelState
{
    public Skill Skill { get; set; }
    public float Elapsed { get; set; } = 0f;
    public float NextTickTime { get; set; } = 0f;
    public bool IsActive { get; set; } = false;
    
    public void Reset()
    {
        Elapsed = 0f;
        NextTickTime = 0f;
        IsActive = false;
    }
}
Task 2.2: Add Channel State to SkillManager
File: Scripts/Player/Components/SkillManager.cs Add state management:
private ChannelState _currentChannel = null;

public void StartChannel(Skill skill)
{
    _currentChannel = new ChannelState 
    { 
        Skill = skill, 
        IsActive = true 
    };
    
    // Play channeling animation if any
    if (skill.HasPlayerAnimation)
    {
        string animName = _player.GetSkillAnimationName(skill);
        _player.AnimationPlayer.Play(animName);
        _player.AnimationPlayer.SetLoopMode(Animation.LoopMode.Loop);
    }
    
    _player.SetState(PlayerState.CastingSkill);
}

public void StopChannel()
{
    if (_currentChannel != null)
    {
        _currentChannel.Reset();
        _currentChannel = null;
        _player.AnimationPlayer.Stop();
        _player.SetState(PlayerState.Idle);
    }
}

public override void _Process(double delta)
{
    base._Process(delta);
    
    UpdateCooldowns((float)delta);
    
    if (_currentChannel?.IsActive == true)
    {
        UpdateChannel((float)delta);
    }
}

private void UpdateChannel(float delta)
{
    _currentChannel.Elapsed += delta;
    _currentChannel.NextTickTime -= delta;
    
    // Apply tick damage/effect
    if (_currentChannel.NextTickTime <= 0f)
    {
        ApplyChannelTick(_currentChannel.Skill);
        _currentChannel.NextTickTime = _currentChannel.Skill.ChannelTickRate;
    }
    
    // Check for button release or max duration
    if (!Input.IsActionPressed(_currentChannel.Skill.InputAction) || 
        _currentChannel.Elapsed >= _currentChannel.Skill.ChannelMaxDuration)
    {
        StopChannel();
    }
}

private void ApplyChannelTick(Skill skill)
{
    // Spawn tick effect or apply damage in radius
    // Implementation depends on skill type
}
Task 2.3: Create ChanneledExecutor
New File: Scripts/Skills/Executors/ChanneledExecutor.cs
public class ChanneledExecutor : ISkillExecutor
{
    public bool ExecuteSkill(Player player, Skill skill)
    {
        // Delegate to SkillManager for state tracking
        player.SkillManager.StartChannel(skill);
        return true;
    }
}
Task 2.4: Add Channel Properties to SkillBalanceEntry
File: Scripts/Skills/Balance/SkillBalanceEntry.cs Add ChannelData sub-resource:
[Export] public ChannelData Channel { get; set; }
New File: Scripts/Skills/Balance/Data/ChannelData.cs
[GlobalClass]
public partial class ChannelData : Resource
{
    [Export] public float TickRate { get; set; } = 0.5f;
    [Export] public float TickDamage { get; set; } = 10f;
    [Export] public float MaxDuration { get; set; } = 5f;
    [Export] public float ResourceCostPerSecond { get; set; } = 10f;
}
Task 2.5: Create Test Channeled Skill
Example: "Laser Beam" for Warrior
Create warrior_laser_beam database entry with Channel sub-resource
Create looping beam animation (optional for testing)
Test: Hold button → continuous damage ticks → release stops
Validate: Animation loops, damage applies every tick, stops on release
Phase 1C: Charged Skills (3-4 hours)
Task 3.1-3.5: Similar structure to Phase 1B
Create:
ChargeState.cs (tracks charge level 0.0-1.0)
ChargedExecutor.cs (starts charge)
SkillManager.StartCharge() / UpdateCharge() / ReleaseCharge()
ChargeData.cs sub-resource (ChargeRate, MinCharge, MaxCharge, DamageScaling)
Test skill: "Charged Shot" for Ranger
Key Mechanic: Hold button → charge builds → release fires scaled projectile
Phase 1D: Combo Skills (4-5 hours)
Task 4.1-4.5: Similar structure to Phases 1B/1C
Create:
ComboState.cs (tracks sequence step, timing windows)
ComboExecutor.cs (advances or starts combo)
SkillManager.StartCombo() / AdvanceCombo() / ResetCombo()
ComboData.cs sub-resource (sequence steps, timing windows, damage per step)
Test skill: "Three-Strike Combo" for Warrior
Key Mechanic: Press button → first attack → press again within window → second attack → etc.
Phase 1E: Toggle Skills (2-3 hours)
Task 5.1-5.5: Similar structure
Create:
ToggleState.cs (tracks on/off state)
ToggleExecutor.cs (toggles on/off)
SkillManager.ToggleSkill() / UpdateToggle()
ToggleData.cs sub-resource (buffs while active, resource drain if any)
Test skill: "Overwatch" precision mode for Ranger
Key Mechanic: Press button → buff activates → press again → buff deactivates
Phase 1F: Validation & Testing (2-3 hours)
Task 6.1: Create One Test Skill Per CastBehavior
Instant: "Combat Stim" (buff, non-interrupting)
Database: CastBehavior.Instant, DamageSource.None
Apply speed boost without interrupting current action
Test: Can cast while attacking/dashing
AnimationDriven: "Fireball" (refactor with cast animation)
Database: CastBehavior.AnimationDriven, DamageSource.EffectCollision
Add warrior_fireball_cast animation with wind-up
Call Method track spawns projectile at apex
Test: Animation plays, projectile spawns mid-animation
Channeled: "Laser Beam"
Already created in Phase 1B
Test: Hold for continuous damage, stops on release
Charged: "Charged Shot"
Already created in Phase 1C
Test: Hold to charge, damage scales with charge level
Combo: "Three-Strike"
Already created in Phase 1D
Test: Sequence tracking, timing windows work
Toggle: "Overwatch"
Already created in Phase 1E
Test: On/off toggle, buff applies while active
Task 6.2: Playtest All Types
For each skill:
✅ Activates correctly (input responsiveness)
✅ FSM state transitions properly (interrupting vs non-interrupting)
✅ Effects spawn/apply correctly
✅ Cooldowns work
✅ No console errors
✅ Animations play (if applicable)
Edge Cases:
Interrupt channeled skill mid-cast
Release charged skill at 0% charge
Break combo timing window
Toggle skill on/off rapidly
Cast instant skill while channeling
Die during skill cast
Task 6.3: Document Patterns
Create Docs/02-IMPLEMENTATION/executor-pattern.md: Contents:
Overview of 6 CastBehavior types
How to add new skill (step-by-step)
Templates for each executor type
Common pitfalls and solutions
Example skill implementations
✅ Success Criteria
After implementation:
✅ 6 CastBehavior types working: Instant, AnimationDriven, Channeled, Charged, Combo, Toggle
✅ 6 test skills functional: One per type, all tested in-game
✅ Player.TryCastSkill() clean: ~15 lines, pure routing logic
✅ SkillManager owns state: Channeling, charging, comboing tracked centrally
✅ Executors are stateless: Pure logic classes, no persistent fields
✅ Add new skill = .tres file: No Player.cs or executor edits needed
✅ Architecture validated: Ready to scale to 18+ skills
⏱️ Time Estimates
Phase 1A (Foundation): 2-3 hours
Phase 1B (Channeled): 3-4 hours
Phase 1C (Charged): 3-4 hours
Phase 1D (Combo): 4-5 hours
Phase 1E (Toggle): 2-3 hours
Phase 1F (Validation): 2-3 hours
Total: 16-22 hours over 3-5 sessions
📝 Implementation Order
Session 1: Phase 1A (Foundation - get AnimationDrivenExecutor working) Session 2: Phase 1B (Channeled skills) Session 3: Phase 1C (Charged skills) Session 4: Phase 1D + 1E (Combo + Toggle skills) Session 5: Phase 1F (Validation & documentation)
🚧 Risks & Mitigations
Risk 1: Executors need too much Player exposure
Mitigation: Only expose what's needed (AnimationPlayer, SetState, GetSkillAnimationName)
Risk 2: State management gets complex
Mitigation: Keep state classes simple (ChannelState, ChargeState, ComboState)
Risk 3: Input handling for hold/release detection
Mitigation: SkillManager._Process() polls Input.IsActionPressed()
Risk 4: Animation loop modes conflict
Mitigation: ChanneledExecutor sets loop mode, AnimationFinished resets it
🔄 After This Plan
Once executor architecture validated: Phase 2: Implement remaining Warrior skills using executors
Fusion Cutter (AnimationDriven + hit counter)
Breaching Charge (AnimationDriven + dash + stun)
Crowd Suppression (AnimationDriven + AOE)
Fortify (Instant + damage reduction buff)
Combat Stim (Instant + speed buff)
Last Stand (Instant + invulnerability buff)
Phase 3: Add Effect Layer extensions as needed
Hitscan (Precision Rifle)
Cone hitbox (Psionic Wave)
Chain/bounce (Arc Lightning)
Gravity/pull (Singularity)
Autonomous turret (Killzone)
Phase 4: Add Systems Layer extensions
Hit counter (Fusion Cutter)
Debuff system (Psionic Pulse)
Lifesteal (Void Rift)
Knockback (Breaching Charge)
🤔 Open Questions Before Starting
Resource System: Do skills consume mana/energy? If yes, Channeled needs resource drain logic.
If no: Can implement later without affecting executor architecture
Combo Input: Should combos use same button (repeated presses) or different buttons (light vs heavy)?
Suggestion: Same button for simplicity (Three-Strike = Q → Q → Q)
UI Feedback: Do we need charge bar UI? Combo sequence indicator?
Suggestion: Defer UI, validate mechanics first
Animation Looping: How to handle loop mode changes for channeled skills?
Proposal: ChanneledExecutor sets loop=true, StopChannel() sets loop=false
Ready to begin Phase 1A: Foundation & Refactoring?
