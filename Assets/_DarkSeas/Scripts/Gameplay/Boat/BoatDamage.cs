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
            var controller = GetComponent<BoatController>();
            if (controller != null) controller.enabled = false;
            StartCoroutine(SinkCoroutine());

            // End the run due to sinking
            DarkSeas.Core.RunSignals.InvokeRunEnd("Sank", 0);
        }

        private System.Collections.IEnumerator SinkCoroutine()
        {
            float t = 0f;
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + new Vector3(0, -2f, 0);
            while (t < _sinkDuration)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / _sinkDuration);
                transform.position = Vector3.Lerp(startPos, endPos, k);
                yield return null;
            }
            // Disable after sinking
            gameObject.SetActive(false);
        }
    }
}
