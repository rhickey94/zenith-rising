Skill Implementation Standardization Plan
Problem Statement
Before proceeding with animation implementation, we need to establish a consistent, plug-and-play approach for implementing all 18 planned skills across 3 classes. The current question: Should hitboxes/damage be in Player (animation-driven) or in Effects (entity-driven)? Answer: Both - use a hybrid approach based on skill mechanics.
Comprehensive Skill Inventory
Analyzed all 18 planned skills from class-abilities.md:
Marcus (Warrior) - 6 Skills
Fusion Cutter (Basic) - Melee swing, auto-aims, 5th hit cleaves
Breaching Charge (Special) - Dash forward, stun/knockback
Crowd Suppression (Q) - Ground slam 360° AOE
Fortify (E) - Deploy shield, block damage
Combat Stim (R) - Self-buff (attack/move speed)
Last Stand (Ultimate) - Invulnerability buff
Aria (Ranger) - 6 Skills
Precision Rifle (Basic) - Hitscan ranged
Charged Shot (Special) - Hold to charge, piercing
Tactical Grenade (Q) - Throw grenade, delayed explosion
Evasive Roll (E) - Dash + spawn decoy
Overwatch (R) - Precision mode buff
Killzone (Ultimate) - Spawn turret + self-buff
Elias (Mage) - 6 Skills
Psionic Pulse (Basic) - Projectile, stacking debuff
Psionic Wave (Special) - Cone pushback
Arc Lightning (Q) - Chain lightning
Void Rift (E) - Persistent zone (DOT/slow/lifesteal)
Architect's Blessing (R) - Attack chain buff
Singularity (Ultimate) - Gravity well + implosion
Standardization Framework
Core Principle: Two-Axis Classification
Every skill is classified along two independent axes:
Axis 1: Cast Behavior
Instant - Executes immediately when activated (no animation lock)
AnimationDriven - Requires cast animation, player locked during cast
Axis 2: Damage Source
PlayerHitbox - Damage dealt by player-attached Area2D hitboxes
EffectCollision - Damage dealt by spawned Effect's collision system
None - No damage (pure buffs/utility)
Implementation Matrix (6 Patterns)
Cast Behavior	Damage Source	Pattern Name	Examples	Implementation
AnimationDriven	PlayerHitbox	Melee Pattern	Fusion Cutter, Crowd Suppression, Whirlwind	Player hitbox enabled by animation track
AnimationDriven	EffectCollision	Cast-Spawn Pattern	Breaching Charge (dash melee)	Animation track spawns effect at key frame
AnimationDriven	None	Cast-Buff Pattern	Fortify (deployment animation)	Animation track applies buff at key frame
Instant	PlayerHitbox	Instant-Melee Pattern	(Rare, not used in current design)	Hitbox enabled immediately
Instant	EffectCollision	Projectile Pattern	Fireball, Psionic Pulse, Grenade	Existing system - executor spawns effect
Instant	None	Instant-Buff Pattern	Combat Stim, Overwatch	Existing system - BuffExecutor applies stats
Skill Type Classification
1. Melee Pattern (AnimationDriven + PlayerHitbox)
Skills: Fusion Cutter, Psionic Wave (cone) Flow:
Input → SkillManager.UseSkill()
  → Player.TryBasicAttack() / TryCastSkill()
  → Player state: CastingSkill, play animation
  → Animation frame 0.1s: Call Player.EnableMeleeHitbox()
  → Player.OnHitboxBodyEntered(): Apply damage via CombatSystem
  → Animation frame 0.25s: Call Player.DisableMeleeHitbox()
  → Animation finishes: Return to Idle/Running
Implementation:
Hitbox: Child Area2D in player.tscn, disabled by default
Damage: Player.cs calculates and applies directly
Visual: Optional - spawn visual effect via animation track
Effect scene: Purely visual (particles, sprites) - NO collision logic
2. Instant AOE Pattern (AnimationDriven + PlayerHitbox)
Skills: Whirlwind, Crowd Suppression Flow:
Input → SkillManager.UseSkill()
  → Player.TryCastSkill()
  → Player state: CastingSkill, play animation
  → Animation frame 0.1s: Call Player.EnableAOEHitbox()
  → Player.OnHitboxBodyEntered(): Track hit enemies, apply damage
  → Animation frame 0.15s: Call Player.SpawnVisualEffect() (optional)
  → Animation frame 0.7s: Call Player.DisableAOEHitbox()
  → Animation finishes: Return to Idle/Running
