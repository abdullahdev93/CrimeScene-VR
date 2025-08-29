using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct EvidenceEvent
{
    public string evidenceId;  // e.g., "KNIFE_001"
    public string action;      // e.g., "BAGGED"
}

public static class EvidenceLog
{
    static readonly List<EvidenceEvent> events = new();

    public static void Record(string evidenceId, string action)
    {
        events.Add(new EvidenceEvent{ evidenceId = evidenceId, action = action });
    }

    public static IReadOnlyList<EvidenceEvent> All => events;
}
