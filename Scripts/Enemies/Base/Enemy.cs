using Godot;
using ZenithRising.Scripts.Items.Collectibles;
using ZenithRising.Scripts.PlayerScripts;

namespace ZenithRising.Scripts.Enemies.Base;

public partial class Enemy : CharacterBody2D
{
    // ===== EXPORT PROPERTIES - Stats =====
    [Export] public float Speed = 200.0f;
    [Export] public float MaxHealth = 100.0f;
    [Export] public float Damage = 10.0f;
    [Export] public float AttackCooldown = 1.0f;
    [Export] public int ExperienceReward = 20;
    [Export] public int ExperienceShardCount = 1;

    // ===== EXPORT PROPERTIES - Dependencies =====
    [Export] public PackedScene ExperienceShardScene;

    // ===== PUBLIC PROPERTIES =====
    public float Health { get; private set; }

    // ===== SIGNALS =====
    [Signal] public delegate void EnemyDiedEventHandler(Enemy enemy);

    // ===== PROTECTED FIELDS =====
    protected Player _player;
    protected float _timeSinceLastAttack = 0f;

    // ===== PRIVATE FIELDS =====
    private Sprite2D _sprite;

    // ===== LIFECYCLE METHODS =====
    public override void _Ready()
    {
        AddToGroup("enemies");
        Health = MaxHealth;

        _sprite = GetNode<Sprite2D>("Sprite2D");

        // Find player via group (singleton pattern)
        _player = GetTree().GetFirstNodeInGroup("player") as Player;

        if (_player == null)
        {
            GD.PrintErr("Enemy: No player found! Ensure Player is in 'player' group.");
        }

        if (ExperienceShardScene == null)
        {
            GD.PrintErr("Enemy: ExperienceShardScene not assigned!");
        }

        // DEBUG: Print actual color at runtime
        GD.Print($"Enemy spawned - Modulate: {Modulate}");
        if (_sprite != null)
        {
            GD.Print($"Enemy sprite modulate: {_sprite.Modulate}");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null)
        {
            return;
        }

        Vector2 direction = GetMovementDirection();
        Velocity = direction * Speed;

        if (direction != Vector2.Zero)
        {
            // Rotation = direction.Angle();
        }

        MoveAndSlide();

        TryAttack(delta);
    }

    // ===== PUBLIC API =====
    public void Initialize(float healthMultiplier, float damageMultiplier)
    {
        MaxHealth *= healthMultiplier;
        Health = MaxHealth;
        Damage *= damageMultiplier;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die(); // Destroy enemy immediately
            return; // IMPORTANT: Exit before trying to flash
        }

        // Flash effect (only if alive)
        if (_sprite != null && IsInstanceValid(_sprite))
        {
            var originalColor = _sprite.Modulate;

            _sprite.Modulate = Colors.White;
            GetTree().CreateTimer(0.1).Timeout += () =>
            {
                if (IsInstanceValid(_sprite) && !IsQueuedForDeletion())
                {
                    _sprite.Modulate = originalColor;
                }
            };
        }
    }

    // ===== PROTECTED VIRTUAL METHODS =====
    /// <summary>
    /// Override this to customize enemy movement behavior (e.g., ranged kiting).
    /// Default: move directly toward player.
    /// </summary>
    protected virtual Vector2 GetMovementDirection()
    {
        return (_player.GlobalPosition - GlobalPosition).Normalized();
    }

    /// <summary>
    /// Override this to customize enemy attack logic (e.g., ranged projectiles).
    /// Default: contact-based melee attack on collision.
    /// </summary>
    protected virtual void TryAttack(double delta)
    {
        _timeSinceLastAttack += (float)delta;

        if (_timeSinceLastAttack >= AttackCooldown)
        {
            // Check if touching player
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);
                if (collision.GetCollider() is Player player)
                {
                    Attack(player);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Override this to customize attack execution (e.g., spawn projectile).
    /// Default: deal damage directly to player.
    /// </summary>
    protected virtual void Attack(Player player)
    {
        player.TakeDamage(Damage);
        _timeSinceLastAttack = 0f;
    }

    // ===== PRIVATE HELPERS =====
    private void Die()
    {
        EmitSignal(SignalName.EnemyDied, this);

        CallDeferred(MethodName.SpawnExperienceShards);

        QueueFree();
    }

    private void SpawnExperienceShards()
    {
        if (ExperienceShardScene != null && _player != null)
        {
            for (int i = 0; i < ExperienceShardCount; i++)
            {
                var shard = ExperienceShardScene.Instantiate<ExperienceShard>();
                shard.Position = GlobalPosition + new Vector2(GD.Randf() * 20 - 10, GD.Randf() * 20 - 10);
                shard.ExperienceValue = ExperienceReward / ExperienceShardCount;
                GetTree().Root.CallDeferred(Node.MethodName.AddChild, shard);
            }
        }
    }
}
