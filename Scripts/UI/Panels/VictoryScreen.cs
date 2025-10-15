using Godot;
using System;

namespace SpaceTower.Scripts.UI.Panels;

public partial class VictoryScreen : Control
{
    [Export] private Label _timeLabel;
    [Export] private Label _enemiesLabel;
    [Export] private Label _levelLabel;
    [Export] private Label _floorsLabel;
    [Export] private string _menuScenePath = "res://Scenes/UI/Menus/main_menu.tscn";
    [Signal] public delegate void ContinueButtonPressedEventHandler();

    private Button _continueButton;

    public override void _Ready()
    {
        _continueButton = GetNode<Button>("%VictoryContinueButton");
        _continueButton.Pressed += OnContinueButtonPressed;
    }

    public void ShowScreen(float timeSurvived, int enemiesKilled, int playerLevel, int floorsCompleted)
    {
        _timeLabel.Text = $"Time Survived: {FormatTime(timeSurvived)}";
        _enemiesLabel.Text = $"Enemies Killed: {enemiesKilled}";
        _levelLabel.Text = $"Final Level: {playerLevel}";
        _floorsLabel.Text = $"Floors Completed: {floorsCompleted}";

        Show();
        GetTree().Paused = true;
    }

    private string FormatTime(float seconds)
    {
        int minutes = (int)(seconds / 60);
        int secs = (int)(seconds % 60);
        return $"{minutes:D2}:{secs:D2}";
    }

    private void OnContinueButtonPressed()
    {
        Hide(); // Hide this screen
        EmitSignal(SignalName.ContinueButtonPressed);
        // Game.cs will handle showing ResultsScreen
    }
}
