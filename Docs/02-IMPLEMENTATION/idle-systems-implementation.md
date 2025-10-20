# Idle Systems Implementation Guide

> **Status:** 📝 PLANNED (Phases 4-7)  
> **Last Updated:** 2025-10-20  
> **Dependencies:** Phase 4+ features, Workshop.cs, Treasury.cs, Forge.cs, Ascension.cs

> **🎯 IMPLEMENTATION REFERENCE:** This document provides detailed mechanics and implementation guidance for the idle/meta-progression systems planned for Phases 4-7.
>
> **Source:** Research and design work done with Claude Desktop analyzing successful hybrid action-RPG idle games (AFK Arena, Last Epoch, Path of Exile, Dead Cells).
>
> **Related Documentation:**
> - [`../01-GAME-DESIGN/systems-progression.md`](../01-GAME-DESIGN/systems-progression.md) - High-level player-facing design
> - [`../01-GAME-DESIGN/design-overview.md`](../01-GAME-DESIGN/design-overview.md) - Core philosophy
> - [`phase-plan.md`](phase-plan.md) - Development phases and current focus
> - [`../../CLAUDE.md`](../../CLAUDE.md) - Current implementation status

---

## Executive Summary

Your design challenge—integrating idle game progression into an action RPG where **"combat replaces waiting"**—maps perfectly onto several proven patterns. Research reveals that successful hybrid games make **active play 3-5x more rewarding than idle time** while ensuring idle systems create **optimization puzzles** rather than **mandatory grinds**.

**Bottom line:** Your 25-30 minute dungeon runs are the perfect cadence for deep between-run systems. The workshop/treasury/forge hub should take 5-15 minutes to interact with, provide meaningful choices, and make players excited for the next run—not feel like homework.

---

## Core Design Problem: Solved Patterns

### Your Constraint: "Combat Replaces Waiting"

**This principle maps to three proven patterns:**

1. **Active Advancement Gates** (AFK Arena, Idle Slayer): Idle accumulation scales with active progression. Your dungeon performance determines idle generation rate.

2. **Hard Progression Gates** (Dead Cells, Nonstop Knight): Bosses and milestones require active play. Materials from dungeons can't be obtained idling.

3. **Active Multiplier Systems** (Tap Titans 2): Playing actively generates 3-5x total value through materials, rare drops, quest completion, and system unlocks.

**Implementation for your game:** Dungeon runs generate materials + unlock idle efficiency upgrades. Idle systems process materials into useful outputs but CAN'T generate raw materials. Players never wait for materials—they play to earn them.

---

## Recommended Architecture: "Processing Chain with Prestige Loop"

### Core Structure

```
ACTIVE GAMEPLAY (Dungeons)
↓ generates
RAW MATERIALS (5 types: Essence, Ore, Fragments, Souls, Crystals)
↓ feed into
WORKSHOP (converts materials → refined resources)
↓ produces
REFINED MATERIALS (deterministic gear improvement)
↓ enables
GEAR OPTIMIZATION (Forge with Forging Potential system)
↓ improves
DUNGEON PERFORMANCE
↓ unlocks
PRESTIGE SYSTEMS (Ascension: multiply material gain, unlock new conversions)
```

### The Four Interconnected Systems

**1. Workshop (Material Processing)**
- Converts raw materials into refined resources over time
- Multiple conversion chains (Essence→Refined Essence, Ore→Ingots, etc.)
- Offline processing continues (12-24 hour cap)
- Upgrades multiply conversion efficiency
- **Decision**: Which materials to prioritize processing

**2. Treasury (Passive Gold Generation)**
- Generates gold based on highest dungeon zone reached
- Generation rate = (Zone)² × 10 gold per hour
- 12-hour accumulation cap creates daily check-ins
- Gold spent on workshop upgrades and forge costs
- **Active play generates 3-5x more gold than idle**

