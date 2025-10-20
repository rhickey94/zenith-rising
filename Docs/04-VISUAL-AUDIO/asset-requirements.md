# Asset Requirements & Resources

## Current Assets Inventory

### ✅ What We Have

**Kenney Particle Pack** (Complete VFX Library)
- Location: `Assets/kenney_particle-pack/`
- 70+ unique particle types (transparent + black background versions)
- Categories: Energy, Combat, Fire, Environmental, Trails, Symbols
- **Perfect for:** All visual effects needs
- License: CC0 (Public Domain)

**Kenney RPG Urban Pack** (Placeholder Tiles)
- Location: `Assets/kenney_rpg-urban-pack/`
- 330 individual tiles
- **Can use for:** Placeholder level geometry (recolor for sci-fi)
- **Not ideal:** Modern urban style (not sci-fi metallic)
- License: CC0 (Public Domain)

---

## Priority Asset Needs

### 🔴 Critical (Phase 1-2)

#### **Character Sprites** (Top Priority) ⭐⭐⭐⭐⭐

**PRIMARY RECOMMENDATION: Tatermand's Top-Down Sci-fi Shooter Characters 2.0**
- Link: https://opengameart.org/content/top-down-sci-fi-shooter-characters-20
- Cost: **100% FREE** (CC-BY-SA 3.0)
- What: 9 professional character classes in powered armor
- Style: Dark industrial sci-fi, exactly our aesthetic
- Classes include:
  - **Heavy** (perfect for Warrior/Breacher)
  - **Sniper** (perfect for Ranger/Sharpshooter)
  - **Engineer** (perfect for Mage/Conduit with recolor)
  - Plus 6 more bonus classes
- Format: PSD (layered, customizable) + PNG exports
- Quality: **Legendary** - universally praised as "best on OpenGameArt"
- **Why it's #1:** Free, professional quality, matches our theme perfectly, human soldiers

**See detailed comparison:** [`../05-RESEARCH/human-vs-robot-sprites.md`](../05-RESEARCH/human-vs-robot-sprites.md)

**Alternative Option: PenUsbMic Robots**
- If you prefer robots over humans
- Pack 10: https://penusbmic.itch.io/sci-fi-character-pack-10
- Cost: FREE for 1 robot, $7.50 for 3 class packs
- Style: Metallic sci-fi robots
- **See:** [`../05-RESEARCH/character-sprites-research.md`](../05-RESEARCH/character-sprites-research.md)

#### **Enemy Sprites**

**RECOMMENDED: Mix of sources**

1. **Tatermand's Mutant class** (from character pack above)
   - Use for corrupted enemies
   - FREE in same pack

2. **PenUsbMic Pack 10 FREE robot** 
   - Link: https://penusbmic.itch.io/sci-fi-character-pack-10
   - Cost: FREE (1 robot)
   - Use for robot enemies

3. **Kenney Top-Down Shooter zombies**
   - Link: https://kenney.nl/assets/top-down-shooter
   - Cost: FREE (CC0)
   - Use for basic enemies

#### **UI Elements**

**SunGraphica's FREE Sci-Fi UI** ⭐⭐⭐⭐
- Link: https://sungraphica.itch.io/free-sci-fi-ui
- Cost: FREE (CC-BY, credit required)
- What: Complete UI kit (PSD, PNG, Vector)
- Includes: Health bars, panels, buttons, borders, icons
- Why: Professional sci-fi UI, matches our aesthetic

---

## How to Use PSD Files

### What is a PSD?

**PSD = Photoshop Document**
- Adobe Photoshop's native format
- Contains **layers** (like transparent sheets stacked on top of each other)
- Each layer = one part (body, arms, legs, weapon, effects)
- You can show/hide, edit, or export individual layers

**Think of it like:**
```
Layer 5: Weapon (top)
Layer 4: Arms
Layer 3: Body
Layer 2: Legs
Layer 1: Shadow (bottom)
```

