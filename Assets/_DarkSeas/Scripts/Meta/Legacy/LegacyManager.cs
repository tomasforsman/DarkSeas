using UnityEngine;
using System.Collections.Generic;
using DarkSeas.Data;

namespace DarkSeas.Meta.Legacy
{
    /// <summary>
    /// Manages Legacy Points accumulation and upgrade purchases
    /// </summary>
    public class LegacyManager : MonoBehaviour
    {
        [Header("Legacy State")]
        [SerializeField] private int _currentLegacyPoints = 0;
        [SerializeField] private List<string> _purchasedUpgrades = new List<string>();

        public int CurrentLegacyPoints => _currentLegacyPoints;
        public IReadOnlyList<string> PurchasedUpgrades => _purchasedUpgrades;

        private void Awake()
        {
            LoadLegacyData();
        }

        public void AddLegacyPoints(int points)
        {
            _currentLegacyPoints += points;
            SaveLegacyData();
        }

        public bool CanPurchaseUpgrade(UpgradeDef upgrade)
        {
            if (upgrade == null) return false;
            if (_currentLegacyPoints < upgrade.legacyCost) return false;
            if (!upgrade.stackable && _purchasedUpgrades.Contains(upgrade.id)) return false;
            
            return true;
        }

        public bool PurchaseUpgrade(UpgradeDef upgrade)
        {
            if (!CanPurchaseUpgrade(upgrade)) return false;

            _currentLegacyPoints -= upgrade.legacyCost;
            _purchasedUpgrades.Add(upgrade.id);
            
            SaveLegacyData();
            return true;
        }

        public int GetUpgradeCount(string upgradeId)
        {
            int count = 0;
            foreach (string id in _purchasedUpgrades)
            {
                if (id == upgradeId) count++;
            }
            return count;
        }

        private void LoadLegacyData()
        {
            _currentLegacyPoints = PlayerPrefs.GetInt("Legacy_Points", 0);
            
            string upgradesJson = PlayerPrefs.GetString("Legacy_Upgrades", "");
            if (!string.IsNullOrEmpty(upgradesJson))
            {
                // TODO: Implement JSON deserialization for upgrades list
            }
        }

        private void SaveLegacyData()
        {
            PlayerPrefs.SetInt("Legacy_Points", _currentLegacyPoints);
            
            // TODO: Implement JSON serialization for upgrades list
            PlayerPrefs.Save();
        }
    }
}