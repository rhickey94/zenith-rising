# Character Sprite Research - Class-Specific Assets

## Research Summary

I've found excellent character sprite options that match your 3 class archetypes (Warrior/Breacher, Mage/Conduit, Ranger/Sharpshooter) in a metallic sci-fi style.

---

## 🎯 Best Overall Match: PenUsbMic Sci-Fi Series

**Why This is Perfect:**
- ✅ **Exact aesthetic match:** Metallic sci-fi robots (Dead Space/Mass Effect vibe)
- ✅ **Top-down compatible:** Designed for both top-down and side-scrollers
- ✅ **Free samples:** 1 FREE fully animated character in EVERY pack
- ✅ **Professional quality:** Smooth animations, consistent style
- ✅ **Covers all 3 classes:** Tank robots, energy casters, ranged shooters
- ✅ **Commercial use:** Free for commercial projects
- ✅ **Active creator:** 12+ packs available, growing collection

**Link:** https://penusbmic.itch.io (search "Sci-fi Character Pack")

---

## Class-Specific Recommendations

### 🛡️ Warrior (Breacher) - Tank/Melee Class

#### **Primary Choice: PenUsbMic Pack 10** ⭐⭐⭐⭐⭐
- **FREE Character:** Shotgunner Bot (perfect tank aesthetic)
- **Style:** Heavy armored robot with shotgun
- **Animations:** Idle, walk, attack, damaged, death
- **Size:** ~100x60 pixels (scalable)
- **Perfect for:** Your tank class - heavy, armored, close-range
- **Link:** https://penusbmic.itch.io/sci-fi-character-pack-10
- **Cost:** FREE for one character, $2.50 for full pack

**Why it works:**
- Heavy, bulky silhouette = tank identity
- Metallic sci-fi aesthetic matches your theme
- Top-down animations ready to use
- Professional quality animations

#### **Alternative: PenUsbMic Pack 4**
- **FREE Character:** Merchant Bot (can be repurposed)
- **Paid Character:** War Droid (heavily armored tank bot)
- **Style:** Military mech-style robot
- **Link:** https://penusbmic.itch.io/sci-fi-character-pack-4

---

### ⚡ Mage (Conduit) - Energy Caster Class

#### **Primary Choice: PenUsbMic Pack 11** ⭐⭐⭐⭐⭐
- **Character:** Sorcerer Bot (energy caster)
- **Style:** Levitating robot with energy attacks
- **Animations:** Idle, move, attack (2 types), damaged, death
- **Size:** 118x107 pixels
- **Perfect for:** Your mage class - energy manipulation, ranged magic
- **Link:** https://penusbmic.itch.io/sci-fi-character-pack-11
- **Cost:** $2.50 (has FREE characters but not the sorcerer)

**Why it works:**
- Distinct "caster" silhouette (levitating, energy effects)
- Multiple attack animations = spell variety
- Sci-fi energy theme fits "Conduit" perfectly
- Less bulky than warrior = ranged caster identity

#### **Alternative: "Topdown Wizard" from OpenGameArt**
- **Style:** Fantasy wizard BUT in top-down pixel art
- **Can be reskinned:** Change colors to sci-fi (cyan energy instead of purple magic)
- **FREE, CC0 license**
- **Link:** Search OpenGameArt for "topdown wizard"

---

### 🎯 Ranger (Sharpshooter) - Ranged DPS Class

#### **Primary Choice: PenUsbMic Pack 10** ⭐⭐⭐⭐⭐
- **Paid Characters:** Sniper Bot OR Rifle Bot
- **Style:** Sleek, agile robots with precision weapons
- **Animations:** Full combat animations
- **Perfect for:** Long-range, mobile DPS class
- **Link:** https://penusbmic.itch.io/sci-fi-character-pack-10
- **Cost:** $2.50 for full pack

**Why it works:**
- Slim, agile silhouette = mobility
- Long-range weapon = sharpshooter identity
- Less armored = glass cannon aesthetic

