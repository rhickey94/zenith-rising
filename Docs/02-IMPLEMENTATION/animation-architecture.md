# Animation Architecture: Custom FSM + AnimationPlayer

> **Status:** ✅ CURRENT  
> **Last Updated:** 2025-10-20  
> **Dependencies:** Player.cs, AnimationPlayer, skill-standardization.md

## Decision Summary

**Chosen Approach:** Custom Finite State Machine in Player.cs + AnimationPlayer

**Rejected Approach:** AnimationTree

**Date Decided:** Session 10

---

## Why Custom FSM Over AnimationTree?

### The Problem
Need to integrate combat animations with gameplay mechanics (hitboxes, damage timing, skill execution) for ability-based combat (Whirlwind, Leap Slam, Chain Lightning, etc.).

### AnimationTree Approach (Rejected)
- Creates visual state machine (AnimationTree) + game logic state machine (code)
- Two sources of truth that must be constantly synchronized
- Animation state changes require mirroring in game code
- Complex for sprite-based abilities without skeletal rigging
- Gameplay logic (cooldowns, resources, targeting) must live in code anyway

### Custom FSM Approach (Chosen)
- Single state machine tracks both gameplay state AND animation state
- State machine validates rules, plays animations, executes logic - all in one place
- AnimationPlayer controlled directly via code
- Call Method tracks trigger gameplay events at precise frames
- No synchronization needed - one authoritative source

---

## Architecture Components

### 1. State Machine (Player.cs)

**PlayerState Enum:**
```csharp
public enum PlayerState
{
    Idle,           // Standing still
    Running,        // Moving
    BasicAttacking, // Left-click melee attack
    CastingSkill,   // Using Q/E/R skill
    Hurt,           // Taking damage (interrupt)
    Dead            // Death state
}
```

**State Transition Logic:**
- `CanTransitionTo(PlayerState)` - Validates legal transitions
- `ChangeState(PlayerState)` - Updates state, executes exit/enter logic
- `TryBasicAttack()` - Requests basic attack state
- `TryCastSkill(Skill)` - Requests skill cast state
- `OnAnimationFinished(string)` - Returns to locomotion state

**Transition Rules:**
- Idle/Running ↔ freely transition based on movement input
- Idle/Running → BasicAttacking/CastingSkill (if conditions met)
- BasicAttacking/CastingSkill → Idle/Running (on animation finish)
- Hurt/Dead can interrupt any state (high priority)

### 2. AnimationPlayer (player.tscn)

**Locomotion Animations:**
- walk_down, walk_up, walk_left, walk_right (0.9s, loop)
- idle_down, idle_up, idle_left, idle_right (0.1s, no loop)

**Combat Animations:**
- warrior_attack_down/up/left/right (0.3s, no loop)
- warrior_whirlwind (0.8s, no loop)
- (Future: mage_cast, ranger_shoot, etc.)

**Animation Tracks:**
1. **Value Track:** Keyframes `Sprite2D:region_rect` to show atlas frames
2. **Call Method Track:** Invokes Player methods at specific frames
   - `EnableMeleeHitbox()` at frame when attack visually connects
   - `DisableMeleeHitbox()` when attack animation ends
   - `ExecuteSkillEffect()` when cast animation completes

### 3. Sprite2D (player.tscn)

**Setup:**
- Texture: warrior.png (576x832 atlas)
- Region Enabled: true
- Region Rect: Animated via AnimationPlayer

**Atlas Layout:**
- Row 512 (y=512): walk_up frames (9 frames, 64x64 each)
- Row 576 (y=576): walk_left frames
- Row 640 (y=640): walk_down frames
- Row 704 (y=704): walk_right frames

**Frame Animation:**
AnimationPlayer keyframes region_rect property:
- Frame 0.0s: `Rect2(0, 640, 64, 64)` (first frame)
- Frame 0.1s: `Rect2(64, 640, 64, 64)` (second frame)
- Frame 0.2s: `Rect2(128, 640, 64, 64)` (third frame)
- etc.

