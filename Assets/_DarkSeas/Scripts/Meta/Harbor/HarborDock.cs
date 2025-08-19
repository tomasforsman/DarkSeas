using UnityEngine;
using DarkSeas.Gameplay.Interaction;
using DarkSeas.Meta.Legacy;

namespace DarkSeas.Meta.Harbor
{
    /// <summary>
    /// Trigger zone that delivers passengers and awards Legacy Points.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class HarborDock : MonoBehaviour
    {
        [SerializeField] private bool _deliverOnEnter = true;
        [SerializeField] private float _cooldownSeconds = 1f;

        private float _lastDeliveryTime = -999f;

        private void Reset()
        {
            var col = GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_deliverOnEnter)
            {
                TryDeliver(other);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_deliverOnEnter)
            {
                TryDeliver(other);
            }
        }

        private void TryDeliver(Collider other)
        {
            if (Time.time - _lastDeliveryTime < _cooldownSeconds) return;

            var interactor = other.GetComponentInParent<RescueInteractor>();
            if (interactor != null && interactor.PassengerCount > 0)
            {
                int count = interactor.PassengerCount;
                LegacyManager.Instance.AddFromPassengers(count);
                interactor.DeliverPassengers();
                _lastDeliveryTime = Time.time;
                Debug.Log($"Delivered {count} passengers at harbor.");
            }
        }
    }
}

