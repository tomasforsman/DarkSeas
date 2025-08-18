using System;

namespace DarkSeas.Core
{
    /// <summary>
    /// Static signal hub for run lifecycle events
    /// </summary>
    public static class RunSignals
    {
        public static event Action<int> RunStart;
        public static event Action<string, int> RunEnd;

        public static void InvokeRunStart(int seed) => RunStart?.Invoke(seed);
        public static void InvokeRunEnd(string result, int rescuedCount) => RunEnd?.Invoke(result, rescuedCount);
    }
}