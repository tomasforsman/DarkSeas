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

        private void Update()
        {
            UpdateFuelDisplay();
            UpdateHullDisplay();
            UpdatePassengerDisplay();
            UpdateCompassDisplay();
        }

        private void UpdateFuelDisplay()
        {
            if (_fuelBar != null && _boatFuel != null)
            {
                _fuelBar.value = _boatFuel.CurrentFuel / _boatFuel.FuelCapacity;
            }
        }

        private void UpdateHullDisplay()
        {
            if (_hullBar != null && _boatDamage != null)
            {
                _hullBar.value = (float)_boatDamage.CurrentHullHP / _boatDamage.MaxHullHP;
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
            if (_compassText != null && _harborTransform != null)
            {
                float distance = Vector3.Distance(transform.position, _harborTransform.position);
                Vector3 direction = (_harborTransform.position - transform.position).normalized;
                
                _compassText.text = $"Harbor: {distance:F0}m";
                
                // TODO: Add directional arrow or bearing
            }
        }
    }
}