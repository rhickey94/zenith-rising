# Class Skills: Narrative-Integrated Abilities (✅ SOURCE OF TRUTH - Fantasy Theme)

> **📌 AUTHORITATIVE REFERENCE:** This document contains the complete, detailed skill specifications for all three classes. All other skill documentation should reference this file for specific mechanics and values.
>
> **Related Documentation:**
> - [`../01-GAME-DESIGN/combat-skills.md`](../01-GAME-DESIGN/combat-skills.md) - High-level combat philosophy and design
> - [`../02-IMPLEMENTATION/skill-standardization.md`](../02-IMPLEMENTATION/skill-standardization.md) - Technical implementation patterns
> - [`../../CLAUDE.md`](../../CLAUDE.md) - Current implementation status (2 of 18 skills working)

This document designs skills that feel mechanically distinct while reflecting each character's personality and story role.

---

## Garrick Stonefist - The Vanguard (Melee Fighter)

### Core Identity
Heavy armor, close combat, protective. Gets stronger when surrounded. Abilities focused on crowd control, tanking, and creating space for allies (or himself in solo play).

### Basic Attack (Left Click)
**"Ironbreaker Blade"**

- Melee swing with enchanted war sword
- Auto-aims at nearest enemy within melee range
- Fast attack speed, moderate damage
- Every 5th hit: Cleaving strike (hits multiple enemies)

**Narrative Flavor:**
Standard issue royal guard equipment. Garrick added his own enchantments—runes of sharpness etched into the blade. "If it's worth hitting once, it's worth hitting hard."

### Special Attack (Right Click - 3s cooldown)
**"Shield Bash"**

- Short dash forward with tower shield raised
- Stuns first enemy hit for 1.5 seconds
- Knocks back other enemies in path
- Brief invulnerability during dash

**Narrative Flavor:**
Breaching castle gates, breaching enemy lines—same principle. Hit hard, hit first, create an opening.

---

### Active Skills (Q, E, R, Ultimate)

#### Q - "Earthshaker" (8s cooldown)

**Mechanical Effect:**
- Ground slam in 360° radius
- Moderate damage + knockback
- Enemies hit are slowed by 40% for 3 seconds
- Briefly breaks enemy targeting (they lose aggro for 1s)

**Visual:**
- Garrick slams gauntlet into ground
- Shockwave of earth and stone ripples outward
- Dust and debris fly up
- Small cracks appear in ground

