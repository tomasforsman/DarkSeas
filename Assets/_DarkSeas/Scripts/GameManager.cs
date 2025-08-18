using UnityEngine;
using DarkSeas.Gameplay.Run;

namespace DarkSeas
{
    /// <summary>
    /// Main game manager handling high-level game state
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RunStateMachine _runStateMachine;
        
        private static GameManager _instance;
        public static GameManager Instance => _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            InitializeGame();
        }
        
        private void InitializeGame()
        {
            // TODO: Initialize core systems
        }
    }
}