All layers together = complete character sprite

### Do I Need Photoshop?

**No!** Free alternatives work perfectly:

1. **GIMP** (FREE, recommended)
   - Download: https://www.gimp.org/
   - Opens PSD files
   - Cross-platform (Windows, Mac, Linux)
   - Full PSD support

2. **Photopea** (FREE, online)
   - Website: https://www.photopea.com/
   - No download needed
   - Works in browser
   - Opens PSD directly

3. **Krita** (FREE)
   - Download: https://krita.org/
   - Good PSD support
   - More painting-focused

**For Tatermand's sprites:** Use GIMP or Photopea

### How to Export PSD to PNG for Godot

#### Using Photopea (Easiest - No Install)

1. **Go to:** https://www.photopea.com/
2. **File → Open** → Select Tatermand's PSD
3. **You'll see:** Layers panel on right (Body, Arms, Legs, etc.)
4. **Select the character you want:**
   - Hide layers you don't need (click eye icon)
   - Show only the character you want (Heavy, Sniper, Engineer)
5. **Export:**
   - File → Export As → PNG
   - Name it: `warrior_idle.png`
6. **Repeat for each character class**

#### Using GIMP (More Control)

1. **Install GIMP** from https://www.gimp.org/
2. **File → Open** → Select PSD
3. **Layers panel shows all parts**
4. **Hide/show layers** by clicking eye icons
5. **Flatten when ready:**
   - Image → Flatten Image (merges visible layers)
6. **Export:**
   - File → Export As → PNG
   - Name: `warrior_idle.png`

### Step-by-Step: Tatermand Pack to Godot

**Step 1: Download Tatermand Pack** (5 min)
1. Visit https://opengameart.org/content/top-down-sci-fi-shooter-characters-20
2. Click "Download" (large PSD file ~20MB)
3. Extract ZIP to `Downloads/Tatermand/`

**Step 2: Open in Photopea** (2 min)
1. Go to https://www.photopea.com/
2. Drag PSD file into browser window
3. Wait for it to load (10-20 seconds)

**Step 3: Export Heavy Class (Warrior)** (5 min)
1. In Layers panel, find "Heavy" group
2. Hide all other character groups (click eye icons)
3. Show only Heavy layers
4. File → Export As → PNG
5. Name: `heavy_idle.png`
6. Save to `Assets/Sprites/Characters/Warrior/`

**Step 4: Repeat for Sniper (Ranger)** (5 min)
- Export as `sniper_idle.png`
- Save to `Assets/Sprites/Characters/Ranger/`

**Step 5: Repeat for Engineer (Mage)** (5 min)
- Export as `engineer_idle.png`
- Save to `Assets/Sprites/Characters/Mage/`

**Step 6: Import to Godot** (5 min)
1. Godot auto-imports PNGs
2. Select sprite in FileSystem
3. In Import tab: Filter = Nearest (for pixel art)
4. Reimport

**Total time: ~30 minutes to have all 3 class sprites ready**

### What if the PSD is Too Complex?

**Tatermand provides PNG exports too!**
- Look for "flat" or "PNG" version in the download
- These are pre-flattened, ready to use
- Just import directly to Godot

**Or use pre-made sprite sheets:**
- Some PSDs have sprite sheet layers
- Export the whole sheet
- Use in Godot's AnimatedSprite2D

---

## Recommended Free Asset Packs

### Essential Downloads (Do These First)

1. **Tatermand Top-Down Sci-fi Shooter Characters 2.0** ⭐⭐⭐⭐⭐
   - Link: https://opengameart.org/content/top-down-sci-fi-shooter-characters-20
   - Cost: FREE (CC-BY-SA 3.0)
   - What: 9 character classes, professional quality
   - Why: Perfect match for our theme, legendary quality

