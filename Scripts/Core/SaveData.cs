using System.Collections.Generic;
using ZenithRising.Scripts.Progression.Upgrades;

namespace ZenithRising.Scripts.Core;

public struct SaveData
{
    // ===== VERSION & METADATA =====
    public int Version { get; set; }
    public string LastSaved { get; set; }
    public bool IsInHub { get; set; }

    // ===== CHARACTER STATS (PERMANENT) =====
    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Agility { get; set; }
    public int Vitality { get; set; }
    public int Fortune { get; set; }

    // ===== CHARACTER PROGRESSION =====
    public int CharacterLevel { get; set; }
    public int CharacterExperience { get; set; }  // Current XP toward next level
    public int UnallocatedStatPoints { get; set; }

    // ===== RUN PROGRESSION =====
    public int HighestFloorReached { get; set; }

    // ===== RUN STATE (CHECKPOINT) =====
    public int CurrentFloor { get; set; }        // Floor to resume at (1-5)
    public int PowerLevel { get; set; }          // Current run level
    public bool HasActiveRun { get; set; }       // Is there an active run to resume?

    // Active upgrades (serialized as Dictionary)
    public Dictionary<UpgradeType, float> ActiveUpgrades { get; set; }
}
