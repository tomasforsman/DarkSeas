using UnityEngine;

namespace DarkSeas.Data
{
    public enum UpgradeType 
    { 
        Light, 
        Hull, 
        Fuel, 
        Seats 
    }

    /// <summary>
    /// ScriptableObject defining an upgrade that can be purchased with Legacy Points
    /// </summary>
    [CreateAssetMenu(menuName = "DarkSeas/UpgradeDef", fileName = "New Upgrade")]
    public class UpgradeDef : ScriptableObject
    {
        [Header("Identification")]
        public string id;
        
        [Header("Properties")]
        public UpgradeType type;
        public float value; // % or flat; interpret by type
        public int legacyCost;
        public bool stackable = false;
        
        [Header("UI")]
        [TextArea(2, 4)]
        public string description;
    }
}