# Dungeon Structure & Design - Fantasy Theme

## Core Philosophy

**Zenith Rising respects your time through varied dungeon lengths.**

Players have different amounts of time for different sessions:

- Quick adventure? 15-minute dungeon
- Evening session? 45-minute dungeon
- Want a challenge? Long, epic dungeon

**All session lengths provide meaningful progression.** No "correct" way to play.

---

## Dungeon Overview

### The 6 Ancient Towers (Phase 7)

| Dungeon Name | Floors | Time | Difficulty | Target Level | Theme |
|--------------|--------|------|------------|--------------|-------|
| **The Cursed Tower** | 5 | 25-30 min | Normal | 1-25 | Royal greed, magical corruption (MVP) |
| **The Forgotten Catacombs** | 4 | 20-25 min | Normal+ | 15-35 | Desperate villagers, preservation |
| **The Iron Citadel** | 7 | 35-40 min | Hard | 25-50 | Weaponized magic |
| **The Hive of Unity** | 8 | 40-45 min | Hard+ | 40-60 | Collective consciousness |
| **The Frozen Spire** | 6 | 30-35 min | Expert | 50-75 | Time distortion |
| **The Living Labyrinth** | 9 | 45-50 min | Master | 60-100 | Nature-magic fusion |

### Design Principles

**Varied Length:**
- Short (4 floors): Quick sessions, focused challenges
- Medium (5-6 floors): Standard roguelite length
- Long (7-9 floors): Epic runs, maximum rewards

**Overlapping Levels:**
- Players choose their challenge level
- Can replay easier dungeons for farming
- Can attempt harder dungeons for push progression

**Shorter ≠ Easier:**
- The Catacombs (4 floors) is HARDER than The Cursed Tower (5 floors)
- Length = time commitment, not difficulty
- Difficulty = enemy stats, mechanics, build requirements

---

## Dungeon 1: The Cursed Tower (MVP)

**Status:** Phase 1-3 (Currently Building)

### Overview
- **Floors:** 5
- **Time per floor:** ~5 minutes
- **Total run time:** 25-30 minutes
- **Difficulty:** Normal (tutorial → challenging)
- **Target level:** 1-25

### Structure

**Each floor:**
- 10 waves of enemies
- Boss at 5:00 mark
- 10-20 seconds between waves
- Extract or Continue after boss

**Run flow:**
```
Floor 1 → Floor 2 → Floor 3 → Floor 4 → Floor 5
  ↓         ↓         ↓         ↓         ↓
Boss 1    Boss 2    Boss 3    Boss 4    Boss 5 (Final)
  ↓         ↓         ↓         ↓         ↓
Extract?  Extract?  Extract?  Extract?  Victory!
```

### Visual Progression
- **Floors 1-2:** Lower tower chambers (medieval stone)
- **Floor 3:** Transition zone (corruption spreading)
- **Floors 4-5:** Heart Chamber interior (corrupted, magical)

### Enemy Types (Phase 1)
- Corrupted Guard (basic melee chaser)
- Shadow Wraith (fast aggressive rushdown)
- Dark Archer (slow ranged kiter)

### Scaling (Per Floor)
- **Wave scaling:** +10% HP, +5% damage per wave
- **Floor scaling:** +50% HP, +50% damage per floor
- **Boss multiplier:** 5x HP, 2x damage

**Example:**
- Floor 1, Wave 1: 100 HP, 10 damage
- Floor 1, Boss: 500 HP, 20 damage
- Floor 5, Wave 10: 290 HP, 24.5 damage
- Floor 5, Boss: 1450 HP, 49 damage

---

## Dungeon 2: The Forgotten Catacombs

**Status:** Phase 7 (Post-Launch)

### Overview
- **Floors:** 4
- **Time:** 20-25 minutes
- **Difficulty:** Normal+ (harder than Cursed Tower despite being shorter)
- **Target level:** 15-35
- **Theme:** Ancient burial complex, villagers seeking the Chalice of Plenty, preservation gone wrong

### Design Intent

**"The Quick Challenge"**
- Shorter than Cursed Tower but harder per floor
- Good for daily runs when time is limited
- Tests refined builds against tougher enemies
- Proves length ≠ difficulty

