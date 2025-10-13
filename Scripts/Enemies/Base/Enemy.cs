using Godot;
using SpaceTower.Scripts.Items.Collectibles;
using SpaceTower.Scripts.PlayerScripts;

namespace SpaceTower.Scripts.Enemies.Base;

public partial class Enemy : CharacterBody2D
{
    // Stats
    [Export] public float Speed = 200.0f;
    [Export] public float MaxHealth = 100.0f;
    [Export] public float Damage = 10.0f;
    [Export] public float AttackCooldown = 1.0f;
    [Export] public int ExperienceReward = 20;
    [Export] public int ExperienceShardCount = 1;

    public float Health { get; private set; }

    // Dependencies
    [Export] public PackedScene ExperienceShardScene;
    protected Player _player;
    protected float _timeSinceLastAttack = 0f;

    private Sprite2D _sprite;

    [Signal] public delegate void EnemyDiedEventHandler(Enemy enemy);

    protected virtual void Attack(Player player)
    {
        player.TakeDamage(Damage);
        _timeSinceLastAttack = 0f;
    }

    protected virtual Vector2 GetMovementDirection()
    {
        return (_player.GlobalPosition - GlobalPosition).Normalized();
    }

    protected virtual void TryAttack(double delta)
    {
        // Attack player on collision
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

    public void Initialize(float healthMultiplier, float damageMultiplier)
    {
        MaxHealth *= healthMultiplier;
        Health = MaxHealth;
        Damage *= damageMultiplier;
    }

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
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null)
        {
            return;
        }

        Vector2 direction = GetMovementDirection();
        Velocity = direction * Speed;
        Rotation = direction.Angle();

        MoveAndSlide();

        TryAttack(delta);
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
