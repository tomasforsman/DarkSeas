using DarkSeas.Gameplay.Interaction;
using DarkSeas.MetaProgression.Legacy;
using UnityEngine;

namespace DarkSeas.MetaProgression.Harbor
{
    /// <summary>
    /// Manages harbor functionality and survivor delivery
    /// </summary>
    public class HarborManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LegacyManager _legacyManager;
        [SerializeField] private Transform _dockingPoint;
        [SerializeField] private float _dockingRadius = 5f;

        private void Update()
        {
            CheckForDocking();
        }

        private void CheckForDocking()
        {
            // TODO: Find boat in scene and check distance to docking point
            var rescueInteractor = FindObjectOfType<RescueInteractor>();
            if (rescueInteractor != null && IsInDockingRange(rescueInteractor.transform))
            {
                ProcessDocking(rescueInteractor);
            }
        }

        private bool IsInDockingRange(Transform boat)
        {
            if (_dockingPoint == null || boat == null) return false;
            
            float distance = Vector3.Distance(_dockingPoint.position, boat.position);
            return distance <= _dockingRadius;
        }

        private void ProcessDocking(RescueInteractor rescueInteractor)
        {
            int passengerCount = rescueInteractor.PassengerCount;
            if (passengerCount > 0)
            {
                // Convert rescued survivors to Legacy Points
                _legacyManager.AddFromPassengers(passengerCount);
                rescueInteractor.DeliverPassengers();
                
                // TODO: Trigger visual/audio feedback
                // TODO: Update harbor visuals based on total rescued
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_dockingPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_dockingPoint.position, _dockingRadius);
            }
        }
    }
}
