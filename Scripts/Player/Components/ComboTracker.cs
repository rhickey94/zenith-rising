using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Tracks combo state for basic attack chains.
/// Handles timing windows, auto-reset, and combo advancement.
/// </summary>
[GlobalClass]
public partial class ComboTracker : Node
{
    // Dependencies
    private Player _player;
    private SkillManager _skillManager;

    // Combo state
    private int _currentComboStep = 0; // 0 = not in combo, 1-4 = strike number
    private double _comboWindowExpiry = 0.0;

    // Configuration
    private const float ComboWindowDuration = 0.3f; // Time to press next input

    public void Initialize(Player player, SkillManager skillManager)
    {
        _player = player;
        _skillManager = skillManager;
    }

    public override void _Process(double delta)
    {
        // Check if combo window expired
        if (_currentComboStep > 0)
        {
            double currentTime = Time.GetTicksMsec() / 1000.0;
            if (currentTime > _comboWindowExpiry)
            {
                ResetCombo();
            }
        }
    }

    /// <summary>
    /// Called when player presses basic attack.
    /// Returns which strike in the combo to execute (1-4).
    /// </summary>
    public int GetNextComboStrike(Skill basicAttackSkill)
    {
        if (basicAttackSkill == null) return 1;

        // Get max combo length based on mastery tier
        int maxComboLength = GetMaxComboLength(basicAttackSkill.CurrentTier);

        // Determine next strike
        int nextStrike;
        if (_currentComboStep == 0)
        {
            // Starting fresh combo
            nextStrike = 1;
        }
        else if (_currentComboStep >= maxComboLength)
        {
            // Combo complete, start over
            nextStrike = 1;
            _currentComboStep = 0;
        }
        else
        {
            // Continue combo
            nextStrike = _currentComboStep + 1;
        }

        return nextStrike;
    }

    /// <summary>
    /// Called after a strike successfully executes.
    /// Updates combo state and sets timing window.
    /// </summary>
    public void OnStrikeExecuted(int strikeNumber, float strikeDuration)
    {
        _currentComboStep = strikeNumber;

        // Set combo window to expire after animation + window duration
        double currentTime = Time.GetTicksMsec() / 1000.0;
        _comboWindowExpiry = currentTime + strikeDuration + ComboWindowDuration;
    }

    /// <summary>
    /// Resets combo to Strike 1.
    /// Called when: timing window expires, player uses different skill, player gets hit, etc.
    /// </summary>
    public void ResetCombo()
    {
        _currentComboStep = 0;
        _comboWindowExpiry = 0.0;
    }

    /// <summary>
    /// Returns current combo step (0 = not in combo, 1-4 = strike number).
    /// </summary>
    public int GetCurrentComboStep() => _currentComboStep;

    /// <summary>
    /// Gets max combo length based on mastery tier.
    /// Bronze = 1, Silver = 2, Gold = 3, Diamond = 4
    /// </summary>
    private int GetMaxComboLength(SkillMasteryTier tier)
    {
        return tier switch
        {
            SkillMasteryTier.Bronze => 1,
            SkillMasteryTier.Silver => 2,
            SkillMasteryTier.Gold => 3,
            SkillMasteryTier.Diamond => 4,
            _ => 1
        };
    }
}
