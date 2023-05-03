using System;
using UnityEngine;

namespace TowerDefence
{
    public class Upgrades : SingletonBase<Upgrades>
    {
        public const string filename = "upgrades.dat";

        [Serializable]
        private class UpgradeSave
        {
            public UpgradeAsset Asset;
            public int Level = 0;
        }

        [SerializeField] private UpgradeSave[] m_Saves;

        private new void Awake()
        {
            base.Awake();
            Saver<UpgradeSave[]>.TryLoad(filename, ref m_Saves);
        }

        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.Asset == asset)
                {
                    upgrade.Level++;
                    Saver<UpgradeSave[]>.Save(filename, Instance.m_Saves);
                }
            }
        }

        public static int GetTotalCost()
        {
            int result = 0;
            foreach (var upgrade in Instance.m_Saves)
            {
                for (int i = 0; i < upgrade.Level; i++)
                {
                    result += upgrade.Asset.CostByLevel[i];
                }
            }
            return result;
        }

        public static int GetUpdateLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.Asset == asset)
                {
                    return upgrade.Level;
                }
            }
            return 0;
        }
    }
}
