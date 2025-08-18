using UnityEngine;

namespace DarkSeas.Gameplay.Light
{
    /// <summary>
    /// Controls boat headlight spotlight with URP integration
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Light))]
    public class HeadlightController : MonoBehaviour
    {
        [Header("Light Settings")]
        [SerializeField] private float _lightRange = 50f;
        [SerializeField] private float _lightAngle = 45f;
        [SerializeField] private float _lightIntensity = 2f;

        private UnityEngine.Light _spotlight;

        private void Awake()
        {
            _spotlight = GetComponent<UnityEngine.Light>();
            ConfigureSpotlight();
        }

        private void ConfigureSpotlight()
        {
            _spotlight.type = LightType.Spot;
            _spotlight.range = _lightRange;
            _spotlight.spotAngle = _lightAngle;
            _spotlight.intensity = _lightIntensity;
        }

        public void SetLightProperties(float range, float angle, float intensity)
        {
            _lightRange = range;
            _lightAngle = angle;
            _lightIntensity = intensity;
            ConfigureSpotlight();
        }
    }
}