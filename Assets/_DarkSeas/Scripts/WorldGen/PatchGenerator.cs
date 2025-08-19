using UnityEngine;
using System.Collections.Generic;
using DarkSeas.Gameplay.Hazards;
using DarkSeas.Gameplay.Interaction;
using DarkSeas.Data;

namespace DarkSeas.WorldGen
{
    /// <summary>
    /// Generates procedural patches with hazards and rescue targets
    /// </summary>
    public class PatchGenerator : MonoBehaviour
    {
        [Header("Generation Settings")]
        [SerializeField] private RunConfig _runConfig;
        [SerializeField] private Vector2 _patchSize = new Vector2(100f, 100f);

        [Header("Prefab References")]
        [SerializeField] private GameObject[] _iceHazardPrefabs;
        [SerializeField] private GameObject _rescueTargetPrefab;

        [Header("Spawn Points")]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _harborPoint;

        private List<GameObject> _generatedObjects = new List<GameObject>();

        public void GeneratePatch(int seed)
        {
            ClearExistingPatch();
            
            Random.InitState(seed);
            
            SpawnHazards();
            SpawnRescueTargets();
        }

        private void SpawnHazards()
        {
            var hazardSpawner = GetComponent<HazardSpawner>();
            if (hazardSpawner != null)
            {
                // Use existing HazardSpawner if available
                hazardSpawner.SpawnHazards(Random.Range(0, int.MaxValue));
                return;
            }

            // Fallback manual spawning
            int hazardCount = _runConfig != null ? _runConfig.minIceCount : 20;
            
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 position = GetRandomPatchPosition();
                if (IsValidSpawnLocation(position))
                {
                    GameObject hazardPrefab = GetRandomHazardPrefab();
                    if (hazardPrefab != null)
                    {
                        GameObject hazard = Instantiate(hazardPrefab, position, Random.rotation);
                        _generatedObjects.Add(hazard);
                    }
                }
            }
        }

        private void SpawnRescueTargets()
        {
            // Spawn 1-2 rescue targets per patch
            int targetCount = Random.Range(1, 3);
            
            for (int i = 0; i < targetCount; i++)
            {
                Vector3 position = GetRandomPatchPosition();
                if (IsValidSpawnLocation(position) && _rescueTargetPrefab != null)
                {
                    GameObject target = Instantiate(_rescueTargetPrefab, position, Quaternion.identity);
                    _generatedObjects.Add(target);
                }
            }
        }

        private Vector3 GetRandomPatchPosition()
        {
            float x = Random.Range(-_patchSize.x / 2, _patchSize.x / 2);
            float z = Random.Range(-_patchSize.y / 2, _patchSize.y / 2);
            return new Vector3(x, 0, z);
        }

        private bool IsValidSpawnLocation(Vector3 position)
        {
            float minDistance = _runConfig != null ? _runConfig.minHazardSpacing : 5f;
            
            // Check distance from player spawn
            if (_playerSpawnPoint != null && Vector3.Distance(position, _playerSpawnPoint.position) < minDistance * 2f)
                return false;
            
            // Check distance from harbor
            if (_harborPoint != null && Vector3.Distance(position, _harborPoint.position) < minDistance * 2f)
                return false;

            // Check distance from existing objects
            foreach (var obj in _generatedObjects)
            {
                if (obj != null && Vector3.Distance(position, obj.transform.position) < minDistance)
                    return false;
            }

            return true;
        }

        private GameObject GetRandomHazardPrefab()
        {
            if (_iceHazardPrefabs == null || _iceHazardPrefabs.Length == 0) return null;
            
            int index = Random.Range(0, _iceHazardPrefabs.Length);
            return _iceHazardPrefabs[index];
        }

        private void ClearExistingPatch()
        {
            foreach (var obj in _generatedObjects)
            {
                if (obj != null)
                {
#if UNITY_EDITOR
                    // Use DestroyImmediate only in editor for immediate cleanup during edit-time generation
                    if (!Application.isPlaying)
                    {
                        DestroyImmediate(obj);
                    }
                    else
                    {
                        Destroy(obj);
                    }
#else
                    // In builds, always use Destroy for runtime safety
                    Destroy(obj);
#endif
                }
            }
            _generatedObjects.Clear();
        }
    }
}