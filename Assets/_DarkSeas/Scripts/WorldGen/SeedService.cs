using UnityEngine;
using System;

namespace DarkSeas.WorldGen
{
    /// <summary>
    /// Service for managing world generation seeds
    /// </summary>
    public static class SeedService
    {
        public static int GetDailySeed()
        {
            DateTime today = DateTime.Today;
            return today.GetHashCode();
        }

        public static int GetRandomSeed()
        {
            return UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

        public static int CreateSeedFromString(string input)
        {
            return string.IsNullOrEmpty(input) ? 0 : input.GetHashCode();
        }
    }
}