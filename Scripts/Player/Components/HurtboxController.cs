using Godot;
using ZenithRising.Scripts.Enemies.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Manages player's hurtbox - where the player can be hit by enemies.
/// Handles i-frames, invincibility, and damage reception.
/// </summary>
[GlobalClass]
public partial class HurtboxController : Node
{
    // === DEPENDENCIES ===
    private Player _player;
    private StatsManager _statsManager;

    // === HURTBOX REFERENCE ===
    private Area2D _hurtbox;

    // === INITIALIZATION ===
    public void Initialize(Player player, StatsManager statsManager)
    {
        _player = player;
        _statsManager = statsManager;
    }

    public override void _Ready()
    {
        // Get hurtbox from player scene
        _hurtbox = GetParent().GetNodeOrNull<Area2D>("Hurtbox");
        if (_hurtbox != null)
        {
            _hurtbox.AreaEntered += OnHurtboxAreaEntered;
            _hurtbox.BodyEntered += OnHurtboxBodyEntered;
        }
        else
        {
            GD.PrintErr("HurtboxController: Hurtbox not found in player scene!");
        }
    }

    // === COLLISION HANDLING ===

    private void OnHurtboxAreaEntered(Area2D area)
    {
        // Handle enemy projectile hits
        // (Projectiles have Area2D collision)
        // TODO: Check if area is enemy projectile, apply damage
    }

    private void OnHurtboxBodyEntered(Node2D body)
    {
        // Handle enemy contact damage
        if (body is Enemy enemy)
        {
            TakeDamageFromEnemy(enemy);
        }
    }

    private void TakeDamageFromEnemy(Enemy enemy)
    {
        // Delegate to StatsManager (which already handles invincibility, damage reduction)
        // StatsManager.TakeDamage() already exists and works correctly

        // This is a placeholder - actual damage comes from enemy attacks
        // For now, contact damage is handled in Enemy.cs directly
    }
}
