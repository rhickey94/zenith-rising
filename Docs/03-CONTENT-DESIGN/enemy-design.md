# Enemy Design

## Current Enemy Types (Phase 1) ✅

### Basic Melee Enemy
- **Speed:** 200
- **Health:** 100 base (scales with wave/floor)
- **Damage:** 10 base (scales with wave/floor)
- **Behavior:** Chase player directly, attack on contact
- **Visual:** Red square (placeholder)

### Fast Melee Enemy
- **Speed:** 300 (+50% faster)
- **Health:** 50 base (−50% HP)
- **Damage:** 10 base (same damage)
- **Behavior:** Aggressive chaser, high threat
- **Visual:** Orange square (placeholder)

### Slow Ranged Enemy
- **Speed:** 100 (−50% slower)
- **Health:** 150 base (+50% HP)
- **Damage:** 15 base (+50% damage)
- **Behavior:** Kites player, shoots projectiles at range
- **Attack:** Projectile with travel time, stops to fire
- **Visual:** Purple square (placeholder)

### Boss Enemy
- **Speed:** 150
- **Health:** 500 base (5x normal, scales with floor)
- **Damage:** 20 base (2x normal, scales with floor)
- **Behavior:** Aggressive, high priority target
- **Visual:** Large red square (placeholder)

## Scaling System

**Per Wave (within a floor):**
- Health: +10% per wave
- Damage: +5% per wave
- Example: Wave 10 has +100% HP, +50% damage

**Per Floor:**
- Health: +50% base per floor
- Damage: +50% base per floor
- Example: Floor 3 enemy has +150% base stats

**Implementation:**
```csharp
float healthMult = (1 + currentWave * 0.1f) * (1 + currentFloor * 0.5f);
float damageMult = (1 + currentWave * 0.05f) * (1 + currentFloor * 0.5f);
enemy.Initialize(healthMult, damageMult);
```

## Planned Enemy Types (Phase 2+)

### Elite Variants
- Golden/glowing versions of existing enemies
- 3x HP, 2x damage, 2x XP
- 10% chance to spawn instead of normal
- Guaranteed gear drops

### Flying Enemies
- Ignores terrain/obstacles
- Swoops in patterns
- Lower HP, higher evasion

### Tank Enemies
- Very slow, very high HP
- AOE stomp attack
- Blocks smaller enemies

### Summoner Enemies
- Low HP, medium range
- Spawns smaller enemies
- Priority target

## Enemy Behavior Patterns

**Implemented (Phase 1):**
- Chase (Basic/Fast)
- Kite + Shoot (Slow Ranged)

**Planned (Phase 2+):**
- Strafe (circle player)
- Ambush (hide, then charge)
- Support (buff nearby enemies)
- Burst (charge powerful attack)

## Related Documentation

- Combat system: [`../01-GAME-DESIGN/combat-skills.md`](../01-GAME-DESIGN/combat-skills.md)
- Current implementation: [`../../CLAUDE.md`](../../CLAUDE.md)