**3. Forge (Deterministic Gear Crafting)**
- **Forging Potential (FP) System** (inspired by Last Epoch):
  - Each item drops with 50-200 FP
  - Crafting costs 5-20 FP per action
  - Add/upgrade affixes deterministically
  - At 0 FP, item is "complete"
- Uses refined materials + gold
- Creates natural stopping points
- **Decision**: Which gear to perfect vs. "good enough"

**4. Ascension Tree (Prestige Layer)**
- Soft reset every 15-25 hours
- Reset dungeon progress, keep workshop/forge/gear
- Gain Ascension Points → permanent multipliers
- 3 branches: Combat, Economy, Utility
- **Decision**: When to ascend for optimal efficiency

---

## Material Economy Design

### Five Material Types (Prevents Bottlenecks)

1. **Essence** (Common): All enemies, used for basic crafts
2. **Ore** (Uncommon): Mini-bosses, used for gear upgrades
3. **Fragments** (Rare): Zone completion, used for advanced crafts
4. **Souls** (Very Rare): Bosses only, used for ascension
5. **Crystals** (Ultra Rare): Challenge runs, used for endgame

### Material Flow

```
Dungeon Run → Raw Materials (direct to inventory)
↓
Workshop Processing (3-5 parallel slots)
  • Essence → Refined Essence (4 hours)
  • Ore → Ingots (8 hours)
  • Fragments → Gems (12 hours)
↓
Forge Crafting (Refined materials + gold)
  • Add/Upgrade gear affixes
  • Consumes Forging Potential
↓
Better Gear → Higher Zones → More Materials per Run (1.5-3x)
```

### Why Time-Gated Processing?

- Creates appointment mechanic (check every 4-12 hours)
- Prevents instant spending of all materials
- Strategic choice: Fast conversions vs. slow conversions
- **Crucially**: Never blocks playing dungeons

---

## Workshop System: Detailed Mechanics

### Processing Queue

**Starting Configuration**: 3 slots
**Unlockable**: Up to 5 slots via gold upgrades

**Conversion Options**:
- **Fast** (4 hours): Essence → Refined Essence
- **Medium** (8 hours): Ore → Ingots
- **Slow** (12 hours): Fragments → Gems

### Upgrades (Purchased with Gold)

**Tier 1** (Early game, 1k-5k gold):
- Processing Speed I: 1.2x faster
- Extra Slot: Unlock 4th slot
- Bulk Convert: Process 10x materials (same time)

**Tier 2** (Mid game, 20k-50k gold):
- Processing Speed II: 1.5x faster
- Quality Improvement: 10% chance for 2x output
- Auto-Collect: Completed conversions auto-deposit

**Tier 3** (Late game, 100k+ gold):
- Master Craftsman: 2x speed
- Overflow Slot: 5th processing slot
- Legendary Refinement: 5% chance for rare upgrade

### Progression Feel

- **Early:** Feels slow, creates anticipation
- **Mid:** Speed boosts feel HUGE (compounding multipliers)
- **Late:** Near-instant processing, focus shifts to material acquisition

### Between-Run Interaction

**Time Investment**: 2-5 minutes per session

**Flow**:
1. Collect completed conversions (30 sec)
2. Queue new materials (1-2 min)
3. Consider upgrades (2 min if planning)

**Mobile Integration** (Optional):
- Push notifications when conversions complete
- Check status via companion app
- Reduces "must be at computer" friction

---

## Forge System: Deterministic Crafting

### Forging Potential (FP) Mechanics

**Design Philosophy**: Crafting fills 80% of needs, drops provide 100%

**How It Works**:
- All gear drops with 50-200 FP (based on rarity, zone level)
- Each craft costs 5-20 FP (higher tier = higher cost)
- Random cost variance: Same upgrade might cost 8 FP or 18 FP
- At 0 FP, item cannot be crafted further → natural stopping point

### Crafting Actions

**1. Add Affix** (5-10 FP)
- Choose which stat to add (Strength, Crit, Attack Speed)
- Adds Tier 1 of that stat
- Costs: 10 Refined Essence + 100 Gold

