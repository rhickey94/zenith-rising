# Project Structure & Organization

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
│   │   └── game.tscn                # Main game loop
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
│       ├── hud.tscn
│       ├── level_up_panel.tscn
│       └── main_menu.tscn
│
├── Scripts/
│   ├── Core/
│   │   ├── Game.cs                  # Main game controller
│   │   └── CombatSystem.cs          # Damage calculations
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
│       ├── Hud.cs
│       ├── LevelUpPanel.cs
│       └── MainMenu.cs
│
├── Resources/
│   ├── Skills/
│   │   ├── Mage/
│   │   │   └── Fireball.tres
│   │   └── Warrior/
│   │       └── Whirlwind.tres
│   └── Upgrades/ (TODO)
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