### 4. Hitboxes (player.tscn)

**Nodes:**
- BasicAttackHitbox (Area2D, disabled by default)
- AOEHitbox (Area2D, disabled by default)
- DashHitbox (Area2D, disabled by default)

**Control:**
- Enabled/disabled by AnimationPlayer Call Method tracks
- Positioned dynamically based on player facing direction
- Collision handlers in Player.cs apply damage directly

---

## Execution Flow Example: Basic Attack

```
1. Player presses left-click
   ↓
2. SkillManager.HandleInput() detects mouse button
   ↓
3. SkillManager.UseSkill(BasicAttackSkill)
   ↓
4. Player.TryBasicAttack() - Check if can attack
   ↓
5. CanTransitionTo(BasicAttacking)? → true
   ↓
6. ChangeState(BasicAttacking)
   ↓
7. AnimationPlayer.Play("warrior_attack_down")
   ↓
8. [Frame 0.1s] Call Method: EnableBasicAttackHitbox()
   ↓
9. Hitbox detects enemy collision
   ↓
10. OnBasicAttackHit(Enemy) - Apply damage
   ↓
11. [Frame 0.25s] Call Method: DisableBasicAttackHitbox()
   ↓
12. Animation finishes → OnAnimationFinished("warrior_attack_down")
   ↓
13. ChangeState(Idle or Running based on input)
```

---

## Integration with Movement

**_PhysicsProcess() Respects State:**
```csharp
public override void _PhysicsProcess(double delta)
{
    Vector2 direction = Vector2.Zero;

    // Only accept movement input in locomotion states
    if (_currentState == PlayerState.Idle || _currentState == PlayerState.Running)
    {
        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
    }
    else if (_currentState == PlayerState.CastingSkill)
    {
        // Block movement during skill cast
        direction = Vector2.Zero;
    }
    else if (_currentState == PlayerState.BasicAttacking)
    {
        // Allow movement during basic attack (responsive combat)
        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
    }

    Velocity = direction * _statsManager.CurrentSpeed;

    // Update animations only in locomotion states
    if (_currentState == PlayerState.Idle || _currentState == PlayerState.Running)
    {
        UpdateLocomotionAnimation(direction);
    }

    MoveAndSlide();
}
```

---

## Benefits

### For Development
- ✅ Single source of truth for character state
- ✅ Easy to trace execution (input → animation → gameplay)
- ✅ Adding abilities = add animation + state method
- ✅ No node graph complexity

### For Debugging
- ✅ State flow explicit in code (not visual graph)
- ✅ Can log state transitions
- ✅ Breakpoints work naturally
- ✅ No "which state machine is wrong?" confusion

### For Performance
- ✅ Simpler than AnimationTree node graph processing
- ✅ Direct animation playback
- ✅ No overhead from unused features

### For Iteration
- ✅ Fast to add new animations
- ✅ Easy to adjust hitbox timing (change Call Method frame)
- ✅ State machine refactorable in code (not locked in visual editor)

---

## Extensibility

### Adding New Entities
Each entity type gets own FSM tailored to its needs:

**Enemy.cs:**
```csharp
enum EnemyState { Idle, Chasing, Attacking, Hurt, Dead }
```

**Boss.cs:**
```csharp
enum BossState { Phase1Idle, Phase1Attack, Transitioning, Phase2Enraged, SpecialAttack, Dead }
```

State logic stays local to each entity - no shared generic component needed.

### Adding New Animations
1. Create animation in AnimationPlayer
2. Add state to PlayerState enum (if needed)
3. Add transition method (e.g., `TryCastNewSkill()`)
4. Call from SkillManager based on input

### Adding Call Method Events
AnimationPlayer can invoke any public method on Player:
- Hitbox control (enable/disable)
- Skill execution (spawn effect at key frame)
- Sound effects (play audio at impact frame)
- VFX triggers (spawn particles)
- Camera shake (on heavy attack)

---

## When to Use Each Approach

