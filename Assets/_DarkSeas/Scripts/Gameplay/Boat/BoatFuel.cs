using UnityEngine;
using DarkSeas.Core;

namespace DarkSeas.Gameplay.Boat
{
    /// <summary>
    /// Handles boat fuel consumption and management
    /// </summary>
    public class BoatFuel : MonoBehaviour
    {
        [Header("Fuel Settings")]
        [SerializeField] private float _fuelCapacity = 180f;
        [SerializeField] private float _baseConsumptionRate = 1f;
        [SerializeField] private float _throttleMultiplier = 1.5f;

        private float _currentFuel;
        private bool _isEmpty = false;
        private float _throttleAmount = 0f;

        public float CurrentFuel => _currentFuel;
        public float FuelCapacity => _fuelCapacity;
        public bool IsEmpty => _isEmpty;

        private void Awake()
        {
            _currentFuel = _fuelCapacity;
        }

        private void Update()
        {
            ConsumeFuel();
        }

        public void SetThrottleInput(float throttleAmount)
        {
            _throttleAmount = Mathf.Clamp01(Mathf.Abs(throttleAmount));
        }

        private void ConsumeFuel()
        {
            if (_isEmpty) return;

            float throttleMultiplier = 1f + (_throttleAmount * _throttleMultiplier);
            float consumption = _baseConsumptionRate * throttleMultiplier * Time.deltaTime;

            _currentFuel = Mathf.Max(0, _currentFuel - consumption);

            if (_currentFuel <= 0 && !_isEmpty)
            {
                _isEmpty = true;
                FuelSignals.InvokeEmpty(Time.time);
            }
        }

        public void RefillFuel()
        {
            _currentFuel = _fuelCapacity;
            _isEmpty = false;
        }
    }
}