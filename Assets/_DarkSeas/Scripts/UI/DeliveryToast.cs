using UnityEngine;
using UnityEngine.UI;
using DarkSeas.Core;

namespace DarkSeas.UI
{
    /// <summary>
    /// Shows a brief toast on delivery.
    /// </summary>
    public class DeliveryToast : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Text _text;
        [SerializeField] private float _showSeconds = 2f;
        [SerializeField] private float _fadeSpeed = 6f;

        private float _timer = 0f;

        private void Awake()
        {
            if (_group == null) _group = GetComponent<CanvasGroup>();
            if (_group != null) _group.alpha = 0f;
            if (_text == null) _text = GetComponentInChildren<Text>();
        }

        private void OnEnable()
        {
            DeliverySignals.Delivered += OnDelivered;
        }

        private void OnDisable()
        {
            DeliverySignals.Delivered -= OnDelivered;
        }

        private void OnDelivered(int count, int points)
        {
            if (_text != null) _text.text = $"Delivered {count} → +{points} Legacy";
            _timer = _showSeconds;
        }

        private void Update()
        {
            if (_group == null) return;
            float target = _timer > 0 ? 1f : 0f;
            _group.alpha = Mathf.MoveTowards(_group.alpha, target, Time.deltaTime * _fadeSpeed);
            if (_timer > 0) _timer -= Time.deltaTime;
        }
    }
}

