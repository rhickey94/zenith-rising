using Godot;

namespace SpaceTower.Scripts.UI.Panels;

public partial class DeathScreen : Control
{
    [Export] private Label _timeLabel;
    [Export] private Label _enemiesLabel;
    [Export] private Label _levelLabel;
    [Export] private Label _floorsLabel;
    [Export] private string _menuScenePath = "res://Scenes/UI/Menus/main_menu.tscn";

    public void ShowScreen(float timeSurvived, int enemiesKilled, int playerLevel, int floorsReached)
    {
        _timeLabel.Text = $"Time Survived: {FormatTime(timeSurvived)}";
        _enemiesLabel.Text = $"Enemies Killed: {enemiesKilled}";
        _levelLabel.Text = $"Final Level: {playerLevel}";
        _floorsLabel.Text = $"Floors Reached: {floorsReached}";

        Show();
        GetTree().Paused = true;
    }

    private string FormatTime(float seconds)
    {
        int minutes = (int)(seconds / 60);
        int secs = (int)(seconds % 60);
        return $"{minutes:D2}:{secs:D2}";
    }

    private void OnMenuButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile(_menuScenePath);
    }
}
