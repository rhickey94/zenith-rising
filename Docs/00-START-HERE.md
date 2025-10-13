# 🎮 Tower Ascension - Documentation Hub

## Quick Navigation

**Choose your path:**

### 🎯 New to the Project?
Start here: **[`01-GAME-DESIGN/design-overview.md`](01-GAME-DESIGN/design-overview.md)**
- Understand the core concept and philosophy
- Learn what makes this game unique
- See the MVP scope and vision

### 👨‍💻 Ready to Code?
Go here: **[`02-IMPLEMENTATION/phase-plan.md`](02-IMPLEMENTATION/phase-plan.md)**
- See current development phase and priorities
- Understand sequential risk reduction strategy
- Check what's implemented vs planned

### 🎨 Designing Content?
Browse: **[`03-CONTENT-DESIGN/`](03-CONTENT-DESIGN/)**
- Class abilities and skill designs
- Upgrade pool definitions
- Enemy types and behaviors

### 🖼️ Working on UI/Art?
Check: **[`04-VISUAL-AUDIO/visual-style-guide.md`](04-VISUAL-AUDIO/visual-style-guide.md)**
- Complete visual style guide
- Color palettes and typography
- UI mockups and patterns

---

## 📊 Current Project Status

**Phase:** Phase 1 (Proving Combat) - ~60% to MVP

**🎉 MAJOR MILESTONE: Phase 1 Hypothesis PROVEN 🎉**

Combat is fun and engaging through multiple playtests. The core loop works.

### ✅ Actually Working
- Player movement and combat
- 3 enemy types with distinct behaviors
- XP/level-up system with 8 working upgrades
- Skill system (Whirlwind, Fireball, basic attacks)
- Wave/floor progression system
- Boss spawning
- Stats and upgrade systems

### ⏳ In Progress
- Floor transition UI
- Victory screen
- HUD integration with Game.cs

### 📝 Not Started
- Character stat system (STR/VIT/AGI/RES/FOR)
- Gear/equipment system
- Materials or idle systems
- Save/load system
- Hub system

**For detailed status:** See [`CLAUDE.md`](../CLAUDE.md) in project root (updated daily)

---

## 📚 Documentation Structure

### **01-GAME-DESIGN/** - What We're Building
High-level game design, systems, and narrative
- `design-overview.md` - Core concept and philosophy
- `systems-progression.md` - Stats, gear, idle mechanics
- `combat-skills.md` - Combat system and abilities
- `narrative-framework.md` - Story, characters, world

### **02-IMPLEMENTATION/** - How We're Building It
Technical architecture and development plan
- `phase-plan.md` - Sequential development phases
- `skill-system-architecture.md` - Technical skill system
- `project-structure.md` - File organization and patterns
- `godot-patterns.md` - Godot-specific best practices

### **03-CONTENT-DESIGN/** - Detailed Content Specs
Specific designs for classes, upgrades, enemies
- `class-abilities.md` - Full class and skill designs
- `upgrade-pool.md` - All upgrade definitions
- `enemy-design.md` - Enemy types and behaviors

### **04-VISUAL-AUDIO/** - Look and Feel
Visual design, UI/UX, and asset requirements
- `visual-style-guide.md` - Complete UI style guide
- `asset-requirements.md` - Asset needs and sources

---

## 🎯 Development Philosophy

### Core Principles
1. **Be honest about progress** - No aspirational completion percentages
2. **Prove before building** - Each phase validates a hypothesis
3. **Simplify ruthlessly** - Cut features that don't serve the core loop
4. **Playtest constantly** - Record yourself playing, watch it back
5. **Ship small** - 2-week MVP beats 6-month vaporware

### Sequential Risk Reduction
We build in phases where **each phase proves a hypothesis** before investing in the next:

- **Phase 1:** Prove combat is fun ✅ **PROVEN**
- **Phase 2:** Prove progression hook works
- **Phase 3:** Prove idle systems add value
- **Phase 4:** Add depth (mastery, mods)
- **Phase 5:** Endgame content (multiple dungeons)

