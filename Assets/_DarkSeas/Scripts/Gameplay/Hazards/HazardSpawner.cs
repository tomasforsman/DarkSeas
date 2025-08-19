using UnityEngine;
using System.Collections.Generic;

namespace DarkSeas.Gameplay.Hazards
{
    /// <summary>
    /// Spawns ice hazards in procedural patches
    /// </summary>
    public class HazardSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private int _minIceCount = 20;
        [SerializeField] private float _minSpacing = 5f;
        [SerializeField] private Vector2 _spawnArea = new Vector2(100f, 100f);

        [Header("Prefab References")]
        [SerializeField] private GameObject _smallIcePrefab;
        [SerializeField] private GameObject _mediumIcePrefab;
        [SerializeField] private GameObject _largeIcePrefab;

        private List<GameObject> _spawnedHazards = new List<GameObject>();

        private void Awake()
        {
            // Pull defaults from RunConfig if available
            var cfg = DarkSeas.GameManager.Instance != null ? DarkSeas.GameManager.Instance.RunConfig : null;
            if (cfg != null)
            {
                _minIceCount = cfg.minIceCount;
                _minSpacing = cfg.minHazardSpacing;
                _spawnArea = cfg.patchSize;
            }

            // Auto-assign prefabs from Resources if missing
            if (_smallIcePrefab == null) _smallIcePrefab = Resources.Load<GameObject>("Hazards/Ice_Small");
            if (_mediumIcePrefab == null) _mediumIcePrefab = Resources.Load<GameObject>("Hazards/Ice_Medium");
            if (_largeIcePrefab == null) _largeIcePrefab = Resources.Load<GameObject>("Hazards/Ice_Large");
        }

        public void SpawnHazards(int seed)
        {
            ClearExistingHazards();
            
            Random.InitState(seed);
            
            for (int i = 0; i < _minIceCount; i++)
            {
                SpawnRandomIce();
            }
        }

        private void SpawnRandomIce()
        {
            Vector3 position = GetRandomSpawnPosition();
            GameObject prefab = SelectRandomIcePrefab();
            
            if (prefab != null && IsValidSpawnPosition(position))
            {
                GameObject ice = Instantiate(prefab, position, Random.rotation);
                _spawnedHazards.Add(ice);
            }
            else if (prefab == null && IsValidSpawnPosition(position))
            {
                // Fallback: create simple cube with IceHazard
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = position;
                var ice = go.AddComponent<IceHazard>();
                _spawnedHazards.Add(go);
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            float x = Random.Range(-_spawnArea.x / 2, _spawnArea.x / 2);
            float z = Random.Range(-_spawnArea.y / 2, _spawnArea.y / 2);
            return new Vector3(x, 0, z);
        }

        private GameObject SelectRandomIcePrefab()
        {
            float rand = Random.value;
            if (rand < 0.6f) return _smallIcePrefab;
            if (rand < 0.9f) return _mediumIcePrefab;
            return _largeIcePrefab;
        }

        private bool IsValidSpawnPosition(Vector3 position)
        {
            foreach (var hazard in _spawnedHazards)
            {
                if (hazard != null && Vector3.Distance(position, hazard.transform.position) < _minSpacing)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearExistingHazards()
        {
            foreach (var hazard in _spawnedHazards)
            {
                if (hazard != null)
                {
                    DestroyImmediate(hazard);
                }
            }
            _spawnedHazards.Clear();
        }
    }
}
