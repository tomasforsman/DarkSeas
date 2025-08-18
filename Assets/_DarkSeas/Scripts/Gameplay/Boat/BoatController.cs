using UnityEngine;

namespace DarkSeas.Gameplay.Boat
{
    /// <summary>
    /// Handles boat movement and physics using Rigidbody
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _maxSpeed = 8f;
        [SerializeField] private float _acceleration = 2f;
        [SerializeField] private float _turnRateDegPerSec = 90f;

        private Rigidbody _rigidbody;
        private Vector2 _inputVector;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleInput();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void HandleInput()
        {
            // TODO: Implement Input System integration
            _inputVector.x = Input.GetAxis("Horizontal");
            _inputVector.y = Input.GetAxis("Vertical");
        }

        private void ApplyMovement()
        {
            // TODO: Implement boat physics
        }
    }
}