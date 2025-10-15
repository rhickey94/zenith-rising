# Terminology Update Summary

**Date:** 2025-01-15  
**Purpose:** Align all documentation with new Power Level vs Character Level distinction

## Changes Made

### New Terminology

#### Power Level System (Temporary, In-Run)
- **Old terms:** "run level", "level up", "upgrades", "XP"
- **New terms:** "Power Level", "Power XP", "Power Upgrades", "gain Power Levels"
- **Range:** 1-20 per run
- **Persistence:** Resets every run
- **Purpose:** Short-term tactical progression

#### Character Level System (Permanent, Cross-Run)
- **Old terms:** "character level" (ambiguous usage)
- **New terms:** "Character Level", "Character XP" (capitalized for clarity)
- **Range:** 1-100
- **Persistence:** Never resets, permanent progression
- **Purpose:** Long-term strategic character building

### Documents Updated

#### Created
- ✅ **`01-GAME-DESIGN/glossary.md`** - New comprehensive glossary with all terms defined

#### Updated
1. ✅ **`01-GAME-DESIGN/design-overview.md`**
   - Changed "Collect XP shards → Level up → Choose upgrades" to "Collect Power XP shards → Gain Power Levels → Choose Power Upgrades"
   - Changed "Character XP and levels" to "Character XP and Character Levels"
   - Changed "20+ upgrade types" to "20+ Power Upgrade types"
   - Changed "Upgrade choices feel meaningful" to "Power Upgrade choices feel meaningful"

2. ✅ **`01-GAME-DESIGN/systems-progression.md`**
   - Changed "per character level" to "per Character Level"
   - Changed "Level cap: 100" to "Character Level cap: 100"
   - Changed "100 levels" to "100 Character Levels"
   - Changed "character level" in costs to "Character Level"
   - Updated progression loops to show "Power XP" and "Power Levels" vs "Character XP" and "Character Levels"
   - Added "(permanent)" and "(temporary)" clarifications

3. ✅ **`01-GAME-DESIGN/combat-skills.md`**
   - Changed overview to mention "Power Upgrades" and "Power Levels"
   - Changed "Spice = RNG (in-run upgrades)" to "Spice = RNG (Power Upgrades during run)"
   - Renamed "Level-Up System" section to "Power Level System"
   - Changed all "XP" references to "Power XP"
   - Changed "Level up" to "Gain Power Level"
   - Changed "upgrades" to "Power Upgrades"
   - Changed "Max Level Per Run: 20" to "Max Power Level Per Run: 20"
   - Updated section headers: "Upgrade Types" to "Power Upgrade Types", "Upgrade Rarity System" to "Power Upgrade Rarity System"
   - Updated philosophy section to use "Power Upgrades" and "Power Level moments"

4. ✅ **`02-IMPLEMENTATION/phase-plan.md`**
   - Changed "XP/level-up flow" to "Power XP/Power Level flow (1-20 per run)"
   - Changed "8 working upgrades" to "8 working Power Upgrades"
   - Changed "Upgrades matter" to "Power Upgrades matter"
   - Added new Phase 2 task: "Character XP & Leveling" with details on tracking and awarding
   - Updated save system task to include Character XP
   - Renumbered subsequent Phase 2 tasks
   - Updated Ascension system to use "Character Level" terminology
   - Changed "unlock new upgrade tiers" to "unlock new Power Upgrade tiers"

5. ✅ **`03-CONTENT-DESIGN/upgrade-pool.md`**
   - Changed "Upgrade Progression: Lv1/Lv2/Lv3" to "Power Upgrade Progression: Base/Upgrade 1/Upgrade 2"

6. ✅ **`00-START-HERE.md`**
   - Changed "XP/level-up system with 8 working upgrades" to "Power XP/Power Level system (1-20) with 8 working Power Upgrades"

#### Not Changed (Already Clear)
- ✅ **`01-GAME-DESIGN/dungeon-structure.md`** - No ambiguous level terminology
- ✅ **`01-GAME-DESIGN/narrative-framework.md`** - No leveling references
- ✅ **`02-IMPLEMENTATION/skill-system-architecture.md`** - Technical doc, no ambiguous terms
- ✅ **`02-IMPLEMENTATION/godot-patterns.md`** - Technical patterns only
- ✅ **`02-IMPLEMENTATION/project-structure.md`** - File structure only
- ✅ **`03-CONTENT-DESIGN/class-abilities.md`** - Skill designs only
- ✅ **`03-CONTENT-DESIGN/enemy-design.md`** - Enemy specs only
- ✅ **`04-VISUAL-AUDIO/`** - Visual/audio specs only

## Key Takeaways

### When to Use Each Term

**Power Level/Power XP:**
- During active combat
- Discussing in-run progression
- Temporary upgrades and builds
- Examples: "reached Power Level 12", "gained Power XP from kills", "chose a Power Upgrade"

**Character Level/Character XP:**
- Between runs progression
- Permanent stat allocation
- Long-term character building
- Examples: "Marcus is Character Level 15", "earned 850 Character XP", "gained a stat point"

### Design Impact

This terminology change clarifies:
1. **Two separate progression systems** that serve different purposes
2. **When leveling happens** (Power = during run, Character = between runs)
3. **What persists** (Power = nothing, Character = everything)
4. **Player decision points** (Power = tactical choices, Character = strategic builds)

## Next Steps

### For Developers
- Use "Power Level" and "Character Level" consistently in code variable names
- Update UI labels to match this terminology
- Create separate XP tracking systems as outlined in glossary

### For Documentation
- Reference `glossary.md` when adding new features
- Keep terminology consistent across all new docs
- Update this summary when new terms are introduced

### For Communication
- Use this terminology when discussing features with team
- Explain the distinction to new contributors
- Point players to glossary if they're confused

---

**All documentation is now aligned with the official glossary.**

_This was a comprehensive pass through all design and implementation docs._  
_Future updates should reference the glossary to maintain consistency._