Implementation:
Hitbox: Circular Area2D child in player.tscn, disabled by default
Damage: Player.cs calculates damage, uses HashSet to prevent multi-hit
Visual: Spawn separate Node2D for spinning/slam visual
Effect scene: Purely visual (rotation animation, particles) - NO collision logic
3. Projectile Pattern (Instant + EffectCollision)
Skills: Fireball, Psionic Pulse, Precision Rifle, Arc Lightning, Tactical Grenade Flow:
Input → SkillManager.UseSkill()
  → Skill.Execute() immediately
  → Executor spawns Effect (existing system)
  → Effect handles: movement, collision, damage, lifetime
  → Effect.OnBodyEntered(): Apply damage, track kills
  → Effect.QueueFree() when done
Implementation:
Hitbox: Effect's own Area2D collision shape
Damage: Effect calculates via CalculateDamage() helper
Visual: Effect controls its own sprite/particles
NO changes to existing system - already works perfectly
4. Cast-Spawn Pattern (AnimationDriven + EffectCollision)
Skills: Breaching Charge (dash + hit) Flow:
Input → SkillManager.UseSkill()
  → Player.TryCastSkill()
  → Player state: CastingSkill, play dash animation
  → Animation moves player via position keyframes
  → Animation frame 0.2s: Call Player.ExecuteDashSkill()
  → ExecuteDashSkill() spawns hitbox effect that follows player
  → Effect handles collision/damage during dash
  → Animation finishes: Return to Idle/Running
Implementation:
Hitbox: Spawned Effect (Area2D) that follows player during dash
Damage: Effect calculates and applies
Visual: Effect includes dash trail, impact effects
Hybrid: Animation controls player movement, Effect handles collision
5. Buff Pattern (Instant or AnimationDriven + None)
Skills: Combat Stim (instant), Fortify (animated), Overwatch (instant), Architect's Blessing (instant), Last Stand (instant) Flow (Instant):
Input → SkillManager.UseSkill()
  → Skill.Execute() immediately
  → BuffExecutor spawns BuffEffect
  → BuffEffect modifies player stats (via StatsManager or direct)
  → BuffEffect persists for duration
  → BuffEffect removes stat changes on destruction
Flow (Animated - e.g., Fortify):
Input → SkillManager.UseSkill()
  → Player.TryCastSkill()
  → Player state: CastingSkill, play deploy animation
  → Animation frame 0.3s: Call Player.ExecuteBuffSkill()
  → ExecuteBuffSkill() spawns BuffEffect (same as instant)
  → Animation finishes: Return to Idle/Running
Implementation:
No hitbox needed
Damage: N/A
Visual: BuffEffect or shader on player
Stats: BuffEffect modifies StatsManager properties or UpgradeManager
6. Persistent Zone Pattern (Instant + EffectCollision)
Skills: Void Rift, Singularity, Killzone (turret) Flow:
Input → SkillManager.UseSkill()
  → Skill.Execute() immediately at cursor position
  → Executor spawns PersistentZoneEffect at location
  → Effect creates Area2D, monitors overlapping enemies
  → Effect applies continuous damage/effects in _Process()
  → Effect persists for duration or until destroyed
  → Effect.QueueFree() when duration expires
Implementation:
Hitbox: Effect's own Area2D with Monitoring enabled
Damage: Effect applies continuous damage to overlapping bodies
Visual: Effect manages its own visuals (zone sprite, particles)
Location: Spawned at cursor/target position, not attached to player
Skill Data Extensions
Add two new properties to base Skill.cs:
public enum CastBehavior
{
    Instant,           // Execute immediately
    AnimationDriven    // Requires animation with timing
}

