using Godot;
using ZenithRising.Scripts.PlayerScripts;

namespace ZenithRising.Scripts.UI.HUD;

public partial class Hud : Control
{
    // Health UI

    private Label _healthLabel;
    private ProgressBar _healthBar;

    // Experience and Level UI

    private Label _experienceLabel;
    private ProgressBar _experienceBar;
    private Label _levelLabel;

    // Resources UI

    private Label _goldLabel;
    private Label _essenceLabel;
    private Label _partsLabel;
    private Label _fragmentsLabel;

    // Floor and Wave Info UI

    private Label _floorTitle;
    private Label _waveLabel;

    public override void _Ready()
    {
        AddToGroup("hud");

        _healthLabel = GetNode<Label>("TopLeft/HealthLabel");
        _healthBar = GetNode<ProgressBar>("TopLeft/HealthBar");
        _experienceLabel = GetNode<Label>("TopLeft/ExperienceLabel");
        _experienceBar = GetNode<ProgressBar>("TopLeft/ExperienceBar");
        _levelLabel = GetNode<Label>("TopLeft/LevelLabel");

        _goldLabel = GetNode<Label>("TopRight/GoldContainer/GoldLabel");
        _essenceLabel = GetNode<Label>("TopRight/EssenceContainer/EssenceLabel");
        _partsLabel = GetNode<Label>("TopRight/PartsContainer/PartsLabel");
        _fragmentsLabel = GetNode<Label>("TopRight/FragmentsContainer/FragmentsLabel");

        _floorTitle = GetNode<Label>("FloorInfo/FloorContent/FloorTitle");
        _waveLabel = GetNode<Label>("FloorInfo/FloorContent/WaveLabel");

        // Validate critical UI elements

        if (_healthBar == null || _experienceBar == null)
        {
            GD.PrintErr("HUD: Critical UI elements missing! Check node paths.");
        }

        // NEW: Connect to Player signals


        if (GetTree().GetFirstNodeInGroup("player") is Player player)
        {
            player.HealthChanged += UpdateHealth;
            player.ExperienceChanged += UpdateExperience;
            // player.ResourcesChanged += UpdateResources;
            // player.FloorInfoChanged += UpdateFloorInfo;
            // player.WaveInfoChanged += UpdateWaveInfo;
        }
        else
        {
            GD.PrintErr("HUD: Could not find Player to connect signals!");
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (_healthBar == null || _healthLabel == null)
        {
            return;
        }


        _healthBar.MaxValue = maxHealth;
        _healthBar.Value = currentHealth;
        _healthLabel.Text = $"Health: {currentHealth:F0}/{maxHealth:F0}";
    }

    public void UpdateExperience(int currentExp, int expToNextLevel, int level)
    {
        if (_experienceBar == null || _experienceLabel == null || _levelLabel == null)
        {
            return;
        }


        _experienceBar.MaxValue = expToNextLevel;
        _experienceBar.Value = currentExp;
        _experienceLabel.Text = $"EXP: {currentExp}/{expToNextLevel}";
        _levelLabel.Text = $"Level: {level}";
    }

    public void UpdateResources(int gold, int essence, int parts, int fragments)
    {
        if (_goldLabel == null || _essenceLabel == null || _partsLabel == null || _fragmentsLabel == null)
        {
            return;
        }


        _goldLabel.Text = $"Gold: {gold}";
        _essenceLabel.Text = $"Essence: {essence}";
        _partsLabel.Text = $"Parts: {parts}";
        _fragmentsLabel.Text = $"Fragments: {fragments}";
    }

    public void UpdateFloorInfo(int floor, string floorName)
    {
        if (_floorTitle == null)
        {
            return;
        }


        _floorTitle.Text = $"Floor {floor} - {floorName}";
    }

    public void UpdateWaveInfo(int wave, int enemiesRemaining)
    {
        if (_waveLabel == null)
        {
            return;
        }


        _waveLabel.Text = $"Wave {wave} - Enemies Left: {enemiesRemaining}";
    }
}
