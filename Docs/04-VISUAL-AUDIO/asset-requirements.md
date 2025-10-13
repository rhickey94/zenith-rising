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

**Character Sprites** (Top Priority)
- Player character (1 per class × 3 classes)
- Animation states: Idle, walk, attack, death
- Style: Top-down, 32x32 or 64x64
- **Recommended:** Kenney's Top-Down Shooter Pack (FREE)
  - Link: https://kenney.nl/assets/top-down-shooter
  - 580+ assets including characters, weapons, objects
  - CC0 licensed

**Enemy Sprites**
- 3-5 enemy types minimum
- Boss variants (larger, distinct)
- **Recommended:** PenUsbMic's Sci-Fi Character Packs (FREE samples)
  - Pack 10: https://penusbmic.itch.io/sci-fi-character-pack-10
  - Pack 12: https://penusbmic.itch.io/sci-fi-character-pack-12
  - 1 FREE robot per pack, paid versions $2.50 each
  - Perfect metallic sci-fi aesthetic

**UI Elements**
- Health bars, XP bars
- Buttons (normal, hover, pressed states)
- Panels and borders
- Icons (skills, stats, resources)
- **Recommended:** SunGraphica's FREE Sci-Fi UI
  - Link: https://sungraphica.itch.io/free-sci-fi-ui
  - Complete UI kit with PSD/PNG/Vector
  - CC-BY license (credit required)

### 🟡 Important (Phase 2-3)

**Weapon Sprites**
- Swords, guns, staves
- Visual distinction per class
- **Recommended:** Kenney Top-Down Shooter (included in pack above)

**Projectile Sprites**
- Bullets, arrows, energy bolts
- **Current Solution:** Use particle pack circles/stars
- **Upgrade Option:** Kenney pack has dedicated projectiles

**Loot/Item Sprites**
- Gear drops (weapon, armor, accessory icons)
- Materials (energy cores, mod chips)
- Gold coins
- **Temporary:** Use colored squares with icons
- **Upgrade:** Kenney Game Assets or custom commission

### 🟢 Nice-to-Have (Phase 3+)

**Tileset - Metallic Sci-Fi**
- Industrial metal floors/walls
- Tech panels, machinery
- **Recommended:** Atomic Realm Industrial Tileset
  - Link: https://atomicrealm.itch.io/industrial-tileset
  - FREE sample, full version $9.99
  - Metallic sci-fi aesthetic (perfect match)
  - Animated tiles available

**Sound Effects**
- Weapon fire, explosions, hits
- UI clicks, level-up fanfare
- Footsteps, ambient
- **Recommended:** Kenney Audio Packs (FREE, CC0)

**Music**
- Combat theme
- Boss theme
- Hub/menu music
- **Recommended:** OpenGameArt or commission

---

## Recommended Free Asset Packs

### Essential Downloads (Do These First)

1. **Kenney Top-Down Shooter** ⭐⭐⭐⭐⭐
   - Link: https://kenney.nl/assets/top-down-shooter
   - Cost: FREE (CC0)
   - What: 580 assets (characters, weapons, tiles, objects)
   - Why: Covers 70% of sprite needs instantly

2. **PenUsbMic Sci-Fi Pack 10** ⭐⭐⭐⭐⭐
   - Link: https://penusbmic.itch.io/sci-fi-character-pack-10
   - Cost: FREE (1 robot), $2.50 for full pack
   - What: Animated robot enemies (perfect for our theme)
   - Why: Metallic sci-fi robots = exactly our aesthetic

3. **SunGraphica FREE Sci-Fi UI** ⭐⭐⭐⭐
   - Link: https://sungraphica.itch.io/free-sci-fi-ui
   - Cost: FREE (CC-BY, credit required)
   - What: Complete UI kit (PSD, PNG, Vector)
   - Why: Professional sci-fi UI instantly

### Secondary Recommendations

4. **0x72 16x16 Industrial Tileset**
   - Link: https://0x72.itch.io/16x16-industrial-tileset
   - Cost: FREE (no credit required)
   - What: Dark industrial tileset + character + enemies
   - Why: Good placeholder, includes enemies

5. **Atomic Realm Industrial Tileset** (Sample)
   - Link: https://atomicrealm.itch.io/industrial-tileset
   - Cost: FREE sample (full $9.99)
   - What: Metallic tiles, animated options
   - Why: Perfect aesthetic match for full game

---

## Asset Integration Guide

