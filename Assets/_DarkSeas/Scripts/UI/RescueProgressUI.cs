using UnityEngine;
using UnityEngine.UI;
using DarkSeas.Core;

namespace DarkSeas.UI
{
    /// <summary>
    /// Optional UI feedback for rescue progress. Assign a Slider and/or AudioSource.
    /// </summary>
    public class RescueProgressUI : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip _startClip;
        [SerializeField] private AudioClip _completeClip;
        [SerializeField] private AudioClip _cancelClip;

        private void OnEnable()
        {
            RescueSignals.Started += OnStarted;
            RescueSignals.Progress += OnProgress;
            RescueSignals.Canceled += OnCanceled;
            RescueSignals.PickedUp += OnPickedUp;
        }

        private void OnDisable()
        {
            RescueSignals.Started -= OnStarted;
            RescueSignals.Progress -= OnProgress;
            RescueSignals.Canceled -= OnCanceled;
            RescueSignals.PickedUp -= OnPickedUp;
        }

        private void Show(bool show)
        {
            if (_group != null)
            {
                _group.alpha = show ? 1f : 0f;
                _group.interactable = show;
                _group.blocksRaycasts = show;
            }
            if (_progressBar != null && !show)
            {
                _progressBar.value = 0f;
            }
        }

        private void OnStarted(string _)
        {
            Show(true);
            if (_audio != null && _startClip != null) _audio.PlayOneShot(_startClip);
        }

        private void OnProgress(string _, float pct)
        {
            if (_progressBar != null)
            {
                _progressBar.value = pct;
            }
        }

        private void OnCanceled(string _)
        {
            if (_audio != null && _cancelClip != null) _audio.PlayOneShot(_cancelClip);
            Show(false);
        }

        private void OnPickedUp(string _, float __)
        {
            if (_audio != null && _completeClip != null) _audio.PlayOneShot(_completeClip);
            Show(false);
        }
    }
}

