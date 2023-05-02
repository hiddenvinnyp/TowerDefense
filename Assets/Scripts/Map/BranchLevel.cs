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

        internal void TryActivate()
        {
            gameObject.SetActive(m_RootLevel.IsComplete);
            if (m_NeedPoints > MapComplition.Instance.TotalScore)
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
