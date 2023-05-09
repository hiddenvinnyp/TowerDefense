using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TowerBuyControl : MonoBehaviour
    {
        [SerializeField] private TowerAsset m_TowerAsset;
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private Button m_BuyButton;
        [SerializeField] private Transform m_BuildPlace;
        public void SetBuildPlace(Transform value) 
        { 
            m_BuildPlace = value; 
        }

        private void Start()
        {
            TDPlayer.Instance.GoldUpdateSubscribe(GoldStatusCheck); // подписки в Awake
            m_Text.text = m_TowerAsset.GoldCost.ToString();
            m_BuyButton.GetComponent<Image>().sprite = m_TowerAsset.GUISprite;
        }

        private void GoldStatusCheck(int gold)
        {
            if (gold >= m_TowerAsset.GoldCost != m_BuyButton.interactable)
            {
                m_BuyButton.interactable = !m_BuyButton.interactable;
                m_Text.color = m_BuyButton.interactable ? Color.white : Color.red;
            }
        }

        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_TowerAsset, m_BuildPlace);
            BuildPlace.HideControls();
        }
    }
}