using UnityEngine;
using DarkSeas.Core;

namespace DarkSeas.Gameplay.Hazards
{
    /// <summary>
    /// Ice hazard that damages boat on collision
    /// </summary>
    public class IceHazard : MonoBehaviour
    {
        [Header("Ice Properties")]
        [SerializeField] private int _size = 1; // 1=Small, 2=Medium, 3=Large
        [SerializeField] private int _baseDamage = 10;
        [SerializeField] private float _mass = 1f;

        public int Size => _size;
        public float Mass => _mass;

        private void Awake()
        {
            ConfigureCollider();
        }

        private void ConfigureCollider()
        {
            var collider = GetComponent<Collider>();
            if (collider == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
        }

        public int ComputeDamage(float relativeSpeed)
        {
            // Damage scales with speed (via RunConfig curve) and ice size
            var cfg = DarkSeas.GameManager.Instance != null ? DarkSeas.GameManager.Instance.RunConfig : null;
            float curve = cfg != null && cfg.collisionDamageBySpeed != null
                ? Mathf.Clamp01(cfg.collisionDamageBySpeed.Evaluate(relativeSpeed))
                : Mathf.Clamp01(relativeSpeed / 10f);
            return Mathf.RoundToInt(_baseDamage * _size * curve);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var boatDamage = collision.gameObject.GetComponent<Boat.BoatDamage>();
            if (boatDamage != null)
            {
                float relativeSpeed = collision.relativeVelocity.magnitude;
                int damage = ComputeDamage(relativeSpeed);
                
                boatDamage.ApplyDamage(damage);
                HazardSignals.InvokeCollideIce(_size, relativeSpeed);
            }
        }
    }
}
