using UnityEngine;
using UnityEngine.UI;
using DarkSeas.Gameplay.Boat;
using DarkSeas.Gameplay.Interaction;

namespace DarkSeas.UI
{
    /// <summary>
    /// Main HUD controller displaying fuel, hull, passengers, and compass
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Slider _fuelBar;
        [SerializeField] private Slider _hullBar;
        [SerializeField] private Text _passengerText;
        [SerializeField] private Text _compassText;

        [Header("References")]
        [SerializeField] private BoatFuel _boatFuel;
        [SerializeField] private BoatDamage _boatDamage;
        [SerializeField] private RescueInteractor _rescueInteractor;
        [SerializeField] private Transform _harborTransform;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _smoothSpeed = 8f;

        private void Update()
        {
            if (_playerTransform == null && _boatFuel != null)
            {
                _playerTransform = _boatFuel.transform;
            }
            UpdateFuelDisplay();
            UpdateHullDisplay();
            UpdatePassengerDisplay();
            UpdateCompassDisplay();
        }

        private void UpdateFuelDisplay()
        {
            if (_fuelBar != null && _boatFuel != null)
            {
                float target = _boatFuel.CurrentFuel / _boatFuel.FuelCapacity;
                _fuelBar.value = Mathf.Lerp(_fuelBar.value, target, Time.deltaTime * _smoothSpeed);
            }
        }

        private void UpdateHullDisplay()
        {
            if (_hullBar != null && _boatDamage != null)
            {
                float target = (float)_boatDamage.CurrentHullHP / _boatDamage.MaxHullHP;
                _hullBar.value = Mathf.Lerp(_hullBar.value, target, Time.deltaTime * _smoothSpeed);
            }
        }

        private void UpdatePassengerDisplay()
        {
            if (_passengerText != null && _rescueInteractor != null)
            {
                _passengerText.text = $"{_rescueInteractor.PassengerCount}/{_rescueInteractor.MaxPassengers}";
            }
        }

        private void UpdateCompassDisplay()
        {
            if (_compassText != null && _harborTransform != null && _playerTransform != null)
            {
                float distance = Vector3.Distance(_playerTransform.position, _harborTransform.position);
                Vector3 direction = (_harborTransform.position - _playerTransform.position).normalized;
                
                _compassText.text = $"Harbor: {distance:F0}m";
                
                // TODO: Add directional arrow or bearing
            }
        }
    }
}