### Enemy Focus
- Bound Spirits (villagers' souls)
- Animated Grave Guardians
- Spectral Echoes
- More swarm/crowd focus than Cursed Tower

### Loot Strategy
- Higher drop rates to compensate for fewer floors
- Better gear quality per floor
- Encourages "quick farming runs"

---

## Dungeon 3: The Iron Citadel

**Status:** Phase 7 (Post-Launch)

### Overview
- **Floors:** 7
- **Time:** 35-40 minutes
- **Difficulty:** Hard
- **Target level:** 25-50
- **Theme:** Military fortress, weaponized magic, traumatized Guardian

### Design Intent

**"The Commitment Run"**
- Longer than average, rewards sustained play
- Tests endurance and resource management
- Multiple difficulty spikes across 7 floors
- Risk vs reward: higher stakes extraction decisions

### Unique Mechanics
- Floor 4 introduces "Enhanced Soldiers" (magically-augmented warriors)
- Boss fight at Floor 7 has unique Guardian behavior (paranoid, aggressive)
- Moral choice at end: Mercy kill or rehabilitate the Sentinel

### Enemy Focus
- Magically-enhanced soldiers (faster, tougher)
- Prototype war golems
- Failed enhancement experiments
- Heavy melee focus with armor

---

## Dungeon 4: The Hive of Unity

**Status:** Phase 7 (Post-Launch)

### Overview
- **Floors:** 8
- **Time:** 40-45 minutes
- **Difficulty:** Hard+
- **Target level:** 40-60
- **Theme:** Entire city merged into collective consciousness

### Design Intent

**"The Epic Run"**
- Long session for dedicated players
- Highest rewards per completion
- Multiple boss encounters
- Story-heavy with moral dilemmas

### Unique Mechanics
- Enemies move in synchronized patterns (hive mind)
- Architecture reshapes dynamically
- Floor 8 boss: Fight the collective consciousness of thousands
- Philosophical ending: Was the hive mind evil or transcendent?

### Enemy Focus
- Synchronized swarms (perfect coordination)
- Living architecture (buildings attack)
- Perfected Forms (fully merged citizens)
- Heavy crowd control and positioning challenges

---

## Dungeon 5: The Frozen Spire

**Status:** Phase 7 (Post-Launch)

### Overview
- **Floors:** 6
- **Time:** 30-35 minutes
- **Difficulty:** Expert
- **Target level:** 50-75
- **Theme:** Time distortion, archaeological horror

### Design Intent

**"The Mind-Bender"**
- Medium length but extreme difficulty
- Time mechanics create unique challenges
- Enemies from past/future appear
- Reality feels unstable

### Unique Mechanics
- Time echoes (fight past versions of enemies)
- Temporal paradoxes (enemies that shouldn't exist)
- Boss fight: Fight yourself from alternate timelines
- Guardian is dying, confused, not hostile

### Enemy Focus
- Temporal anomalies
- Frozen scholars (preserved by magic)
- Paradox constructs
- Unpredictable spawns and behaviors

---

## Dungeon 6: The Living Labyrinth

**Status:** Phase 7 (Post-Launch)

### Overview
- **Floors:** 9
- **Time:** 45-50 minutes
- **Difficulty:** Master
- **Target level:** 60-100
- **Theme:** Bio-magical evolution, nature vs magic

### Design Intent

**"The Ultimate Challenge"**
- Longest dungeon, maximum rewards
- Endgame content for veteran players
- Requires perfect builds and execution
- Tests everything learned across all dungeons

### Unique Mechanics
- Living environment (terrain changes mid-combat)
- Bio-magical fusion enemies
- Boss fight: The forest itself (entire ecosystem)
- Philosophical ending: Preservation or destruction?

### Enemy Focus
- Evolved beasts (animals + magic)
- Intelligent aggressive flora
- Hybrid guardians (organic + magical)
- Extreme variety and adaptability

---

## Difficulty Scaling Between Dungeons

### Base Stat Multipliers (Floor 1 of Each Dungeon)

| Dungeon | HP Multiplier | Damage Multiplier |
|---------|--------------|-------------------|
| The Cursed Tower | 1.0x (base 100 HP) | 1.0x (base 10 dmg) |
| The Forgotten Catacombs | 2.0x (200 HP) | 1.5x (15 dmg) |
| The Iron Citadel | 4.0x (400 HP) | 2.5x (25 dmg) |
| The Hive of Unity | 8.0x (800 HP) | 4.0x (40 dmg) |
| The Frozen Spire | 15.0x (1500 HP) | 6.0x (60 dmg) |
| The Living Labyrinth | 25.0x (2500 HP) | 10.0x (100 dmg) |

### Within-Dungeon Scaling (Same as Cursed Tower)
- **Per wave:** +10% HP, +5% damage
- **Per floor:** +50% HP, +50% damage
- **Boss:** 5x HP, 2x damage

### Why This Works

**Prevents Trivialization:**
- Can't one-shot Living Labyrinth with level 100 character
- Each dungeon has appropriate challenge for level range

**Allows Farming:**
- Can return to Cursed Tower at level 50 for easy materials
- Faster runs when overgeared = efficient farming

**Creates Progression:**
- Clear difficulty curve across dungeons
- Natural gates without artificial locks
- Player chooses when ready to advance

---

## Reward Scaling

### Drop Rate Multipliers

| Dungeon | Material Drop | Gear Quality | Gold Multiplier |
|---------|--------------|-------------|-----------------|
| The Cursed Tower | 1.0x | Common-Rare | 1.0x |
| The Forgotten Catacombs | 1.3x | Uncommon-Rare | 1.5x |
| The Iron Citadel | 1.7x | Rare-Epic | 2.0x |
| The Hive of Unity | 2.2x | Rare-Epic | 3.0x |
| The Frozen Spire | 3.0x | Epic-Legendary | 4.0x |
| The Living Labyrinth | 4.0x | Epic-Legendary | 5.0x |

### Time-to-Reward Balance

**Philosophy:** Longer dungeons should reward proportionally more per minute

**Example:**
- The Cursed Tower (25 min): 100 materials = 4 materials/min
- The Living Labyrinth (45 min): 300 materials = 6.7 materials/min

**Why:** Compensates for risk and time commitment

---

## Player Progression Through Dungeons

### Typical Journey (Levels 1-100)

**Levels 1-15 (Learning Phase):**
- Run The Cursed Tower repeatedly
- Learn mechanics and upgrade system
- Experiment with builds

**Levels 15-30 (Branching):**
- Can farm The Cursed Tower quickly OR
- Challenge The Forgotten Catacombs for better loot
- Choice based on playstyle

**Levels 30-50 (Midgame):**
- Pushing The Iron Citadel
- Occasionally return to The Cursed Tower for fast materials
- Building optimized gear sets

**Levels 50-70 (Late Game):**
- Rotating between The Hive of Unity and The Frozen Spire
- Targeting specific legendary drops
- Refining perfect builds

**Levels 70-100 (Endgame):**
- The Living Labyrinth attempts
- Ascension system unlocks
- Push for completion

---

## Session Time Philosophy

### Respecting Player Time

**15-20 Minutes Available?**
- Run The Forgotten Catacombs (4 floors)
- Quick but meaningful progress
- Good loot per minute

**25-30 Minutes Available?**
- Run The Cursed Tower (5 floors)
- Standard roguelite session
- Balanced experience

**30-40 Minutes Available?**
- Run The Iron Citadel (7 floors) or The Frozen Spire (6 floors)
- Deeper run, better rewards
- Test endurance and build

**45+ Minutes Available?**
- Run The Hive of Unity (8 floors) or The Living Labyrinth (9 floors)
- Epic sessions
- Maximum challenge and rewards

### No "Wasted" Sessions

**Every dungeon run provides:**
- Character XP (permanent)
- Skill mastery progress (permanent)
- Materials (persistent)
- Gear drops (persistent)
- Gold (persistent)

**Extract after any floor:**
- Keep all rewards earned
- No "all or nothing" pressure
- Safer to quit early if needed

---

## Implementation Priority

### Phase 1-3 (MVP)

**The Cursed Tower Only**
- Prove the core loop works
- 5 floors is sufficient to validate
- Don't build other dungeons until proven

### Phase 4 (Post-MVP Validation)

**If Phase 3 succeeds, plan Phase 7:**
- Design next 2 dungeons in detail
- The Forgotten Catacombs (short alternative)
- The Iron Citadel (long challenge)

### Phase 7 (Endgame Expansion)

**Ship remaining dungeons sequentially:**
1. The Forgotten Catacombs + The Iron Citadel
2. The Hive of Unity + The Frozen Spire
3. The Living Labyrinth (grand finale)

**Each release = content update, renewed engagement**

---

## Design Guidelines for Future Dungeons

### When Adding New Dungeons

**Must Have:**
- Clear theme and visual identity
- Unique enemy types (not just reskins)
- Distinct mechanical challenges
- Narrative justification
- Different session length from existing dungeons

**Should Have:**
- Unique boss mechanics
- Environmental hazards
- Meaningful choices (moral, tactical)
- Lore connections to other dungeons

**Nice to Have:**
- Class-specific interactions
- Branching paths within dungeon
- Secret floors/rooms
- Achievement challenges

---

## Conclusion

**Multiple dungeons with varied lengths = respect for player time.**

- Short dungeons for busy days
- Long dungeons for deep sessions
- All dungeons provide meaningful progress
- Difficulty independent of length
- Replayability through choice

This structure supports the core philosophy: **Your time matters, whether you have 15 minutes or 2 hours.**

---

_See [`narrative-framework.md`](narrative-framework.md) for full story details on each dungeon._
_See [`../02-IMPLEMENTATION/phase-plan.md`](../02-IMPLEMENTATION/phase-plan.md) for implementation timeline._