2. **PenUsbMic Sci-Fi Pack 10** (for enemies) ⭐⭐⭐⭐⭐
   - Link: https://penusbmic.itch.io/sci-fi-character-pack-10
   - Cost: FREE (1 robot), $2.50 for full pack
   - What: Animated robot enemies
   - Why: Perfect for enemy variety

3. **SunGraphica FREE Sci-Fi UI** ⭐⭐⭐⭐
   - Link: https://sungraphica.itch.io/free-sci-fi-ui
   - Cost: FREE (CC-BY, credit required)
   - What: Complete UI kit
   - Why: Professional sci-fi UI

4. **Kenney Top-Down Shooter** (backup/variety) ⭐⭐⭐⭐
   - Link: https://kenney.nl/assets/top-down-shooter
   - Cost: FREE (CC0)
   - What: 580 assets (soldiers, zombies, props)
   - Why: Backup sprites, enemy variety, props

### Secondary Recommendations

5. **0x72 16x16 Industrial Tileset**
   - Link: https://0x72.itch.io/16x16-industrial-tileset
   - Cost: FREE
   - What: Dark industrial tileset
   - Why: Good placeholder

6. **Atomic Realm Industrial Tileset** (Sample)
   - Link: https://atomicrealm.itch.io/industrial-tileset
   - Cost: FREE sample (full $9.99)
   - What: Metallic tiles, animated
   - Why: Perfect aesthetic match

---

## Asset Integration Guide

### Quick Start (This Week)

**Phase 1 Character Sprites:**
1. Download Tatermand pack
2. Open PSD in Photopea (or GIMP)
3. Export Heavy, Sniper, Engineer as PNGs
4. Import to `Assets/Sprites/Characters/`
5. Replace placeholder squares in player.tscn
6. Test in game

**Phase 1 Enemy Sprites:**
7. Download PenUsbMic Pack 10 (free robot)
8. Import robot PNG to `Assets/Sprites/Enemies/`
9. Use Tatermand's Mutant class for another enemy
10. Use Kenney zombies for basic enemies

**Phase 1 UI:**
11. Download SunGraphica UI pack
12. Extract to `Assets/UI/`
13. Replace HUD elements with styled panels
14. Apply sci-fi button textures

### Asset Workflow

**For PSD Sprites:**
1. Open in Photopea or GIMP
2. Export needed layers as PNG
3. Save to `Assets/Sprites/[Category]/`
4. Set Godot import: Filter = Nearest
5. Create AnimatedSprite2D in scenes

**For Ready-Made PNG:**
1. Import to `Assets/Sprites/[Category]/`
2. Set import: Pixel Art mode, No Filter
3. Attach to scene nodes
4. Test in game

**For UI:**
1. Import to `Assets/UI/`
2. Use StyleBoxTexture for textured panels
3. Load icons as Texture2D
4. Follow visual-style-guide.md for colors

---

## Budget-Friendly Acquisition Strategy

### $0 Budget (MVP) ← **RECOMMENDED TO START**
Use only free assets:
- **Tatermand characters** (9 classes FREE)
- PenUsbMic free samples (enemies)
- SunGraphica free UI
- Kenney packs (props, variety)
- **Result:** Professional-looking game, $0 spent

### $10 Budget (Add Robot Variety)
- Tatermand FREE
- PenUsbMic full packs ($2.50 × 3 = $7.50)
- Atomic Realm tileset sample (FREE) or full ($9.99)
- **Result:** Maximum variety, professional quality

### $20 Budget (Ultimate Free Assets + Premium Tools)
- All free assets above
- Kenney Game Assets All-in-1 ($19.95)
  - 60,000+ assets (2D + 3D + Audio)
  - Lifetime access
  - Best value in game assets
- **Result:** Never need another asset pack

### $50-100 Budget (Near-Commercial Quality)
- Kenney All-in-1 ($20)
- PenUsbMic character packs ($7.50)
- Atomic Realm tileset ($10)
- Custom UI commission ($50)
- **Result:** Unique visual identity

---

