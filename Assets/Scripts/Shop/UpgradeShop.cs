using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UpgradeShop : MonoBehaviour
    {
        /*[Serializable]
        private class UpgradeSlot
        {
            public BuyUpgrade Slot;
            public UpgradeAsset Upgrade;

            public void AssignUpgrade()
            {
                Slot.SetUpgrade(Upgrade);
            }
        }*/


        [SerializeField] private int m_Money;
        [SerializeField] private TextMeshProUGUI m_MoneyText;
        [SerializeField] private BuyUpgrade[] m_Upgrades;

        private void Start()
        {
            foreach (var slot in m_Upgrades)
            {
                slot.Initialize();
                slot.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(UpdateMoney);
            }
            UpdateMoney();
        }

        public void UpdateMoney()
        {
            m_Money = MapCompletion.Instance.TotalScore;
            m_Money -= Upgrades.GetTotalCost();
            m_MoneyText.text = m_Money.ToString();

            foreach (var slot in m_Upgrades)
            {
                slot.CheckCost(m_Money);
            }
        }
    }
}
