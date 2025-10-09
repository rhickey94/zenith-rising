using System.Collections.Generic;
using Godot;
using SpaceTower.Progression.Upgrades;

namespace SpaceTower.Scripts.UI.Menus;

public partial class LevelUpPanel : Control
{
    private Label _title;
    private Button _upgradeButton1;
    private Button _upgradeButton2;
    private Button _upgradeButton3;

    private List<Upgrade> _currentOptions;

    [Signal]
    public delegate void UpgradeSelectedEventHandler(Upgrade upgradeOption);

    public override void _Ready()
    {
        _title = GetNode<Label>("Panel/Content/Title");
        _upgradeButton1 = GetNode<Button>("Panel/Content/UpgradeOptions/UpgradeButton1");
        _upgradeButton2 = GetNode<Button>("Panel/Content/UpgradeOptions/UpgradeButton2");
        _upgradeButton3 = GetNode<Button>("Panel/Content/UpgradeOptions/UpgradeButton3");

        _upgradeButton1.Pressed += () => SelectUpgrade(0);
        _upgradeButton2.Pressed += () => SelectUpgrade(1);
        _upgradeButton3.Pressed += () => SelectUpgrade(2);

        Hide(); // Hidden by default

    }

    public void ShowUpgrades(List<Upgrade> options)
    {
        _currentOptions = options;

        _upgradeButton1.Text = FormatUpgrade(options[0]);
        _upgradeButton2.Text = FormatUpgrade(options[1]);
        _upgradeButton3.Text = FormatUpgrade(options[2]);

        Show();
        GetTree().Paused = true; // Pause game

    }

    private string FormatUpgrade(Upgrade upgrade)
    {
        return $"{upgrade.UpgradeName}\n{upgrade.Description}";
    }

    private void SelectUpgrade(int index)
    {
        EmitSignal(SignalName.UpgradeSelected, _currentOptions[index]);
        Hide();
        GetTree().Paused = false; // Resume game

    }
}
