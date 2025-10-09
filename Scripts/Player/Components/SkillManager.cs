using Godot;
using System;

[GlobalClass]
public partial class SkillManager : Node
{
  [Export] public Skill PrimarySkill { get; set; }
  [Export] public Skill SecondarySkill { get; set; }
  [Export] public Skill UltimateSkill { get; set; }

  private float _primarySkillCooldownTimer = 0.0f;
  private float _secondarySkillCooldownTimer = 0.0f;
  private float _ultimateSkillCooldownTimer = 0.0f;

  private Player _player;

  public override void _Ready()
  {
    _player = GetParent<Player>();
    if (_player == null)
    {
      GD.PrintErr("SkillManager: Could not find Player parent!");
    }
  }

  public void Update(float delta)
  {
    if (_primarySkillCooldownTimer > 0)
      _primarySkillCooldownTimer -= delta;
    if (_secondarySkillCooldownTimer > 0)
      _secondarySkillCooldownTimer -= delta;
    if (_ultimateSkillCooldownTimer > 0)
      _ultimateSkillCooldownTimer -= delta;
  }

  public void HandleInput(InputEvent @event)
  {
    if (@event is InputEventKey eventKey && eventKey.Pressed)
    {
      if (eventKey.Keycode == Key.Q && PrimarySkill != null)
      {
        UseSkill(PrimarySkill, ref _primarySkillCooldownTimer);
      }
      else if (eventKey.Keycode == Key.E && SecondarySkill != null)
      {
        UseSkill(SecondarySkill, ref _secondarySkillCooldownTimer);
      }
      else if (eventKey.Keycode == Key.R && UltimateSkill != null)
      {
        UseSkill(UltimateSkill, ref _ultimateSkillCooldownTimer);
      }
    }
  }

  private void UseSkill(Skill skill, ref float cooldownRemaining)
  {
    if (skill == null)
    {
      GD.Print("No skill equipped!");
      return;
    }

    if (cooldownRemaining > 0)
    {
      GD.Print($"{skill.SkillName} on cooldown: {cooldownRemaining:F1}s remaining");
      return;
    }

    GD.Print($"Using {skill.SkillName}!");

    // Execute skill effect
    skill.Execute(_player);

    // Start cooldown
    cooldownRemaining = skill.Cooldown;
  }
}
