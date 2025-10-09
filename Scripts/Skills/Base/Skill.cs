using Godot;
using SpaceTower.Scripts.PlayerScripts;
using SpaceTower.Scripts.Skills.Executors.Mage;
using SpaceTower.Scripts.Skills.Executors.Warrior;

namespace SpaceTower.Scripts.Skills.Base;

[GlobalClass]
public partial class Skill : Resource
{
    [Export] public string SkillName { get; set; }
    [Export] public string Description { get; set; }
    [Export] public SkillType Type { get; set; }
    [Export] public float Cooldown { get; set; } = 0f; // For active skills

    [Export] public PlayerClass AllowedClass { get; set; }
    [Export] public SkillSlot Slot { get; set; }

    [Export] public PackedScene SkillEffectScene { get; set; } // Optional visual effect scene

    private ISkillExecutor _executor;

    public void Execute(Player player)
    {
        // Instantiate executor based on skill name

        _executor ??= CreateExecutor();

        _executor?.ExecuteSkill(player, this);
    }

    private ISkillExecutor CreateExecutor()
    {
        // Simple factory based on skill name

        return SkillName switch
        {
            "Whirlwind" => new Whirlwind(),
            "Fireball" => new ProjectileSkillExecutor(),
            "Basic Strike" => new WarriorBasicAttack(),
            _ => null
        };
    }
}

public enum SkillType
{
    Active,
    Passive
}

public enum SkillSlot
{
    BasicAttack,
    SpecialAttack,

    Primary,    // Q key
    Secondary,  // E key
    Ultimate    // R key
}
