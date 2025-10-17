# Documentation Consistency Review - Phase 2 Stat System

**Review Date:** Current session preparing for Phase 2 implementation
**Reviewed By:** Claude (AI Assistant)
**Purpose:** Ensure all documentation accurately reflects the revised 5-stat system

---

## ✅ CONSISTENT DOCUMENTS

### 1. **systems-progression.md** ✅
**Status:** UPDATED - Fully consistent with Phase 2 design

**Current Content:**
- 5 stats: STR, INT, AGI, VIT, FOR (RES removed)
- STR: +3% Physical damage, +10 HP
- INT: +3% Magical damage, +2% Skill CDR
- AGI: +2% Attack speed, +1% Crit chance (cap 50%)
- VIT: +25 HP, +0.5 HP regen
- FOR: +2% Crit damage, +1% Rare drop rate
- Damage types explained (Physical/Magical)
- Weapon determines Basic/Special damage type
- Class affinities documented (Marcus/STR, Aria/AGI, Elias/INT)
- 15 starting stat points confirmed
- Respec cost: 100 gold × character level

**No issues found.**

---

### 2. **dungeon-structure.md** ✅
**Status:** CONSISTENT - No stat references, focuses on dungeon design

**Content:** Dungeon lengths, session times, scaling formulas
**No changes needed.**

---

### 3. **design-overview.md** ✅
**Status:** CONSISTENT - General overview, no detailed stat mechanics

**References:** Mentions "5 core stats (STR/VIT/AGI/RES/FOR)" in design philosophy section

**Issue Found:** ⚠️ Still lists RES in one place

**Location:** Line ~147 in design philosophy section:
> "5 core stats (STR/VIT/AGI/RES/FOR)"

**Required Fix:** Change to:
> "5 core stats (STR/INT/AGI/VIT/FOR)"

---

## ⚠️ INCONSISTENT DOCUMENTS

### 4. **combat-skills.md** ⚠️
**Status:** INCONSISTENT - Uses old class names, no stat integration

**Issues Found:**

#### Issue 4.1: Class Names
**Current:** Uses generic "Warrior/Mage/Ranger"
**Should Be:** Marcus (Warrior), Aria (Ranger), Elias (Mage)

**Locations:**
- Line 23: "**Warrior:** Melee sword swing"
- Line 24: "**Mage:** Magic bolt"
- Line 25: "**Ranger:** Bow shot"
- Line 33: "**Warrior:** Shield bash"
- Line 34: "**Mage:** Fireball"
- Line 35: "**Ranger:** Charged shot"
- Line 42-57: Class Combat Identities section
- Line 67-87: Example loadouts

**Recommended Fix:** Add character names in parentheses or replace entirely:
- "**Marcus (Warrior):** Melee sword swing"
- "**Elias (Mage):** Magic bolt"  
- "**Aria (Ranger):** Bow shot"

#### Issue 4.2: No Damage Type Integration
**Current:** Says "Scales with character stats" but doesn't explain Physical vs Magical

**Location:** Line 20:
> "- Scales with character stats"

**Recommended Addition:** After line 20, add:
> "- Damage type determined by weapon (Physical for most weapons, Magical for staves/wands)
> - Physical damage scales with STR, Magical damage scales with INT"

#### Issue 4.3: Stat Boost Upgrade Examples
**Current:** Shows "+5 Strength" as example

**Location:** Line 269:
> "- \"+5 Strength\" (temporary for run)"

**Issue:** This is fine, but should clarify it's STR (Physical damage) not generic damage

**Recommended Fix:** Change to:
> "- \"+5 STR\" (temporary for run, +15% Physical damage)"

---

### 5. **class-abilities.md** ⚠️
**Status:** INCONSISTENT - Detailed class designs but no stat integration

**Issues Found:**

#### Issue 5.1: Character Names Correct ✅
**Status:** Good - Uses Marcus, Aria, Elias properly

#### Issue 5.2: No Stat Scaling Information
**Problem:** Detailed skill descriptions never mention which stats scale damage

**Example:** "Crowd Suppression" (Marcus Q skill) - Line 30
- Shows "Moderate damage + knockback"
- Doesn't mention this is Physical damage scaling with STR

**Recommended Fix:** Add damage type tags to all damaging skills:
- "**Damage Type:** Physical (scales with STR)"
- "**Damage Type:** Magical (scales with INT)"

**Locations needing updates:**
- All Marcus skills (Physical damage)
- All Aria skills (likely Physical, weapon-dependent)
- All Elias skills (likely Magical)

#### Issue 5.3: Missing Stat Recommendations
**Problem:** No guidance on which stats each character should prioritize

**Recommended Addition:** Add section at top of each character:

```markdown
### Recommended Stats
**Primary:** STR (Physical damage scaling)
**Secondary:** VIT (Survivability)
**Alternative Builds:**
- Hybrid: 8 STR, 4 INT, 3 VIT (if using magical weapon)
- Tank: 5 STR, 0 INT, 0 AGI, 10 VIT, 0 FOR
```

---

### 6. **upgrade-pool.md** ⚠️
**Status:** INCONSISTENT - Uses correct character names but no stat integration

**Issues Found:**

#### Issue 6.1: Character Names ✅
**Status:** Good - Uses Marcus, Aria, Elias

#### Issue 6.2: No Damage Type Tags on Skills
**Problem:** Lists all skills but doesn't specify Physical vs Magical

