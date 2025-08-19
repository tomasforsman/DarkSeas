using UnityEngine;
using UnityEngine.UI;
using DarkSeas.Meta.Legacy;
using DarkSeas.Core;

namespace DarkSeas.UI
{
    /// <summary>
    /// Displays total Legacy points. Works with a Text component.
    /// </summary>
    public class LegacyHUD : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private void Awake()
        {
            if (_text == null) _text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            DeliverySignals.Delivered += OnDelivered;
            UpdateText();
        }

        private void OnDisable()
        {
            DeliverySignals.Delivered -= OnDelivered;
        }

        private void OnDelivered(int count, int points)
        {
            UpdateText();
        }

        private void Update()
        {
            // Fallback: keep updated even without signals
            if (Time.frameCount % 30 == 0) UpdateText();
        }

        private void UpdateText()
        {
            if (_text != null)
            {
                _text.text = $"Legacy: {LegacyManager.Instance.LegacyPoints}";
            }
        }
    }
}