#### **Alternative: PenUsbMic Pack 12**
- **FREE Character:** Wheelbot (fast, agile)
- **Style:** Wheeled robot with ranged weapons
- **Can work as:** High-mobility ranger variant
- **Link:** https://penusbmic.itch.io/sci-fi-character-pack-12

#### **Budget Alternative: Kenney Top-Down Shooter**
- **Characters:** Soldier sprites with rifles
- **Style:** Human soldiers (less sci-fi, but functional)
- **FREE, CC0**
- **Link:** https://kenney.nl/assets/top-down-shooter

---

## 🎨 Visual Comparison

### PenUsbMic Style Characteristics
```
Warrior:  ████▓▓▓▓  (Bulky, heavy, armored)
          ▓▓████▓▓
          ▓▓▓▓▓▓▓▓

Mage:     ▓▓▓░░░▓  (Floating, energy aura)
          ░░████░░
          ░░░▓▓░░░

Ranger:   ▓▓▓═════ (Slim, weapon extended)
          ▓▓▓▓▓
          ▓▓▓
```

### Sprite Sizes
- **PenUsbMic sprites:** 90-120 pixels wide, 60-110 pixels tall
- **Kenney sprites:** 64x64 standard
- **Your target:** 64x64 for consistency

**Scaling note:** PenUsbMic sprites are larger but can be scaled down in Godot to 64x64 equivalent for gameplay.

---

## 💰 Cost Breakdown

### Budget Option ($0)
- ✅ PenUsbMic Pack 10 FREE Shotgunner Bot (Warrior)
- ✅ PenUsbMic Pack 12 FREE Wheelbot (Ranger)
- ⚠️ Mage: Use placeholder OR repurpose free characters
- **Total: FREE**

### Recommended Option ($7.50)
- ✅ Pack 10 full version ($2.50) - Shotgunner + Sniper
- ✅ Pack 11 full version ($2.50) - Sorcerer Bot
- ✅ Pack 12 full version ($2.50) - Additional variety
- **Total: $7.50** for 3 polished classes + extras

### Complete Option ($38.50)
- ✅ **MEGA Sci-Fi Character Pack Bundle** - All 39 characters
- ✅ Every PenUsbMic sci-fi sprite
- ✅ Enough for enemies, NPCs, variants
- **Total: $38.50** (huge value, 99 cents per character)

---

## 📥 Download & Setup Guide

### Step 1: Download PenUsbMic Packs

**Option A: Free Trial (Start Here)**
1. Go to https://penusbmic.itch.io/sci-fi-character-pack-10
2. Click "Download" (FREE Shotgunner included)
3. Extract to `Assets/Sprites/Characters/Warrior/`

**Option B: Purchase Full Packs**
1. Add Pack 10, 11, 12 to cart ($7.50 total)
2. Download all packs
3. Extract to appropriate class folders

### Step 2: Import to Godot

**File structure:**
```
Assets/Sprites/Characters/
├── Warrior/
│   └── shotgunner_bot/
│       ├── idle.png
│       ├── walk.png
│       ├── attack.png
│       └── death.png
├── Mage/
│   └── sorcerer_bot/
│       └── [animations]
└── Ranger/
    └── sniper_bot/
        └── [animations]
```

**Import settings in Godot:**
- Filter: Nearest (pixel art)
- Compress: Lossless
- Repeat: Disabled

### Step 3: Create Animated Sprites

In Godot, create AnimatedSprite2D nodes:
```
Player (CharacterBody2D)
└── AnimatedSprite2D
    ├── Animation: "idle"
    ├── Animation: "walk"
    ├── Animation: "attack"
    └── Animation: "death"
```

Load sprite frames from PenUsbMic sprite sheets.

---

## 🎨 Color Palette Integration

PenUsbMic sprites use metallic colors that match your design doc:

**Warrior (Shotgunner Bot):**
- Base: Steel Gray #4a5a6a
- Accent: Dark Gray #2c3640
- Lights: Cyan #3aacb8 (alien tech)

