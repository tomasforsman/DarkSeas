using UnityEngine;
using DarkSeas.Data;

namespace DarkSeas
{
    /// <summary>
    /// Main game manager handling high-level game state
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RunConfig _runConfig;
        
        private static GameManager _instance;
        public static GameManager Instance => _instance;
        public RunConfig RunConfig => _runConfig;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                EnsureConfigs();
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

        private void EnsureConfigs()
        {
            if (_runConfig == null)
            {
                _runConfig = Resources.Load<RunConfig>("DefaultRunConfig");
                if (_runConfig == null)
                {
                    Debug.LogWarning("GameManager: DefaultRunConfig not found in Resources. Assign a RunConfig.");
                }
            }
        }
    }
}
