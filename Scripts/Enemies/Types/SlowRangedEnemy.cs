using Godot;
using SpaceTower.Scripts.Enemies.Base;
using SpaceTower.Scripts.Enemies.Effects;
using SpaceTower.Scripts.PlayerScripts;

namespace SpaceTower.Scripts.Enemies.Types;

public partial class SlowRangedEnemy : Enemy
{
    [Export] public PackedScene ProjectileScene;
    [Export] public float AttackRange = 300.0f;
    [Export] public float PreferredDistance = 250.0f;

    protected override Vector2 GetMovementDirection()
    {
        float distance = GlobalPosition.DistanceTo(_player.GlobalPosition);
        if (distance < PreferredDistance)
        {
            return (GlobalPosition - _player.GlobalPosition).Normalized();
        }
        else if (distance > AttackRange)
        {
            return (_player.GlobalPosition - GlobalPosition).Normalized();
        }
        else
        {
            return Vector2.Zero;
        }
    }

    protected override void Attack(Player player)
    {
        GD.Print("SlowRangedEnemy attacking!");

        // Shoot projectile
        if (ProjectileScene != null)
        {
            var projectile = ProjectileScene.Instantiate<EnemyProjectile>();
            projectile.GlobalPosition = GlobalPosition;

            projectile.Initialize((player.GlobalPosition - GlobalPosition).Normalized(), Damage);
            GetTree().Root.CallDeferred(Node.MethodName.AddChild, projectile);

            _timeSinceLastAttack = 0f;
        }
        else
        {
            GD.Print("ProjectileScene is null!");  // ← Add this too
        }
    }

    protected override void TryAttack(double delta)
    {
        _timeSinceLastAttack += (float)delta;

        if (_timeSinceLastAttack >= AttackCooldown && _player != null)
        {
            float distance = GlobalPosition.DistanceTo(_player.GlobalPosition);
            if (distance <= AttackRange)
            {
                Attack(_player);
            }
        }
    }
}