**2. Upgrade Affix** (8-15 FP per tier)
- Increase tier: T1 → T2 → T3 → T4 → T5
- Each tier significantly stronger
- Costs: 5 Ingots + 200 Gold per tier
- Diminishing returns: T4→T5 costs more than T1→T2

**3. Reroll Numeric Value** (3-5 FP)
- Keep affix tier, reroll number within range
- Cheap optimization after getting desired affixes
- Costs: 3 Gems + 50 Gold

### Affix Tier System

- **T1-T5**: Craftable at Forge
- **T6-T7**: DROP-ONLY (cannot be crafted)
- **Design Intent**: Rare drops remain exciting

**Example**: Finding gear with T6 Life is valuable even if other stats are mediocre—you craft offense around it.

### Example Crafting Session

```
Found: Rare Sword, 150 FP, has T3 Strength, T2 Crit

Goal: Optimize for STR build

Session 1:
- Upgrade Strength T3→T5 (costs 30 FP, materials, gold)
- Result: 120 FP remaining

Session 2:
- Add Attack Speed T1 (costs 8 FP)
- Upgrade Attack Speed to T3 (costs 20 FP)
- Result: 92 FP remaining

Session 3:
- Add Vitality T1 (costs 8 FP)
- Upgrade Vitality to T2 (costs 10 FP)
- Result: 74 FP remaining

Decision: Continue optimizing or save FP for later?
Item is already very usable but not perfect.
```

### Why This Creates Optimization Puzzles

**Decision Points**:
1. **Which gear to invest in?** High FP bad stats vs. Low FP good stats
2. **How much to optimize?** "Good enough" vs. "Perfect"
3. **Build focus**: Target specific stats for synergies
4. **When to start fresh?** Found new item, current has 10 FP left

**Multiple Valid Strategies**:
- Safe: Stop at "good enough" (T3-T4), save FP
- Aggressive: Push to T5 everything, risk low FP
- Experimental: Unusual stat combinations for hybrid builds

---

## Treasury System: Idle Gold Generation

### Purpose

Generate gold for workshop upgrades and forge costs without forcing gold farming.

### Mechanics

**Passive Generation**: Earns gold per hour based on dungeon progress

**Formula**: Gold/hour = (Highest Zone Reached)² × 10
- Zone 20 → 4,000 gold/hour
- Zone 50 → 25,000 gold/hour
- Zone 100 → 100,000 gold/hour

**Accumulation Cap**: 12 hours maximum (prevents weekly check-ins)

**Active Bonus**: Completing dungeons gives 3-5x more gold than idle time

### Upgrades (Gold Cost)

- **Treasury Expansion**: Increase cap (12 → 18 → 24 hours)
- **Interest Rate**: +10% generation per tier (5 tiers)
- **Instant Collection**: Tap for 2 hours worth (1/day)

### Between-Run Interaction

**Time**: 30 seconds
**Flow**: Open Treasury → Collect → See total
**Satisfaction**: Big numbers, coin animation, satisfying sound

**Why This Works**:
- Never blocks gameplay
- Active play significantly better (3-5x multiplier)
- Scales with progression (higher zones = better rate)
- Appointment mechanic (12-hour cap) without punishment

---

## Ascension System: Prestige Loop

### Purpose

Long-term meta-progression providing goals spanning weeks while maintaining challenge.

### Mechanics

**When to Ascend** (Player Choice):
- **Soft reset**: Keep workshop upgrades, forge gear, treasury level
- **Reset**: Dungeon progress (back to Zone 1), stat points, per-run upgrades
- **Gain**: Ascension Points (based on zones + challenges)
- **Optimal timing**: Every 15-25 hours of play

### Ascension Tree Structure

**Three Branches** (Permanent upgrades):

**Combat Branch**:
- +5% damage (costs 1 AP)
- +10% HP (costs 2 AP)
- Start with extra life (costs 5 AP)
- Enemies drop +25% materials (costs 10 AP)