### Quick Start (This Week)

**Phase 1 Minimum:**
1. Download Kenney Top-Down Shooter
2. Extract to `Assets/kenney_topdown_shooter/`
3. Replace player square sprite with soldier sprite
4. Replace enemy squares with zombie/robot sprites
5. Test in game

**Phase 2 Additions:**
6. Download PenUsbMic Pack 10 (free robot)
7. Import robot as new enemy type
8. Set up animations (idle, attack, death)
9. Download SunGraphica UI pack
10. Replace HUD elements with styled UI

### Asset Workflow

**For Sprites:**
1. Import to `Assets/Sprites/[Category]/`
2. Set import settings: Pixel Art mode, No Filter
3. Create scenes in `Scenes/[Category]/`
4. Attach sprites to scene nodes
5. Test in game

**For UI:**
1. Import to `Assets/UI/`
2. Use StyleBoxFlat for buttons/panels
3. Load textures for icons
4. Follow visual-style-guide.md for colors

---

## Budget-Friendly Acquisition Strategy

### $0 Budget (MVP)
Use only free assets:
- Kenney packs (everything CC0)
- PenUsbMic free samples
- SunGraphica free UI
- 0x72 free tileset
- **Result:** Functional game with decent art

### $20 Budget (Polished MVP)
- Kenney Game Assets All-in-1 ($19.95)
  - 60,000+ assets (2D + 3D + Audio)
  - Lifetime access, all updates
  - Best value in game assets
- **Result:** Professional-looking MVP

### $50-100 Budget (Near-Commercial Quality)
- Kenney All-in-1 ($20)
- PenUsbMic full character packs ($2.50 × 3-5 classes)
- Atomic Realm Industrial Tileset ($10)
- Commission custom UI theme ($50)
- **Result:** Unique visual identity

### $500+ Budget (Commercial Polish)
- All above
- Custom character sprites ($100-200 per character)
- Custom weapon sprites ($50-100 set)
- Professional UI design ($200-300)
- Custom music ($500+)
- **Result:** Fully custom art direction

---

## Asset Style Guidelines

**Visual Identity:**
- Metallic sci-fi (Mass Effect / Dead Space inspired)
- NOT neon Tron or bright cyberpunk
- Industrial, worn, grounded
- See [`visual-style-guide.md`](visual-style-guide.md) for details

**Color Palette:**
- Primary: Steel Gray #2c3e50
- Secondary: Rust Orange #e67e22
- Accent: Cyan #1abc9c (alien tech)
- Backgrounds: Deep Black #0a0a0a

**Sprite Specifications:**
- Resolution: 32x32 or 64x64 (consistent per asset type)
- Format: PNG with transparency
- Style: Clean pixel art or low-poly 3D renders
- Animation: 4-8 frames per action

---

## Asset Priority by Phase

**Phase 1 (Current):**
- ✅ Particles (have Kenney pack)
- 🔴 Character sprites (use Kenney top-down)
- 🔴 Enemy sprites (use PenUsbMic + Kenney)
- 🟡 Basic UI (use SunGraphica)

**Phase 2:**
- 🟡 Weapon sprites
- 🟡 Loot icons
- 🟡 Polished UI
- 🟢 Sound effects

**Phase 3:**
- 🟢 Sci-fi tileset
- 🟢 More enemy variety
- 🟢 Music tracks

**Phase 4+:**
- Custom commissioned art
- Unique boss sprites
- Polish and juice

---

## License Compliance

**CC0 (Public Domain) - Kenney, 0x72:**
- ✅ Use commercially
- ✅ Modify freely
- ✅ No attribution required (but appreciated)

**CC-BY - SunGraphica:**
- ✅ Use commercially
- ✅ Modify freely
- ⚠️ Must credit "SunGraphica"

**PenUsbMic Assets:**
- ✅ Use commercially
- ✅ Credit "PenUsbMic" if desired (not required)
- ⚠️ Don't resell asset packs themselves

---

## Next Steps

**This Week:**
1. Download Kenney Top-Down Shooter
2. Download PenUsbMic Pack 10 (free)
3. Download SunGraphica UI Pack
4. Import to project
5. Replace placeholder sprites

**Related Documentation:**
- Visual style: [`visual-style-guide.md`](visual-style-guide.md)
- Current status: [`../../CLAUDE.md`](../../CLAUDE.md)

---

*Keep this updated as we acquire new assets.*
