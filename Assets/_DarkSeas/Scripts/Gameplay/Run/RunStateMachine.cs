using UnityEngine;
using DarkSeas.Core;

namespace DarkSeas.Gameplay.Run
{
    public enum RunState { Harbor, Expedition, Debrief }

    /// <summary>
    /// Minimal run state machine for MVP flows.
    /// </summary>
    public class RunStateMachine : MonoBehaviour
    {
        [SerializeField] private RunState _state = RunState.Harbor;
        [SerializeField] private int _currentSeed = 0;

        public RunState State => _state;
        public int CurrentSeed => _currentSeed;

        private void OnEnable()
        {
            RunSignals.RunEnd += OnRunEnd;
        }

        private void OnDisable()
        {
            RunSignals.RunEnd -= OnRunEnd;
        }

        public void StartRun(int seed)
        {
            _currentSeed = seed;
            _state = RunState.Expedition;
            RunSignals.InvokeRunStart(seed);
        }

        public void ReturnToHarbor()
        {
            _state = RunState.Harbor;
        }

        public void EndRun(string result, int rescuedCount)
        {
            RunSignals.InvokeRunEnd(result, rescuedCount);
            _state = RunState.Debrief;
        }

        private void OnRunEnd(string result, int rescuedCount)
        {
            _state = RunState.Debrief;
        }
    }
}