**Economy Branch**:
- Treasury generates 1.2x gold (costs 1 AP)
- Workshop processes 1.2x faster (costs 2 AP)
- Forge costs 10% less materials (costs 5 AP)
- Start runs with gold (costs 10 AP)

**Utility Branch**:
- Unlock 4th workshop slot (costs 3 AP)
- Extended offline processing (costs 5 AP)
- Unlimited treasury collection (costs 8 AP)
- Free forge respec (costs 12 AP)

### Power Budget

- Ascension provides ~5-10% power at baseline
- After 50+ ascensions: ~15-20%
- Focus on convenience and variety, not pure stats
- Diminishing returns: First 10 AP huge, next 50 incremental

### Design Philosophy

**Always Have Next Goal**:
- Short-term: 5 AP (achievable in days)
- Mid-term: 20 AP (achievable in 1-2 weeks)
- Long-term: 100 AP (months)

**Build Diversity**: Multiple viable paths
**Free Respec**: Costs 1 AP, encourages experimentation
**Scales with Time**: Early ascensions take days, late game takes weeks

---

## System Interconnections: The Optimization Puzzle

### What Makes Systems "Deep"

The magic emerges from how systems **multiply** each other, not just exist independently.

### Interconnection Examples

**Loop 1: Workshop Speed ↔ Gear Quality**
```
Faster Workshop → More refined materials per day
→ More forge attempts
→ Better gear faster
→ Higher zones reached
→ More materials per run
→ Faster workshop upgrades
```

**Loop 2: Treasury Gold ↔ All Systems**
```
Treasury gold → Workshop upgrades
→ Faster processing
→ More crafts per session
→ Better gear
→ Higher zones
→ Better treasury generation
```

**Loop 3: Ascension Timing ↔ Efficiency**
```
Ascend early → More ascensions per week → More AP
Ascend late → Better materials before reset
Ascension bonuses → More materials per future run
```

**Loop 4: Build Synergies ↔ Forge Choices**
```
STR build needs: Attack damage + HP + Physical resist
Forge targets these stats deterministically
FP limits force prioritization
Which gear pieces to perfect vs. "good enough"?
```

### The Emergent Optimization Space

**Players discover strategies like**:
- "If I rush workshop speed early, I can craft 2x more per week"
- "Ascending at Zone 30 every 16 hours is more efficient than Zone 50 every 32 hours"
- "Weapons should get perfect rolls, armor just needs 'good enough'"
- "Economy branch first accelerates everything else"

**This is the goal**: Players finding optimal strategies through experimentation, not following a guide.

---

## Power Budget: Keeping Systems Optional

### Critical Balance Target

**ALL content must be completable with**:
- Base stats from dungeon runs
- Random gear drops (no crafting)
- Zero workshop upgrades
- Zero ascension points

**Target**: New player should reach Zone 30-40 with NO idle system engagement.

### Optional Power Breakdown

| System | Power Contribution | Purpose |
|--------|-------------------|----------|
| Core stats from dungeons | 50% | Baseline progression |
| Gear from drops | 30% | RNG excitement |
| Forge crafting | 15-20% | Targeted optimization |
| Ascension tree | 5-10% | Long-term scaling |
| Workshop efficiency | 0% direct | Enables other systems faster |

**Total optional power**: 25-30% (fits research "optimal range")

### Why This Works

**25-40% power from optional systems**:
- Feels worth pursuing
- Doesn't feel mandatory
- Creates optimization depth
- Respects different playstyles

**Research finding**:
- <10% = "Barely noticeable"
- 25-40% = **"Worth pursuing"** ← TARGET
- >60% = "Mandatory despite label"

---

## Decision Cadence: When Players Make Choices

### Micro (During Dungeon Runs - 25-30 min)

- Which power upgrades to take
- Which skills to unlock
- Risk/reward on boss encounters
- **NOT related to idle systems** (combat focus maintained)

### Meso (Between Runs - 5-15 min)

**Immediate** (30 seconds):
- Collect treasury gold
- Collect workshop conversions
- Quick gear comparison

