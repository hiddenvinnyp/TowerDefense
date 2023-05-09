using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private Image m_Icon;
        [SerializeField] private TextMeshProUGUI m_LevelText, m_CostText;
        [SerializeField] private Button m_BuyButton;
        [SerializeField] private UpgradeAsset m_UpgradeAsset;

        private int m_CurrentCost;

        public void Initialize()
        {
            m_Icon.sprite = m_UpgradeAsset.Icon;
            int savedLevel = Upgrades.GetUpdateLevel(m_UpgradeAsset);
            if (savedLevel >= m_UpgradeAsset.CostByLevel.Length)
            {
                m_LevelText.text = $"Lvl: {savedLevel} (max)";
                m_BuyButton.interactable = false;
                m_BuyButton.transform.Find("starImage").gameObject.SetActive(false);
                m_BuyButton.transform.Find("Text (TMP)").gameObject.SetActive(false);
                m_CostText.text = "X";
                m_CurrentCost = int.MaxValue;
            }
            else
            {
                m_LevelText.text = $"Lvl: {savedLevel + 1}";
                m_CurrentCost = m_UpgradeAsset.CostByLevel[savedLevel];
                m_CostText.text = m_CurrentCost.ToString();
            }
        }

        public void Buy()
        {
            Upgrades.BuyUpgrade(m_UpgradeAsset);
            Initialize();
        }

        public void CheckCost(int money)
        {
            m_BuyButton.interactable = money >= m_CurrentCost;
        }
    }
}