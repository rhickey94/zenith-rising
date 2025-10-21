using System.Collections.Generic;
using Godot;

namespace ZenithRising.Scripts.PlayerScripts.Components;

public partial class BuffManager : Node
{
    [Signal] public delegate void BuffsChangedEventHandler();

    private readonly Dictionary<string, BuffInstance> _activeBuffs = [];

    public override void _Process(double delta)
    {
        UpdateBuffs((float)delta);
    }

    public void ApplyBuff(string buffId, float duration,
    float attackSpeedBonus = 0f,
    float moveSpeedBonus = 0f,
    float castSpeedBonus = 0f,
    float damageBonus = 0f,
    float cooldownReductionBonus = 0f,
    float damageReductionBonus = 0f)
    {
        _activeBuffs[buffId] = new BuffInstance
        {
            AttackSpeedBonus = attackSpeedBonus,
            MoveSpeedBonus = moveSpeedBonus,
            CastSpeedBonus = castSpeedBonus,
            DamageBonus = damageBonus,
            CooldownReductionBonus = cooldownReductionBonus,
            DamageReductionBonus = damageReductionBonus,
            Duration = duration
        };

        TriggerStatRecalculation();
    }

    public bool HasBuff(string buffId)
    {
        return _activeBuffs.ContainsKey(buffId);
    }

    public void RemoveBuff(string buffId)
    {
        if (_activeBuffs.Remove(buffId))
        {
            TriggerStatRecalculation();
        }
    }

    // PUBLIC API: UpgradeManager calls this to get current buff modifiers
    public StatModifiers GetStatModifiers()
    {
        var modifiers = new StatModifiers();

        foreach (var buff in _activeBuffs.Values)
        {
            modifiers.PercentAttackSpeed += buff.AttackSpeedBonus;
            modifiers.PercentSpeed += buff.MoveSpeedBonus;
            modifiers.PercentDamage += buff.DamageBonus;
            modifiers.PercentCastSpeed += buff.CastSpeedBonus; // NEW
            modifiers.PercentCooldownReduction += buff.CooldownReductionBonus; // NEW
        }

        return modifiers;
    }

    private void UpdateBuffs(float delta)
    {
        var expiredBuffs = new List<string>();

        foreach (var kvp in _activeBuffs)
        {
            kvp.Value.Duration -= delta;
            if (kvp.Value.Duration <= 0)
            {
                expiredBuffs.Add(kvp.Key);
            }
        }

        foreach (var buffId in expiredBuffs)
        {
            _activeBuffs.Remove(buffId);
        }

        if (expiredBuffs.Count > 0)
        {
            TriggerStatRecalculation();
        }
    }

    private void TriggerStatRecalculation()
    {
        EmitSignal(SignalName.BuffsChanged);
    }

    private class BuffInstance
    {
        public float AttackSpeedBonus;
        public float CastSpeedBonus;
        public float CooldownReductionBonus;
        public float MoveSpeedBonus;
        public float DamageBonus;
        public float DamageReductionBonus;
        public float Duration;
    }
}
