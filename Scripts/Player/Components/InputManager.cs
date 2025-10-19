using Godot;
using System;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class InputManager : Node
{
    // Called every frame for polling-based input (movement)
    public Vector2 GetMovementInput()
    {
        return Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
    }

    // Called from _UnhandledInput for event-based input (skills, UI)
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

        // UI inputs
        if (keyEvent.Keycode == Key.C)
        {
            EmitSignal(SignalName.StatPanelPressed);
        }
    }

    // Signals
    [Signal] public delegate void SkillPressedEventHandler(int skillSlot);
    [Signal] public delegate void StatPanelPressedEventHandler();
}
