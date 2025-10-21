using Godot;

namespace ZenithRising.Scripts.UI.Menus;

/// <summary>
/// Pause menu accessible via ESC key from any scene.
/// Context-aware: hides "Return to Hub" button when already in hub.
/// </summary>
public partial class PauseMenu : Control
{
    // ===== NODE REFERENCES =====
    private Button _resumeButton;
    private Button _returnToHubButton;
    private Button _mainMenuButton;

    // ===== SIGNALS =====
    [Signal] public delegate void ResumePressedEventHandler();
    [Signal] public delegate void ReturnToHubPressedEventHandler();
    [Signal] public delegate void MainMenuPressedEventHandler();

    // ===== LIFECYCLE =====
    public override void _Ready()
    {
        // Get button references
        _resumeButton = GetNode<Button>("%ResumeButton");
        _returnToHubButton = GetNode<Button>("%ReturnToHubButton");
        _mainMenuButton = GetNode<Button>("%MainMenuButton");

        ValidateDependencies();

        // Connect signals
        _resumeButton.Pressed += OnResumePressed;
        _returnToHubButton.Pressed += OnReturnToHubPressed;
        _mainMenuButton.Pressed += OnMainMenuPressed;

        Hide();
    }

    // ===== PUBLIC API =====

    /// <summary>
    /// Shows the pause menu with context-aware buttons.
    /// </summary>
    /// <param name="inHub">Whether the player is currently in the hub (hides "Return to Hub" if true)</param>
    public void ShowMenu(bool inHub)
    {
        // Hide "Return to Hub" button if already in hub
        _returnToHubButton.Visible = !inHub;

        Show();
    }

    /// <summary>
    /// Hides the pause menu.
    /// </summary>
    public void HideMenu()
    {
        Hide();
    }

    // ===== PRIVATE EVENT HANDLERS =====

    private void OnResumePressed()
    {
        EmitSignal(SignalName.ResumePressed);
    }

    private void OnReturnToHubPressed()
    {
        EmitSignal(SignalName.ReturnToHubPressed);
    }

    private void OnMainMenuPressed()
    {
        EmitSignal(SignalName.MainMenuPressed);
    }

    private void ValidateDependencies()
    {
        if (_resumeButton == null || _returnToHubButton == null || _mainMenuButton == null)
        {
            GD.PrintErr("PauseMenu: Missing button references!");
        }
    }
}
