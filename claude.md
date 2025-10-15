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

**Phase:** Phase 3 (Hub World & First Dungeon) - ⏳ **IN PROGRESS**

**🎉 PHASES 1 & 2 COMPLETE! 🎉**

**Phase 1 - Combat:** Proven fun and engaging through multiple playtests
**Phase 2 - Progression:** Character stats, save/load, stat allocation working

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

### ⏳ In Progress

- Testing complete hub → dungeon → hub flow
- Visual polish for hub (background, atmosphere)

### 📝 Not Started (Phase 4+)

- Gear/equipment drops
- Materials or idle systems
- NPC vendors/systems
- More hub features

---

## Current Phase Focus

**Phase 3: Hub World & First Dungeon** ⏳ **IN PROGRESS**

**Current goals:**

1. ✅ Create hub scene as safe zone
2. ✅ Implement dungeon portal interaction
3. ✅ Establish scene flow: Main Menu → Hub → Dungeon → Hub
4. ✅ Fix player initialization for new/saved games
5. ⏳ Test complete hub → dungeon → hub flow
6. 📝 Polish hub visuals and atmosphere
7. 📝 Add placeholder NPCs/vendors (future)

**Completed this phase:**
- Hub.cs with player initialization
- hub.tscn scene with player and portal
- DungeonPortal.cs with interaction system
- Fixed critical Player.Initialize() bug (new game support)
- Renamed game.tscn → dungeon.tscn for clarity
- Updated all scene transitions to use hub

---

**Next Phase: Gear & Loot System (Phase 4)**

**Future goals:**

1. Item drops from enemies
2. Equipment slots and inventory
3. Stat bonuses from gear

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

```
STR (Strength): +3% Physical Dmg, +10 HP per point
INT (Intelligence): +3% Magical Dmg, +2% CDR per point
AGI (Agility): +2% Attack Speed, +1% Crit Chance per point (cap 50%)
VIT (Vitality): +25 HP, +0.5 HP/sec regen per point
FOR (Fortune): +2% Crit Damage, +1% Drop Rate per point
```

**Progression:**
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

_Last updated: Session 9 - Hub World implementation_
_🎉 PHASES 1 & 2 COMPLETE - Phase 3 Hub World in progress! 🎉_