**Example:** Line 8 - "Shockwave Strike"
- Shows damage and effects
- Doesn't mention it's Physical damage

**Recommended Fix:** Add damage type to all skill descriptions

#### Issue 6.3: Stat Boost Section Incomplete
**Problem:** Shows stat boost upgrades but doesn't explain new system

**Current:** Shows temporary stat increases
**Missing:** Explanation of how STR/INT split works

**Recommended Addition:** Add note explaining:
- STR boosts Physical damage only
- INT boosts Magical damage only
- AGI, VIT, FOR are universal

---

### 7. **visual-style-guide.md** ⚠️
**Status:** MINOR INCONSISTENCY - Stat UI mockup needs updating

**Issues Found:**

#### Issue 7.1: Stat Display Shows OLD Stats
**Location:** Character Panel mockup (Lines 160-200+)

**Current Mockup Shows:**
```
Strength:     32 (+12)
Dexterity:    18 (+3)
Intelligence: 12 (+0)
Vitality:     25 (+8)
Fortune:      14 (+2)
```

**Problems:**
1. Uses "Dexterity" instead of "Agility" (AGI)
2. No explanation of stat effects in hover tooltips
3. Stat order doesn't match: STR, INT, AGI, VIT, FOR

**Current Hover Tooltip:**
```
┌────────────────────────────┐
│ STRENGTH                   │
│ +2% physical damage per pt │
│ +10 max HP per point       │
└────────────────────────────┘
```

**Issue:** Shows "+2% physical damage" but should be "+3%"

**Recommended Fixes:**
1. Rename "Dexterity" → "Agility"
2. Fix STRENGTH hover to "+3% physical damage per pt"
3. Update stat order to: STR, INT, AGI, VIT, FOR
4. Add missing hover tooltips for all stats matching systems-progression.md

---

## 📋 PRIORITY FIXES SUMMARY

### 🔴 HIGH PRIORITY (Block Phase 2 Implementation)

1. **design-overview.md** - Remove RES from stat list
   - Location: Line ~147
   - Change: "STR/VIT/AGI/RES/FOR" → "STR/INT/AGI/VIT/FOR"

2. **visual-style-guide.md** - Fix stat UI mockups
   - Rename Dexterity → Agility
   - Fix STR hover tooltip (+2% → +3%)
   - Reorder stats: STR, INT, AGI, VIT, FOR
   - Add complete hover tooltips for all 5 stats

### 🟡 MEDIUM PRIORITY (Improve Consistency)

3. **combat-skills.md** - Add damage type integration
   - Add Physical/Magical damage explanations
   - Clarify STR vs INT scaling
   - Update stat boost examples with clarity

4. **combat-skills.md** - Add character names to class references
   - Replace "Warrior" with "Marcus (Warrior)"
   - Replace "Mage" with "Elias (Mage)"
   - Replace "Ranger" with "Aria (Ranger)"

### 🟢 LOW PRIORITY (Nice to Have)

5. **class-abilities.md** - Add stat scaling info to skills
   - Tag all skills with damage type
   - Add recommended stat builds per character
   - Clarify which stats scale which abilities

6. **upgrade-pool.md** - Add damage type tags
   - Tag all skills with Physical/Magical
   - Add stat system explanation to stat boost section
   - Clarify STR/INT split effects

---

## 📝 RECOMMENDED ACTION ITEMS

### Before Starting Phase 2 Implementation:

1. **Fix HIGH PRIORITY items** (design-overview.md, visual-style-guide.md)
2. **Fix MEDIUM PRIORITY items** (combat-skills.md stat integration)
3. **Create Phase 2 implementation notes** documenting:
   - DamageType enum (Physical/Magical)
   - How weapon type determines Basic/Special attack damage type
   - How to tag skills with damage types
   - How CombatSystem.DealDamage should check damage type

### After Phase 2 Implementation:

4. **Update class-abilities.md** with stat recommendations
5. **Update upgrade-pool.md** with damage type tags
6. **Add Phase 2 retrospective** to CLAUDE.md

---

## 🎯 CRITICAL DECISION CONFIRMATIONS

### Confirmed Decisions from This Session:

✅ **5 stats total:** STR, INT, AGI, VIT, FOR (RES removed)
✅ **Damage type split:** Physical (STR), Magical (INT)
✅ **Character names:** Marcus (Warrior), Aria (Ranger), Elias (Mage)
✅ **Weapon determines Basic/Special damage type:** Weapon-locked skills use weapon's damage type
✅ **15 starting stat points:** Distributed at character creation
✅ **Respec cost:** 100 gold × character level
✅ **Active abilities (Q/W/E/R):** Individually tagged with damage type, swappable
✅ **Basic/Special skills:** Weapon-locked, damage type from weapon

---

## ✨ DOCUMENTATION HEALTH: 85%

**Strong Points:**
- systems-progression.md is perfect ✅
- dungeon-structure.md is consistent ✅
- Character names (Marcus/Aria/Elias) used correctly in most places ✅

**Weak Points:**
- RES still mentioned in 1 place (design-overview.md)
- Stat UI mockup uses old stats (visual-style-guide.md)
- No damage type integration in skill docs (class-abilities.md, upgrade-pool.md, combat-skills.md)

**Recommendation:** Fix HIGH and MEDIUM priority items before Phase 2 implementation. LOW priority items can be addressed during Phase 2 development as you encounter them.

---

*Generated during Phase 2 planning session*
*Review again after Phase 2 implementation complete*
