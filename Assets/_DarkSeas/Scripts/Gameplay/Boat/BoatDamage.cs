using UnityEngine;
using DarkSeas.Core;

namespace DarkSeas.Gameplay.Boat
{
    /// <summary>
    /// Handles boat health and damage system
    /// </summary>
    public class BoatDamage : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int _maxHullHP = 100;
        [SerializeField] private float _sinkDuration = 5f;

        private int _currentHullHP;
        private bool _isSinking = false;

        public int CurrentHullHP => _currentHullHP;
        public int MaxHullHP => _maxHullHP;
        public bool IsSinking => _isSinking;

        private void Awake()
        {
            _currentHullHP = _maxHullHP;
        }

        public void ApplyDamage(int damage)
        {
            _currentHullHP = Mathf.Max(0, _currentHullHP - damage);
            
            if (_currentHullHP <= 0 && !_isSinking)
            {
                StartSinking();
            }
        }

        private void StartSinking()
        {
            _isSinking = true;
            // TODO: Implement sinking sequence
        }

        private void OnCollisionEnter(Collision collision)
        {
            // TODO: Handle collision damage based on relative velocity
        }
    }
}