**Current Focus:** Finishing Phase 1 UI → Move to Phase 2

---

## 🔄 How to Use These Docs

### For Daily Development
- **Working doc:** [`CLAUDE.md`](../CLAUDE.md) (root) - tracks day-to-day progress
- **Reference docs:** `Docs/` folder - stable knowledge base
- **CLAUDE.md** updates frequently, **Docs/** updates when stable

### For Deep Dives
1. Start with overview docs in `01-GAME-DESIGN/`
2. Check implementation details in `02-IMPLEMENTATION/`
3. Review specific content in `03-CONTENT-DESIGN/`
4. Reference visual style in `04-VISUAL-AUDIO/`

### For Claude AI Assistance
When asking Claude for help:
- "Read `02-IMPLEMENTATION/phase-plan.md` for context"
- "Follow patterns from `02-IMPLEMENTATION/godot-patterns.md`"
- "Check `CLAUDE.md` for current implementation status"

---

## 🛠️ Tech Stack

- **Engine:** Godot 4.3+ with .NET support
- **Language:** C# (prefer C# over GDScript)
- **Platform:** PC (Steam) for MVP
- **Art Style:** 2D top-down with placeholder sprites
- **Version Control:** Git + GitHub

---

## 📖 Documentation Standards

### File Naming
- Use kebab-case: `design-overview.md`
- Number folders for priority: `01-GAME-DESIGN/`
- Keep names descriptive but concise

### Content Structure
- Start with clear purpose statement
- Use hierarchical headings (##, ###)
- Include code examples where relevant
- Cross-reference related docs
- Mark sections as ✅ Done, ⏳ In Progress, or 📝 Planned

### Updating Docs
- **CLAUDE.md** - Update after every session
- **Docs/** - Update when features are stable
- Always commit doc changes with code changes

---

## 🚀 Getting Started Checklist

### For New Developers
- [ ] Read `01-GAME-DESIGN/design-overview.md`
- [ ] Review `02-IMPLEMENTATION/phase-plan.md`
- [ ] Check `CLAUDE.md` for current status
- [ ] Review `02-IMPLEMENTATION/project-structure.md`
- [ ] Check `02-IMPLEMENTATION/godot-patterns.md`

### For Designers
- [ ] Read `01-GAME-DESIGN/design-overview.md`
- [ ] Review `03-CONTENT-DESIGN/class-abilities.md`
- [ ] Check `04-VISUAL-AUDIO/visual-style-guide.md`

### For Artists
- [ ] Review `04-VISUAL-AUDIO/visual-style-guide.md`
- [ ] Check `04-VISUAL-AUDIO/asset-requirements.md`
- [ ] See mockups: `hub-ui-mockup.html`, `tower-ascension-ui-mockup.html` (root)

---

## 💡 Quick Tips

### Finding Information
- **Game design questions?** → `01-GAME-DESIGN/`
- **Technical questions?** → `02-IMPLEMENTATION/`
- **Content specs?** → `03-CONTENT-DESIGN/`
- **Visual questions?** → `04-VISUAL-AUDIO/`
- **Current status?** → `CLAUDE.md`

### Common Questions
- "What phase are we in?" → See top of this doc or `CLAUDE.md`
- "How do I add a skill?" → `02-IMPLEMENTATION/skill-system-architecture.md`
- "What's the color palette?" → `04-VISUAL-AUDIO/visual-style-guide.md`
- "What are we building next?" → `02-IMPLEMENTATION/phase-plan.md`

---

## 📞 Need Help?

**This documentation should answer most questions, but if you're stuck:**

1. Check `CLAUDE.md` for latest updates
2. Search this documentation (Ctrl+Shift+F in VS Code)
3. Review the relevant folder based on your question
4. Ask Claude AI with specific doc references

**Remember:** These docs are living documents. If something is unclear or outdated, update it!

---

*Last major update: [Current Date] - Full documentation reorganization*
*Living document maintained by the development team*
