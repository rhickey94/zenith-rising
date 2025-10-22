using Godot;
using System;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Input handling component for player character.
/// Responsibilities:
/// - Movement input polling (WASD keys via GetMovementInput)
/// - Skill input events (mouse buttons, number keys, spacebar)
/// - Signal emission to Player for skill execution
/// Pattern: Converts Godot input events → SkillPressed signals with SkillSlot enum
/// Does NOT handle: Input buffering (SkillManager), skill execution (Player)
/// </summary>
[GlobalClass]
public partial class InputManager : Node
{
    // ═══════════════════════════════════════════════════════════════
    // SIGNALS
    // ═══════════════════════════════════════════════════════════════
    [Signal] public delegate void SkillPressedEventHandler(int skillSlot);

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - MOVEMENT INPUT
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Returns normalized movement vector from WASD input.
    /// Called every frame from Player._PhysicsProcess().
    /// </summary>
    public Vector2 GetMovementInput()
    {
        return Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - SKILL INPUT
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Processes skill input events (mouse clicks, keyboard presses).
    /// Called from Player._UnhandledInput().
    /// Emits SkillPressed signal with appropriate SkillSlot.
    /// </summary>
    public void HandleInputEvent(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            HandleMouseInput(mouseEvent);
        }
        else if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            HandleKeyInput(keyEvent);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - INPUT ROUTING
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Routes mouse button inputs to skill slots.
    /// LMB = BasicAttack, RMB = SpecialAttack.
    /// </summary>
    private void HandleMouseInput(InputEventMouseButton mouseEvent)
    {
        if (mouseEvent.ButtonIndex == MouseButton.Left)
        {
            EmitSignal(SignalName.SkillPressed, (int)SkillSlot.BasicAttack);
        }
        else if (mouseEvent.ButtonIndex == MouseButton.Right)
        {
            EmitSignal(SignalName.SkillPressed, (int)SkillSlot.SpecialAttack);
        }
    }

    /// <summary>
    /// Routes keyboard inputs to skill slots.
    /// 1 = Primary, 2 = Secondary, 3 = Ultimate, Space = Utility.
    /// </summary>
    private void HandleKeyInput(InputEventKey keyEvent)
    {
        switch (keyEvent.Keycode)
        {
            case Key.Key1:
                EmitSignal(SignalName.SkillPressed, (int)SkillSlot.Primary);
                break;
            case Key.Key2:
                EmitSignal(SignalName.SkillPressed, (int)SkillSlot.Secondary);
                break;
            case Key.Key3:
                EmitSignal(SignalName.SkillPressed, (int)SkillSlot.Ultimate);
                break;
            case Key.Space:
                EmitSignal(SignalName.SkillPressed, (int)SkillSlot.Utility);
                break;
        }
    }
}
