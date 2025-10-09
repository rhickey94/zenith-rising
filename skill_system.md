🎮 Skill System Architecture Guide

  Overview

  The skill system is built on a three-component architecture that separates data, behavior, and effects. This enables skill variants,
  data-driven balancing, and reusable components.

  ---
  🏗️ Architecture Components

  1️⃣ Skill Resources (Data Layer)

  Purpose: Define what a skill is (stats, values, metadata)

  Location: Scripts/Skills/SkillTypes/

  Pattern: Godot Resources that inherit from base Skill class

  Example:
  // ProjectileSkill.cs - Defines data for projectile-based skills
  public partial class ProjectileSkill : Skill
  {
      [Export] public float DirectDamage { get; set; } = 40f;
      [Export] public float ExplosionDamage { get; set; } = 25f;
      [Export] public float ExplosionRadius { get; set; } = 100f;
  }

  Created in Godot as .tres files:
  - Resources/Skills/Mage/Fireball.tres (type: ProjectileSkill)
  - Resources/Skills/Mage/Iceball.tres (type: ProjectileSkill, different values)

  ---
  2️⃣ Skill Executors (Behavior Layer)

  Purpose: Define what happens when you press the button

  Location: Scripts/Skills/{Class}/{Category}/

  Pattern: Classes implementing ISkillExecutor interface

  Example:
  // Fireball.cs - Spawns and initializes a fireball projectile
  public class Fireball : ISkillExecutor
  {
      public void ExecuteSkill(Player player, Skill baseSkill)
      {
          // 1. Cast to specific skill type
          var skill = baseSkill as ProjectileSkill;

          // 2. Calculate aim direction
          Vector2 direction = (player.GetGlobalMousePosition() - player.GlobalPosition).Normalized();

          // 3. Spawn effect from skill's scene
          var fireball = skill.SkillEffectScene.Instantiate<FireballProjectile>();

          // 4. Initialize with skill data
          fireball.Initialize(direction, player.GlobalPosition,
              skill.DirectDamage, skill.ExplosionDamage, skill.ExplosionRadius);

          // 5. Add to scene
          player.GetTree().Root.AddChild(fireball);
      }
  }

  Responsibility: Spawn command only—no ongoing behavior logic

  ---
  3️⃣ Skill Effects (Implementation Layer)

  Purpose: Define how the spawned entity behaves

  Location: Scripts/Effects/

  Pattern: Godot nodes (Area2D, Node2D, etc.) with custom scripts

  Example:
  // FireballProjectile.cs - Projectile that travels and explodes
  public partial class FireballProjectile : Area2D
  {
      private float _directDamage;
      private float _explosionDamage;
      private float _explosionRadius;

      public void Initialize(Vector2 direction, Vector2 startPos,
          float directDmg, float explosionDmg, float radius)
      {
          _directDamage = directDmg;
          _explosionDamage = explosionDmg;
          _explosionRadius = radius;
          // ... store parameters
      }

      public override void _PhysicsProcess(double delta)
      {
          // Move forward
          Position += _direction * Speed * (float)delta;

          // Explode at max range
          if (DistanceTraveled >= MaxRange)
              Explode();
      }

      private void Explode()
      {
          // Damage enemies in radius, spawn VFX, destroy self
      }
  }

  Responsibility: All ongoing behavior (movement, collision, lifetime, damage application)

  ---
  🔄 How They Work Together

  Execution Flow:

  Player presses Q
      ↓
  SkillManager calls Skill.Execute(player)
      ↓
  Skill.CreateExecutor() creates appropriate executor
      ↓
  Executor.ExecuteSkill(player, skill)
      ↓
  Executor spawns Effect with skill data
      ↓
  Effect handles its own behavior until destroyed

  Visual Diagram:

  ┌─────────────────┐
  │  Fireball.tres  │  (ProjectileSkill Resource)
  │  - DirectDmg: 40│
  │  - ExplosionDmg │
  │  - Radius: 100  │
  └────────┬────────┘
           │
           ↓ (passed to)
  ┌─────────────────┐
  │  Fireball.cs    │  (Executor)
  │  - Spawns       │
  │  - Initializes  │
  └────────┬────────┘
           │
           ↓ (creates)
  ┌─────────────────┐
  │FireballProj.cs  │  (Effect)
  │  - Travels      │
  │  - Explodes     │
  │  - Damages      │
  └─────────────────┘

  ---
  🎯 Class System Integration

  Skills are restricted to specific classes:

  // In Skill Resource
  [Export] public PlayerClass AllowedClass { get; set; }  // Warrior, Mage, Ranger
  [Export] public SkillSlot Slot { get; set; }            // Primary (Q), Secondary (E), Ultimate (R)

  SkillManager validates skills on equip:
  - Fireball (Mage, Primary) can only be equipped by Mage in Q slot
  - Whirlwind (Warrior, Primary) rejected if player is Mage

  ---
  📂 File Organization

  Scripts/Skills/
  ├── Base/
  │   ├── Skill.cs                    # Base class (common fields)
  │   └── ISkillExecutor.cs           # Executor interface
  │
  ├── SkillTypes/                     # Skill data subclasses
  │   ├── InstantAOESkill.cs         # For Whirlwind-like skills
  │   └── ProjectileSkill.cs         # For Fireball-like skills
  │
  ├── Warrior/Offensive/              # Executors organized by class
  │   └── Whirlwind.cs
  │
  └── Mage/Offensive/
      └── Fireball.cs

  Scripts/Effects/                    # Effect implementations
  ├── WhirlwindEffect.cs             # Spinning AOE damage visual
  └── FireballProjectile.cs          # Traveling explosive projectile

  Resources/Skills/                   # Skill data files (.tres)
  ├── Warrior/
  │   └── Whirlwind.tres             # (InstantAOESkill type)
  └── Mage/
      ├── Fireball.tres              # (ProjectileSkill type)
      └── Iceball.tres               # (ProjectileSkill type, different values!)

  ---
  🛠️ Adding a New Skill (Step-by-Step)

  Example: Add "Shield Block" (Warrior E skill)

  Step 1: Create or Reuse Skill Type

  // NEW: Scripts/Skills/SkillTypes/ShieldSkill.cs
  public partial class ShieldSkill : Skill
  {
      [Export] public float ShieldHP { get; set; } = 100f;
      [Export] public float Duration { get; set; } = 3f;
  }

  Step 2: Create Executor

  // NEW: Scripts/Skills/Warrior/Defensive/ShieldBlock.cs
  public class ShieldBlock : ISkillExecutor
  {
      public void ExecuteSkill(Player player, Skill baseSkill)
      {
          var skill = baseSkill as ShieldSkill;

          var shield = skill.SkillEffectScene.Instantiate<ShieldEffect>();
          shield.Initialize(skill.ShieldHP, skill.Duration);
          player.AddChild(shield);
      }
  }

  Step 3: Create Effect

  // NEW: Scripts/Effects/ShieldEffect.cs
  public partial class ShieldEffect : Node2D
  {
      private float _shieldHP;
      private float _duration;

      public void Initialize(float hp, float duration)
      {
          _shieldHP = hp;
          _duration = duration;
      }

      public override void _Process(double delta)
      {
          _duration -= (float)delta;
          if (_duration <= 0 || _shieldHP <= 0)
              QueueFree();
      }

      public void AbsorbDamage(float damage)
      {
          _shieldHP -= damage;
      }
  }

  Step 4: Register in Factory

  // In Skill.cs CreateExecutor()
  return SkillName switch
  {
      "Whirlwind" => new Whirlwind(),
      "Fireball" => new Fireball(),
      "Shield Block" => new ShieldBlock(),  // ADD THIS
      _ => null
  };

  Step 5: Create Resource in Godot

  1. Create Resources/Skills/Warrior/ShieldBlock.tres
  2. Set type to ShieldSkill
  3. Fill fields:
  SkillName: "Shield Block"
  AllowedClass: Warrior
  Slot: Secondary
  ShieldHP: 100
  Duration: 3.0
  SkillEffectScene: [Drag ShieldEffect.tscn]

  Step 6: Create Effect Scene

  1. Create Scenes/Effects/ShieldEffect.tscn (Node2D root)
  2. Attach ShieldEffect.cs script
  3. Add visual children (Sprite2D, particles, etc.)

  ---
  🎨 Key Design Decisions

  ✅ Why Skill Subclasses?

  - Type Safety: ProjectileSkill.DirectDamage vs. generic Skill.EffectValue
  - Self-Documenting: Clear what each field means
  - CSV Import Safe: Named columns validate against real fields
  - Inspector Clarity: Only shows relevant fields per skill type

  ✅ Why Separate Executors?

  - Reusability: Fireball and Iceball can share ProjectileExecutor
  - Testability: Easy to unit test executor logic
  - SRP: Data (Resource) separated from behavior (Executor)

  ✅ Why Effect Scripts?

  - Consistency: All skills follow spawn → effect pattern
  - Encapsulation: Effect owns its behavior (move, collide, damage)
  - Reusability: Effects can be spawned by other systems (enemies, traps)

  ---
  🧪 Testing a Skill

  1. Set player class in Player.tscn (CurrentClass = Mage)
  2. Equip skill in SkillManager (PrimarySkill = Fireball.tres)
  3. Run game (F5)
  4. Press Q to activate
  5. Check console for debug prints (damage dealt, hit counts)

  ---
  🔮 Future Extensions

  Skill Variants (Already Supported!)

  Fireball.tres (orange, 40 damage)
  Iceball.tres (blue, 30 damage, larger radius)
  PoisonBall.tres (green, 20 damage, DOT effect)

  All use same ProjectileExecutor + FireballProjectile effect!

  Upgrade System Integration

  Upgrades can modify skill resources:
  // Upgrade: "Fireball +10 Damage"
  fireballSkill.DirectDamage += 10;

  Skill Mastery Tracking

  Track kills per skill:
  // In effect when enemy dies
  SkillMasteryTracker.RecordKill(skill.SkillName);

  ---
  📝 Common Patterns

  Pattern 1: Instant Effect (Whirlwind)

  Executor → Spawn effect → Effect damages immediately → Destroy after visual

  Pattern 2: Projectile (Fireball)

  Executor → Spawn projectile → Projectile travels → Collision triggers damage → Destroy

  Pattern 3: Buff/State (Shield - future)

  Executor → Add component to player → Component intercepts damage → Remove after duration

  ---
  🐛 Troubleshooting

  Skill doesn't cast:
  - Check SkillName matches factory in Skill.CreateExecutor()
  - Verify AllowedClass matches player class
  - Ensure SkillEffectScene is assigned

  Effect doesn't spawn:
  - Check executor calls skill.SkillEffectScene.Instantiate<T>()
  - Verify effect scene has script attached
  - Check AddChild() or GetTree().Root.AddChild()

  Damage not working:
  - Ensure Initialize() parameters are passed correctly
  - Check enemies are in "enemies" group
  - Verify collision layers/masks on effect Area2D

  ---
  📚 Key Files Reference

  | File                 | Purpose                                   |
  |----------------------|-------------------------------------------|
  | Skill.cs             | Base class, defines common fields         |
  | ISkillExecutor.cs    | Interface for all executors               |
  | SkillManager.cs      | Handles input, cooldowns, executes skills |
  | {SkillName}Skill.cs  | Data subclass for specific skill type     |
  | {SkillName}.cs       | Executor logic (spawning)                 |
  | {SkillName}Effect.cs | Effect behavior (movement, damage, etc.)  |

  ---
  That's the skill system! The architecture is designed to scale from 6 skills to 50+ while keeping code organized, testable, and
  data-driven.
