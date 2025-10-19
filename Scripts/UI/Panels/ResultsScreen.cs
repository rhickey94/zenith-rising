using Godot;
using ZenithRising.Scripts.PlayerScripts.Components;

namespace ZenithRising.Scripts.UI.Panels;

public partial class ResultsScreen : Control
{
    [Signal] public delegate void AllocateStatsRequestedEventHandler();
    [Signal] public delegate void ReturnToMenuRequestedEventHandler();

    // UI References (use % unique names)
    private Label _floorsLabel;
    private Label _bossesLabel;
    private Label _timeLabel;
    private Label _enemiesLabel;
    private Label _xpEarnedLabel;
    private Label _characterLevelLabel;
    private ProgressBar _xpProgressBar;
    private Label _xpProgressLabel;
    private Label _levelUpNotification;
    private Button _allocateStatsButton;
    private Button _returnToMenuButton;

    private StatsManager _statsManager;
    private int _previousLevel;

    public override void _Ready()
    {
        // Get all node references using %UniqueName syntax
        _floorsLabel = GetNode<Label>("%FloorsLabel");
        _bossesLabel = GetNode<Label>("%BossesLabel");
        _timeLabel = GetNode<Label>("%TimeLabel");
        _enemiesLabel = GetNode<Label>("%EnemiesLabel");
        _xpEarnedLabel = GetNode<Label>("%XPEarnedLabel");
        _characterLevelLabel = GetNode<Label>("%CharacterLevelLabel");
        _xpProgressBar = GetNode<ProgressBar>("%XPProgressBar");
        _xpProgressLabel = GetNode<Label>("%XPProgressLabel");
        _levelUpNotification = GetNode<Label>("%LevelUpNotification");
        _allocateStatsButton = GetNode<Button>("%AllocateStatsButton");
        _returnToMenuButton = GetNode<Button>("%ReturnToMenuButton");

        // Connect button signals
        _allocateStatsButton.Pressed += OnAllocateStatsPressed;
        _returnToMenuButton.Pressed += OnReturnToMenuPressed;

        Hide();
    }

    public void ShowScreen(
        float timeSurvived,
        int enemiesKilled,
        int floorsCleared,
        int bossesKilled,
        int characterXPEarned,
        StatsManager statsManager)
    {
        _statsManager = statsManager;

        // Format time as MM:SS
        int minutes = (int)(timeSurvived / 60);
        int seconds = (int)(timeSurvived % 60);
        string timeString = $"{minutes:D2}:{seconds:D2}";

        // Update run stats
        _floorsLabel.Text = $"Floors Cleared: {floorsCleared}";
        _bossesLabel.Text = $"Bosses Defeated: {bossesKilled}";
        _timeLabel.Text = $"Time Survived: {timeString}";
        _enemiesLabel.Text = $"Enemies Killed: {enemiesKilled}";

        // Update character progression
        _xpEarnedLabel.Text = $"💫 Character XP Earned: +{characterXPEarned}";
        _characterLevelLabel.Text = $"Character Level {statsManager.CharacterLevel}";

        // XP Progress Bar
        _xpProgressBar.MaxValue = statsManager.CharacterExperienceRequired;
        _xpProgressBar.Value = statsManager.CharacterExperience;

        float percentToNext = statsManager.CharacterExperience / (float)statsManager.CharacterExperienceRequired * 100f;
        _xpProgressLabel.Text = $"{statsManager.CharacterExperience} / {statsManager.CharacterExperienceRequired} XP({percentToNext:F0}% to Level {statsManager.CharacterLevel + 1})";

        // Show level up notification if player has unspent points
        if (statsManager.AvailableStatPoints > 0)
        {
            _levelUpNotification.Text = $"⭐ LEVEL UP! You have {statsManager.AvailableStatPoints} unspent stat point(s)!";
            _levelUpNotification.Visible = true;
            _allocateStatsButton.Disabled = false;
        }
        else
        {
            _levelUpNotification.Visible = false;
            _allocateStatsButton.Disabled = true;
        }

        Show();
        GetTree().Paused = true;
    }

    private void OnAllocateStatsPressed()
    {
        Hide();
        EmitSignal(SignalName.AllocateStatsRequested);
    }

    private void OnReturnToMenuPressed()
    {
        Hide();
        GetTree().Paused = false;
        EmitSignal(SignalName.ReturnToMenuRequested);
    }
}
