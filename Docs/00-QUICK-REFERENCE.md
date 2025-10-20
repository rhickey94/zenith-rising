# Quick Reference - Zenith Rising

> **Fast lookups for common information**

## 🎯 Current Phase Status

**Current:** Phase 3.5-B: Warrior Skills Implementation
**MVP Scope:** Phases 1-3.5 (Combat + Progression + Hub + Balance + Skills)
**Status:** ⏳ IN PROGRESS

### Phase Checklist

| Phase | Status | Hypothesis | Duration |
|-------|--------|-----------|----------|
| 1 | ✅ COMPLETE | Combat is fun | 2 weeks |
| 2 | ✅ COMPLETE | Progression hooks players | 2 weeks |
| 3 | ✅ COMPLETE | Hub enables meta-progression | 1 week |
| 3.5-A | ✅ COMPLETE | Balance systems enable tuning | 1 week |
| 3.5-B | ⏳ IN PROGRESS | Warrior skills validate architecture | 2 weeks |
| 4 | 📝 NEXT | Gear & loot add variety | 2 weeks |
| 5 | 📝 PLANNED | Idle systems add value | 2 weeks |
| 6 | 📝 PLANNED | Depth increases retention | 2 weeks |
| 7 | 📝 PLANNED | Endgame sustains interest | 2+ weeks |

---

## 📊 Character Stats

### The 5 Core Stats

| Stat | Name | Primary Effect | Secondary Effect |
|------|------|----------------|------------------|
| **STR** | Strength | +2 Physical Damage per point (flat) | None |
| **INT** | Intelligence | +2 Magical Damage per point (flat) | +1% Cast Speed, +1% CDR (50% cap) |
| **AGI** | Agility | +1% Attack Speed per point | None (crit not implemented) |
| **VIT** | Vitality | +10 Max HP per point | None (regen not implemented) |
| **FOR** | Fortitude | +0.5% Damage Reduction per point | None (75% cap) |

### Starting Stats

- **All characters:** 0/0/0/0/0 base
- **Character creation:** +15 points to allocate
- **Per character level:** +1 point
- **Max character level (MVP):** 100
- **Total possible points:** 115 (15 + 100)
- **Respec cost:** 100 gold × character level

### Class Affinities

- **Marcus (Warrior):** STR, VIT - Physical damage + survivability
- **Aria (Ranger):** AGI, FOR - Fast attacks + damage reduction
- **Elias (Mage):** INT, FOR - Magical damage + defense

---

## ⚔️ Skills Overview

### All 18 Planned Skills

**Marcus (Warrior)** - 6 skills
1. ✅ Fusion Cutter (Basic) - Melee auto-attack
2. ⏳ Breaching Charge (Special) - Dash + stun
3. 📝 Crowd Suppression (Q) - AOE slam
4. 📝 Fortify (E) - Deploy shield
5. 📝 Combat Stim (R) - Attack speed buff
6. 📝 Last Stand (Ultimate) - Invulnerability

**Aria (Ranger)** - 6 skills
1. 📝 Precision Rifle (Basic) - Hitscan ranged
2. 📝 Charged Shot (Special) - Hold to charge
3. 📝 Tactical Grenade (Q) - Delayed explosion
4. 📝 Evasive Roll (E) - Dash + decoy
5. 📝 Overwatch (R) - Precision mode
6. 📝 Killzone (Ultimate) - Spawn turret

**Elias (Mage)** - 6 skills
1. 📝 Psionic Pulse (Basic) - Projectile with debuff
2. 📝 Psionic Wave (Special) - Cone pushback
3. 📝 Arc Lightning (Q) - Chain lightning
4. 📝 Void Rift (E) - Damage zone
5. 📝 Architect's Blessing (R) - Chain buff
6. 📝 Singularity (Ultimate) - Gravity well

### Skill Patterns (Implementation)

