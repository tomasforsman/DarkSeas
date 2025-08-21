using UnityEngine;

namespace DarkSeas.Data
{
    /// <summary>
    /// ScriptableObject defining boat properties and capabilities
    /// </summary>
    [CreateAssetMenu(menuName = "DarkSeas/BoatConfig", fileName = "New Boat Config")]
    public class BoatConfig : ScriptableObject
    {
        [Header("Identification")]
        public string id;
        
        [Header("Movement")]
        public float maxSpeed = 8f;
        public float acceleration = 2f;
        public float turnRateDegPerSec = 90f;
        
        [Header("Physics")]
        public float drag = 1f;
        public float angularDrag = 5f;
        
        [Header("Durability")]
        public int hullHP = 100;
        public float fuelCapacity = 180f;
        
        [Header("Capacity")]
        public int seats = 1;
        
        [Header("Light")]
        public float headlightRange = 50f;
        public float headlightAngle = 45f;
    }
}