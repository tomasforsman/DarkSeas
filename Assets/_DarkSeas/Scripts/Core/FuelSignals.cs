using System;

namespace DarkSeas.Core
{
    /// <summary>
    /// Static signal hub for fuel-related events
    /// </summary>
    public static class FuelSignals
    {
        public static event Action<float> Empty;

        public static void InvokeEmpty(float time) => Empty?.Invoke(time);
    }
}