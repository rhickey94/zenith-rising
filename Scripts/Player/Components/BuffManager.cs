using System.Collections.Generic;
using Godot;

namespace ZenithRising.Scripts.PlayerScripts.Components;

/// <summary>
/// Temporary buff (de)buff lifecycle management for player character.
/// Responsibilities:
/// - Buff storage and lifetime tracking (duration countdown)
/// - Buff stacking behavior (duration refresh, stat bonuses accumulate)
/// - Stat modifier aggregation for UpgradeManager
/// - Triggering stat recalculation when buffs change
/// Example: Combat Stim skill applies +40% attack speed, +100% move speed for 5 seconds.
/// Does NOT handle: Buff application logic (SkillManager), stat calculation (StatsManager)
/// </summary>
[GlobalClass]
public partial class BuffManager : Node
{
    // ═══════════════════════════════════════════════════════════════
    // SIGNALS
    // ═══════════════════════════════════════════════════════════════
    [Signal] public delegate void BuffsChangedEventHandler();

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE FIELDS
    // ═══════════════════════════════════════════════════════════════
    private readonly Dictionary<string, BuffInstance> _activeBuffs = [];

    // ═══════════════════════════════════════════════════════════════
    // LIFECYCLE METHODS
    // ═══════════════════════════════════════════════════════════════
    public override void _Process(double delta)
    {
        UpdateBuffs((float)delta);
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - BUFF APPLICATION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Applies or refreshes a buff. If buffId exists, duration refreshes (stacking).
    /// Triggers BuffsChanged signal → UpgradeManager → StatsManager.RecalculateStats().
    /// </summary>
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

    /// <summary>
    /// Checks if a specific buff is currently active.
    /// </summary>
    public bool HasBuff(string buffId)
    {
        return _activeBuffs.ContainsKey(buffId);
    }

    /// <summary>
    /// Manually removes a buff by ID. Triggers stat recalculation.
    /// </summary>
    public void RemoveBuff(string buffId)
    {
        if (_activeBuffs.Remove(buffId))
        {
            TriggerStatRecalculation();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // PUBLIC API - STAT MODIFIER AGGREGATION
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Aggregates stat bonuses from all active buffs.
    /// Called by UpgradeManager.RecalculateAllStats() to combine with upgrade modifiers.
    /// </summary>
    public StatModifiers GetStatModifiers()
    {
        var modifiers = new StatModifiers();

        foreach (var buff in _activeBuffs.Values)
        {
            modifiers.PercentAttackSpeed += buff.AttackSpeedBonus;
            modifiers.PercentSpeed += buff.MoveSpeedBonus;
            modifiers.PercentDamage += buff.DamageBonus;
            modifiers.PercentCastSpeed += buff.CastSpeedBonus;
            modifiers.PercentCooldownReduction += buff.CooldownReductionBonus;
        }

        return modifiers;
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS - BUFF LIFETIME
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Counts down buff durations and removes expired buffs.
    /// Called every frame from _Process().
    /// </summary>
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

    /// <summary>
    /// Emits BuffsChanged signal to trigger stat recalculation.
    /// UpgradeManager listens to this signal and calls StatsManager.RecalculateStats().
    /// </summary>
    private void TriggerStatRecalculation()
    {
        EmitSignal(SignalName.BuffsChanged);
    }

    // ═══════════════════════════════════════════════════════════════
    // PRIVATE CLASSES
    // ═══════════════════════════════════════════════════════════════
    /// <summary>
    /// Internal storage for a single buff's data.
    /// Duration counts down each frame until buff expires.
    /// </summary>
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

