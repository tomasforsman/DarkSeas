using UnityEngine;

namespace DarkSeas.Data
{
    /// <summary>
    /// ScriptableObject containing configuration for expedition runs
    /// </summary>
    [CreateAssetMenu(menuName = "DarkSeas/RunConfig", fileName = "New Run Config")]
    public class RunConfig : ScriptableObject
    {
        [Header("Timing")]
        public float baseFuelSeconds = 180f;
        public float rescueHoldSeconds = 1.5f;
        
        [Header("Hazards")]
        public int minIceCount = 20;
        public AnimationCurve collisionDamageBySpeed = AnimationCurve.Linear(0f, 0f, 10f, 1f);
        
        [Header("World Generation")]
        public Vector2 patchSize = new Vector2(100f, 100f);
        public float minHazardSpacing = 5f;
    }
}