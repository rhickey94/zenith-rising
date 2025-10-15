using System.Collections.Generic;
using Godot;
using SpaceTower.Scripts.Core;

namespace SpaceTower.Scripts.UI.Menus;

public partial class MainMenu : Control
{
    // ===== CONSTANTS =====
    private const float HoverScaleAmount = 1.05f;
    private const float HoverAnimationDuration = 0.15f;

    // ===== EXPORT FIELDS =====
    [Export] public string HubScenePath = "res://Scenes/Core/hub.tscn";

    // ===== PRIVATE FIELDS - UI References =====
    private Button _startButton;
    private Button _continueButton;
    private Button _settingsButton;
    private Button _quitButton;
    private Label _progressLabel;

    // ===== PRIVATE FIELDS - State =====
    private readonly Dictionary<Button, Tween> _activeHoverTweens = new();

    // ===== LIFECYCLE METHODS =====
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

    // ===== PRIVATE HELPERS - Save System =====
    private void CheckForSaveFile()
    {
        bool saveExists = SaveManager.Instance?.SaveExists() ?? false;
        _continueButton.Visible = saveExists;

        GD.Print($"Save file exists: {saveExists}");
    }
    private void UpdateProgressDisplay()
    {
        if (SaveManager.Instance == null || !SaveManager.Instance.SaveExists())
        {
            _progressLabel.Text = "No Save Data";
            return;
        }

        SaveData? saveData = SaveManager.Instance.LoadGame();
        if (saveData.HasValue)
        {
            var data = saveData.Value;
            _progressLabel.Text = $"Level {data.CharacterLevel} | Floor {data.HighestFloorReached}";
        }
        else
        {
            _progressLabel.Text = "Save Load Error";
        }
    }

    // ===== BUTTON HANDLERS =====
    private void OnStartPressed()
    {
        GD.Print("Starting new game...");

        // Delete existing save for fresh start
        SaveManager.Instance?.DeleteSave();

        GetTree().ChangeSceneToFile(HubScenePath);
    }

    private void OnContinuePressed()
    {
        GD.Print("Continuing saved game...");

        // Load will happen in Hub._Ready() via Player.Initialize()
        GetTree().ChangeSceneToFile(HubScenePath);
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
        // Kill existing tween to prevent memory leak
        if (_activeHoverTweens.TryGetValue(button, out var oldTween))
        {
            oldTween?.Kill();
        }

        // Create smooth scale animation
        var tween = CreateTween();
        _activeHoverTweens[button] = tween;

        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Cubic);

        if (isHovering)
        {
            tween.TweenProperty(button, "scale", new Vector2(HoverScaleAmount, HoverScaleAmount), HoverAnimationDuration);
        }
        else
        {
            tween.TweenProperty(button, "scale", Vector2.One, HoverAnimationDuration);
        }
    }
}