**Tactical** (5 min):
- Which materials to queue
- Which gear piece to forge next
- Spend gold on which upgrade

**Strategic** (10-15 min):
- Plan next build (STR vs INT)
- Long-term forge goals
- Workshop upgrade path

### Macro (Per Session - Daily)

- Long-term forge projects ("Perfect STR sword")
- Workshop upgrade priorities
- Treasury upgrade planning
- Material stockpiling strategy

### Meta (Weekly)

- **Ascension timing**: Now or push further?
- **Ascension tree build**: Combat vs Economy path
- **Challenge runs**: Attempt for rare Crystals
- **Build experimentation**: Try hybrid approach

---

## UI Design: Between-Run Interactions

### Hub Landing Page

```
╔════════════════════════════════════════════════════════════╗
║  BLACKSMITH'S FORGE                          [Queue] [?]   ║
╠═══════════╦════════════════════════════════════════════════╣
║ Quick     ║  WHAT WOULD YOU LIKE TO DO?                    ║
║ Access    ║                                                ║
║           ║  [💰] Collect Treasury      +2,450 gold       ║
║ Treasury  ║  [⚙️] Workshop (2 ready)     View →          ║
║ Workshop  ║  [🔨] Forge Items            Open →          ║
║ Forge     ║  [⭐] Ascension Tree         5 AP            ║
║ Ascension ║                                                ║
╚═══════════╩════════════════════════════════════════════════╝
```

**Design Principles**:
- Pause-menu access (single keypress)
- Clear visual hierarchy (completed items highlighted)
- One-click actions for collection
- 5-15 minute interaction target
- Satisfying feedback (animations, sounds)

### Workshop Interface Key Features

- **Visual progress bars** with time estimates
- **Bulk conversion** options (1, 10, Max)
- **Upgrade shop** clearly shows costs and effects
- **Completed items** animate collection
- **Mobile notifications** (optional)

### Forge Interface Key Features