| Pattern | CastBehavior | DamageSource | Examples |
|---------|--------------|--------------|----------|
| Melee | AnimationDriven | PlayerHitbox | Fusion Cutter, Psionic Wave |
| Instant AOE | AnimationDriven | PlayerHitbox | Whirlwind, Crowd Suppression |
| Projectile | Instant | EffectCollision | Fireball, Psionic Pulse |
| Cast-Spawn | AnimationDriven | EffectCollision | Breaching Charge |
| Buff (Instant) | Instant | None | Combat Stim, Overwatch |
| Buff (Animated) | AnimationDriven | None | Fortify |
| Persistent Zone | Instant | EffectCollision | Void Rift, Singularity |

---

## 🎮 Progression Systems

### Power Level (Temporary, In-Run)

- **Range:** 1-20 per run
- **XP Source:** Killing enemies (Power XP shards)
- **Rewards:** Choose 1 of 3 Power Upgrades per level
- **Persistence:** Resets every run
- **Purpose:** Short-term tactical power scaling

### Character Level (Permanent, Cross-Run)

- **Range:** 1-100 (MVP cap)
- **XP Source:** Run completion (floors cleared, bosses killed)
- **Rewards:** +1 stat point per level
- **Persistence:** Never resets
- **Purpose:** Long-term strategic character building

### Skill Mastery (Permanent, Per-Skill)

| Tier | Kills Required | Cumulative | Bonus |
|------|----------------|------------|-------|
| Bronze | 10 | 10 | Base skill |
| Silver | 40 | 50 | +50% effectiveness |
| Gold | 150 | 200 | +100% effectiveness + special |
| Diamond | 300 | 500 | +150% effectiveness + enhanced special |

---

## 🗂️ File Structure

### Critical Paths

```
Docs/
├── 00-START-HERE.md         ← Navigation hub
├── 00-QUICK-REFERENCE.md    ← This document
│
├── 01-GAME-DESIGN/
│   ├── design-overview.md   ← Start here for new devs
│   ├── systems-progression.md
│   ├── combat-skills.md
│   ├── dungeon-structure.md
│   ├── glossary.md
│   └── narrative-framework.md
│
├── 02-IMPLEMENTATION/
│   ├── phase-plan.md        ← Development roadmap
│   ├── skill-standardization.md  ✅ Current approach
│   ├── balance-systems-architecture.md
│   ├── skill-mastery-system.md
│   ├── stat-system.md
│   ├── buff-system.md
│   ├── animation-architecture.md
│   ├── collision-layer-architecture.md
│   ├── idle-systems-implementation.md
│   ├── project-structure.md
│   └── godot-patterns.md
│
├── 03-CONTENT-DESIGN/
│   ├── class-abilities.md   ✅ SOURCE OF TRUTH for skills
│   ├── upgrade-pool.md
│   └── enemy-design.md
│
├── 04-VISUAL-AUDIO/
│   ├── visual-style-guide.md
│   ├── asset-requirements.md
│   └── lpc-character-generator-guide.md
│
├── 05-RESEARCH/
│   ├── character-sprites-research.md
│   └── human-vs-robot-sprites.md
│
├── 06-PROMPTS/
│   ├── add-skill.md
│   ├── balance-test.md
│   ├── debug-godot.md
│   └── session-start.md
│
├── 07-MAINTENANCE/
│   ├── consistency-review.md
│   └── terminology-update-summary.md
│
└── Archive/
    └── research-idle-systems-2025.md

CLAUDE.md (root)            ← Daily status updates
```

---

## 🎨 Color Palette

### Human Tech (Floors 1-2, Base UI)

| Use | Color | Hex |
|-----|-------|-----|
| Primary | Steel Gray | #2c3e50 |
| Secondary | Rust Orange | #e67e22 |
| Accent | Warning Red | #e74c3c |
| Text | Light Gray | #ecf0f1 |
| Background | Deep Black | #0a0a0a |

### Alien Tech (Floors 3+)

| Use | Color | Hex |
|-----|-------|-----|
| Primary | Bronze | #b87333 |
| Secondary | Purple | #9b59b6 |
| Accent | Cyan | #1abc9c |
| Background | Deep Blue | #0c1445 |

### Character Colors

| Character | Color | Hex |
|-----------|-------|-----|
| Marcus (Warrior) | Red | #e74c3c |
| Aria (Ranger) | Blue | #3498db |
| Elias (Mage) | Purple | #9b59b6 |