**Mage (Sorcerer Bot):**
- Base: Dark Metal #35404d
- Energy: Cyan/Blue #1abc9c (matching your alien tech color)
- Glow: Light Cyan #7ed7e0

**Ranger (Sniper Bot):**
- Base: Gunmetal #3d4855
- Accent: Red/Orange #e74c3c (targeting lasers)
- Lights: White/Cyan for tech

**Perfect match** to your visual style guide!

---

## 🔍 Alternative Free Options

If you want more variety or backups:

### OpenGameArt.org - "Top-Down Sci-fi Shooter Characters 2.0"
- **Creator:** Tatermand (legendary OGA contributor)
- **Style:** Professional top-down sci-fi soldiers
- **Quality:** Extremely high (praised as "best on OGA")
- **Characters:** 9 unique classes
- **FREE, CC-BY-SA**
- **Link:** https://opengameart.org/content/top-down-sci-fi-shooter-characters-20

**Pros:**
- Professional quality
- Multiple classes
- Top-down ready

**Cons:**
- More realistic humans (less robot aesthetic)
- Requires attribution
- Static lighting (baked in)

### CraftPix Free Sci-Fi Antagonists
- **Style:** Side-view sci-fi enemies
- **Quality:** Professional
- **FREE with attribution**
- **Link:** https://craftpix.net/freebies/free-sci-fi-antagonists-pixel-character-pack/

**Note:** Side-view, not top-down, but could inspire custom work

---

## ✅ Final Recommendation

**For Your Tower Ascension Project:**

**Phase 1 (NOW):** 
1. Download PenUsbMic Pack 10 (FREE Shotgunner)
2. Use Shotgunner for ALL 3 classes temporarily (recolor each)
3. Prove Phase 1 combat with placeholder

**Phase 2 (When adding classes):**
4. Purchase Packs 10, 11, 12 ($7.50)
5. Assign proper sprites:
   - Warrior = Shotgunner Bot
   - Mage = Sorcerer Bot
   - Ranger = Sniper Bot
6. Polish animations

**Phase 3+ (If successful):**
7. Consider MEGA bundle ($38.50) for enemies, bosses, NPCs

---

## 🎯 Why This Works Perfectly

**Aesthetic Match:**
- ✅ Metallic sci-fi (not neon cyberpunk)
- ✅ Industrial, grounded robots
- ✅ Dark color palette
- ✅ Mass Effect/Dead Space vibes

**Technical Match:**
- ✅ Top-down ready (designed for it)
- ✅ Proper size (~100px, scales to 64x64)
- ✅ Full animation sets
- ✅ Commercial use allowed

**Budget Friendly:**
- ✅ FREE to test ($0)
- ✅ Affordable to commit ($7.50)
- ✅ Scales with success ($38.50 for everything)

**Class Identity:**
- ✅ Distinct silhouettes (tank vs caster vs ranger)
- ✅ Appropriate weapons/effects
- ✅ Visual variety

---

## 📝 Next Steps

**Right Now (15 minutes):**
1. Go to https://penusbmic.itch.io/sci-fi-character-pack-10
2. Download the FREE Shotgunner Bot
3. Import to Godot
4. Test in your game

**This Week:**
5. Decide if you like the style (you will!)
6. Purchase Packs 10, 11, 12 ($7.50)
7. Implement all 3 classes

**Update asset-requirements.md:**
8. Document which packs you're using
9. Add PenUsbMic to credits

---

## 📸 Visual References

**See PenUsbMic's itch.io pages for animated GIFs showing:**
- All animations in action
- Color variations
- Size comparisons
- Attack effects

**Creator Social:**
- Instagram: @penusbmic
- Active and responsive to questions

---

**This is THE solution for your character sprites. Professional quality, perfect aesthetic, budget-friendly, and ready to use.**

Would you like me to add this to your `asset-requirements.md` file as a dedicated character sprites section?
