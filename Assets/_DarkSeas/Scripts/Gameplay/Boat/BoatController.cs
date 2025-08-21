using UnityEngine;
using DarkSeas.Data;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace DarkSeas.Gameplay.Boat
{
    /// <summary>
    /// Handles boat movement and physics using Rigidbody
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private BoatConfig _boatConfig;
        
        [Header("Physics")]
        [SerializeField] private float _fuelImpactMultiplier = 0.3f;
        
#if ENABLE_INPUT_SYSTEM
        [Header("Input (New Input System)")]
        [SerializeField] private InputActionAsset _inputActions;
        private InputAction _moveAction;
#endif

        private Rigidbody _rigidbody;
        private BoatFuel _boatFuel;
        private Vector2 _inputVector;
        
        public Vector2 InputVector => _inputVector;
        public float CurrentSpeed => _rigidbody.velocity.magnitude;
        public BoatConfig Config => _boatConfig;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _boatFuel = GetComponent<BoatFuel>();
            
            // Fallback: load default config if none assigned
            if (_boatConfig == null)
            {
                _boatConfig = Resources.Load<BoatConfig>("DefaultBoatConfig");
                if (_boatConfig == null)
                {
                    Debug.LogWarning("BoatController: DefaultBoatConfig not found in Resources. Assign a BoatConfig.");
                }
            }

            if (_rigidbody == null)
            {
                Debug.LogError($"BoatController on {gameObject.name} requires a Rigidbody component");
                enabled = false;
            }
            
            // Configure physics properties from BoatConfig
            if (_rigidbody != null && _boatConfig != null)
            {
                _rigidbody.drag = _boatConfig.drag;
                _rigidbody.angularDrag = _boatConfig.angularDrag;
            }
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
#if ENABLE_INPUT_SYSTEM
            if (_moveAction == null && _inputActions != null)
            {
                var map = _inputActions.FindActionMap("Player", throwIfNotFound: false);
                if (map != null)
                {
                    _moveAction = map.FindAction("Move", throwIfNotFound: false);
                    _moveAction?.Enable();
                }
            }
            _inputVector = _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
#else
            _inputVector = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );
#endif
        }

#if ENABLE_INPUT_SYSTEM
        private void OnEnable()
        {
            if (_moveAction != null) _moveAction.Enable();
        }

        private void OnDisable()
        {
            if (_moveAction != null) _moveAction.Disable();
        }
#endif

        private void ApplyMovement()
        {
            if (_boatConfig == null) return;

            float fuelMultiplier = GetFuelMultiplier();
            
            // Update fuel consumption based on throttle input
            if (_boatFuel != null)
            {
                _boatFuel.SetThrottleInput(_inputVector.y);
            }
            
            // Forward/backward movement
            if (Mathf.Abs(_inputVector.y) > 0.01f)
            {
                Vector3 forwardForce = transform.forward * _inputVector.y * _boatConfig.acceleration * fuelMultiplier;
                _rigidbody.AddForce(forwardForce, ForceMode.Acceleration);
            }

            // Turning (only when moving) - use physics-friendly rotation
            if (Mathf.Abs(_inputVector.x) > 0.01f && CurrentSpeed > 0.5f)
            {
                float turnRate = _inputVector.x * _boatConfig.turnRateDegPerSec * Time.fixedDeltaTime * fuelMultiplier;
                
                // Scale turning by current speed (realistic boat physics)
                float speedFactor = Mathf.Clamp01(CurrentSpeed / _boatConfig.maxSpeed);
                turnRate *= speedFactor;
                
                // Use physics-friendly rotation
                Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, turnRate, 0);
                _rigidbody.MoveRotation(targetRotation);
            }

            // Apply speed limit using force-based braking instead of direct velocity writes
            if (_rigidbody.velocity.magnitude > _boatConfig.maxSpeed)
            {
                Vector3 excessVelocity = _rigidbody.velocity - _rigidbody.velocity.normalized * _boatConfig.maxSpeed;
                _rigidbody.AddForce(-excessVelocity * 10f, ForceMode.Acceleration); // Brake force to limit speed
            }
        }

        private float GetFuelMultiplier()
        {
            if (_boatFuel == null || !_boatFuel.IsEmpty)
                return 1f;
                
            // Reduced control when out of fuel, but not completely disabled
            return _fuelImpactMultiplier;
        }

        public void SetBoatConfig(BoatConfig config)
        {
            _boatConfig = config;
        }
    }
}