public enum DamageSource
{
    PlayerHitbox,      // Damage from player-attached hitbox
    EffectCollision,   // Damage from spawned effect
    None              // No damage (buffs/utility)
}

[Export] public CastBehavior CastType { get; set; } = CastBehavior.Instant;
[Export] public DamageSource DamageType { get; set; } = DamageSource.EffectCollision;
[Export] public string AnimationName { get; set; } = "";  // For AnimationDriven skills
Usage in SkillManager.UseSkill():
private void UseSkill(Skill skill, ref float cooldownRemaining)
{
    if (cooldown checks fail) return;
    
    // Route based on cast behavior
    if (skill.CastType == CastBehavior.AnimationDriven)
    {
        // Request animation from player
        if (_player.TryCastSkill(skill))
        {
            cooldownRemaining = skill.Cooldown;
        }
    }
    else // CastBehavior.Instant
    {
        // Execute immediately (existing system)
        skill.Execute(_player);
        cooldownRemaining = skill.Cooldown;
    }
}
Player.TryCastSkill() uses DamageSource to determine execution:
public bool TryCastSkill(Skill skill)
{
    if (!CanTransitionTo(PlayerState.CastingSkill)) return false;
    
    ChangeState(PlayerState.CastingSkill);
    
    // Store skill for animation callbacks to access
    _currentCastingSkill = skill;
    
    // Play animation
    _animationPlayer.Play(skill.AnimationName);
    
    return true;
}

// Called by AnimationPlayer Call Method track
public void ExecuteCurrentSkillEffect()
{
    if (_currentCastingSkill == null) return;
    
    if (_currentCastingSkill.DamageSource == DamageSource.EffectCollision)
    {
        // Spawn effect using existing executor system
        _currentCastingSkill.Execute(this);
    }
    // If DamageSource.PlayerHitbox, damage already handled by hitbox collision
    // If DamageSource.None, BuffEffect already applied
}
Player Hitbox Standardization
Hitbox Nodes in player.tscn
Create standardized hitbox nodes as children of Player:
Player (CharacterBody2D)
├── Sprite2D
├── AnimationPlayer
├── CollisionShape2D (player body)
├── MeleeHitbox (Area2D, disabled)
│   └── CollisionShape2D (RectangleShape2D 60x40)
├── AOEHitbox (Area2D, disabled)
│   └── CollisionShape2D (CircleShape2D radius 150)
└── DashHitbox (Area2D, disabled)
    └── CollisionShape2D (CapsuleShape2D for dash trail)
Hitbox Control Methods in Player.cs
// Generic hitbox control
private Area2D _meleeHitbox;
private Area2D _aoeHitbox;
private Area2D _dashHitbox;
private HashSet<Enemy> _hitEnemiesThisCast = new();
private Skill _currentCastingSkill;

public void EnableMeleeHitbox()
{
    _hitEnemiesThisCast.Clear();
    UpdateMeleeHitboxPosition(); // Position based on _lastDirection
    _meleeHitbox.Monitoring = true;
}

public void DisableMeleeHitbox()
{
    _meleeHitbox.Monitoring = false;
}

public void EnableAOEHitbox()
{
    _hitEnemiesThisCast.Clear();
    _aoeHitbox.Monitoring = true;
}

public void DisableAOEHitbox()
{
    _aoeHitbox.Monitoring = false;
}

// Collision handlers
private void OnMeleeHitboxBodyEntered(Node2D body)
{
    if (body is not Enemy enemy) return;
    if (_hitEnemiesThisCast.Contains(enemy)) return;
    
    _hitEnemiesThisCast.Add(enemy);
    ApplyHitboxDamage(enemy);
}

private void OnAOEHitboxBodyEntered(Node2D body)
{
    if (body is not Enemy enemy) return;
    if (_hitEnemiesThisCast.Contains(enemy)) return;
    
    _hitEnemiesThisCast.Add(enemy);
    ApplyHitboxDamage(enemy);
}

private void ApplyHitboxDamage(Enemy enemy)
{
    if (_currentCastingSkill == null || _statsManager == null) return;
    
    // Get base damage from skill
    float baseDamage = GetSkillBaseDamage(_currentCastingSkill);
    
    // Calculate with stats
    float damage = CombatSystem.CalculateDamage(
        baseDamage,
        _statsManager,
        _currentCastingSkill.DamageType
    );
    
    enemy.TakeDamage(damage);
}

