# Documentation Maintenance Guide

> **Purpose:** Keep documentation accurate and synchronized with implementation  
> **Last Updated:** 2025-10-20

## Overview

This guide helps maintain documentation quality by providing checklists for common changes. Use this when modifying code or design to ensure documentation stays synchronized.

---

## 🎯 When You Make Changes

### Changed a Stat Formula?

**Update these 4 documents:**

1. ✅ `02-IMPLEMENTATION/stat-system.md` (implementation details)
2. ✅ `01-GAME-DESIGN/systems-progression.md` (design philosophy)
3. ✅ `01-GAME-DESIGN/glossary.md` (stat definitions)
4. ✅ `00-QUICK-REFERENCE.md` (quick lookup table)

**Checklist:**
- [ ] Config values match (CharacterProgressionConfig)
- [ ] Examples show same calculations
- [ ] Caps documented consistently
- [ ] Update date at bottom of each file

---

### Changed Phase/Status?

**Update these 3 documents:**

1. ✅ `02-IMPLEMENTATION/phase-plan.md` (phase details)
2. ✅ `00-START-HERE.md` (current status section)
3. ✅ `00-QUICK-REFERENCE.md` (phase checklist table)

**Checklist:**
- [ ] Phase number matches everywhere
- [ ] Status emoji consistent (✅/⏳/📝)
- [ ] Hypothesis and criteria match
- [ ] Duration estimates aligned

**Note:** CLAUDE.md in project root is the daily status document - update that separately.

---

### Added/Changed a Skill?

**Update these 4 documents:**

1. ✅ `03-CONTENT-DESIGN/class-abilities.md` (SOURCE OF TRUTH - full specs)
2. ✅ `01-GAME-DESIGN/combat-skills.md` (design overview)
3. ✅ `00-QUICK-REFERENCE.md` (skill list with status)
4. ✅ `02-IMPLEMENTATION/skill-standardization.md` (if pattern changes)

**Checklist:**
- [ ] class-abilities.md has complete specification
- [ ] Skill pattern documented (AnimationDriven vs Instant, etc.)
- [ ] Status marker correct (✅ working, ⏳ in progress, 📝 planned)
- [ ] Character name used (Marcus/Aria/Elias, not Warrior/Ranger/Mage)

---

### Changed Balance Config?

**Update these 2 documents:**

1. ✅ `02-IMPLEMENTATION/balance-systems-architecture.md` (config structure)
2. ✅ `01-GAME-DESIGN/glossary.md` (if definitions changed)

**Checklist:**
- [ ] Config resource structure documented
- [ ] Example values match inspector
- [ ] Export annotations documented

---

### Changed File Structure?

**Update these 3 documents:**

1. ✅ `02-IMPLEMENTATION/project-structure.md` (folder hierarchy)
2. ✅ `00-QUICK-REFERENCE.md` (file tree diagram)
3. ✅ `00-START-HERE.md` (documentation structure section)

**Checklist:**
- [ ] All three show same structure
- [ ] Cross-references use correct paths
- [ ] Folder numbers sequential

---

## 📋 Regular Maintenance Tasks

### Weekly (After Development Sessions)

- [ ] Update CLAUDE.md in project root (daily development log)
- [ ] Check START-HERE current status matches reality
- [ ] Verify phase-plan.md phase table is current

### When Completing a Phase

- [ ] Update phase status from ⏳ to ✅ in all 3 hub docs
- [ ] Add "Completed ✅" section to phase-plan.md with details
- [ ] Update "Success Criteria" to show ✅ Met
- [ ] Move to next phase in START-HERE

### Monthly (Documentation Audit)

- [ ] Check stat formulas match implementation (use stat-system.md as source)
- [ ] Verify all dates are current (use YYYY-MM-DD format)
- [ ] Test all cross-reference links (relative paths correct?)
- [ ] Check for orphaned content (files not referenced anywhere)
- [ ] Verify skill lists match class-abilities.md (SOURCE OF TRUTH)

---

## 🚨 Common Mistakes to Avoid

### ❌ Don't Do This

1. **Update implementation without updating design docs**
   - Result: Design shows wrong formulas
   - Fix: Use checklists above

2. **Change phase number in one doc only**
   - Result: Three hub docs disagree
   - Fix: Update all 3 together

3. **Hardcode values in code without updating config**
   - Result: Documentation shows config value, code ignores it
   - Fix: Always use GameBalance.Instance.Config values

4. **Add skill without updating SOURCE OF TRUTH**
   - Result: class-abilities.md incomplete
   - Fix: Update class-abilities.md FIRST, then other docs

5. **Use placeholder dates like "[Current Date]"**
   - Result: Impossible to tell if doc is current
   - Fix: Use actual date in YYYY-MM-DD format

---

## 🎯 Documentation Hierarchy

**When multiple docs cover the same topic, follow this authority order:**

### For Implementation Details:
1. **Actual code** (C# files are ultimate truth)
2. **Implementation docs** (02-IMPLEMENTATION/*.md)
3. **Design docs** (01-GAME-DESIGN/*.md)

### For Design Intent:
1. **Design docs** (01-GAME-DESIGN/*.md)
2. **Content specs** (03-CONTENT-DESIGN/*.md)
3. **Implementation docs** (02-IMPLEMENTATION/*.md)

### For Current Status:
1. **CLAUDE.md** (project root - daily updates)
2. **phase-plan.md** (detailed phase breakdown)
3. **START-HERE.md** (current status summary)

### For Skill Specifications:
1. **class-abilities.md** (✅ SOURCE OF TRUTH)
2. **skill-standardization.md** (implementation patterns)
3. **combat-skills.md** (design philosophy)

---

## 📝 Document Type Legend

Understanding which docs need which updates:

| Type | Examples | Update Frequency | Update Trigger |
|------|----------|------------------|----------------|
| **🎯 Design Philosophy** | design-overview.md, combat-skills.md | Rarely (design changes) | Major design pivots |
| **✅ Implementation** | stat-system.md, animation-architecture.md | Often (code changes) | Code/config changes |
| **📋 Content Specs** | class-abilities.md, enemy-design.md | Occasionally (balance) | New content or balance |
| **🗺️ Navigation** | START-HERE.md, QUICK-REFERENCE.md | Weekly (status) | Phase changes, milestones |
| **📚 Reference** | glossary.md, project-structure.md | As needed | New terms, file moves |

---

## 🔍 Quick Verification Checklist

**Before committing documentation changes:**

- [ ] Dates updated (YYYY-MM-DD format)
- [ ] Cross-references point to existing files
- [ ] Status markers accurate (✅/⏳/📝)
- [ ] No placeholder text like "[Current Date]" or "TODO"
- [ ] Stat formulas match stat-system.md
- [ ] Phase status consistent across 3 hub docs
- [ ] Skill lists reference class-abilities.md
- [ ] Character names used (Marcus/Aria/Elias), not class names

---

## 📖 See Also

- [consistency-review.md](consistency-review.md) - Past consistency audits
- [terminology-update-summary.md](terminology-update-summary.md) - Terminology change logs
- [../00-START-HERE.md](../00-START-HERE.md) - Documentation hierarchy explanation

---

_This guide prevents the documentation drift that occurred in 2025-10-20 audit._  
_Update this guide when new maintenance patterns emerge._
