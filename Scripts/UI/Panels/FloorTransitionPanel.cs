using Godot;

namespace ZenithRising.Scripts.UI.Panels;

public partial class FloorTransitionPanel : Control
{
    // Signals
    [Signal]
    public delegate void ContinueButtonPressedEventHandler();

    [Signal]
    public delegate void EndRunButtonPressedEventHandler();

    [Export] public NodePath TitleLabelPath = "CenterContainer/ContentPanel/ContentVBox/TitleLabel";
    [Export] public NodePath FloorInfoLabelPath = "CenterContainer/ContentPanel/ContentVBox/FloorInfoLabel";
    [Export] public NodePath HintLabelPath = "CenterContainer/ContentPanel/ContentVBox/HintLabel";
    [Export] public NodePath ContinueButtonPath = "CenterContainer/ContentPanel/ContentVBox/ButtonsContainer/ContinueButton";
    [Export] public NodePath EndRunButtonPath = "CenterContainer/ContentPanel/ContentVBox/ButtonsContainer/EndRunButton";

    private const float HoverScaleAmount = 1.03f;
    private const float HoverAnimationDuration = 0.15f;

    // UI References
    private Label _titleLabel;
    private Label _floorInfoLabel;
    private Label _hintLabel;
    private Button _continueButton;
    private Button _endRunButton;
    private Tween _activeTween;

    public override void _Ready()
    {
        // Get node references
        _titleLabel = GetNode<Label>(TitleLabelPath);
        _floorInfoLabel = GetNode<Label>(FloorInfoLabelPath);
        _hintLabel = GetNode<Label>(HintLabelPath);
        _continueButton = GetNode<Button>(ContinueButtonPath);
        _endRunButton = GetNode<Button>(EndRunButtonPath);

        // Connect button signals
        _continueButton.Pressed += OnContinuePressed;
        _endRunButton.Pressed += OnEndRunPressed;

        // Add hover effects
        SetupButtonHoverEffect(_continueButton);
        SetupButtonHoverEffect(_endRunButton);

        // Initially hidden
        Visible = false;
    }

    /// <summary>
    /// Shows the panel with information about current and next floor
    /// </summary>
    public void ShowPanel(int currentFloor, int nextFloor)
    {
        if (currentFloor < 1 || nextFloor < 1)
        {
            GD.PrintErr($"Invalid floor values: {currentFloor} → {nextFloor}");
            return;
        }

        // Update text
        _titleLabel.Text = $"FLOOR {currentFloor} CLEARED!";
        _floorInfoLabel.Text = $"Floor {currentFloor} → Floor {nextFloor}";
        _continueButton.Text = $"Continue to Floor {nextFloor}";
        _hintLabel.Text = $"Enemies get +50% stronger each floor";

        // Show panel and pause game
        Visible = true;
        GetTree().Paused = true;
    }

    /// <summary>
    /// Hides the panel and unpauses the game
    /// </summary>
    public void HidePanel()
    {
        Visible = false;
        GetTree().Paused = false;
    }

    private void OnContinuePressed()
    {
        HidePanel();
        EmitSignal(SignalName.ContinueButtonPressed);
    }

    private void OnEndRunPressed()
    {
        HidePanel();
        EmitSignal(SignalName.EndRunButtonPressed);
    }

    // Optional: Hover effect for buttons
    private void SetupButtonHoverEffect(Button button)
    {
        button.MouseEntered += () => OnButtonHover(button, true);
        button.MouseExited += () => OnButtonHover(button, false);
    }

    private void OnButtonHover(Button button, bool isHovering)
    {
        // Kill previous tween if it exists
        if (_activeTween != null && _activeTween.IsValid())
        {
            _activeTween.Kill();
        }

        _activeTween = CreateTween();
        _activeTween.SetEase(Tween.EaseType.Out);
        _activeTween.SetTrans(Tween.TransitionType.Cubic);

        Vector2 targetScale = isHovering
            ? new Vector2(HoverScaleAmount, HoverScaleAmount)
            : Vector2.One;

        _activeTween.TweenProperty(button, "scale", targetScale, HoverAnimationDuration);
    }
}
