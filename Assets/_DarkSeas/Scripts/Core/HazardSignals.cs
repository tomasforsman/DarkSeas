using System;

namespace DarkSeas.Core
{
    /// <summary>
    /// Static signal hub for hazard-related events
    /// </summary>
    public static class HazardSignals
    {
        public static event Action<int, float> CollideIce;

        public static void InvokeCollideIce(int size, float relativeSpeed) => CollideIce?.Invoke(size, relativeSpeed);
    }
}