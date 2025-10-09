using Godot;
using System;

[GlobalClass]
public partial class Skill : Resource
{
  [Export] public string SkillName { get; set; }
  [Export] public string Description { get; set; }
  [Export] public SkillType Type { get; set; }
  [Export] public float Cooldown { get; set; } = 0f; // For active skills
  [Export] public float EffectValue { get; set; } = 0f; // Generic effect value, interpretation depends on skill
  [Export] public float EffectDuration { get; set; } = 0f; // Duration for effects like buffs/debuffs
  [Export] public float Range { get; set; } = 150f; 
  [Export] public PackedScene SkillEffectScene { get; set; } // Optional visual effect scene

  private ISkillExecutor _executor;

  public void Execute(Player player)
  {
    if (_executor == null)
    {
      // Instantiate executor based on skill name
      _executor = CreateExecutor();
    }

    if (_executor != null)
    {
      _executor.ExecuteSkill(player, this);
    }
  }

  private ISkillExecutor CreateExecutor()
  {
    // Simple factory based on skill name
    return SkillName switch
    {
      "Whirlwind" => new Whirlwind(),
      // "Fireball" => new Fireball(), // You'll add this later
      _ => null
    };
  }
}

public enum SkillType
{
  Active,
  Passive
}