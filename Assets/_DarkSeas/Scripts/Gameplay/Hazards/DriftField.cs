using UnityEngine;

namespace DarkSeas.Gameplay.Hazards
{
    /// <summary>
    /// Creates Perlin-based drift field for ice movement
    /// </summary>
    public class DriftField : MonoBehaviour
    {
        [Header("Drift Settings")]
        [SerializeField] private float _windStrength = 1f;
        [SerializeField] private Vector2 _windDirection = Vector2.right;
        [SerializeField] private float _noiseScale = 0.1f;
        [SerializeField] private float _noiseSpeed = 0.5f;

        public Vector3 GetDriftForceAtPosition(Vector3 position)
        {
            float time = Time.time * _noiseSpeed;
            
            float noiseX = Mathf.PerlinNoise(position.x * _noiseScale + time, position.z * _noiseScale);
            float noiseZ = Mathf.PerlinNoise(position.x * _noiseScale + 100f, position.z * _noiseScale + time);
            
            Vector3 noiseVector = new Vector3(noiseX - 0.5f, 0, noiseZ - 0.5f) * 2f;
            Vector3 windVector = new Vector3(_windDirection.x, 0, _windDirection.y);
            
            return (windVector + noiseVector) * _windStrength;
        }

        public void SetWindParameters(float strength, Vector2 direction)
        {
            _windStrength = strength;
            _windDirection = direction.normalized;
        }
    }
}