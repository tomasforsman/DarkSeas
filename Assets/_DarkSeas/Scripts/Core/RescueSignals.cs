using System;

namespace DarkSeas.Core
{
    /// <summary>
    /// Static signal hub for rescue-related events
    /// </summary>
    public static class RescueSignals
    {
        public static event Action<string> Started;
        public static event Action<string, float> Progress; // id, 0..1
        public static event Action<string> Canceled;
        public static event Action<string, float> PickedUp;

        public static void InvokeStarted(string id) => Started?.Invoke(id);
        public static void InvokeProgress(string id, float pct) => Progress?.Invoke(id, pct);
        public static void InvokeCanceled(string id) => Canceled?.Invoke(id);
        public static void InvokePickedUp(string id, float time) => PickedUp?.Invoke(id, time);
    }
}
