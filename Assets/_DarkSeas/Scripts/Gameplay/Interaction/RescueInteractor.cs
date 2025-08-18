using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DarkSeas.Core;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace DarkSeas.Gameplay.Interaction
{
    /// <summary>
    /// Handles rescue interactions with hold-to-rescue mechanic
    /// </summary>
    public class RescueInteractor : MonoBehaviour
    {
        [Header("Rescue Settings")]
        [SerializeField] private float _rescueHoldSeconds = 1.5f;
        [SerializeField] private int _maxPassengers = 2;

#if ENABLE_INPUT_SYSTEM
        [Header("Input (New Input System)")]
        [SerializeField] private InputActionAsset _inputActions;
        private InputAction _interactAction;
#endif

        private List<string> _passengers = new List<string>();
        private Coroutine _rescueCoroutine;
        private RescueTarget _currentTarget;

        public int PassengerCount => _passengers.Count;
        public int MaxPassengers => _maxPassengers;
        public bool HasCapacity => _passengers.Count < _maxPassengers;

        private void Update()
        {
            HandleRescueInput();
        }

        private void HandleRescueInput()
        {
#if ENABLE_INPUT_SYSTEM
            if (_interactAction == null && _inputActions != null)
            {
                var map = _inputActions.FindActionMap("Player", throwIfNotFound: false);
                if (map != null)
                {
                    _interactAction = map.FindAction("Interact", throwIfNotFound: false);
                    if (_interactAction != null)
                    {
                        _interactAction.started += OnInteractStarted;
                        _interactAction.canceled += OnInteractCanceled;
                        _interactAction.Enable();
                    }
                }
            }
#else
            // Legacy input fallback (to be removed once Input System is fully wired in scenes)
            if (Input.GetKeyDown(KeyCode.E)) StartRescueAttempt();
            else if (Input.GetKeyUp(KeyCode.E)) StopRescueAttempt();
#endif
        }

#if ENABLE_INPUT_SYSTEM
        private void OnDisable()
        {
            if (_interactAction != null)
            {
                _interactAction.started -= OnInteractStarted;
                _interactAction.canceled -= OnInteractCanceled;
                _interactAction.Disable();
            }
        }

        private void OnInteractStarted(InputAction.CallbackContext _)
        {
            StartRescueAttempt();
        }

        private void OnInteractCanceled(InputAction.CallbackContext _)
        {
            StopRescueAttempt();
        }
#endif

        private void StartRescueAttempt()
        {
            if (!HasCapacity) return;

            RescueTarget target = FindNearbyTarget();
            if (target != null && target.InRange(transform))
            {
                _currentTarget = target;
                _rescueCoroutine = StartCoroutine(RescueCoroutine(target));
            }
        }

        private void StopRescueAttempt()
        {
            if (_rescueCoroutine != null)
            {
                StopCoroutine(_rescueCoroutine);
                _rescueCoroutine = null;
                _currentTarget = null;
            }
        }

        private IEnumerator RescueCoroutine(RescueTarget target)
        {
            float elapsed = 0f;
            
            while (elapsed < _rescueHoldSeconds)
            {
                if (!target.InRange(transform) || IsInterrupted())
                {
                    yield break;
                }
                
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Rescue successful
            string passengerID = target.Claim();
            if (passengerID != null)
            {
                _passengers.Add(passengerID);
                RescueSignals.InvokePickedUp(passengerID, Time.time);
            }

            _rescueCoroutine = null;
            _currentTarget = null;
        }

        private RescueTarget FindNearbyTarget()
        {
            RescueTarget[] targets = FindObjectsOfType<RescueTarget>();
            foreach (var target in targets)
            {
                if (target.InRange(transform))
                {
                    return target;
                }
            }
            return null;
        }

        private bool IsInterrupted()
        {
            // TODO: Add interruption conditions (collision, etc.)
            return false;
        }

        public void DeliverPassengers()
        {
            _passengers.Clear();
        }

        public void SetMaxPassengers(int maxPassengers)
        {
            _maxPassengers = maxPassengers;
        }
    }
}
