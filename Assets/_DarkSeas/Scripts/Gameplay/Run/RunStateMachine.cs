using UnityEngine;
using DarkSeas.Core;

namespace DarkSeas.Gameplay.Run
{
    /// <summary>
    /// State machine handling Harbor → Expedition → Debrief transitions
    /// </summary>
    public class RunStateMachine : MonoBehaviour
    {
        public enum RunState
        {
            Harbor,
            Expedition,
            Debrief
        }

        [Header("Current State")]
        [SerializeField] private RunState _currentState = RunState.Harbor;

        public RunState CurrentState => _currentState;

        public void TransitionToExpedition(int seed)
        {
            if (_currentState != RunState.Harbor) return;

            _currentState = RunState.Expedition;
            RunSignals.InvokeRunStart(seed);
            
            // TODO: Load expedition scene or enable expedition systems
        }

        public void TransitionToDebrief(string result, int rescuedCount)
        {
            if (_currentState != RunState.Expedition) return;

            _currentState = RunState.Debrief;
            RunSignals.InvokeRunEnd(result, rescuedCount);
            
            // TODO: Show debrief screen
        }

        public void ReturnToHarbor()
        {
            _currentState = RunState.Harbor;
            
            // TODO: Return to harbor scene or enable harbor systems
        }
    }
}