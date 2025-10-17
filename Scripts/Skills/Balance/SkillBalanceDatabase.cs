using Godot;
using System.Collections.Generic;

namespace ZenithRising.Scripts.Skills.Balance;

[GlobalClass]
public partial class SkillBalanceDatabase : Resource
{
    [Export] public Godot.Collections.Array<SkillBalanceEntry> Skills { get; set; } = [];

    private Dictionary<string, SkillBalanceEntry> _skillLookup;

    /// <summary>
    /// Gets skill balance data by SkillId. Call this during skill initialization.
    /// </summary>
    public SkillBalanceEntry GetSkillBalance(string skillId)
    {
        // Lazy-load lookup dictionary
        if (_skillLookup == null)
        {
            BuildLookupTable();
        }

        if (_skillLookup.TryGetValue(skillId, out var entry))
        {
            return entry;
        }

        GD.PrintErr($"SkillBalanceDatabase: No entry found for skillId '{skillId}'!");
        return null;
    }

    private void BuildLookupTable()
    {
        _skillLookup = [];

        foreach (var skill in Skills)
        {
            if (skill == null || string.IsNullOrEmpty(skill.SkillId))
            {
                GD.PrintErr("SkillBalanceDatabase: Found null or invalid skill entry!");
                continue;
            }

            if (_skillLookup.ContainsKey(skill.SkillId))
            {
                GD.PrintErr($"SkillBalanceDatabase: Duplicate SkillId '{skill.SkillId}' detected!");
                continue;
            }

            _skillLookup[skill.SkillId] = skill;
        }

        GD.Print($"SkillBalanceDatabase: Loaded {_skillLookup.Count} skill entries");
    }
}
