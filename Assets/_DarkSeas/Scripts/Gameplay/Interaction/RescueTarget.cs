using UnityEngine;

namespace DarkSeas.Gameplay.Interaction
{
    /// <summary>
    /// Represents a survivor that can be rescued
    /// </summary>
    public class RescueTarget : MonoBehaviour
    {
        [Header("Rescue Properties")]
        [SerializeField] private string _id;
        [SerializeField] private float _interactionRadius = 3f;
        [SerializeField] private bool _isClaimed = false;

        public string Id => _id;
        public bool IsClaimed => _isClaimed;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = System.Guid.NewGuid().ToString();
            }
        }
        
        private void Start()
        {
            // Register this target for efficient detection
            RescueInteractor.RegisterTarget(this);
        }
        
        private void OnDestroy()
        {
            // Unregister when destroyed
            RescueInteractor.UnregisterTarget(this);
        }

        public bool InRange(Transform target)
        {
            if (_isClaimed || target == null) return false;
            
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= _interactionRadius;
        }

        public string Claim()
        {
            if (_isClaimed) return null;
            
            _isClaimed = true;
            // Unregister when claimed to remove from available targets
            RescueInteractor.UnregisterTarget(this);
            gameObject.SetActive(false);
            return _id;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _interactionRadius);
        }
    }
}