using Godot;
using ZenithRising.Scripts.PlayerScripts.Components;
using ZenithRising.Scripts.UI.Menus;
using ZenithRising.Scripts.UI.Panels;

namespace ZenithRising.Scripts.Core;

/// <summary>
/// Singleton autoload that manages persistent UI elements accessible from any scene.
/// Handles pause menu, stat allocation panel, settings, and future UI systems.
/// </summary>
public partial class UIManager : CanvasLayer
{
    // ===== UI PANEL REFERENCES =====
    private StatAllocationPanel _statAllocationPanel;
    private PauseMenu _pauseMenu;

    // ===== STATE TRACKING =====
    private bool _isPaused = false;

    // ===== SINGLETON ACCESS =====
    public static UIManager Instance { get; private set; }

    // ===== LIFECYCLE =====
    public override void _Ready()
    {
        // Singleton pattern
        if (Instance != null)
        {
            GD.PrintErr("UIManager: Multiple instances detected! Removing duplicate.");
            QueueFree();
            return;
        }
        Instance = this;

        // Set CanvasLayer to highest layer for UI
        Layer = 200; // Above HUD (layer 100)

        // Get panel references
        _statAllocationPanel = GetNodeOrNull<StatAllocationPanel>("StatAllocationPanel");
        if (_statAllocationPanel == null)
        {
            GD.PrintErr("UIManager: StatAllocationPanel not found!");
        }

        _pauseMenu = GetNodeOrNull<PauseMenu>("PauseMenu");
        if (_pauseMenu == null)
        {
            GD.PrintErr("UIManager: PauseMenu not found!");
        }
        else
        {
            _pauseMenu.ResumePressed += OnResumePressed;
            _pauseMenu.ReturnToHubPressed += OnReturnToHubPressed;
            _pauseMenu.MainMenuPressed += OnMainMenuPressed;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
        {
            // ESC - Toggle pause menu
            if (keyEvent.Keycode == Key.Escape)
            {
                TogglePauseMenu();
                GetViewport().SetInputAsHandled();
            }
            // C - Toggle stat allocation panel
            else if (keyEvent.Keycode == Key.C)
            {
                ToggleStatAllocationPanel();
                GetViewport().SetInputAsHandled();
            }
        }
    }

    // ===== PUBLIC API =====

    /// <summary>
    /// Shows the stat allocation panel with player stats.
    /// </summary>
    public void ShowStatAllocationPanel(StatsManager statsManager, UpgradeManager upgradeManager, bool shouldPause)
    {
        if (_statAllocationPanel == null) return;

        _statAllocationPanel.ShowPanel(statsManager, upgradeManager, shouldPause);

        if (shouldPause)
        {
            _isPaused = true;
        }
    }

    /// <summary>
    /// Toggles the stat allocation panel.
    /// </summary>
    public void ToggleStatAllocationPanel()
    {
        if (_statAllocationPanel == null) return;

        if (_statAllocationPanel.Visible)
        {
            _statAllocationPanel.HidePanel();
            _isPaused = false;
        }
        else
        {
            // Get player components
            var player = GetTree().GetFirstNodeInGroup("player");
            if (player != null)
            {
                var statsManager = player.GetNodeOrNull<StatsManager>("StatsManager");
                var upgradeManager = player.GetNodeOrNull<UpgradeManager>("UpgradeManager");

                if (statsManager != null && upgradeManager != null)
                {
                    bool inDungeon = IsInDungeon();
                    ShowStatAllocationPanel(statsManager, upgradeManager, shouldPause: inDungeon);
                }
            }
        }
    }

    /// <summary>
    /// Toggles the pause menu.
    /// </summary>
    public void TogglePauseMenu()
    {
        if (_pauseMenu == null) return;

        if (_pauseMenu.Visible)
        {
            _pauseMenu.HideMenu();
            GetTree().Paused = false;
            _isPaused = false;
        }
        else
        {
            bool inHub = IsInHub();
            _pauseMenu.ShowMenu(inHub: inHub);
            GetTree().Paused = true;
            _isPaused = true;
        }
    }

    // ===== PRIVATE HELPERS =====

    /// <summary>
    /// Checks if the current scene is the hub scene.
    /// Type-based detection is more robust than string matching.
    /// </summary>
    private bool IsInHub()
    {
        var currentScene = GetTree().CurrentScene;
        if (currentScene == null) return false;

        // Type-based check (rename-safe)
        return currentScene is Hub;
    }

    /// <summary>
    /// Checks if the current scene is the dungeon scene.
    /// Type-based detection is more robust than string matching.
    /// </summary>
    private bool IsInDungeon()
    {
        var currentScene = GetTree().CurrentScene;
        if (currentScene == null) return false;

        // Type-based check (rename-safe)
        return currentScene is Dungeon;
    }

    // ===== EVENT HANDLERS =====

    private void OnResumePressed()
    {
        TogglePauseMenu();
    }

    private void OnReturnToHubPressed()
    {
        GetTree().Paused = false;
        _isPaused = false;
        GetTree().ChangeSceneToFile("res://Scenes/Core/hub.tscn");
    }

    private void OnMainMenuPressed()
    {
        GetTree().Paused = false;
        _isPaused = false;
        GetTree().ChangeSceneToFile("res://Scenes/UI/Menus/main_menu.tscn");
    }
}