**Narrative Flavor:**
"Royal guard training taught me crowd control. You don't kill rioters—you make them think twice about getting closer."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Increased radius, enemies are grounded (can't fly/dash for 2s)
- **Lv3:** Creates a lingering "suppression zone" that slows enemies for 5 seconds

---

#### E - "Stalwart Defense" (12s cooldown, 5s duration)

**Mechanical Effect:**
- Raises enchanted tower shield
- Blocks 80% of incoming damage from the front
- Can still attack while shielded
- Shield HP: 200 (breaks if depleted)
- Movement speed reduced by 30% while active

**Visual:**
- Magical ward appears over shield surface (glowing runes)
- Shield glows brighter when absorbing hits
- Cracks/flickers as it takes damage
- Golden-white magical barrier effect

**Narrative Flavor:**
"You don't leave people behind. You hold the line. Even if you're the only one left holding it."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Shield covers 270° (front + sides), reflects 20% of blocked damage
- **Lv3:** When shield breaks or expires, releases a magical pulse that damages nearby enemies

---

#### R - "Berserker's Fury" (15s cooldown, 6s duration)

**Mechanical Effect:**
- Channels ancient battle rage magic
- +40% attack speed
- +20% movement speed
- +30% damage
- Gain 10 HP per kill while active

**Visual:**
- Garrick's eyes glow red
- Screen edges pulse with heartbeat effect
- Character model slightly blurred (moving faster)
- Red battle aura surrounds him
- Armor glows with runic power

**Narrative Flavor:**
"Battle magic. Forbidden in most kingdoms. Good thing I'm no longer bound by their laws."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Duration increased to 8s, also gain 20% damage resistance
- **Lv3:** Kills extend duration by 2s (can maintain indefinitely with enough kills)

---

#### ULTIMATE - "Unbreakable Oath" (60s cooldown, 10s duration)

**Mechanical Effect:**
- Garrick cannot drop below 1 HP for 10 seconds
- Gains +50% damage
- +50% attack speed
- Melee attacks create small shockwaves (extended range)
- At end of duration: Heals for 30% of damage dealt during Unbreakable Oath

**Visual:**
- Golden-red energy aura surrounds Garrick
- Health bar turns gold with "UNBREAKABLE OATH" indicator
- Attacks leave red energy trails
- When near death (below 20% HP), screen desaturates except for Garrick (stays vibrant)
- Ancient warrior spirits appear around him (translucent)

**Narrative Flavor:**
"I failed them once. I won't fail again. Not while I'm still breathing."

**Audio:**
- Heavy breathing, heartbeat thumping
- Garrick muttering: "Not yet... not done yet..."
- On activation: "I'M NOT DONE!"
- Faint echoes of ancient battle cries

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Duration increased to 12s, heal increased to 40%
- **Lv3:** If Garrick would die during Unbreakable Oath, he instead survives with 1 HP and gains full heal over 5 seconds after duration ends (once per run)

---

## Lyra Swiftarrow - The Ranger (Ranged Fighter)

### Core Identity
High mobility, precision damage, tactical positioning. Rewarded for staying at optimal range and exploiting weaknesses. Abilities focused on kiting, burst damage, and area control.

### Basic Attack (Left Click)
**"Hunting Bow"**

- Ranged projectile weapon
- Auto-aims at nearest enemy
- Moderate fire rate, high accuracy
- Critical hits on headshots (top 20% of enemy hitbox)

**Narrative Flavor:**
Hand-crafted from ironwood. Lyra knows every grain of the wood. "A bow's only as reliable as the archer who tends it."

### Special Attack (Right Click - 5s cooldown)
**"Power Shot"**

- Hold to charge (max 2 seconds)
- Releases high-damage piercing arrow
- Pierces through up to 3 enemies
- Charge level indicated by arrow glow

**Narrative Flavor:**
"Patience. Wait for them to line up. One shot, multiple problems solved."

---

### Active Skills (Q, E, R, Ultimate)

#### Q - "Alchemical Bomb" (10s cooldown)

**Mechanical Effect:**
- Throw explosive flask at cursor location
- 1-second delay before explosion
- Moderate AOE damage
- Enemies hit are marked for 5 seconds (+25% damage taken from all sources)

**Visual:**
- Flask has glowing liquid inside
- Explosion: Orange-red blast with glass shards
- Marked enemies have glowing target mark over them (appears as magical sigil)

**Narrative Flavor:**
"Homemade. Little bit of this, little bit of that. Royal guards hate these—too effective, too cheap."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Splits into 3 smaller bombs on impact (cluster explosion)
- **Lv3:** Marked enemies take additional damage from crits (+50% total crit damage)

---

#### E - "Shadowstep" (8s cooldown)

**Mechanical Effect:**
- Quick dash in movement direction (or backward if stationary)
- Invulnerable during roll (0.5 seconds)
- Automatically reloads weapon
- Leaves behind an illusory double for 3 seconds (enemies target it)

**Visual:**
- Fast dash with motion blur and shadow trail
- Illusory copy flickers into existence where she was
- Decoy looks like Lyra but translucent/shadowy
- Magical mist dissipates when decoy expires

**Narrative Flavor:**
"Misdirection. Let them shoot at where you were, not where you are."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Decoy explodes when destroyed or after duration (damage + stun)
- **Lv3:** Can store 2 charges of Shadowstep

---

#### R - "Eagle Eye" (18s cooldown, 8s duration)

**Mechanical Effect:**
- Enter precision targeting mode (magical sight enhancement)
- Slows Lyra's movement by 50%
- +100% critical hit chance
- +50% critical damage
- Highlights enemy weak points (glowing spots on enemies)
- Weapon becomes semi-auto (deliberate shots)

**Visual:**
- Screen edges darken (tunnel vision)
- Enemies outlined in golden light
- Weak points glow bright yellow
- Targeting reticle becomes hawk-eye style (magical rangefinder)
- Time feels slightly slower (visual effect, not actual slow-mo)
- Lyra's eyes glow golden

**Narrative Flavor:**
"When everything slows down, you see what others miss. The weak point. The opening. The shot."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** No movement penalty, +75% crit damage
- **Lv3:** Kills during Eagle Eye refund 50% cooldown and extend duration by 2 seconds

---

#### ULTIMATE - "Arcane Sentinel" (70s cooldown, 12s duration)

**Mechanical Effect:**
- Summon arcane construct at cursor location
- Construct fires magical bolts at enemies within range (medium-long)
- Lyra gains +50% attack speed
- +30% movement speed
- Ammo is infinite during duration (magically replenishing)
- Construct lasts until destroyed or duration ends (500 HP)

**Visual:**
- Magical runic circle appears where Lyra throws a summoning stone
- Construct materializes from magical energy
- Crystalline/ethereal design with rotating magical cores
- Golden energy beams track enemies
- Lyra's weapon glows with arcane power (arrows materialize magically)

**Narrative Flavor:**
"I learned this from a wizard's tome I... acquired. They wanted me dead. Now their magic works for me."

**Audio:**
- Construct summoning: Magical chimes, energy coalescing
- Construct firing: Magical pulse sounds
- On activation: "Let's light them up."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Construct gains magical shield (1000 HP shield + 500 base HP), shoots faster
- **Lv3:** Can summon 2 constructs simultaneously

---

## Theron Voidcaller - The Arcanist (Spellcaster)

### Core Identity
Medium range, hybrid damage (burst + DoT), area control. Channels ancient tower magic. Abilities focused on crowd control, debuffs, and manipulating the battlefield. Has some healing/sustain.

### Basic Attack (Left Click)
**"Void Bolt"**

- Fires bolt of void magic
- Auto-aims at nearest enemy
- Medium fire rate, moderate damage
- Applies "Resonance" debuff (stacks up to 5x)
- At 5 stacks: Next basic attack detonates Resonance for AOE damage

**Narrative Flavor:**
"I don't carry a weapon. I don't need one. The Tower responds to my will. Terrifying. Exhilarating."

### Special Attack (Right Click - 4s cooldown)
**"Arcane Wave"**

- Sweeping cone attack in front of Theron
- Pushes enemies back with magical force
- Deals minor damage
- Interrupts enemy attacks (stun 0.5s)

**Narrative Flavor:**
"Telekinesis, essentially. I manipulate magical force fields. Or... that's my best guess."

---

### Active Skills (Q, E, R, Ultimate)

#### Q - "Mind Fray" (9s cooldown)

**Mechanical Effect:**
- Targeted enemy becomes confused (mind control)
- Confused enemies attack other enemies for 4 seconds
- Deals 50% of their normal damage to allies
- After duration: Confused enemy takes 20% increased damage for 3s

**Visual:**
- Purple/cyan magical energy swirls around target's head
- Enemy model twitches/jerks (confused behavior)
- Purple magical beam connects Theron to target
- Victim's eyes glow with mind magic

**Narrative Flavor:**
"I can sense their thoughts. Simple creatures, really. Easy to redirect."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Can affect 2 enemies simultaneously, confusion spreads to nearby enemies if target dies while confused
- **Lv3:** Confused enemies explode on death (AOE damage + confusion spreads)

---

#### E - "Void Rift" (14s cooldown, 6s duration)

**Mechanical Effect:**
- Create a rift at cursor location
- 20% of all damage dealt to enemies in rift is returned to Theron as healing
- Enemies inside move 30% slower
- Deals minor damage per second (DOT)
- Rift radius: Medium (covers small room)

**Visual:**
- Swirling purple/black portal effect on ground
- Void energy tendrils reaching up from rift
- Enemies inside have distorted, wavy appearance
- Healing numbers float from rift to Theron (green)
- Stars and cosmos visible in the rift

**Narrative Flavor:**
"The Tower exists partially outside our reality. I can... tap into that space. It's cold there. Empty. But useful."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Healing increased to 30%, slows increased to 50%
- **Lv3:** Rift pulls enemies toward center (gravity well effect)

---

#### R - "Arcane Ascension" (16s cooldown, 8s duration)

**Mechanical Effect:**
- Theron channels ancient tower magic
- Attacks chain to nearby enemies (2 additional targets)
- +40% ability damage
- Abilities cost no cooldown during duration (but still have cast times)
- Gain shields (100 HP, regenerates slowly during duration)

**Visual:**
- Theron glows with iridescent magical energy (shifting colors)
- Ancient runes appear around him (tower glyphs)
- Attacks leave geometric magical trails
- Eyes glow cyan/purple
- Ethereal magical aura surrounds him

**Narrative Flavor:**
"For a moment, I can feel what they felt. The Archmages. Omnipresent. Omniscient. It's addictive."

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Duration 10s, chains hit 4 targets
- **Lv3:** Movement speed +30%, abilities cast 50% faster

---

#### ULTIMATE - "Gravity Collapse" (80s cooldown)

**Mechanical Effect:**
- Creates massive gravity well at cursor location
- Pulls all enemies toward center over 5 seconds
- Deals heavy damage per second
- Enemies cannot escape (immobilized while in range)
- At end (5s mark): Implodes, dealing massive AOE damage
- Large radius (entire room)

**Visual:**
- Black sphere appears, distorting space around it
- Purple lightning arcing between pulled enemies
- Screen warps/bends toward the sphere (visual distortion)
- Magical void energy swirling
- Implosion: Bright flash, shockwave, debris pulled inward
- Reality tears and mends

**Narrative Flavor:**
"The Archmages could collapse reality itself. I'm... borrowing that knowledge. Just a fraction. But it's enough."

**Audio:**
- Deep, resonant hum (gets louder)
- Crackling magical energy
- On activation: "Witness the void."
- Enemies screaming as pulled in
- Implosion: Deafening silence, then thunderous boom
- Reality tearing sound

**Upgrade Path:**
- **Lv1:** Base version
- **Lv2:** Duration 7 seconds, leaves behind a damaging void field for 5s after implosion
- **Lv3:** Enemies killed by Gravity Collapse drop double loot

---

## Skill Philosophy Summary

### Garrick (Melee)
- **Playstyle:** Get in, stay in, survive the chaos
- **Strengths:** Tanking, crowd control, sustained fights
- **Weaknesses:** Low mobility, needs to close distance
- **Fantasy:** "I am the wall. Nothing gets past me."

### Lyra (Ranged)
- **Playstyle:** Kite, reposition, punish mistakes
- **Strengths:** Burst damage, mobility, precision
- **Weaknesses:** Fragile, needs space, struggles when surrounded
- **Fantasy:** "Every shot counts. Every move calculated."

### Theron (Caster)
- **Playstyle:** Control battlefield, debuff enemies, sustain through healing
- **Strengths:** AOE damage, utility, self-sustain
- **Weaknesses:** Medium range (dangerous sweet spot), requires good positioning
- **Fantasy:** "I bend reality. The Tower bends with me."

---

## Narrative Integration: How Skills Reflect Story

### Garrick's Royal Guard Past
- **Earthshaker:** Riot control training
- **Stalwart Defense:** "Hold the line" mentality
- **Berserker's Fury:** Forbidden battle magic (no longer bound by rules)
- **Unbreakable Oath:** His defining trauma (couldn't save his comrades, won't fail again)

### Lyra's Survivor Skills
- **Alchemical Bomb:** Improvised weapons (learned young)
- **Shadowstep:** Always have an escape route
- **Eagle Eye:** Hunter training (patience, precision)
- **Arcane Sentinel:** Stolen/acquired magic (adapted enemy techniques)

### Theron's Magical Connection
- **Mind Fray:** Understands minds (even corrupted ones)
- **Void Rift:** Taps into the void beyond reality (Tower's realm)
- **Arcane Ascension:** Channels ancient power (addiction risk)
- **Gravity Collapse:** Borrowed godlike power (dangerous knowledge)

---

## Unique Class Mechanics

### Garrick: "Defiance" (Passive)
- When health drops below 30%, gain +20% damage and +10% damage resistance
- Stacks with Unbreakable Oath
- **Narrative:** "I'm most dangerous when I'm desperate."

### Lyra: "Precision" (Passive)
- Standing still for 2 seconds: Next shot is automatic critical hit
- Encourages hit-and-run, repositioning gameplay
- **Narrative:** "Take your time. Make it count."

### Theron: "Resonance" (Passive)
- Enemies killed by Theron have a 20% chance to drop "Void Echoes"
- Collecting echoes reduces all cooldowns by 1 second
- **Narrative:** "Their energy doesn't die. I can redirect it."

---

## In-Run Upgrade Synergies

### Example Level-Up Upgrades (Apply to All Classes)

**For Garrick:**
- "Ironbreaker Blade gains +20% attack speed"
- "Shield Bash cooldown reduced by 1s"
- "Stalwart Defense shield HP increased by 100"
- "Gain +50 max HP"
- "Life steal: Heal 2 HP per melee hit"

**For Lyra:**
- "Hunting Bow gains +1 pierce"
- "Critical hits refund 0.5s on all cooldowns"
- "Power Shot charges 50% faster"
- "Shadowstep cooldown reduced by 2s"
- "Attacks slow enemies by 20% for 1s"

**For Theron:**
- "Void Bolt chains to 1 additional target"
- "Resonance detonation radius increased by 50%"
- "Mind Fray duration increased by 2s"
- "Abilities have 15% chance to not trigger cooldown"
- "Void Rift radius increased by 30%"

---

## Boss Fight Interactions (Narrative)

### How Bosses React to Each Class

**Example: Floor 5 Boss - The Corrupted Guardian**

**vs Garrick:**
- "YOU FIGHT WITH HONOR. WE HAD WARRIORS ONCE. THEY FELL."
- Summons melee-oriented constructs (tests Garrick's tanking)
- Respects his refusal to retreat

**vs Lyra:**
- "CLEVER. EVASIVE. LIKE THE PREY-CREATURES OF THE OLD FOREST."
- Deploys ranged attacks and area denial (forces Lyra to outmaneuver)
- Frustrated by her hit-and-run tactics

**vs Theron:**
- "YOU... TOUCH THE MAGIC. YOU HEAR US. WHY DO YOU RESIST?"
- Attempts to corrupt him magically (unique mechanic: resist corruption minigame)
- Sees him as potential convert rather than enemy

---

## Skill Unlock Progression

### Starting Skills (Tutorial/Floor 1)
- Basic Attack (unlocked)
- Special Attack (unlocked)
- Q Skill (unlocked at Level 2)
- E Skill (unlocked at Level 5)
- R Skill (unlocked at Level 8)
- Ultimate (unlocked after Floor 1 Boss)

### Training Ground Upgrades (Between Runs)
- Spend Training Points to upgrade skills to Lv2 and Lv3
- **Lv2:** Costs 5 points per skill
- **Lv3:** Costs 15 points per skill
- Forces meaningful choices (can't max everything quickly)

---

## Visual Distinctiveness

### Color Coding
- **Garrick:** Red/Orange/Gold (strength, courage, protection)
- **Lyra:** Blue/Silver/White (precision, cold calculation, clarity)
- **Theron:** Purple/Cyan/Black (arcane, void, mysterious)

### Animation Style
- **Garrick:** Heavy, grounded, forceful (every action has weight)
- **Lyra:** Fluid, graceful, efficient (minimal wasted motion)
- **Theron:** Floating, otherworldly, eerie (slightly detached from reality)

### Audio Identity
- **Garrick:** Metal on metal, earth rumbling, battle cries
- **Lyra:** Bowstring twangs, wind whistling, precision clicks
- **Theron:** Magical resonance, whispers, reality-bending sounds

---

## Implementation Notes

### Phase 1: Basic Attacks + Special (MVP)
- Get these feeling good first
- Differentiate the three classes clearly
- Test combat loop with just these

### Phase 2: Q and E Skills
- Add first two active skills
- Test cooldown balance
- Introduce combo potential

### Phase 3: R Skill
- Add third active skill
- More complex ability interactions
- Test build diversity

### Phase 4: Ultimates
- High-impact, satisfying finishers
- Test boss fights with ultimates
- Balance cooldown vs power

---

## Chain Lightning Discussion (Theron's Arc Lightning - Alternative Q)

### Simplified Approach: Arc Lightning as ONE Great Skill

**If replacing Mind Fray with Arc Lightning:**

**Mechanical Effect:**
- Fire chain lightning at target enemy
- Chains to up to 4 nearby enemies (5 total hits)
- Each chain does 85% of previous damage
- 8 second cooldown

**Visual:**
- Purple-cyan electricity arcs between enemies
- Clear visual line showing the chain path
- Satisfying audio: CRACK → zap → zap → zap → zap

**Narrative Flavor:**
"The Archmages saw connections everywhere. Now, so do I."

### Narrative Integration
**One Story Moment (Floor 3):**
When Theron reaches the Heart Chamber interior (Floor 3), he has a revelation:

**Theron (touching a corrupted altar):** "I can feel it... the Binding. Everything is connected. The Tower, the kingdom, the corruption... even us."

**Guardian Spirit:** "You perceive truly. Use this understanding."

**Garrick:** "Theron, your eyes just lit up. What happened?"

**Theron:** "I... I understand how they think now. How they saw the world. It's beautiful. And terrifying."

[Arc Lightning slightly buffed after this moment: +1 chain target, becomes 6 total hits]

---

_This document is the SOURCE OF TRUTH for all skill mechanics and values._
_All code implementation should reference this document._
_Last updated: Fantasy theme conversion complete._