private float GetSkillBaseDamage(Skill skill)
{
    return skill switch
    {
        MeleeAttackSkill melee => melee.Damage,
        InstantAOESkill aoe => aoe.Damage,
        _ => 0f
    };
}
Effect Refactoring Guidelines
For Skills Using PlayerHitbox (Melee, Instant AOE)
REMOVE collision logic from Effect:
// OLD WhirlwindEffect.cs - REMOVE THIS
private void ApplyDamage()
{
    var enemies = GetTree().GetNodesInGroup("enemies");
    foreach (Node node in enemies)
    {
        if (node is Enemy enemy)
        {
            float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
            if (distance < _radius) { ... }
        }
    }
}
NEW WhirlwindVisual.cs - Pure visuals:
public partial class WhirlwindVisual : Node2D
{
    [Export] public float Duration = 0.8f;
    [Export] public float RotationSpeed = 10f;
    
    public override void _Ready()
    {
        // Just destroy after duration - no collision logic
        GetTree().CreateTimer(Duration).Timeout += QueueFree;
    }
    
    public override void _Process(double delta)
    {
        // Rotate for visual effect only
        Rotation += RotationSpeed * (float)delta;
    }
}
Effect scene becomes:
Node2D root (NOT Area2D)
Sprite/particles for visuals
No CollisionShape2D
No damage logic
For Skills Using EffectCollision (Projectiles, Zones)
KEEP existing implementation - no changes needed:
// FireballProjectile.cs - ALREADY CORRECT
public partial class FireballProjectile : CollisionSkillEffect
{
    // This pattern works perfectly - don't change it
    private void OnBodyEntered(Node2D body)
    {
        if (body is Enemy enemy)
        {
            float damage = CalculateDamage(_baseDamage);
            enemy.TakeDamage(damage);
            Explode();
        }
    }
}
Implementation Checklist Template
When implementing any new skill, follow this checklist:
1. Classify Skill
 Determine CastBehavior (Instant or AnimationDriven)
 Determine DamageSource (PlayerHitbox, EffectCollision, or None)
 Identify pattern from matrix (Melee, Projectile, Buff, etc.)
2. Create Skill Data
 Choose or create skill data class (MeleeAttackSkill, ProjectileSkill, etc.)
 Set CastType, DamageSource, AnimationName properties
 Create .tres resource with all properties configured
3. Animation (if AnimationDriven)
 Create animation in AnimationPlayer (warrior_attack_down, etc.)
 Add region_rect keyframes for sprite animation
 Add Call Method tracks for timing:
EnableHitbox() at damage frame
DisableHitbox() at end frame
ExecuteCurrentSkillEffect() if spawning effect mid-animation
4. Hitbox (if PlayerHitbox)
 Ensure appropriate hitbox exists in player.tscn (Melee, AOE, Dash)
 Connect hitbox BodyEntered signal to handler in Player._Ready()
 Test hitbox positioning and size
5. Effect (if EffectCollision or visual-only)
 Create Effect script (inherits SkillEffect or CollisionSkillEffect)
 Implement Initialize() to read skill data
 Implement behavior (movement, collision, damage, lifetime)
 Create effect scene with visuals
 Assign scene to skill .tres
6. Executor (usually reuse existing)
 Verify appropriate executor exists for skill type
 If new type needed, create executor following ProjectileSkillExecutor pattern
 Register in Skill.CreateExecutor() factory
7. Testing
 Skill casts correctly (animation plays or instant)
 Damage applied at correct frame/moment
 Cooldown works
 Visual effects spawn and look correct
 Mastery tracking increments
