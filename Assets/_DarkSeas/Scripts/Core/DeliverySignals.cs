using System;

namespace DarkSeas.Core
{
    /// <summary>
    /// Signals related to harbor deliveries.
    /// </summary>
    public static class DeliverySignals
    {
        public static event Action<int, int> Delivered; // count, points

        public static void InvokeDelivered(int count, int points) => Delivered?.Invoke(count, points);
    }
}

