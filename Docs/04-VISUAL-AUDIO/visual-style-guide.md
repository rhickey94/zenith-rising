Zenith Rising: Theme Summary for HUD/Menu Design (REVISED)
Quick reference for Claude Code to design UI elements that match the game's aesthetic and narrative.

Core Visual Identity
Theme: Derelict Space Station + Ancient Alien Spire (Hybrid Sci-Fi)
The Setup:
You're salvaging the Zenith Station, a corporate research facility built around an ancient alien structure called "The Spire." As you climb, human industrial tech gives way to alien architecture.
Visual Transition:

Floors 1-2: Industrial space station (human technology)
Floor 3: Transition zone (human + alien blending)
Floors 4-5: Ancient alien architecture (The Spire's interior)

Color Palette
Human Station Elements (Floors 1-2, Base UI)

Primary: Steel Gray #2c3e50
Secondary: Rust Orange #e67e22 (emergency lighting, warnings)
Accent: Warning Red #e74c3c (danger, low health)
Background: Deep Black #0a0a0a
Text: Light Gray/White #ecf0f1

Alien Spire Elements (Floors 3+, Special Effects)

Primary: Bronze/Copper #b87333 (alien metals)
Secondary: Iridescent Purple #9b59b6
Accent: Bioluminescent Cyan #1abc9c (energy, alien glow)
Background: Deep Blue #0c1445 (space-like)

Character-Specific Accent Colors

Marcus (Breacher): Red/Orange #e74c3c (aggression, fire)
Aria (Sharpshooter): Blue/White #3498db (precision, cold)
Elias (Conduit): Purple/Cyan #9b59b6 / #1abc9c (psionic, alien)

Design Aesthetic
Style: Metallic Sci-Fi (Mass Effect / Star Wars Inspired)
NOT:

❌ Neon Tron (no excessive glows)
❌ Cyberpunk (no hot pinks/magentas)
❌ Cartoony (no bright primaries)

YES:

✅ Industrial panels with rivets
✅ Holographic displays (subtle blue/cyan)
✅ Worn metal textures
✅ Semi-transparent dark overlays
✅ Clean sans-serif fonts (Segoe UI style)
✅ Warning stripes (yellow/black) for hazards
✅ Scan-line effects (subtle, for "monitor" feel)

UI Panel Design
Standard Panel Structure:
┌────────────────────────────┐
│ ╔══════════════════════╗ │ ← 2px steel blue border
│ ║ ║ │
│ ║ PANEL CONTENT ║ │ ← Dark semi-transparent bg
│ ║ ║ │ rgba(44, 62, 80, 0.95)
│ ╚══════════════════════╝ │
└────────────────────────────┘
Properties:

Border: 2px solid #4a90e2 (steel blue)
Corner Radius: 8px for large panels, 4px for small elements
Background: Dark gray with 95% opacity rgba(44, 62, 80, 0.95)
Drop Shadow: Subtle black shadow for depth
Padding: 16px internal spacing

HUD Layout (In-Game)
Top-Left Corner:
┌──────────────────────────┐
│ MARCUS KANE LVL 12 │ ← Character name + level
│ ████████████░░ 850/1000 │ ← Health bar (red fill)
│ ██████████░░░░ 450/600 │ ← XP bar (cyan fill)
└──────────────────────────┘
Elements:

Health Bar: Red gradient #e74c3c → #c0392b, with steel border
XP Bar: Cyan gradient #1abc9c → #16a085
Numbers: White text, right-aligned
Level Badge: Small circular badge with level number

Top-Right Corner:
┌──────────────────────────┐
│ 💰 1,450 Gold │ ← Resource counters
│ ⚡ 327 Essence │
│ 🔩 89 Scrap Metal │
└──────────────────────────┘
Elements:

Icons: Simple, monochrome glyphs
Numbers: Large, bold, white
Labels: Smaller, gray text

Top-Center:
┌──────────────────────────┐
│ FLOOR 3: THE SPIRE │ ← Current floor
│ Wave 5 / 10 │ ← Wave counter
│ Boss in 2:34 │ ← Timer (if applicable)
└──────────────────────────┘
Bottom-Center (Skills Bar):
┌────────────────────────────────────────┐
│ [Q] [E] [R] [SPACE] │ ← Key binds
│ ⚡ 🛡️ 💪 [ULTIMATE] │ ← Skill icons
│ 8.2s Ready 12.1s Ready │ ← Cooldowns
└────────────────────────────────────────┘
Cooldown Display:

Ready: Green glow #2ecc71
Cooling Down: Red text with circular progress indicator
Active: Yellow glow/pulse effect

Main Menu (Title Screen)
Simple Main Menu Structure:
╔════════════════════════════════════════╗
║ ║
║ Zenith Rising ║ ← Title (large, bold)
║ [Subtitle/Tagline] ║
║ ║
║ [START GAME] ║ ← Launch into hub
║ [CONTINUE] ║ ← If save exists
║ [SETTINGS] ║
║ [QUIT] ║
║ ║
║ Progress: Floor 3 | 847 Runs ║ ← Status bar
╚════════════════════════════════════════╝
Button Style:

Default: Dark gray #34495e, white text
Hover: Steel blue glow #4a90e2, slight scale up
Active: Orange highlight #e67e22
Size: Wide rectangular buttons, 60px height
Font: 18-20px, bold

Note: Character selection, workshop, treasury, etc. are accessed through the hub world, not from the main menu.

Hub World UI
Hub Overview (When Exploring Base):
┌──────────────────────────────────────┐
│ [ESC] Menu [TAB] Character │ ← Quick access keys
│ │
│ Available Buildings: │ ← Instructions/hints
│ • Portal (Start Mission) │
│ • Workshop (Refine Materials) │
│ • Treasury (Collect Gold) │
│ • Character Panel (Manage Gear) │
└──────────────────────────────────────┘
On-Screen Prompts When Near Buildings:
┌─────────────────┐
│ [E] WORKSHOP │ ← Appears when player near building
│ Materials: 3 │ ← Context info
└─────────────────┘
Building Interaction:

Walk up to building
Prompt appears: "[E] to enter"
Press E → Opens that building's menu overlay

Character Panel (NEW)
Character Stats & Gear Screen:
╔════════════════════════════════════════════════════╗
║ CHARACTER: MARCUS KANE ║
╠════════════════════════════════════════════════════╣
║ ║
║ ┌──────────────┐ STATS ║
║ │ │ ║
║ │ [CHARACTER │ Strength: 32 (+12) ║
║ │ PORTRAIT] │ Dexterity: 18 (+3) ║
║ │ │ Intelligence: 12 (+0) ║
║ │ │ Vitality: 25 (+8) ║
║ │ Level 15 │ Fortune: 14 (+2) ║
║ │ │ ║
║ └──────────────┘ Unspent Points: 6 ║
║ [ALLOCATE STATS] ║
║ ║
║ EQUIPPED GEAR COMBAT STATS ║
║ ┌─────────────┐ HP: 1240 ║
║ │ ⚔️ WEAPON │ Damage: 185 ║
║ │ Fusion Blade │ Atk Speed: 1.45 ║
║ │ +10 STR │ Crit: 12.5% ║
║ │ Enhancement │ Move Speed: 5.2 ║
║ │ +5 │ ║
║ └─────────────┘ [DETAILS] ║
║ ║
║ ┌─────────────┐ ┌─────────────┐ ║
║ │ 🛡️ ARMOR │ │ 💍 ACCESSORY│ ║
║ │ Combat Plate│ │ Shield Gen │ ║
║ │ +8 VIT │ │ +3 DEX │ ║
║ │ +3 STR │ │ +50 Shield │ ║
║ └─────────────┘ └─────────────┘ ║
║ ║
║ ┌─────────────┐ ║
║ │ ✨ RELIC │ ║
║ │ [EMPTY] │ ║
║ │ Click to │ ║
║ │ equip │ ║
║ └─────────────┘ ║
║ ║
║ [INVENTORY] [CLOSE] ║
╚════════════════════════════════════════════════════╝
Gear Slot Design:
Each Equipment Slot Shows:

Icon: Visual representation (sword, armor, ring, relic)
Name: Item name in white text
Primary Stat: Main bonus (e.g., "+10 STR")
Secondary Stats: Additional bonuses (smaller text)
Enhancement Level: "+5" badge if enhanced
Rarity Border: Color-coded (Common=gray, Uncommon=green, Rare=blue, Epic=purple, Legendary=orange)

Empty Slot:

Grayed out icon silhouette
"[EMPTY]" text
"Click to equip" prompt on hover

Hover State:

Slot highlights with steel blue glow
Tooltip expands showing full item details:

┌──────────────────────────┐
│ FUSION BLADE [RARE] │
│ Enhancement: +5 │
│ │
│ +10 Strength │
│ +3 Attack Speed │
│ │
│ Enchantments: │
│ • +15% Physical Damage │
│ • Attacks apply burn │
│ │
│ "Military-grade combat │
│ blade with integrated │
│ energy cells." │
└──────────────────────────┘
Stats Panel:
Left Side - Core Stats:

Display Format: "Strength: 32 (+12)"

Base value (white)
Bonus from gear (green, in parentheses)

Stat Descriptions on Hover:

┌────────────────────────────┐
│ STRENGTH │
│ +2% physical damage per pt │
│ +10 max HP per point │
└────────────────────────────┘
Unspent Points:

If player has points: Green glow, "[ALLOCATE STATS]" button pulses
Allocate Stats Button: Opens stat point distribution interface

Right Side - Combat Stats (Calculated):

Shows final computed values after all bonuses
These are read-only (informational)
Updates in real-time as gear changes

Character Portrait:

Position: Top-left of panel
Content: Character art or placeholder silhouette
Frame: Character-specific color (Marcus=red, Aria=blue, Elias=purple)
Level Badge: Circular badge overlaying bottom of portrait

Stat Allocation Interface (Sub-Panel):
╔═══════════════════════════════════════╗
║ ALLOCATE STAT POINTS ║
╠═══════════════════════════════════════╣
║ ║
║ Available Points: 6 ║
║ ║
║ Strength: 32 [+] [-] ║ ← +/- buttons
║ Intelligence: 18 [+] [-] ║
║ Agility: 12 [+] [-] ║
║ Vitality: 25 [+] [-] ║
║ Fortune: 14 [+] [-] ║
║ ║
║ [CONFIRM] [RESET] [CANCEL] ║
╚═══════════════════════════════════════╝
Functionality:

[+] Button: Add 1 point (consumes available points)
[-] Button: Remove 1 point (only works on points added this session)
[CONFIRM]: Apply changes permanently
[RESET]: Undo all changes this session
[CANCEL]: Close without saving

Inventory View (From Character Panel):
╔════════════════════════════════════════════════════╗
║ INVENTORY ║
╠════════════════════════════════════════════════════╣
║ ║
║ WEAPONS ║
║ ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐ ║
║ │⚔️ +5 │ │⚔️ +3 │ │⚔️ +0 │ │[EMPTY]│ ║
║ │ EQUIP│ │ │ │ │ │ │ ║
║ └──────┘ └──────┘ └──────┘ └──────┘ ║
║ ║
║ ARMOR ║
║ ┌──────┐ ┌──────┐ ┌──────┐ ║
║ │🛡️ +8 │ │🛡️ +2 │ │[EMPTY]│ ║
║ │ EQUIP│ │ │ │ │ ║
║ └──────┘ └──────┘ └──────┘ ║
║ ║
║ ACCESSORIES ║
║ ┌──────┐ ┌──────┐ ║
║ │💍 +3 │ │💍 +0 │ ║
║ │ EQUIP│ │ │ ║
║ └──────┘ └──────┘ ║
║ ║
║ RELICS ║
║ ┌──────┐ ┌──────┐ ║
║ │✨ Epic│ │[EMPTY]│ ║
║ │ EQUIP│ │ │ ║
║ └──────┘ └──────┘ ║
║ ║
║ [SALVAGE SELECTED] [SORT] [CLOSE] ║
╚════════════════════════════════════════════════════╝
Inventory Features:

Grid Layout: Items organized by type (Weapons, Armor, Accessories, Relics)
Item Cards: Show icon, enhancement level, quick stats
Selected Item: Blue glow border
Click Item:

If in gear slot: Unequip
If in inventory: Shows detailed tooltip
Double-click: Equip to appropriate slot (replaces current)

Salvage: Select items, press [SALVAGE SELECTED] → Convert to materials + gold
Sort Options: By rarity, by level, by type

Facility Menus (Triggered by Buildings in Hub)
Workshop Menu:
╔═══════════════════════════════════════════╗
║ WORKSHOP - Material Refinement ║ ← Header
╠═══════════════════════════════════════════╣
║ ║
║ REFINEMENT QUEUE ║
║ ┌─────────────┐ ┌─────────────┐ ║
║ │ REFINING │ │ REFINING │ ║ ← Active jobs
║ │ Essence │ │ Scrap Metal │ ║
║ │ █████░░░░░ │ │ ██░░░░░░░░░ │ ║ ← Progress bars
║ │ 12:34 left │ │ 24:18 left │ ║
║ │ [COLLECT] │ │ │ ║ ← Collect when done
║ └─────────────┘ └─────────────┘ ║
║ ║
║ ┌─────────────┐ ║
║ │ [EMPTY SLOT]│ ║ ← Available queue slot
║ │ Click to │ ║
║ │ start │ ║
║ └─────────────┘ ║
║ ║
║ AVAILABLE MATERIALS ║
║ • Essence Shards: 327 ║ ← Raw materials
║ • Scrap Metal: 89 ║
║ • Ancient Fragments: 12 ║
║ ║
║ SELECT MATERIAL TO REFINE: ║
║ [Essence Shards] [Scrap Metal] [Frag] ║ ← Start new job
║ ║
║ [COLLECT ALL] [CLOSE] ║
╚═══════════════════════════════════════════╝
Progress Bar Style:

Fill: Cyan gradient for active timers
Background: Dark gray #2c3e50
Border: Steel blue
Animated: Slow pulse/glow when near completion
Complete: Green fill, "[COLLECT]" button appears

Treasury Menu:
╔═══════════════════════════════════════════╗
║ TREASURY - Idle Gold Generation ║
╠═══════════════════════════════════════════╣
║ ║
║ ACCUMULATED GOLD ║
║ ┌──────────────────────────────┐ ║
║ │ │ ║
║ │ 💰 1,450 Gold │ ║ ← Big number
║ │ │ ║
║ │ Ready to collect! │ ║
║ │ [COLLECT GOLD] │ ║
║ │ │ ║
║ └──────────────────────────────┘ ║
║ ║
║ GENERATION RATE ║
║ • 50 gold per hour ║
║ • Based on Floor 5 cleared ║
║ • Cap: 400 gold (8 hours) ║
║ • Time until cap: 2h 15m ║
║ ║
║ UPGRADES ║
║ [Increase Rate] - 500g ║ ← Permanent upgrades
║ [Increase Cap] - 1000g ║
║ ║
║ [CLOSE] ║
╚═══════════════════════════════════════════╝
Visual:

Gold Counter: Large, prominent, animated counting when collected
Progress Bar: Shows time until cap reached
Upgrade Buttons: Disabled (grayed) if can't afford

Level-Up Screen (In-Run)
Upgrade Selection:
╔═══════════════════════════════════════════╗
║ LEVEL UP! ║
║ Choose an Upgrade ║
╠═══════════════════════════════════════════╣
║ ║
║ ┌──────────────┐ ┌──────────────┐ ║
║ │ +15% DAMAGE │ │ ARC LIGHTNING│ ║ ← 3 choices
║ │ │ │ +1 Chain │ ║
║ │ Your attacks │ │ Chains to 1 │ ║ ← Description
║ │ deal more │ │ more enemy │ ║
║ │ damage │ │ │ ║
║ │ │ │ │ ║
║ │ Common │ │ Common │ ║ ← Rarity
║ └──────────────┘ └──────────────┘ ║
║ ║
║ ┌──────────────┐ ║
║ │ ATTACKS CHAIN│ ║
║ │ TO NEARBY │ ║ ← Rare (glows)
║ │ All attacks │ ║
║ │ now chain to │ ║
║ │ 1 enemy │ ║
║ │ Rare ✨ │ ║
║ └──────────────┘ ║
╚═══════════════════════════════════════════╝
Rarity Visual Cues:

Common: Gray border #95a5a6
Uncommon: Green border #2ecc71
Rare: Blue border #3498db, subtle glow
Epic: Purple border #9b59b6, strong glow/pulse

Card Hover:

Scale up 5%
Border brightens
Background lightens slightly

Character Selection Screen (In Hub)
Accessed from Portal Building:
╔═══════════════════════════════════════════╗
║ SELECT OPERATIVE ║
╠═══════════════════════════════════════════╣
║ ║
║ ┌──────────┐ ┌──────────┐ ┌─────────┐ ║
║ │ MARCUS │ │ ARIA │ │ ELIAS │ ║ ← 3 classes
║ │ KANE │ │ ZHANG │ │ KHOURY │ ║
║ │ │ │ │ │ │ ║
║ │ [■■■■■] │ │ [■■■■■] │ │[■■■■■] │ ║ ← Character art
║ │ BREACHER │ │SHARPSHOOT│ │CONDUIT │ ║ ← Role
║ │ Melee │ │ Ranged │ │ Caster │ ║
║ │ Level 15 │ │ Level 12 │ │Level 18 │ ║ ← Current level
║ └──────────┘ └──────────┘ └─────────┘ ║
║ ║
║ MARCUS KANE - The Breacher ║ ← Description
║ Ex-military heavy weapons specialist. ║
║ Excels at close combat and tanking. ║
║ ║
║ Highest Floor: 3 | Runs: 47 ║ ← Stats for this char
║ ║
║ [SELECT] [VIEW CHARACTER] ║ ← Select or view details
╚═══════════════════════════════════════════╝
Selected Character:

Red/Orange glow for Marcus
Blue/White glow for Aria
Purple/Cyan glow for Elias
Border thickens, card elevates

[VIEW CHARACTER] Button:

Opens Character Panel for that character
Allows gear check before starting mission

Typography
Font Choices:

Headers: Bold sans-serif, 24-32px, all caps
Body Text: Regular sans-serif, 14-16px
Numbers: Monospace font for stats/resources
Buttons: Bold sans-serif, 16-18px

Recommended Fonts:

Primary: Segoe UI, Roboto, or similar clean sans-serif
Monospace: Consolas, Courier New for numbers
Avoid: Serif fonts, script fonts, overly stylized fonts

Visual Effects
Holographic Elements:

Color: Cyan #1abc9c or steel blue #4a90e2
Effect: Semi-transparent, scan lines (1px horizontal lines)
Animation: Slow flicker, subtle glow pulse
Usage: Data displays, terminal screens, map overlays

Warning/Alert States:

Low Health: Red pulse on health bar, screen edges red tint
Level Up: Golden flash, screen briefly brightens
Item Drop: Glow effect on item, beam of light
Boss Spawn: Red alert lights, klaxon sound, screen shake

Damage Numbers:

Normal Damage: White, floating upward
Critical Hit: Yellow/orange, larger, "CRIT!" text
Healing: Green, "+50 HP"
Skill Names: Brief popup when ability used

Iconography
Resource Icons:

Gold: Coin symbol or "$"
Essence Shards: Lightning bolt or crystal
Scrap Metal: Gear or wrench
Ancient Fragments: Geometric alien shape

Skill Icons:

Marcus: Sharp angles, shield shapes, red tones
Aria: Crosshairs, bullets, blue tones
Elias: Geometric patterns, energy waves, purple/cyan

Gear Slot Icons:

Weapon: Sword/gun silhouette
Armor: Chest plate/shield
Accessory: Ring/amulet
Relic: Mystical/alien artifact

Style: Simple, readable at small sizes, monochrome with accent color

Animation Principles
Timing:

UI Transitions: 200-300ms (fast but smooth)
Button Hover: 150ms
Panel Open/Close: 400ms with ease-out
Progress Bars: Smooth linear fill

Movement:

Menu Transitions: Slide in from side or fade
Popups: Scale from center with slight bounce
Notifications: Slide down from top, pause, slide up

Feedback:

Button Click: Brief scale down, then return
Selection: Immediate visual change (border/glow)
Error: Quick shake animation

Accessibility Considerations
Contrast:

White text on dark backgrounds (minimum 7:1 ratio)
Important info uses high-contrast colors
Avoid red/green only distinctions (use shapes/icons too)

Readability:

Minimum 14px font size
Clear hierarchy (headers > body > captions)
Sufficient spacing between elements

Color Blindness:

Don't rely solely on color to convey info
Use icons, text labels, patterns
Test with colorblind simulation

Key Design Principles

Clarity Over Flash: Information is always readable
Consistent Language: Steel blues, dark grays, orange accents
Worn Industrial Feel: Not pristine, shows age/damage
Subtle Alien Influence: Grows stronger on later floors
Responsive Feedback: Every action has clear visual response
Minimal Clutter: Only show what's necessary
Character Identity: UI elements reflect chosen class
Hub-Based Navigation: Buildings in hub trigger facility menus, not main menu buttons

Reference Mood:
Think:

Mass Effect (Normandy ship UI)
Dead Space (engineering panels)
Alien: Isolation (retro-future terminals)
The Expanse (functional space tech)

Avoid:

Tron (too neon)
Cyberpunk 2077 (too colorful/chaotic)
Borderlands (too cartoony)
Portal (too clean/sterile)

Quick Color Reference Card
HUMAN TECH:
Primary: #2c3e50 (steel gray)
Secondary: #e67e22 (rust orange)
Accent: #e74c3c (warning red)
Text: #ecf0f1 (light gray)

ALIEN TECH:
Primary: #b87333 (bronze)
Secondary: #9b59b6 (purple)
Accent: #1abc9c (cyan)

CHARACTERS:
Marcus: #e74c3c (red)
Aria: #3498db (blue)
Elias: #9b59b6 (purple)

UI ELEMENTS:
Borders: #4a90e2 (steel blue)
Backgrounds: rgba(44, 62, 80, 0.95)
Success: #2ecc71 (green)
Warning: #f39c12 (orange)
Error: #e74c3c (red)

RARITY COLORS:
Common: #95a5a6 (gray)
Uncommon: #2ecc71 (green)
Rare: #3498db (blue)
Epic: #9b59b6 (purple)
Legendary: #e67e22 (orange)

This revised summary includes the Character Panel with gear/stats display and clarifies that facility menus (Workshop, Treasury, Character Selection) are accessed through hub buildings, not the main menu!
