# Project Structure & Organization

> **Status:** ✅ CURRENT  
> **Last Updated:** 2025-10-20  
> **Dependencies:** Godot project structure, file naming conventions

## Current Folder Structure

```
SpaceTower/
├── Assets/
│   ├── kenney_particle-pack/        # VFX particles (complete)
│   ├── kenney_rpg-urban-pack/       # Tileset (placeholder use)
│   ├── Sprites/ (TODO)
│   ├── Audio/ (TODO)
│   └── Fonts/ (TODO)
│
├── Scenes/
│   ├── Core/
│   │   ├── hub.tscn                 # Safe zone with dungeon portal
│   │   └── dungeon.tscn             # Combat zone (wave/floor system)
│   ├── Player/
│   │   └── player.tscn              # Player character
│   ├── Enemies/
│   │   ├── enemy.tscn               # Base enemy
│   │   ├── fast_melee_enemy.tscn
│   │   ├── slow_ranged_enemy.tscn
│   │   └── boss.tscn
│   ├── Items/
│   │   └── experience_shard.tscn
│   ├── SkillEffects/
│   │   ├── fireball_projectile.tscn
│   │   ├── whirlwind_effect.tscn
│   │   └── melee_attack.tscn
│   └── UI/
│       ├── Menus/
│       │   ├── main_menu.tscn
│       │   └── floor_transition_panel.tscn
│       ├── Panels/
│       │   ├── level_up_panel.tscn
│       │   ├── stat_allocation_panel.tscn
│       │   ├── victory_screen.tscn
│       │   ├── death_screen.tscn
│       │   └── results_screen.tscn
│       └── hud.tscn
│
├── Scripts/
│   ├── Core/
│   │   ├── Hub.cs                   # Hub world management
│   │   ├── Dungeon.cs               # Combat/spawning controller
│   │   ├── DungeonPortal.cs         # Hub → Dungeon interaction
│   │   ├── CombatSystem.cs          # Damage calculations
│   │   ├── SaveManager.cs           # JSON save/load system
│   │   └── SaveData.cs              # Data structures
│   ├── PlayerScripts/
│   │   ├── Player.cs
│   │   └── Components/
│   │       ├── StatsManager.cs
│   │       ├── SkillManager.cs
│   │       └── UpgradeManager.cs
│   ├── Skills/
│   │   ├── Base/
│   │   │   ├── Skill.cs
│   │   │   └── ISkillExecutor.cs
│   │   ├── Data/
│   │   │   ├── ProjectileSkill.cs
│   │   │   ├── InstantAOESkill.cs
│   │   │   └── MeleeAttackSkill.cs
│   │   ├── Executors/
│   │   │   ├── ProjectileSkillExecutor.cs
│   │   │   ├── InstantAOESkillExecutor.cs
│   │   │   └── MeleeSkillExecutor.cs
│   │   └── Effects/
│   │       ├── SkillEffect.cs
│   │       ├── CollisionSkillEffect.cs
│   │       ├── FireballProjectile.cs
│   │       └── WhirlwindEffect.cs
│   ├── Enemies/
│   │   └── Base/
│   │       └── Enemy.cs
│   └── UI/
│       ├── Panels/
│       │   ├── LevelUpPanel.cs
│       │   ├── StatAllocationPanel.cs
│       │   ├── FloorTransitionPanel.cs
│       │   ├── VictoryScreen.cs
│       │   ├── DeathScreen.cs
│       │   └── ResultsScreen.cs
│       ├── HUD/
│       │   └── Hud.cs
│       └── Menus/
│           └── MainMenu.cs
│
├── Resources/
│   ├── Balance/
│   │   ├── balance_config.tres
│   │   └── skill_balance_database.tres
│   └── Skills/
│       ├── Mage/
│       │   ├── Fireball.tres
│       │   └── MageBasicAttack.tres
│       └── Warrior/
│           ├── Whirlwind.tres
│           └── WarriorBasicAttack.tres
│
└── Docs/                            # Documentation (see 00-START-HERE.md)
```

## File Naming Conventions

**Scenes (.tscn):**
- snake_case: `level_up_panel.tscn`
- Descriptive: `fast_melee_enemy.tscn` not `enemy2.tscn`

**Scripts (.cs):**
- PascalCase: `StatsManager.cs`
- Match node name when attached: `Player.cs` for `player.tscn`

**Resources (.tres):**
- PascalCase: `Fireball.tres`
- Organized by type and class

## Component Organization Pattern

**Current Approach (Phase 1):**
- Player has component scripts (StatsManager, SkillManager, UpgradeManager)
- Components are child nodes of Player
- Works well for single-character Phase 1

**Future Approach (Phase 2+):**
- Add singleton managers for shared state
- GameState (Autoload) for run-wide state
- PlayerProgression (Autoload) for persistent data
- See [`../01-GAME-DESIGN/systems-progression.md`](../01-GAME-DESIGN/systems-progression.md)

## Related Documentation

- **Godot patterns:** [`godot-patterns.md`](godot-patterns.md)
- **Skill system:** [`skill-system-architecture.md`](skill-system-architecture.md)
- **Current status:** [`../../CLAUDE.md`](../../CLAUDE.md)
