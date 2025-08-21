using UnityEngine;

namespace DarkSeas.MetaProgression.Legacy
{
    /// <summary>
    /// Tracks Legacy Points earned from deliveries. Simple singleton for MVP.
    /// </summary>
    public class LegacyManager : MonoBehaviour
    {
        private static LegacyManager _instance;
        public static LegacyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("LegacyManager");
                    _instance = go.AddComponent<LegacyManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [SerializeField] private int _legacyPoints = 0;
        [SerializeField] private int _pointsPerPassenger = 1;

        public int LegacyPoints => _legacyPoints;
        public int PointsPerPassenger => _pointsPerPassenger;

        public void AddFromPassengers(int passengerCount)
        {
            if (passengerCount <= 0) return;
            _legacyPoints += passengerCount * _pointsPerPassenger;
            Debug.Log($"LegacyManager: +{passengerCount * _pointsPerPassenger} points (total: {_legacyPoints})");
        }
    }
}

