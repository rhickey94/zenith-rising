using Godot;
using ZenithRising.Scripts.Core;
using System.Collections.Generic;
using ZenithRising.Scripts.PlayerScripts.Components;

namespace ZenithRising.Scripts.UI.Panels;

public partial class StatAllocationPanel : Control
{
    private StatsManager _statsManager;
    private UpgradeManager _upgradeManager;

    private Label _unallocatedLabel;
    private Button _closeButton;

    private readonly Dictionary<StatType, Label> _statValueLabels = [];
    private readonly Dictionary<StatType, Label> _statBonusLabels = [];
    private readonly Dictionary<StatType, Button> _statButtons = [];

    public override void _Ready()
    {
        // Get node references using % unique names
        _unallocatedLabel = GetNode<Label>("%UnallocatedLabel");
        _closeButton = GetNode<Button>("%CloseButton");

        _statValueLabels[StatType.Strength] = GetNode<Label>("%StrengthValueLabel");
        _statValueLabels[StatType.Intelligence] = GetNode<Label>("%IntelligenceValueLabel");
        _statValueLabels[StatType.Agility] = GetNode<Label>("%AgilityValueLabel");
        _statValueLabels[StatType.Vitality] = GetNode<Label>("%VitalityValueLabel");
        _statValueLabels[StatType.Fortune] = GetNode<Label>("%FortuneValueLabel");

        _statBonusLabels[StatType.Strength] = GetNode<Label>("%StrengthBonusLabel");
        _statBonusLabels[StatType.Intelligence] = GetNode<Label>("%IntelligenceBonusLabel");
        _statBonusLabels[StatType.Agility] = GetNode<Label>("%AgilityBonusLabel");
        _statBonusLabels[StatType.Vitality] = GetNode<Label>("%VitalityBonusLabel");
        _statBonusLabels[StatType.Fortune] = GetNode<Label>("%FortuneBonusLabel");

        _statButtons[StatType.Strength] = GetNode<Button>("%StrengthButton");
        _statButtons[StatType.Intelligence] = GetNode<Button>("%IntelligenceButton");
        _statButtons[StatType.Agility] = GetNode<Button>("%AgilityButton");
        _statButtons[StatType.Vitality] = GetNode<Button>("%VitalityButton");
        _statButtons[StatType.Fortune] = GetNode<Button>("%FortuneButton");

        // Connect button signals
        foreach (var kvp in _statButtons)
        {
            StatType statType = kvp.Key;
            kvp.Value.Pressed += () => OnStatButtonPressed(statType);
        }

        _closeButton.Pressed += OnClosePressed;

        Hide();
    }

    public void ShowPanel(StatsManager statsManager, UpgradeManager upgradeManager)
    {
        _statsManager = statsManager;
        _upgradeManager = upgradeManager;

        UpdateDisplay();
        Show();
        GetTree().Paused = true;
    }

    private void OnStatButtonPressed(StatType statType)
    {
        if (_statsManager != null && _statsManager.CanAllocateStat())
        {
            _statsManager.AllocateStat(statType, 1);
            UpdateDisplay();

            // Trigger full stat recalculation
            _upgradeManager?.RecalculateAllStats();
        }
    }

    private void UpdateDisplay()
    {
        if (_statsManager == null)
        {
            return;
        }

        _unallocatedLabel.Text = $"Unallocated Points: {_statsManager.AvailableStatPoints}";

        _statValueLabels[StatType.Strength].Text = $"Strength (STR): {_statsManager.Strength}";
        _statValueLabels[StatType.Intelligence].Text = $"Intelligence (INT): {_statsManager.Intelligence}";
        _statValueLabels[StatType.Agility].Text = $"Agility (AGI): {_statsManager.Agility}";
        _statValueLabels[StatType.Vitality].Text = $"Vitality (VIT): {_statsManager.Vitality}";
        _statValueLabels[StatType.Fortune].Text = $"Fortune (FOR): {_statsManager.Fortune}";

        _statBonusLabels[StatType.Strength].Text = FormatStrengthBonus(_statsManager);
        _statBonusLabels[StatType.Intelligence].Text = FormatIntelligenceBonus(_statsManager);
        _statBonusLabels[StatType.Agility].Text = FormatAgilityBonus(_statsManager);
        _statBonusLabels[StatType.Vitality].Text = FormatVitalityBonus(_statsManager);
        _statBonusLabels[StatType.Fortune].Text = FormatFortuneBonus(_statsManager);

        bool canAllocate = _statsManager.CanAllocateStat();
        foreach (var button in _statButtons.Values)
        {
            button.Disabled = !canAllocate;
        }
    }

    private string FormatStrengthBonus(StatsManager stats)
    {
        if (GameBalance.Instance == null || GameBalance.Instance.Config == null)
        {
            return "Loading...";
        }

        float physDmg = (stats.PhysicalDamageMultiplier - 1.0f) * 100f;
        float hp = stats.Strength * GameBalance.Instance.Config.CharacterProgression.StrengthHealthPerPoint;
        return $"Physical Dmg +{physDmg:F0}%, HP +{hp:F0}";
    }

    private string FormatIntelligenceBonus(StatsManager stats)
    {
        float magDmg = (stats.MagicalDamageMultiplier - 1.0f) * 100f;
        return $"Magical Dmg +{magDmg:F0}%, CDR +{stats.CooldownReduction * 100f:F0}%";
    }

    private string FormatAgilityBonus(StatsManager stats)
    {
        if (GameBalance.Instance == null || GameBalance.Instance.Config == null)
        {
            return "Loading...";
        }

        float atkSpd = stats.Agility * GameBalance.Instance.Config.CharacterProgression.AgilityAttackSpeedPerPoint * 100f;
        float crit = Mathf.Min(stats.Agility * GameBalance.Instance.Config.CharacterProgression.AgilityCritPerPoint, 0.5f) * 100f;
        return $"Attack Speed +{atkSpd:F0}%, Crit +{crit:F0}%";
    }

    private string FormatVitalityBonus(StatsManager stats)
    {
        if (GameBalance.Instance == null || GameBalance.Instance.Config == null)
        {
            return "Loading...";
        }

        float hp = stats.Vitality * GameBalance.Instance.Config.CharacterProgression.VitalityHealthPerPoint;
        float regen = stats.Vitality * GameBalance.Instance.Config.CharacterProgression.VitalityRegenPerPoint;
        return $"HP +{hp:F0}, Regen +{regen:F1}/sec";
    }

    private string FormatFortuneBonus(StatsManager stats)
    {
        if (GameBalance.Instance == null || GameBalance.Instance.Config == null)
        {
            return "Loading...";
        }

        float critDmg = (stats.CurrentCritMultiplier - 1.5f) * 100f;
        float dropRate = stats.Fortune * GameBalance.Instance.Config.CharacterProgression.FortuneDropRatePerPoint * 100f;
        return $"Crit Dmg +{critDmg:F0}%, Drop Rate +{dropRate:F0}%";
    }

    private void OnClosePressed()
    {
        Hide();
        GetTree().Paused = false;
    }
}
