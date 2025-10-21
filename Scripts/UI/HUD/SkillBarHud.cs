using Godot;
using System.Collections.Generic;
using ZenithRising.Scripts.PlayerScripts.Components;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.UI.HUD;

/// <summary>
/// Displays skill cooldowns and keybinds at the bottom-center of the screen.
/// Only visible during combat (dungeon scene).
/// </summary>
public partial class SkillBarHud : Control
{
    // ===== NODE REFERENCES =====
    private readonly Dictionary<SkillSlot, SkillSlotUI> _skillSlotNodes = [];

    private SkillManager _skillManager;
    private StatsManager _statsManager;

    // ===== LIFECYCLE =====
    public override void _Ready()
    {
        // Get skill slot UI elements
        _skillSlotNodes[SkillSlot.Primary] = GetNode<SkillSlotUI>("%PrimarySlot");
        _skillSlotNodes[SkillSlot.Secondary] = GetNode<SkillSlotUI>("%SecondarySlot");
        _skillSlotNodes[SkillSlot.Ultimate] = GetNode<SkillSlotUI>("%UltimateSlot");
        _skillSlotNodes[SkillSlot.BasicAttack] = GetNode<SkillSlotUI>("%BasicAttackSlot");
        _skillSlotNodes[SkillSlot.SpecialAttack] = GetNode<SkillSlotUI>("%SpecialAttackSlot");
        _skillSlotNodes[SkillSlot.Utility] = GetNode<SkillSlotUI>("%UtilitySlot");

        // Find player components
        CallDeferred(nameof(FindPlayerComponents));
    }

    private void FindPlayerComponents()
    {
        var player = GetTree().GetFirstNodeInGroup("player");
        if (player == null)
        {
            GD.PrintErr("SkillBarHUD: Player not found!");
            return;
        }

        _skillManager = player.GetNodeOrNull<SkillManager>("SkillManager");
        if (_skillManager == null)
        {
            GD.PrintErr("SkillBarHUD: SkillManager not found!");
        }

        _statsManager = player.GetNodeOrNull<StatsManager>("StatsManager");
        if (_statsManager == null)
        {
            GD.PrintErr("SkillBarHUD: StatsManager not found!");
        }

        InitializeSkillSlots();
    }

    public override void _Process(double delta)
    {
        if (_skillManager == null || _statsManager == null)
        {
            return;
        }

        UpdateSkillCooldowns();
    }

    // ===== PRIVATE HELPERS =====

    private void InitializeSkillSlots()
    {
        if (_skillManager == null)
        {
            return;
        }

        SetupSkillSlot(SkillSlot.Primary, _skillManager.PrimarySkill, "1");
        SetupSkillSlot(SkillSlot.Secondary, _skillManager.SecondarySkill, "2");
        SetupSkillSlot(SkillSlot.Ultimate, _skillManager.UltimateSkill, "3");
        SetupSkillSlot(SkillSlot.BasicAttack, _skillManager.BasicAttackSkill, "LMB");
        SetupSkillSlot(SkillSlot.SpecialAttack, _skillManager.SpecialAttackSkill, "RMB");
        SetupSkillSlot(SkillSlot.Utility, _skillManager.UtilitySkill, "Space");
    }

    private void SetupSkillSlot(SkillSlot slot, Skill skill, string keybind)
    {
        if (!_skillSlotNodes.TryGetValue(slot, out SkillSlotUI slotNode)) return;
        var keybindLabel = slotNode.GetNodeOrNull<Label>($"%{slot}KeybindLabel");
        var skillNameLabel = slotNode.GetNodeOrNull<Label>($"%{slot}SkillNameLabel");

        if (keybindLabel != null)
        {
            keybindLabel.Text = keybind;
        }

        if (skill != null && skillNameLabel != null)
        {
            skillNameLabel.Text = skill.SkillName;
            slotNode.Show();
        }
        else
        {
            slotNode.Hide();
        }
    }

    private void UpdateSkillCooldowns()
    {
        UpdateSlotCooldown(SkillSlot.Primary);
        UpdateSlotCooldown(SkillSlot.Secondary);
        UpdateSlotCooldown(SkillSlot.Ultimate);
        UpdateSlotCooldown(SkillSlot.BasicAttack);
        UpdateSlotCooldown(SkillSlot.SpecialAttack);
        UpdateSlotCooldown(SkillSlot.Utility);
    }

    private void UpdateSlotCooldown(SkillSlot slot)
    {
        if (!_skillSlotNodes.TryGetValue(slot, out SkillSlotUI slotNode)) return;
        var cooldownBar = slotNode.GetNodeOrNull<ProgressBar>($"%{slot}CooldownBar");
        var cooldownLabel = slotNode.GetNodeOrNull<Label>($"%{slot}CooldownLabel");

        if (cooldownBar == null || cooldownLabel == null) return;

        float remaining = _skillManager.GetCooldownRemaining(slot);
        float total = _skillManager.GetCooldownTotal(slot);

        if (remaining > 0)
        {
            cooldownBar.Show();
            cooldownLabel.Show();
            cooldownBar.MaxValue = total;
            cooldownBar.Value = remaining;
            cooldownLabel.Text = $"{remaining:F1}s";
        }
        else
        {
            cooldownBar.Hide();
            cooldownLabel.Hide();
        }
    }
}
