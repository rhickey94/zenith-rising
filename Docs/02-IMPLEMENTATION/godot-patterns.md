# Godot-Specific Patterns & Best Practices

## Critical Godot Patterns

### Physics & Deferred Calls

**NEVER modify the physics tree during collision processing:**

```csharp
// ❌ WRONG - Will crash
private void OnBodyEntered(Node2D body)
{
    var explosion = ExplosionScene.Instantiate();
    AddChild(explosion);  // ERROR!
}

// ✅ CORRECT - Use CallDeferred
private void OnBodyEntered(Node2D body)
{
    var explosion = ExplosionScene.Instantiate();
    CallDeferred(MethodName.AddChild, explosion);  // Works!
}
```

**When to use CallDeferred:**
- Spawning/removing nodes in collision callbacks
- Changing collision shapes during physics
- Modifying scene tree from physics process

### Scene Instancing

**Standard Pattern:**
```csharp
[Export] public PackedScene EnemyScene;

public void SpawnEnemy(Vector2 position)
{
    var enemy = EnemyScene.Instantiate<Enemy>();
    enemy.GlobalPosition = position;
    AddChild(enemy);  // or GetTree().Root.AddChild(enemy)
}
```

**Where to add nodes:**
- `AddChild(node)` - Adds as child of current node
- `GetTree().Root.AddChild(node)` - Adds to scene root (persists across scenes)
- `GetParent().AddChild(node)` - Adds as sibling

### Signal Pattern

**Godot C# Signal Limitations:**
```csharp
// ❌ WRONG - Custom classes don't work
[Signal] public delegate void DataReadyEventHandler(MyCustomClass data);

// ✅ CORRECT - Use Godot types or Resources
[Signal] public delegate void DataReadyEventHandler(int value, Node source);
[Signal] public delegate void UpgradeChosenEventHandler(Upgrade resource);
```

**Connecting Signals:**
```csharp
// In _Ready()
enemy.Died += OnEnemyDied;

// Handler
private void OnEnemyDied(Enemy enemy)
{
    // Handle death
}
```

### Resource Pattern

**Using Resources for Data:**
```csharp
[GlobalClass]
public partial class Skill : Resource
{
    [Export] public string SkillName { get; set; }
    [Export] public float Damage { get; set; }
    [Export] public PackedScene EffectScene { get; set; }
}
```

**Loading Resources:**
```csharp
// Single resource
var skill = ResourceLoader.Load<Skill>("res://Resources/Skills/Fireball.tres");

// All resources in folder
var skills = new List<Skill>();
var dir = DirAccess.Open("res://Resources/Skills/");
foreach (var file in dir.GetFiles())
{
    if (file.EndsWith(".tres"))
        skills.Add(ResourceLoader.Load<Skill>($"res://Resources/Skills/{file}"));
}
```

---

## Architecture Patterns

### Singleton Pattern (Autoload)

**When to use:**
- Global game state (GameState)
- Persistent progression (PlayerProgression)
- Stateless utilities (CombatSystem)

```csharp
public partial class GameState : Node
{
    public static GameState Instance { get; private set; }
    
    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree();
            return;
        }
        Instance = this;
    }
}
```

**Add to Project Settings → Autoload:**
- Name: `GameState`
- Path: `res://Scripts/Core/GameState.cs`

### Component Pattern

**When to use:**
- Player-specific functionality
- Separating concerns (stats, skills, inventory)
- Single-class Phase 1 development

```csharp
// Player.cs
public partial class Player : CharacterBody2D
{
    private StatsManager _stats;
    private SkillManager _skills;
    
    public override void _Ready()
    {
        _stats = GetNode<StatsManager>("StatsManager");
        _skills = GetNode<SkillManager>("SkillManager");
    }
}
```

### Factory Pattern

**For spawning with type-based logic:**
```csharp
public ISkillExecutor CreateExecutor(Skill skill)
{
    return skill switch
    {
        ProjectileSkill => new ProjectileSkillExecutor(),
        InstantAOESkill => new InstantAOESkillExecutor(),
        MeleeAttackSkill => new MeleeSkillExecutor(),
        _ => null
    };
}
```

---

## Node Finding Best Practices

**Hierarchy:**
1. `GetNode<T>("NodeName")` - Own children (fast)
2. `%UniqueName` syntax - Unique nodes (safe for reorganization)
3. Groups - Collections and singletons
4. `[Export]` - Drag-and-drop references (best for complex UI)

**Examples:**
```csharp
// Own children
var stats = GetNode<StatsManager>("StatsManager");

// Unique node (set % in scene tree)
var hud = GetNode<Hud>("%HUD");

// Groups (for singletons)
var player = GetTree().GetFirstNodeInGroup("player") as Player;

// Export references
[Export] public Label HealthLabel;
```

---

## Animation Approaches

**When to use each:**

**Tween** - Simple property changes
```csharp
var tween = CreateTween();
tween.TweenProperty(this, "modulate:a", 0.0f, 0.5f);
```

**AnimationPlayer** - Complex choreography
- Multiple nodes animating together
- Cutscenes or special attacks
- Frame-perfect timing needed

**AnimatedSprite2D** - Frame-by-frame sprites
- When you have sprite sheets
- Character animations
- VFX loops

---

## Common Gotchas & Solutions

### Problem: HUD doesn't follow camera
**Cause:** CanvasLayer has `follow_viewport_enabled = true`  
**Fix:** Set to false. CanvasLayer should be screen-space.

### Problem: Node not found errors
**Cause:** Using `GetNode()` in `_Ready()` before scene fully loaded  
**Fix:** Use Groups, or `CallDeferred` for initialization

### Problem: Physics error when spawning
**Cause:** Trying to `AddChild()` during collision  
**Fix:** Use `CallDeferred(MethodName.AddChild, node)`

### Problem: Signal parameter error (GD0202)
**Cause:** Trying to pass custom C# class through signal  
**Fix:** Use Godot types (int, string, Node, Resource)

### Problem: Upgrade not applying
**Cause:** Not recalculating stats after applying  
**Fix:** Call `RecalculateStats()` in StatsManager

---

## Component vs Singleton Guidance

**Use Components when:**
- Functionality is player-specific
- Single character for now
- Phase 1 development
- Examples: StatsManager, SkillManager, UpgradeManager

**Use Singletons when:**
- Multiple characters need access
- Global game state
- Stateless utilities
- Examples: GameState, CombatSystem, SaveManager

**Current Approach (Phase 1):**
- Keep components on Player
- Add singletons in Phase 2 when needed

---

## Performance Considerations

**Optimization Targets:**
- 60 FPS minimum with 50+ enemies
- <2 second scene transitions
- Instant save/load

**When to optimize:**
- After core loop is fun
- When hitting performance targets becomes difficult
- Before adding more content

**Object Pooling Triggers:**
- Spawning 50+ enemies total → Consider pooling
- Shooting 20+ projectiles/second → Definitely pool
- 100+ XP shards on screen → Pool them

---

## Related Documentation

- **Project structure:** [`project-structure.md`](project-structure.md)
- **Skill system:** [`skill-system-architecture.md`](skill-system-architecture.md)
- **Current issues:** [`../../CLAUDE.md`](../../CLAUDE.md)

---

*Reference this when implementing new systems or debugging issues.*