Skill Type Mapping (All 18 Skills)
Skill	Class	Pattern	CastBehavior	DamageSource	AnimationName	Notes
Fusion Cutter	Warrior	Melee	AnimationDriven	PlayerHitbox	warrior_attack_[dir]	5th hit cleave = track hit count
Whirlwind	Warrior	Instant AOE	AnimationDriven	PlayerHitbox	warrior_whirlwind	Circular hitbox
Breaching Charge	Warrior	Cast-Spawn	AnimationDriven	EffectCollision	warrior_dash	Dash with collision trail
Crowd Suppression	Warrior	Instant AOE	AnimationDriven	PlayerHitbox	warrior_slam	Ground slam animation
Fortify	Warrior	Cast-Buff	AnimationDriven	None	warrior_fortify	Shield deployment
Combat Stim	Warrior	Instant-Buff	Instant	None	-	Immediate stat boost
Last Stand	Warrior	Instant-Buff	Instant	None	-	Invulnerability buff
Precision Rifle	Ranger	Projectile	Instant	EffectCollision	-	Hitscan projectile
Charged Shot	Ranger	Projectile	AnimationDriven	EffectCollision	ranger_charge	Hold animation, release spawns
Tactical Grenade	Ranger	Projectile	Instant	EffectCollision	-	Grenade with delayed explosion
Evasive Roll	Ranger	Cast-Spawn	AnimationDriven	EffectCollision	ranger_roll	Dash + spawn decoy
Overwatch	Ranger	Instant-Buff	Instant	None	-	Precision mode buff
Killzone	Ranger	Projectile	Instant	EffectCollision	-	Spawn turret entity
Psionic Pulse	Mage	Projectile	Instant	EffectCollision	-	Stacking projectile
Psionic Wave	Mage	Melee	AnimationDriven	PlayerHitbox	mage_wave	Cone-shaped hitbox
Arc Lightning	Mage	Projectile	Instant	EffectCollision	-	Chain effect
Void Rift	Mage	Persistent Zone	Instant	EffectCollision	-	Zone at cursor
Architect's Blessing	Mage	Instant-Buff	Instant	None	-	Attack chain buff
Singularity	Mage	Persistent Zone	Instant	EffectCollision	-	Gravity well
Benefits of This Standardization
For Development
✅ Clear pattern selection - Look up skill in table, follow checklist
✅ Minimal code duplication - Reuse hitboxes, executors, patterns
✅ Predictable debugging - Each pattern has defined flow
✅ Fast iteration - Most skills reuse existing types
For Architecture
✅ Skill system unchanged - Executors/effects remain data-driven
✅ Animation system clean - FSM only manages state, not gameplay logic
✅ Separation of concerns - Collision source is explicit per skill
✅ No breaking changes - Existing projectile skills work as-is
For Future Skills
✅ Plug-and-play - New skills just pick pattern from matrix
✅ Extensible - Can add new patterns (e.g., Channeled skills)
✅ Documented - This plan serves as implementation guide
Implementation Order Recommendation
Phase 1: Foundation (Do First)
Add CastBehavior and DamageSource enums to Skill.cs
Update SkillManager.UseSkill() to route based on CastBehavior
Add hitbox control methods to Player.cs
Create hitbox nodes in player.tscn
Phase 2: Prove Pattern (Validate Approach)
Implement Fusion Cutter (Melee Pattern) - prove PlayerHitbox works
Implement Whirlwind (Instant AOE Pattern) - prove animation-driven AOE works
Test both extensively - ensure damage timing, animation flow correct
Phase 3: Complete Warrior (Build Out)
Breaching Charge (Cast-Spawn Pattern)
Crowd Suppression (reuse Instant AOE Pattern)
Combat Stim (Instant-Buff Pattern)
Fortify and Last Stand later (Phase 4+)
Phase 4: Ranger & Mage
Use established patterns from matrix
Most are Instant + EffectCollision (existing system works)
Only Psionic Wave needs new animation (Melee Pattern)
Success Criteria
✅ All 6 patterns from matrix are proven to work
✅ Fusion Cutter and Whirlwind work with animation-driven hitboxes
✅ Existing Fireball still works (Projectile Pattern unchanged)
✅ New skills can be implemented by following checklist
✅ No confusion about "which approach to use" - table provides answer
✅ Code is maintainable and documented
This standardization provides a comprehensive, coherent framework for implementing all planned skills with clear rules, minimal code duplication, and predictable behavior.
