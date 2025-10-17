# Class Skills: Narrative-Integrated Abilities (✅ SOURCE OF TRUTH)

> **📌 AUTHORITATIVE REFERENCE:** This document contains the complete, detailed skill specifications for all three classes. All other skill documentation should reference this file for specific mechanics and values.
>
> **Related Documentation:**
> - [`../01-GAME-DESIGN/combat-skills.md`](../01-GAME-DESIGN/combat-skills.md) - High-level combat philosophy and design
> - [`../02-IMPLEMENTATION/skill-standardization.md`](../02-IMPLEMENTATION/skill-standardization.md) - Technical implementation patterns
> - [`../../CLAUDE.md`](../../CLAUDE.md) - Current implementation status (2 of 18 skills working)

This document designs skills that feel mechanically distinct while reflecting each character's personality and story role.

Marcus Kane - The Breacher (Melee Fighter)
Core Identity
Heavy armor, close combat, protective. Gets stronger when surrounded. Abilities focused on crowd control, tanking, and creating space for allies (or himself in solo play).

Basic Attack (Left Click)
"Fusion Cutter"

Melee swing with a powered combat blade
Auto-aims at nearest enemy within melee range
Fast attack speed, moderate damage
Every 5th hit: Cleaving strike (hits multiple enemies)

Narrative Flavor:
Standard issue military equipment. Marcus modified it himself—added extra power cells. "If it's worth hitting once, it's worth hitting hard."

Special Attack (Right Click - 3s cooldown)
"Breaching Charge"

Short dash forward with shield raised
Stuns first enemy hit for 1.5 seconds
Knocks back other enemies in path
Brief invulnerability during dash

Narrative Flavor:
Breaching doors, breaching enemy lines—same principle. Hit hard, hit first, create an opening.

Active Skills (Q, E, R, Ultimate)
Q - "Crowd Suppression" (8s cooldown)
Mechanical Effect:

Ground slam in 360° radius
Moderate damage + knockback
Enemies hit are slowed by 40% for 3 seconds
Briefly breaks enemy targeting (they lose aggro for 1s)

Visual:

Marcus slams gauntlet into ground
Shockwave of energy ripples outward
Sparks and debris fly up

Narrative Flavor:
"Crowd control was drilled into me. You don't kill rioters—you make them think twice about getting closer."
Upgrade Path:

Lv1: Base version
Lv2: Increased radius, enemies are grounded (can't fly/dash for 2s)
Lv3: Creates a lingering "suppression zone" that slows enemies for 5 seconds


E - "Fortify" (12s cooldown, 5s duration)
Mechanical Effect:

Deploys energy shield in front of Marcus
Blocks 80% of incoming damage from the front
Can still attack while shielded
Shield HP: 200 (breaks if depleted)
Movement speed reduced by 30% while active

Visual:

Hardlight hexagonal barrier appears
Glows brighter when absorbing hits
Cracks/flickers as it takes damage

Narrative Flavor:
"You don't leave people behind. You hold the line. Even if you're the only one left holding it."
Upgrade Path:

Lv1: Base version
Lv2: Shield covers 270° (front + sides), reflects 20% of blocked damage
Lv3: When shield breaks or expires, releases an energy pulse that damages nearby enemies


R - "Combat Stim" (15s cooldown, 6s duration)
Mechanical Effect:

Injects military-grade combat stimulant
+40% attack speed
+20% movement speed
+30% damage
Gain 10 HP per kill while active

Visual:

Marcus's HUD glows red
Screen edges pulse with heartbeat effect
Character model slightly blurred (moving faster)

Narrative Flavor:
"Military issue. Not supposed to have these anymore after discharge. Good thing I know a guy."
Upgrade Path:

Lv1: Base version
Lv2: Duration increased to 8s, also gain 20% damage resistance
Lv3: Kills extend duration by 2s (can maintain indefinitely with enough kills)


ULTIMATE - "Last Stand" (60s cooldown, 10s duration)
Mechanical Effect:

Marcus cannot drop below 1 HP for 10 seconds
Gains +50% damage
+50% attack speed
Melee attacks create small shockwaves (extended range)
At end of duration: Heals for 30% of damage dealt during Last Stand

Visual:

Red energy aura surrounds Marcus
Health bar turns gold with "LAST STAND" indicator
Attacks leave red energy trails
When near death (below 20% HP), screen desaturates except for Marcus (stays vibrant)

Narrative Flavor:
"I failed them once. I won't fail again. Not while I'm still breathing."
Audio:

Heavy breathing, heartbeat thumping
Marcus muttering: "Not yet... not done yet..."
On activation: "I'M NOT DONE!"

Upgrade Path:

Lv1: Base version
Lv2: Duration increased to 12s, heal increased to 40%
Lv3: If Marcus would die during Last Stand, he instead survives with 1 HP and gains full heal over 5 seconds after duration ends (once per run)


Aria Zhang - The Sharpshooter (Ranged Fighter)
Core Identity
High mobility, precision damage, tactical positioning. Rewarded for staying at optimal range and exploiting weaknesses. Abilities focused on kiting, burst damage, and area control.

Basic Attack (Left Click)
"Precision Rifle"

Ranged hitscan weapon
Auto-aims at nearest enemy
Moderate fire rate, high accuracy
Critical hits on headshots (top 20% of enemy hitbox)

Narrative Flavor:
Custom-built from salvaged parts. Aria knows every component. "A gun's only as reliable as the person who maintains it."

Special Attack (Right Click - 5s cooldown)
"Charged Shot"

Hold to charge (max 2 seconds)
Releases high-damage piercing shot
Pierces through up to 3 enemies
Charge level indicated by reticle glow

Narrative Flavor:
"Patience. Wait for them to line up. One shot, multiple problems solved."

Active Skills (Q, E, R, Ultimate)
Q - "Tactical Grenade" (10s cooldown)
Mechanical Effect:

Throw grenade at cursor location
1-second delay before explosion
Moderate AOE damage
Enemies hit are marked for 5 seconds (+25% damage taken from all sources)

Visual:

Grenade has blinking red light
Explosion: Blue-white flash with shrapnel effect
Marked enemies have holographic target reticle over them

Narrative Flavor:
"Homemade. Little bit of this, little bit of that. Corporate security hates these—too effective, too cheap."
Upgrade Path:

Lv1: Base version
Lv2: Splits into 3 smaller grenades on impact (cluster bomb)
Lv3: Marked enemies take additional damage from crits (+50% total crit damage)


E - "Evasive Roll" (8s cooldown)
Mechanical Effect:

Quick dash in movement direction (or backward if stationary)
Invulnerable during roll (0.5 seconds)
Automatically reloads weapon
Leaves behind a holographic decoy for 3 seconds (enemies target it)

Visual:

Fast dash with motion blur
Holographic copy flickers into existence where she was
Decoy looks like Aria but translucent blue

Narrative Flavor:
"Misdirection. Let them shoot at where you were, not where you are."
Upgrade Path:

Lv1: Base version
Lv2: Decoy explodes when destroyed or after duration (damage + stun)
Lv3: Can store 2 charges of Evasive Roll


R - "Overwatch" (18s cooldown, 8s duration)
Mechanical Effect:

Enter precision targeting mode
Slows Aria's movement by 50%
+100% critical hit chance
+50% critical damage
Highlights enemy weak points (glowing spots on enemies)
Weapon becomes semi-auto (deliberate shots)

Visual:

Screen edges darken (tunnel vision)
Enemies outlined in red
Weak points glow yellow
Targeting reticle becomes sniper-style (rangefinder, windage indicators)
Time feels slightly slower (visual effect, not actual slow-mo)

Narrative Flavor:
"When everything slows down, you see what others miss. The weak point. The opening. The shot."
Upgrade Path:

Lv1: Base version
Lv2: No movement penalty, +75% crit damage
Lv3: Kills during Overwatch refund 50% cooldown and extend duration by 2 seconds


ULTIMATE - "Killzone" (70s cooldown, 12s duration)
Mechanical Effect:

Deploy automated turret at cursor location
Turret fires at enemies within range (medium-long)
Aria gains +50% attack speed
+30% movement speed
Ammo is infinite during duration
Turret lasts until destroyed or duration ends (500 HP)

Visual:

Turret unfolds from compact disc Aria throws
Sleek, triangular design with rotating barrels
Red laser sights track enemies
Aria's weapon glows with energy (no reload needed)

Narrative Flavor:
"I built this from a security drone I hacked on Titan Station. They tried to kill me with it. Now it works for me."
Audio:

Turret deployment: Mechanical whirring, locks engaging
Turret firing: Rapid-fire energy pulses
On activation: "Let's light them up."

Upgrade Path:

Lv1: Base version
Lv2: Turret gains shield (1000 HP shield + 500 base HP), shoots faster
Lv3: Can deploy 2 turrets simultaneously


Dr. Elias Khoury - The Conduit (Spellcaster)
Core Identity
Medium range, hybrid damage (burst + DoT), area control. Interfaces with Architect technology. Abilities focused on crowd control, debuffs, and manipulating the battlefield. Has some healing/sustain.

Basic Attack (Left Click)
"Psionic Pulse"

Fires bolt of psionic energy
Auto-aims at nearest enemy
Medium fire rate, moderate damage
Applies "Resonance" debuff (stacks up to 5x)
At 5 stacks: Next basic attack detonates Resonance for AOE damage

Narrative Flavor:
"I don't carry a weapon. I don't need one. The Spire responds to my thoughts. Terrifying. Exhilarating."

Special Attack (Right Click - 4s cooldown)
"Psionic Wave"

Sweeping cone attack in front of Elias
Pushes enemies back
Deals minor damage
Interrupts enemy attacks (stun 0.5s)

Narrative Flavor:
"Telekinesis, essentially. I manipulate localized gravitational fields. Or... that's my best guess."

Active Skills (Q, E, R, Ultimate)
Q - "Neural Disrupt" (9s cooldown)
Mechanical Effect:

Targeted enemy becomes confused
Confused enemies attack other enemies for 4 seconds
Deals 50% of their normal damage to allies
After duration: Confused enemy takes 20% increased damage for 3s

Visual:

Purple/cyan energy swirls around target's head
Enemy model twitches/jerks (confused behavior)
Purple beam connects Elias to target

Narrative Flavor:
"I can feel their neural patterns. Simple creatures, really. Easy to redirect."
Upgrade Path:

Lv1: Base version
Lv2: Can affect 2 enemies simultaneously, confusion spreads to nearby enemies if target dies while confused
Lv3: Confused enemies explode on death (AOE damage + confusion spreads)


E - "Void Rift" (14s cooldown, 6s duration)
Mechanical Effect:

Create a rift at cursor location
20% of all damage dealt to enemies in rift is returned to Elias as healing
Enemies inside move 30% slower
Deals minor damage per second (DOT)
Rift radius: Medium (covers small room)

Visual:

Swirling purple/black portal effect on ground
Energy tendrils reaching up from rift
Enemies inside have distorted, wavy appearance
Healing numbers float from rift to Elias (green)

Narrative Flavor:
"The Spire exists partially outside our dimension. I can... tap into that space. It's cold there. Empty. But useful."
Upgrade Path:

Lv1: Base version
Lv2: Healing increased to 30%, slows increased to 50%
Lv3: Rift pulls enemies toward center (gravity well effect)


R - "Architect's Blessing" (16s cooldown, 8s duration)
Mechanical Effect:

Elias channels Architect energy
Attacks chain to nearby enemies (2 additional targets)
+40% ability damage
Abilities cost no cooldown during duration (but still have cast times)
Gain shields (100 HP, regenerates slowly during duration)

Visual:

Elias glows with iridescent energy (shifting colors)
Geometric patterns appear around him (Architect glyphs)
Attacks leave geometric particle trails
Eyes glow cyan

Narrative Flavor:
"For a moment, I can feel what they felt. The Architects. Omnipresent. Omniscient. It's addictive."
Upgrade Path:

Lv1: Base version
Lv2: Duration 10s, chains hit 4 targets
Lv3: Movement speed +30%, abilities cast 50% faster


ULTIMATE - "Singularity" (80s cooldown)
Mechanical Effect:

Creates massive gravity well at cursor location
Pulls all enemies toward center over 5 seconds
Deals heavy damage per second
Enemies cannot escape (immobilized while in range)
At end (5s mark): Implodes, dealing massive AOE damage
Large radius (entire room)

Visual:

Black sphere appears, distorting space around it
Purple lightning arcing between pulled enemies
Screen warps/bends toward singularity (visual distortion)
Implosion: Bright flash, shockwave, debris pulled inward

Narrative Flavor:
"The Architects could collapse stars. I'm... borrowing that knowledge. Just a fraction. But it's enough."
Audio:

Deep, resonant hum (gets louder)
Crackling energy
On activation: "Witness infinity."
Enemies screaming as pulled in
Implosion: Deafening silence, then thunderous boom

Upgrade Path:

Lv1: Base version
Lv2: Duration 7 seconds, leaves behind a damaging field for 5s after implosion
Lv3: Enemies killed by Singularity drop double loot


Skill Philosophy Summary
Marcus (Melee)

Playstyle: Get in, stay in, survive the chaos
Strengths: Tanking, crowd control, sustained fights
Weaknesses: Low mobility, needs to close distance
Fantasy: "I am the wall. Nothing gets past me."

Aria (Ranged)

Playstyle: Kite, reposition, punish mistakes
Strengths: Burst damage, mobility, precision
Weaknesses: Fragile, needs space, struggles when surrounded
Fantasy: "Every shot counts. Every move calculated."

Elias (Caster)

Playstyle: Control battlefield, debuff enemies, sustain through healing
Strengths: AOE damage, utility, self-sustain
Weaknesses: Medium range (dangerous sweet spot), requires good positioning
Fantasy: "I bend reality. The Spire bends with me."


Narrative Integration: How Skills Reflect Story
Marcus's Military Past

Crowd Suppression: Riot control training
Fortify: "Hold the line" mentality
Combat Stim: Military-issue drugs (illegal now)
Last Stand: His defining trauma (couldn't save his unit, won't fail again)

Aria's Survivor Skills

Tactical Grenade: Improvised weapons (learned young)
Evasive Roll: Always have an escape route
Overwatch: Sniper training (patience, precision)
Killzone: Repurposed enemy tech (hacked drone)

Elias's Psionic Connection

Neural Disrupt: Understands minds (even alien ones)
Void Rift: Taps into dimensional space (Spire's realm)
Architect's Blessing: Channels ancient power (addiction risk)
Singularity: Borrowed god-like power (dangerous knowledge)


Unique Class Mechanics
Marcus: "Defiance" (Passive)

When health drops below 30%, gain +20% damage and +10% damage resistance
Stacks with Last Stand
Narrative: "I'm most dangerous when I'm desperate."

Aria: "Precision" (Passive)

Standing still for 2 seconds: Next shot is automatic critical hit
Encourages hit-and-run, repositioning gameplay
Narrative: "Take your time. Make it count."

Elias: "Resonance" (Passive)

Enemies killed by Elias have a 20% chance to drop "Psionic Echoes"
Collecting echoes reduces all cooldowns by 1 second
Narrative: "Their energy doesn't die. I can redirect it."


In-Run Upgrade Synergies
Example Level-Up Upgrades (Apply to All Classes)
For Marcus:

"Fusion Cutter gains +20% attack speed"
"Breaching Charge cooldown reduced by 1s"
"Fortify shield HP increased by 100"
"Gain +50 max HP"
"Life steal: Heal 2 HP per melee hit"

For Aria:

"Precision Rifle gains +1 pierce"
"Critical hits refund 0.5s on all cooldowns"
"Charged Shot charges 50% faster"
"Evasive Roll cooldown reduced by 2s"
"Attacks slow enemies by 20% for 1s"

For Elias:

"Psionic Pulse chains to 1 additional target"
"Resonance detonation radius increased by 50%"
"Neural Disrupt duration increased by 2s"
"Abilities have 15% chance to not trigger cooldown"
"Void Rift radius increased by 30%"


Boss Fight Interactions (Narrative)
How Bosses React to Each Class
Example: Floor 5 Boss - The Crystal Nexus
vs Marcus:

"YOU FIGHT WITH HONOR. WE HAD WARRIORS ONCE. THEY FELL."
Summons melee-oriented constructs (tests Marcus's tanking)
Respects his refusal to retreat

vs Aria:

"CLEVER. EVASIVE. LIKE THE PREY-CREATURES OF OUR HOMEWORLD."
Deploys turrets and ranged attacks (forces Aria to outmaneuver)
Frustrated by her hit-and-run tactics

vs Elias:

"YOU... TOUCH THE NETWORK. YOU HEAR US. WHY DO YOU RESIST?"
Attempts to corrupt him psionically (unique mechanic: resist corruption minigame)
Sees him as potential convert rather than enemy


Skill Unlock Progression
Starting Skills (Tutorial/Floor 1)

Basic Attack (unlocked)
Special Attack (unlocked)
Q Skill (unlocked at Level 2)
E Skill (unlocked at Level 5)
R Skill (unlocked at Level 8)
Ultimate (unlocked after Floor 1 Boss)

Training Ground Upgrades (Between Runs)

Spend Training Points to upgrade skills to Lv2 and Lv3
Lv2: Costs 5 points per skill
Lv3: Costs 15 points per skill
Forces meaningful choices (can't max everything quickly)


Visual Distinctiveness
Color Coding

Marcus: Red/Orange (aggression, warning, fire)
Aria: Blue/White (cold, precise, clinical)
Elias: Purple/Cyan (psionic, alien, mysterious)

Animation Style

Marcus: Heavy, grounded, forceful (every action has weight)
Aria: Fluid, graceful, efficient (minimal wasted motion)
Elias: Floating, otherworldly, eerie (slightly detached from reality)

Audio Identity

Marcus: Mechanical sounds, explosions, metal on metal
Aria: Electronic beeps, precision clicks, energy hums
Elias: Resonant tones, whispers, reality-bending sounds


Next Steps for Implementation
Phase 1: Basic Attacks + Special (MVP)

Get these feeling good first
Differentiate the three classes clearly
Test combat loop with just these

Phase 2: Q and E Skills

Add first two active skills
Test cooldown balance
Introduce combo potential

Phase 3: R Skill

Add third active skill
More complex ability interactions
Test build diversity

Phase 4: Ultimates

High-impact, satisfying finishers
Test boss fights with ultimates
Balance cooldown vs power

Want me to:

Design specific in-run upgrade trees for each class?
Create skill synergies and combo guides?
Write dialogue for when each class uses their ultimate?
Design boss-specific skill interactions?
Plan skill visual effects in more detail?


Chain Lightning: Right-Sized Implementation
You're absolutely right. Let me scale this back to something that makes chain lightning special without dominating the entire game.

Simplified Approach: Chain Lightning as ONE Great Skill
Replace Elias's Q Skill with "Arc Lightning"
Mechanical Effect:

Fire chain lightning at target enemy
Chains to up to 4 nearby enemies (5 total hits)
Each chain does 85% of previous damage
8 second cooldown

Visual:

Purple-cyan electricity arcs between enemies
Clear visual line showing the chain path
Satisfying audio: CRACK → zap → zap → zap → zap

Narrative Flavor:
"The Architects saw connections everywhere. Now, so do I."
That's it. One skill. It's his Q ability.

Minimal Narrative Integration
One Story Moment (Floor 3)
When Elias reaches the Spire's interior (Floor 3), he has a revelation:
Elias (touching an Architect terminal): "I can feel it... the Network. Everything is connected. The Spire, the station, the enemies... even us."
The Architect: "You perceive truly. Use this understanding."
ARCHON: "Dr. Khoury, your neural activity just spiked. What happened?"
Elias: "I... I understand how they think now. How they see the world. It's beautiful. And terrifying."
[Arc Lightning slightly buffed after this moment: +1 chain target, becomes 6 total hits]
That's the only major narrative moment for chain lightning specifically.

Character Identity Without Obsession
Elias's Defining Traits:

Primary: He interfaces with Architect technology (can use alien powers)
Secondary: He sees connections (hence chain lightning)
Tertiary: He's tempted by knowledge/power (character flaw)

Chain lightning represents #1 and #2, but it's not his ONLY thing.
His other skills matter equally:

E - Void Rift: Area control, healing (shows his strategic mind)
R - Architect's Blessing: Power boost (channeling alien energy)
Ultimate - Singularity: Big damage nuke (raw power)

Chain lightning is ONE tool in his kit, not the whole identity.

Upgrade Path: Keep It Simple
In-Run Level-Up Options:
When you level up, you might see:
Chain Lightning Upgrades (appear randomly like other upgrades):

"+1 chain target" (appears multiple times, stackable)
"+20% chain lightning damage"
"-2s cooldown on Q ability"
"Chain lightning can bounce back to previous targets"
"Chains move 50% faster"

These compete with other upgrade choices like:

"+15% movement speed"
"+50 max HP"
"Projectiles pierce enemies"
"Void Rift lasts 3s longer"

Result: Chain lightning CAN become very strong if you get lucky/build into it, but it's not guaranteed or mandatory.

Training Ground: One Optional Upgrade
"Network Mastery" (5 Training Points):

Arc Lightning starts with +2 chain targets (7 total)
Slightly reduced cooldown (-1s)

That's it. One unlock. Not a whole skill tree.

Boss Interactions: Subtle
Bosses React Briefly
Floor 3 Boss (when you use chain lightning):
"You wield our Network theory as a weapon. Clever."
Floor 5 Boss (if you use chain lightning a lot):
"I see you favor the cascade. Efficient. We approved of efficiency."
No special mechanics. Just acknowledgment.

The Corruption Arc: Simplified
Subtle Hints, Not Overwhelming
Floor 3 (after touching terminal):

Elias's eyes briefly flash cyan when casting (barely noticeable)
One voice line: "The power flows so easily now..."

Floor 4 (if you use chain lightning frequently):

Aria: "Doc, you're using that Architect tech a lot. You feeling okay?"
Elias: "I'm fine. It's just... efficient."
Marcus: "Just don't lose yourself in there, alright?"

Floor 5 (final choice moment):

Architect offers power
Elias can accept (slight corruption) or refuse (stays human)
This affects his OVERALL relationship with Architect tech, not just chain lightning
Minor stat changes either way (+damage but -HP, or balanced)

Corruption is about using ALL Architect powers, not just chain lightning specifically.

Making Chain Lightning Feel Great (Without Overdoing It)
The Core Experience:
What Makes It Satisfying:

✅ Clear visual feedback (you see every chain)
✅ Great audio (crackling electricity)
✅ Damage numbers pop up sequentially (watch the chain happen)
✅ Small camera shake on cast
✅ Enemies react (briefly stunned/lit up)

What You DON'T Need:

❌ Slow-motion effects
❌ Screen-wide flashes
❌ Complex node systems
❌ Environmental conductivity
❌ Special boss phases

Keep it clean and satisfying, not overwhelming.

Comparison to Other Skills
Why Chain Lightning is Great (But Not Dominant):
Arc Lightning (Q - Chain Lightning):

Strength: Multi-target damage, efficient
Weakness: Single-target is weaker than other options
Use Case: Clearing groups of weak enemies

Void Rift (E):

Strength: Area denial, healing, slows
Weakness: Takes time to deal damage
Use Case: Controlling space, sustaining health

Architect's Blessing (R):

Strength: Makes ALL abilities better, versatile
Weakness: Doesn't do damage itself
Use Case: Boss fights, tough encounters

Singularity (Ultimate):

Strength: Huge burst damage, immobilizes
Weakness: Long cooldown
Use Case: Emergency button, tough packs

All four skills are useful. Chain lightning is the "crowd clear" option, not the only option.

Player Build Variety
Chain Lightning Build (One of Many):
Focus: Stack chain upgrades, become AOE monster
Strengths:

Incredible against groups
Very efficient (one spell, many kills)
Satisfying visual feedback

Weaknesses:

Weaker against bosses (few targets)
Relies on cooldown (gaps in damage)
Less effective vs spread-out enemies

Other Viable Builds:

Basic Attack Focus: Fast, sustained damage
Void Rift Focus: Tank build, healing emphasis
Blessing Spam: Buff-based, ability-focused
Hybrid: Balanced, uses everything

Chain lightning is ONE playstyle among several.

Implementation Priority
Phase 1 (MVP):

Basic chain lightning works (Q ability)
Chains to 4 enemies
Looks/sounds good
That's enough to ship

Phase 2 (Polish):

Add upgrade options ("+1 chain", etc.)
Improve VFX (better lightning arcs)
Add Training Ground unlock

Phase 3 (Narrative):

Add Floor 3 story moment (understanding the Network)
Add boss reaction lines
Add subtle corruption hints

Phase 4 (If Time/Scope Allows):

Special interactions with certain enemy types
Build-specific achievements ("Chain 10+ enemies in one cast")
More upgrade variety

Don't do Phase 4 unless Phases 1-3 are rock solid.

The Real Hook
Why Chain Lightning Will Feel Special:
Not because it has 10 pages of lore.
Not because it dominates gameplay.
But because:

It's ONE skill that does ONE thing really well (clear groups)
The visual/audio feedback is TIGHT
It scales nicely with upgrades (feels good early, great late)
It has just enough narrative weight (Elias understands the Network)
Players will naturally want to use it (it's efficient and satisfying)

That's it. That's all it needs to be.

Dialogue: Minimal But Effective
First Time Casting (Tutorial):
Elias: "Let me try something..."
[Casts, chains between 3 enemies]
Elias: "It worked! The energy followed the electromagnetic connections between them. Fascinating."
Floor 3 (Power Up Moment):
Elias (touching terminal): "I can see the Network now. Everything is connected."
[Arc Lightning upgraded: +1 chain target]
Occasional Use (Random Voice Lines):

"The cascade flows."
"Following the connections."
"One strike, many targets."
"Efficiency."

Floor 5 (If You Use It A Lot):
Architect: "You favor the cascade. We too appreciated elegant solutions."
Elias: "It's effective. That's all."
Architect: "Is it? Or do you enjoy the power?"
Elias: "...Both, if I'm honest."
That's all the dialogue. 5 moments total. Done.

Comparison to Other Games
What You're Going For:
Like Chain Lightning in:

Diablo 2: Sorceress's chain lightning - satisfying, scales well, ONE skill among many
Path of Exile: Arc - great clear skill, but not the only viable build
Vampire Survivors: Lightning Ring - strong AOE, but other weapons compete

NOT like:

Whole game built around one mechanic (too narrow)
Chain lightning in every class (not special)
Required for optimal play (limits build diversity)

Chain lightning should be: "Oh hell yes, I love this skill" not "This is the only skill that matters."

Final Recommendation
What to Build:

Arc Lightning (Q): Chains to 4-5 enemies, 8s cooldown
Upgrade options: +1 chain, +damage, -cooldown (compete with other upgrades)
One story moment: Floor 3, Elias understands the Network
Polish: Great VFX/audio, satisfying feedback
Optional: Training Ground unlock for +2 chains

What NOT to Build:

❌ Multiple versions of chain lightning
❌ Complex node/network systems
❌ Environmental conductivity
❌ Chain lightning-specific corruption arc
❌ Boss mechanics built around it

Result:
A skill that players love, that feels special, that scales well, but that doesn't dominate the game or require 10 pages of design docs.
Chain lightning is great. The game has other great things too.

Does this feel better sized? Want me to:

Design the other skills (E, R, Ultimate) in similar detail?
Create the full upgrade pool for all three classes?
Write enemy designs that work well with chain lightning (without being built around it)?
Plan the VFX/audio specs for chain lightning specifically?
