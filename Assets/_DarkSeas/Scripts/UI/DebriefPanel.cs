using UnityEngine;
using UnityEngine.UI;
using DarkSeas.Core;
using DarkSeas.MetaProgression.Legacy;
using DarkSeas.Gameplay.Run;

namespace DarkSeas.UI
{
    /// <summary>
    /// Simple debrief panel shown at run end.
    /// </summary>
    public class DebriefPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Text _title;
        [SerializeField] private Text _rescuedText;
        [SerializeField] private Text _earnedText;
        [SerializeField] private Text _totalText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private RunStateMachine _runStateMachine;

        private void Awake()
        {
            if (_group == null) _group = GetComponent<CanvasGroup>();
            Hide();
            if (_continueButton != null)
            {
                _continueButton.onClick.AddListener(OnContinue);
            }
        }

        private void OnEnable()
        {
            RunSignals.RunEnd += OnRunEnd;
        }

        private void OnDisable()
        {
            RunSignals.RunEnd -= OnRunEnd;
        }

        private void OnRunEnd(string result, int rescued)
        {
            Show();
            int points = rescued * LegacyManager.Instance.PointsPerPassenger;
            if (_title != null) _title.text = result == "Sank" ? "Run Failed" : "Run Complete";
            if (_rescuedText != null) _rescuedText.text = $"Rescued: {rescued}";
            if (_earnedText != null) _earnedText.text = $"Earned: +{points} Legacy";
            if (_totalText != null) _totalText.text = $"Total: {LegacyManager.Instance.LegacyPoints}";
        }

        private void Show()
        {
            if (_group != null)
            {
                _group.alpha = 1f;
                _group.interactable = true;
                _group.blocksRaycasts = true;
            }
        }

        private void Hide()
        {
            if (_group != null)
            {
                _group.alpha = 0f;
                _group.interactable = false;
                _group.blocksRaycasts = false;
            }
        }

        public void OnContinue()
        {
            Hide();
            if (_runStateMachine != null)
            {
                _runStateMachine.ReturnToHarbor();
            }
        }
    }
}

