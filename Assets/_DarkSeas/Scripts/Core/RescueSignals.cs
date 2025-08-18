using System;

namespace DarkSeas.Core
{
    /// <summary>
    /// Static signal hub for rescue-related events
    /// </summary>
    public static class RescueSignals
    {
        public static event Action<string, float> PickedUp;

        public static void InvokePickedUp(string id, float time) => PickedUp?.Invoke(id, time);
    }
}