### Rarity Colors

| Rarity | Color | Hex |
|--------|-------|-----|
| Common | Gray | #95a5a6 |
| Uncommon | Green | #2ecc71 |
| Rare | Blue | #3498db |
| Epic | Purple | #9b59b6 |
| Legendary | Orange | #e67e22 |

---

## 🔍 Common Lookups

### Where Do I Find...?

| Looking For | Document |
|-------------|----------|
| Current implementation status | `../CLAUDE.md` (root) |
| Exact skill values | `03-CONTENT-DESIGN/class-abilities.md` |
| How to implement a skill | `02-IMPLEMENTATION/skill-standardization.md` |
| Stat formulas | `02-IMPLEMENTATION/stat-system.md` |
| Color palette | This doc OR `04-VISUAL-AUDIO/visual-style-guide.md` |
| What phase are we in | This doc OR `00-START-HERE.md` |
| Development roadmap | `02-IMPLEMENTATION/phase-plan.md` |
| Design philosophy | `01-GAME-DESIGN/design-overview.md` |
| File organization | `02-IMPLEMENTATION/project-structure.md` |
| Godot best practices | `02-IMPLEMENTATION/godot-patterns.md` |

### How Do I...?

| Task | Document |
|------|----------|
| Add a new skill | `02-IMPLEMENTATION/skill-standardization.md` |
| Configure balance values | `02-IMPLEMENTATION/balance-systems-architecture.md` |
| Understand animation system | `02-IMPLEMENTATION/animation-architecture.md` |
| Work with buffs | `02-IMPLEMENTATION/buff-system.md` |
| Set up collision layers | `02-IMPLEMENTATION/collision-layer-architecture.md` |
| Design a new enemy | `03-CONTENT-DESIGN/enemy-design.md` |
| Follow visual style | `04-VISUAL-AUDIO/visual-style-guide.md` |
| Find character sprites | `04-VISUAL-AUDIO/asset-requirements.md` |

---

## 📋 Status Emoji Guide

| Emoji | Meaning | Usage |
|-------|---------|-------|
| ✅ | Complete | Feature implemented and tested |
| ⏳ | In Progress | Currently being worked on |
| 📝 | Planned | Designed but not yet implemented |
| ⚠️ | Historical | Old approach, preserved for context |
| 🔴 | Blocked | Cannot proceed without dependencies |
| 🟡 | Needs Review | Complete but needs testing/review |

---

## 🚀 Quick Start Paths

### For New Developers

1. Read `01-GAME-DESIGN/design-overview.md` (15 min)
2. Read `02-IMPLEMENTATION/phase-plan.md` (10 min)
3. Check `../CLAUDE.md` for current status (5 min)
4. Review `02-IMPLEMENTATION/project-structure.md` (10 min)
5. Start coding!

### For Content Designers

1. Read `01-GAME-DESIGN/design-overview.md` (15 min)
2. Review `03-CONTENT-DESIGN/class-abilities.md` (20 min)
3. Check `04-VISUAL-AUDIO/visual-style-guide.md` (15 min)
4. Start designing!

### For Artists

1. Review `04-VISUAL-AUDIO/visual-style-guide.md` (30 min)
2. Check `04-VISUAL-AUDIO/asset-requirements.md` (15 min)
3. View mockups in project root (10 min)
4. Start creating!

---

## 💡 Key Terminology

- **Power Level:** Temporary in-run progression (1-20, resets)
- **Character Level:** Permanent cross-run progression (1-100)
- **Power Upgrade:** Temporary run modification (choose at Power Level up)
- **Skill Mastery:** Permanent skill improvement (Bronze/Silver/Gold/Diamond)
- **MVP:** Minimum Viable Product = Phases 1-3.5
- **Phase:** Development milestone with specific hypothesis
- **Hub:** Safe base area between dungeon runs
- **Dungeon:** Combat zone with waves + boss
- **Floor:** Single level within a dungeon (~5 min)

For complete terminology, see: `01-GAME-DESIGN/glossary.md`

---

_This quick reference is updated as the project evolves._
_Last updated: 2025-10-20_