### Use Custom FSM When:
- ✅ Sprite-based animation (no skeletal rigging)
- ✅ Ability-based combat (skills with timing)
- ✅ Gameplay mechanics tied to animation frames
- ✅ State logic is complex and benefits from code

### Use AnimationTree When:
- Skeletal animation with blend trees
- Complex locomotion blending (walk → run → sprint)
- Inverse kinematics (IK)
- 3D character animation

**For 2D sprite-based action games: Custom FSM is simpler and more appropriate.**

---

## Implementation Phases

### Phase 1: Foundation ✅ COMPLETE
- Set up Sprite2D node with warrior.png atlas
- Configure AnimationPlayer
- Add animation node references to Player.cs

### Phase 2: Locomotion ✅ COMPLETE
- Create walk animations (4 directions)
- Create idle animations (4 directions)
- Implement locomotion animation control methods

### Phase 3: State Machine ✅ COMPLETE
- Add PlayerState enum
- Implement state transition logic (CanTransitionTo, ChangeState)
- Add public state request methods (TryBasicAttack, TryCastSkill)
- Update _PhysicsProcess to respect state

### Phase 4: Combat Animations ✅ COMPLETE
- Create warrior_attack animations (4 directions)
- Create warrior_whirlwind animation
- Add Call Method tracks for hitbox timing

### Phase 5: Hitboxes ⏳ IN PROGRESS
- Create hitbox nodes in player.tscn
- Add hitbox control methods to Player.cs
- Wire collision signals
- Test damage application

### Phase 6: Skill Integration 📝 PLANNED
- Add CastBehavior/DamageSource enums to Skill.cs
- Update SkillManager to route based on CastBehavior
- Configure warrior skill resources
- Refactor WhirlwindEffect → WhirlwindVisual

### Phase 7: Testing & Polish 📝 PLANNED
- Test all warrior skills
- Adjust animation timings
- Add visual effects
- Tune hitbox sizes

---

## Code Organization

### Player.cs Structure
```csharp
public partial class Player : CharacterBody2D
{
    // Enums
    public enum PlayerState { ... }

    // Fields - Animation
    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;
    private PlayerState _currentState = PlayerState.Idle;
    private Vector2 _lastDirection = Vector2.Down;

    // Fields - Hitboxes
    private Area2D _meleeHitbox;
    private Area2D _aoeHitbox;
    private HashSet<Enemy> _hitEnemiesThisCast = new();
    private Skill _currentCastingSkill;

    // Lifecycle
    public override void _Ready() { ... }
    public override void _PhysicsProcess(double delta) { ... }

    // State Machine
    private bool CanTransitionTo(PlayerState newState) { ... }
    private void ChangeState(PlayerState newState) { ... }
    public bool TryBasicAttack() { ... }
    public bool TryCastSkill(Skill skill) { ... }

    // Animation Control
    private void PlayWalkAnimation(Vector2 direction) { ... }
    private void PlayIdleAnimation(Vector2 direction) { ... }
    private string GetDirectionalAnimationName(string base, Vector2 dir) { ... }
    private void OnAnimationFinished(StringName animName) { ... }

    // Hitbox Control
    public void EnableMeleeHitbox() { ... }
    public void DisableMeleeHitbox() { ... }
    public void EnableAOEHitbox() { ... }
    public void DisableAOEHitbox() { ... }

    // Collision Handlers
    private void OnMeleeHitboxBodyEntered(Node2D body) { ... }
    private void OnAOEHitboxBodyEntered(Node2D body) { ... }
    private void ApplyHitboxDamage(Enemy enemy) { ... }
}
```

---

## References

- **Skill Integration:** [skill-standardization.md](skill-standardization.md)
- **Godot Patterns:** [godot-patterns.md](godot-patterns.md)
- **Phase Plan:** [phase-plan.md](phase-plan.md)
- **Implementation:** [../../Scripts/Player/Player.cs](../../Scripts/Player/Player.cs)
- **Scene:** [../../Scenes/Player/player.tscn](../../Scenes/Player/player.tscn)

---

*This architecture provides a clean, maintainable, and extensible animation system for 2D sprite-based action combat.*
