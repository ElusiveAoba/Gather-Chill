using System.Collections.Generic;

namespace GatherChill.Utilities.Tools;

/// <summary>
/// Each action that has a random delay checks each tick if the delay has passed once it has return true.
/// Reset the delay after <see cref="RestartAfterGapMs"/> has passed.
/// </summary>
internal static class RandomDelay
{
    private const int RestartAfterGapMs = 1000;

    private static readonly Dictionary<string, (DateTime ReadyAt, DateTime LastAsked)> Delays = new();

    public static bool Passed(string key, int minMs, int maxMs)
    {
        var now = DateTime.UtcNow;

        if (!Delays.TryGetValue(key, out var entry) || (now - entry.LastAsked).TotalMilliseconds > RestartAfterGapMs)
        {
            var min = Math.Max(0, Math.Min(minMs, maxMs));
            var max = Math.Max(minMs, maxMs);
            entry.ReadyAt = now.AddMilliseconds(Random.Shared.Next(min, max + 1));
        }

        entry.LastAsked = now;
        Delays[key] = entry;
        return now >= entry.ReadyAt;
    }
}