## Asset Style Guidelines

**Visual Identity:**
- Metallic sci-fi (Mass Effect / Dead Space inspired)
- NOT neon Tron or bright cyberpunk
- Industrial, worn, grounded
- Dark with cyan/orange accents

**Color Palette:**
- Primary: Steel Gray #2c3e50
- Secondary: Rust Orange #e67e22
- Accent: Cyan #1abc9c (alien tech)
- Backgrounds: Deep Black #0a0a0a

**Sprite Specifications:**
- Resolution: 64x64 for characters (can scale Tatermand sprites to this)
- Format: PNG with transparency
- Style: Detailed pixel art
- Animation: 4-8 frames per action

**Tatermand sprites are larger (~100px) but scale beautifully to 64x64 in Godot**

---

## Asset Priority by Phase

**Phase 1 (Current):**
- ✅ Particles (have Kenney pack)
- 🔴 Character sprites (**USE TATERMAND - download now!**)
- 🔴 Enemy sprites (Tatermand Mutant + PenUsbMic robot + Kenney zombies)
- 🟡 Basic UI (SunGraphica)

**Phase 2:**
- 🟡 Weapon sprites (Tatermand pack includes some)
- 🟡 Loot icons (can reuse from packs)
- 🟡 Polished UI
- 🟢 Sound effects (Kenney audio)

**Phase 3:**
- 🟢 Sci-fi tileset (Atomic Realm)
- 🟢 More enemy variety
- 🟢 Music tracks

**Phase 4+:**
- Custom commissioned art
- Unique boss sprites
- Polish and juice

---

## License Compliance

**CC0 (Public Domain) - Kenney:**
- ✅ Use commercially
- ✅ Modify freely
- ✅ No attribution required

**CC-BY-SA 3.0 - Tatermand:**
- ✅ Use commercially
- ✅ Modify freely
- ⚠️ **Must credit "Tatermand"** in game
- ⚠️ Share-alike (derivative sprite edits use same license)
- Example credit: "Character sprites by Tatermand (CC-BY-SA 3.0)"

**CC-BY - SunGraphica:**
- ✅ Use commercially
- ✅ Modify freely
- ⚠️ Must credit "SunGraphica"

**PenUsbMic Assets:**
- ✅ Use commercially
- ✅ Credit "PenUsbMic" optional (appreciated)
- ⚠️ Don't resell asset packs themselves

---

## Next Steps

**Right Now (30 minutes):**
1. **Download Tatermand pack** (5 min)
2. **Open in Photopea** (2 min) - https://www.photopea.com/
3. **Export Heavy, Sniper, Engineer** (15 min)
4. **Import to Godot** (5 min)
5. **Test with one character** (3 min)

**This Week:**
6. Download PenUsbMic Pack 10 (free robot)
7. Download SunGraphica UI pack
8. Replace all placeholder sprites
9. Test complete visual overhaul

**Related Documentation:**
- Human vs Robot comparison: [`../HUMAN-VS-ROBOT-SPRITES.md`](../HUMAN-VS-ROBOT-SPRITES.md)
- Visual style guide: [`visual-style-guide.md`](visual-style-guide.md)
- Current status: [`../../CLAUDE.md`](../../CLAUDE.md)

---

## Quick Reference: PSD to Godot Workflow

```
1. Download Tatermand PSD
   ↓
2. Open in Photopea (photopea.com)
   ↓
3. Hide unwanted layers (click eye icons)
   ↓
4. Show only character you want (Heavy/Sniper/Engineer)
   ↓
5. File → Export As → PNG
   ↓
6. Save to Assets/Sprites/Characters/[Class]/
   ↓
7. Godot auto-imports
   ↓
8. Set Filter: Nearest
   ↓
9. Use in AnimatedSprite2D
   ↓
10. Done!
```

**Total time:** 30 minutes for all 3 classes

---

*Updated with Tatermand as primary recommendation + PSD workflow guide*
