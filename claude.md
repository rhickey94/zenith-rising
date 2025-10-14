# Tower Ascension - Project Documentation

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
**Phase:** Phase 1 (Proving Combat) - ~85% complete, 2-3 tasks from Phase 2

**🎉 MAJOR MILESTONE: Phase 1 Hypothesis PROVEN 🎉**

Combat is fun and engaging through multiple playtests. The core loop works.

### ✅ Actually Working
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
- **HUD integration** - Floor/wave display wired to Game.cs

### ⏳ In Progress (Finishing Phase 1)
- Victory screen (when player beats Floor 5)
- Death screen improvement (current: just reloads scene)

### 📝 Not Started (Phase 2+)
- Character stat system (STR/VIT/AGI/RES/FOR)
- Gear/equipment drops
- Materials or idle systems
- Save/load system
- Hub world

---

## Current Phase Focus

**Phase 1: Prove Combat is Fun** ✅ **HYPOTHESIS PROVEN**

**Remaining Tasks to Complete Phase 1:**
1. ⏳ Victory screen UI (ShowVictoryScreen implementation)
2. ⏳ Death screen improvement (show stats, add delay before menu return)

**Once complete (2-3 tasks) → Move to Phase 2**

**Phase 2 Preview:** Character stats (STR/VIT/AGI/RES/FOR), save/load system, basic gear drops

**See [`Docs/02-IMPLEMENTATION/phase-plan.md`](Docs/02-IMPLEMENTATION/phase-plan.md) for full phase details.**

---

## Session Progress Log

### Session 7 (Current) - Floor Transition UI & Code Quality
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
- Victory screen not implemented (Game.cs:241-244 stub exists)
- Death screen just reloads scene, should show stats (StatsManager.cs:167-171)
- Main menu needs background texture (polish, non-critical)

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
│   ├── Core/game.tscn
│   ├── Player/player.tscn
│   ├── Enemies/ (enemy.tscn, fast_melee_enemy.tscn, slow_ranged_enemy.tscn, boss.tscn)
│   ├── Items/experience_shard.tscn
│   ├── SkillEffects/ (fireball_projectile.tscn, whirlwind_effect.tscn)
│   └── UI/ (hud.tscn, level_up_panel.tscn, main_menu.tscn)
├── Scripts/
│   ├── Core/ (Game.cs, CombatSystem.cs)
│   ├── PlayerScripts/Components/ (StatsManager.cs, SkillManager.cs, UpgradeManager.cs)
│   ├── Skills/ (Skill.cs, executors, effects)
│   └── Enemies/Base/Enemy.cs
└── Resources/Skills/ (Fireball.tres, Whirlwind.tres)
```

### Enemy Scaling Formula
```csharp
float healthMult = (1 + currentWave * 0.1f) * (1 + currentFloor * 0.5f);
float damageMult = (1 + currentWave * 0.05f) * (1 + currentFloor * 0.5f);
```

### Stat System (Phase 2)
```
STR: +3% ALL damage per point
VIT: +25 HP per point
AGI: +2% attack speed per point
RES: +1% damage reduction per point (cap 50%)
FOR: +2% crit chance per point (cap 50%)
```

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

*Last updated: Session 7 - Floor transition UI complete, comprehensive code review*
*Phase 1 is 85% complete - 2 tasks remaining (victory/death screens)*
