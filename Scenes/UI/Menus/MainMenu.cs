using Godot;

namespace SpaceTower.Scripts.UI;

public partial class MainMenu : Control
{
    // UI References
    private Button _startButton;
    private Button _continueButton;
    private Button _settingsButton;
    private Button _quitButton;
    private Label _progressLabel;

    // Scenes to load
    [Export] public string GameScenePath = "res://Scenes/Core/game.tscn";
    [Export] public string HubScenePath = "res://Scenes/Core/hub.tscn"; // For Phase 2

    public override void _Ready()
    {
        // Get node references
        _startButton = GetNode<Button>("CenterContainer/MainContent/ButtonsContainer/StartButton");
        _continueButton = GetNode<Button>("CenterContainer/MainContent/ButtonsContainer/ContinueButton");
        _settingsButton = GetNode<Button>("CenterContainer/MainContent/ButtonsContainer/SettingsButton");
        _quitButton = GetNode<Button>("CenterContainer/MainContent/ButtonsContainer/QuitButton");
        _progressLabel = GetNode<Label>("StatusBar/ProgressLabel");

        // Connect button signals
        _startButton.Pressed += OnStartPressed;
        _continueButton.Pressed += OnContinuePressed;
        _settingsButton.Pressed += OnSettingsPressed;
        _quitButton.Pressed += OnQuitPressed;

        // Check if save file exists
        CheckForSaveFile();

        // Update progress display
        UpdateProgressDisplay();

        // Add hover effects to buttons
        SetupButtonHoverEffect(_startButton);
        SetupButtonHoverEffect(_continueButton);
        SetupButtonHoverEffect(_settingsButton);
        SetupButtonHoverEffect(_quitButton);
    }

    private void CheckForSaveFile()
    {
        // TODO: Phase 2 - Check for actual save file
        // For now, hide Continue button
        _continueButton.Visible = false;

        // Example of how to check for save in Phase 2:
        // bool saveExists = FileAccess.FileExists("user://save_game.json");
        // _continueButton.Visible = saveExists;
    }

    private void UpdateProgressDisplay()
    {
        // TODO: Phase 2 - Load actual progress from save file
        // For now, show placeholder
        _progressLabel.Text = "Floor 1 | 0 Runs";

        // Example Phase 2 implementation:
        // var saveData = LoadSaveData();
        // _progressLabel.Text = $"Floor {saveData.HighestFloor} | {saveData.TotalRuns} Runs";
    }

    private void OnStartPressed()
    {
        GD.Print("Starting new game...");

        // Phase 1: Load directly into game
        GetTree().ChangeSceneToFile(GameScenePath);

        // Phase 2: Load into hub instead
        // GetTree().ChangeSceneToFile(HubScenePath);
    }

    private void OnContinuePressed()
    {
        GD.Print("Continuing saved game...");

        // TODO: Phase 2 - Load save data before transitioning
        // var saveData = LoadSaveData();
        // ApplySaveData(saveData);

        GetTree().ChangeSceneToFile(GameScenePath);
    }

    private void OnSettingsPressed()
    {
        GD.Print("Opening settings...");

        // TODO: Phase 2 - Open settings menu
        // For now, just log
        GD.Print("Settings menu not implemented yet");
    }

    private void OnQuitPressed()
    {
        GD.Print("Quitting game...");
        GetTree().Quit();
    }

    // Phase 2: Add these methods when implementing save system
    /*
    private SaveData LoadSaveData()
    {
        if (!FileAccess.FileExists("user://save_game.json"))
            return null;

        using var file = FileAccess.Open("user://save_game.json", FileAccess.ModeFlags.Read);
        string jsonString = file.GetAsText();
        return JsonSerializer.Deserialize<SaveData>(jsonString);
    }
    */

    private void SetupButtonHoverEffect(Button button)
    {
        button.MouseEntered += () => OnButtonHover(button, true);
        button.MouseExited += () => OnButtonHover(button, false);
    }

    private void OnButtonHover(Button button, bool isHovering)
    {
        // Create smooth scale animation
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Cubic);

        if (isHovering)
        {
            tween.TweenProperty(button, "scale", new Vector2(1.05f, 1.05f), 0.15);
        }
        else
        {
            tween.TweenProperty(button, "scale", Vector2.One, 0.15);
        }
    }
}
