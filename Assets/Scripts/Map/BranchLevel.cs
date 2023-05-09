using System;
using TMPro;
using UnityEngine;

namespace TowerDefence
{
    [RequireComponent (typeof(MapLevel))]
    public class BranchLevel : MonoBehaviour
    {
        [SerializeField] private MapLevel m_RootLevel;
        [SerializeField] private int m_NeedPoints = 1;
        [SerializeField] private TextMeshProUGUI m_NeedPointsText;

        /// <summary>
        /// Try activate a branch level. It needs complite the previous level and 
        /// have some points for activation.
        /// </summary>
        public void TryActivate()
        {
            gameObject.SetActive(m_RootLevel.IsComplete);
            if (m_NeedPoints > MapCompletion.Instance.TotalScore)
            {
                m_NeedPointsText.text = m_NeedPoints.ToString();
            } else
            {
                m_NeedPointsText.transform.parent.transform.parent.gameObject.SetActive(false);
                GetComponent<MapLevel>().Initialize();
            }
        }
    }
}