- **Forging Potential** prominently displayed
- **Affix tiers** color-coded (T1-T5 craftable, T6-T7 special)
- **Material costs** shown before committing
- **Undo warning** (FP is consumed, can't reverse)
- **Comparison view** (current vs. potential)

---

## Common Pitfalls to Avoid

### Pitfall 1: Too Much Waiting

**Symptom**: Players say "I should log off and wait"

**Solution**:
- Materials ONLY from dungeons
- Workshop is convenience, not requirement
- Active play gives 3-5x more value
- Offline caps at 12-24 hours

### Pitfall 2: Mandatory Grind

**Symptom**: Players feel forced to engage to progress

**Solution**:
- Balance for ZERO idle engagement
- Optional gives 25-30% power, not 60%+
- Alternative paths viable (drop-only gear works)
- Systems are convenience, not requirement

### Pitfall 3: Decision Paralysis

**Symptom**: Players overwhelmed, don't know what to do

**Solution**:
- Introduce systems gradually (1 per 10 hours)
- Clear tooltips explain everything
- "Recommended" options for new players
- Allow experimentation (cheap respec)

### Pitfall 4: Weak Optimization

**Symptom**: Min-maxers bored, no depth

**Solution**:
- Multiple interconnected systems
- Compounding effects (workshop → materials → gear → dungeons)
- Long-term goals (100+ hour content)
- Challenge content for perfect builds

### Pitfall 5: No Stopping Points

**Symptom**: "One more craft" addiction loop

**Solution**:
- FP depletion creates natural end
- Workshop slots limit parallel processing
- Ascension cycles create milestones
- Clear "good enough" vs "perfect" thresholds

---

## Implementation Roadmap (Adapted to 2-Week Phases)

### Phase 4: Basic Gear & Forge MVP (2-3 weeks)

**Goal:** Validate FP system feels good

**Tasks:**
1. Material drops (5 types)
2. Basic forge (FP system, add/upgrade affixes T1-T3)
3. Inventory UI
4. Gear save/load

**Test:** Does crafting feel good? Is FP clear?

**Deliverable:** Playable loop with deterministic crafting

---

### Phase 5: Idle Systems MVP (2-3 weeks)

**Goal:** Validate players engage without feeling forced

**Tasks:**
1. Treasury (passive gold, 12-hour cap)
2. Workshop (material processing, 3 slots, basic conversions)
3. Basic upgrades for workshop/treasury (Tier 1)

**Test:** Do players engage between runs? Mandatory-feeling?

**Deliverable:** Complete idle progression layer

---

### Phase 6: Depth & Ascension (2-3 weeks)

**Goal:** Optimizers have goals, casuals not overwhelmed

**Tasks:**
1. Workshop upgrades (Tiers 2-3: speed, quality, slots)
2. Treasury upgrades (expansion, interest rate)
3. Basic ascension (2 branches, simplified tree)
4. Expand forge (T4-T5 affixes, reroll action)

**Test:** Optimizers have goals? Too complex for casuals?

**Deliverable:** Long-term retention hooks

---

### Phase 7: Endgame Expansion (2-3 weeks)

**Goal:** Sustain long-term interest

**Tasks:**
1. Complete ascension tree (3 branches, 30+ upgrades)
2. Advanced forge features (T6-T7 drop-only affixes)
3. Challenge runs (modify dungeon rules, reward Crystals)
4. Additional dungeons (varying session lengths)

**Deliverable:** Polished, endgame content

---

## Key Research Insights Applied

### From Idle Game Research (Trimps, NGU Idle)

**Applied**:
- Layered system complexity (introduce gradually)
- Multiplicative interconnections (systems buff each other)
- Multi-tier prestige (ascension system)
- Resource conversion chains (workshop processing)
- Meta-progression currencies (ascension points)

### From Hybrid Games (Idle Slayer, AFK Arena)

**Applied**:
- Active play 3-5x better than idle
- Hard progression gates (bosses require active)
- Active improves idle efficiency (zone → treasury rate)
- Time caps on accumulation (12-24 hours)
- Separate resources for different modes

### From ARPG Crafting (Path of Exile, Last Epoch)

**Applied**:
- Forging Potential creates stopping points
- Deterministic targeting with RNG variance
- Crafting fills 80%, drops give 100%
- Multiple affix tiers (T1-T5 craftable, T6-T7 drop-only)
- Material economy with conversion chains

### From Roguelite Meta-Progression (Hades, Dead Cells)

**Applied**:
- Convenience > Power upgrades
- Variety > Strength unlocks
- Hub feels rewarding to return to
- Multiple decision cadences (immediate, daily, weekly)
- "Death isn't failure" through ascension narrative

---

## Conclusion: Your Design Philosophy Realized

Your core principle—**"Combat replaces waiting"**—is fully achievable with this architecture.

### The Key Solutions

1. **Materials come from combat only** (never idle generation)
2. **Workshop processes materials** (convenience, not blocking)
3. **Forge creates build variety** (deterministic improvement of RNG drops)
4. **Ascension provides long-term goals** (weeks to months)
5. **Between-run sessions are 5-15 minutes** (not 45-minute homework)

### The Optimization Puzzle Emerges From

- **Limited workshop slots**: Which materials to process?
- **Limited FP per item**: Which gear to perfect?
- **Multiple build paths**: Which stats to prioritize?
- **Ascension timing**: When to reset?
- **Material allocation**: Short vs. long-term investments?

### Why This Works

**For optimization-focused players**:
- Multiple interconnected systems create emergent strategies
- Compounding effects reward planning
- Long-term goals span 100+ hours
- Challenge content tests perfect builds

**For casual players**:
- Can ignore systems and still progress
- Optional power is 25-30% (helpful, not required)
- Clear stopping points (FP depletion)
- Systems introduced gradually

**For solo indie developer**:
- Realistic timeline adapted to 2-week phases
- Core systems are well-scoped
- Can expand post-launch
- No content treadmill required

---

_Last updated: Documentation overhaul based on hybrid action-RPG idle research_
_Living document - Update as systems are implemented and validated_
