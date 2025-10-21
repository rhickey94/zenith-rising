using Godot;

namespace ZenithRising.Scripts.UI.HUD;

/// <summary>
/// UI component for a single skill slot (icon, cooldown, keybind).
/// </summary>
public partial class SkillSlotUI : Control
{
    private Label _keybindLabel;
    private Label _skillNameLabel;
    private ProgressBar _cooldownBar;
    private Label _cooldownLabel;

    public override void _Ready()
    {
        _keybindLabel = GetNodeOrNull<Label>("%KeybindLabel");
        _skillNameLabel = GetNodeOrNull<Label>("%SkillNameLabel");
        _cooldownBar = GetNodeOrNull<ProgressBar>("%CooldownBar");
        _cooldownLabel = GetNodeOrNull<Label>("%CooldownLabel");
    }

    public void SetKeybind(string keybind)
    {
        if (_keybindLabel != null)
        {
            _keybindLabel.Text = keybind;
        }
    }

    public void SetSkillName(string skillName)
    {
        if (_skillNameLabel != null)
        {
            _skillNameLabel.Text = skillName;
        }
    }

    public void UpdateCooldown(float remaining, float total)
    {
        if (_cooldownBar == null || _cooldownLabel == null)
        {
            return;
        }


        if (remaining > 0)
        {
            _cooldownBar.Show();
            _cooldownLabel.Show();
            _cooldownBar.MaxValue = total;
            _cooldownBar.Value = remaining;
            _cooldownLabel.Text = $"{remaining:F1}s";
        }
        else
        {
            _cooldownBar.Hide();
            _cooldownLabel.Hide();
        }
    }
}
