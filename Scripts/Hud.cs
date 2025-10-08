using Godot;
using System;

public partial class Hud : Control
{
  private Label _healthLabel;
  private ProgressBar _healthBar;

  private Label _experienceLabel;
  private ProgressBar _experienceBar;
  private Label _levelLabel;

  private Label _goldLabel;
  private Label _essenceLabel;
  private Label _partsLabel;
  private Label _fragmentsLabel;

  private Label _floorTitle;
  private Label _waveLabel;

  public override void _Ready()
  {
    _healthLabel = GetNode<Label>("TopLeft/HealthLabel");
    _healthBar = GetNode<ProgressBar>("TopLeft/HealthBar");
    _experienceLabel = GetNode<Label>("TopLeft/ExperienceLabel");
    _experienceBar = GetNode<ProgressBar>("TopLeft/ExperienceBar");
    _levelLabel = GetNode<Label>("TopLeft/LevelLabel");

    _goldLabel = GetNode<Label>("TopRight/GoldLabel");
    _essenceLabel = GetNode<Label>("TopRight/EssenceLabel");
    _partsLabel = GetNode<Label>("TopRight/PartsLabel");
    _fragmentsLabel = GetNode<Label>("TopRight/FragmentsLabel");

    _floorTitle = GetNode<Label>("FloorInfo/FloorTitle");
    _waveLabel = GetNode<Label>("FloorInfo/WaveLabel");
  }

  public void UpdateHealth(float currentHealth, float maxHealth)
  {
    _healthBar.MaxValue = maxHealth;
    _healthBar.Value = currentHealth;
    _healthLabel.Text = $"Health: {currentHealth}/{maxHealth}";
  }

  public void UpdateExperience(float currentExp, float expToNextLevel, int level)
  {
    _experienceBar.MaxValue = expToNextLevel;
    _experienceBar.Value = currentExp;
    _experienceLabel.Text = $"EXP: {currentExp}/{expToNextLevel}";
    _levelLabel.Text = $"Level: {level}";
  }

  public void UpdateResources(int gold, int essence, int parts, int fragments)
  {
    _goldLabel.Text = $"Gold: {gold}";
    _essenceLabel.Text = $"Essence: {essence}";
    _partsLabel.Text = $"Parts: {parts}";
    _fragmentsLabel.Text = $"Fragments: {fragments}";
  }

  public void UpdateFloorInfo(int floor, string floorName)
  {
    _floorTitle.Text = $"Floor {floor} - {floorName}";
  }

  public void UpdateWaveInfo(int wave, int enemiesRemaining)
  {
    _waveLabel.Text = $"Wave {wave} - Enemies Left: {enemiesRemaining}";
  }
}
