using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.PlayerScripts.Components;

[GlobalClass]
public partial class SkillManager : Node
{
    [Export] public Skill BasicAttackSkill { get; set; }
    [Export] public Skill SpecialAttackSkill { get; set; }

    [Export] public Skill PrimarySkill { get; set; }
    [Export] public Skill SecondarySkill { get; set; }
    [Export] public Skill UltimateSkill { get; set; }

    private float _basicAttackCooldownTimer = 0.0f;
    private float _specialAttackCooldownTimer = 0.0f;
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

        ValidateSkill(PrimarySkill, SkillSlot.Primary);
    }

    public void Update(float delta)
    {
        if (_basicAttackCooldownTimer > 0)
        {
            _basicAttackCooldownTimer -= delta;
        }

        if (_specialAttackCooldownTimer > 0)
        {
            _specialAttackCooldownTimer -= delta;
        }

        if (_primarySkillCooldownTimer > 0)
        {
            _primarySkillCooldownTimer -= delta;
        }

        if (_secondarySkillCooldownTimer > 0)
        {
            _secondarySkillCooldownTimer -= delta;
        }

        if (_ultimateSkillCooldownTimer > 0)
        {
            _ultimateSkillCooldownTimer -= delta;
        }

    }

    public void HandleInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && BasicAttackSkill != null)
            {
                UseSkill(BasicAttackSkill, ref _basicAttackCooldownTimer);
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Right && SpecialAttackSkill != null)
            {
                UseSkill(SpecialAttackSkill, ref _specialAttackCooldownTimer);
            }
        }

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

    private void ValidateSkill(Skill skill, SkillSlot expectedSlot)
    {
        if (skill == null)
        {
            return;
        }


        if (skill.AllowedClass != _player.CurrentClass)
        {
            GD.PrintErr($"Skill {skill.SkillName} is for {skill.AllowedClass}, but player is {_player.CurrentClass}!");
        }

        if (skill.Slot != expectedSlot)
        {
            GD.PrintErr($"Skill {skill.SkillName} is a {skill.Slot} skill, but equipped in {expectedSlot} slot!");
        }
    }
}
