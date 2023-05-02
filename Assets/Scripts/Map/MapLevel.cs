using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Episode m_Episode;
        [SerializeField] private RectTransform m_ResultPanel;
        [SerializeField] private Image[] m_ResultImages;
        private int m_Score;
        public int Score => m_Score;

        public bool IsComplete { get { return gameObject.activeSelf && m_ResultPanel.gameObject.activeSelf; } }

        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(m_Episode);
        }

        public void Initialize()
        {
            m_Score = MapComplition.Instance.GetEpisodeScore(m_Episode);
            m_ResultPanel.gameObject.SetActive(m_Score > 0);
            for (int i = 0; i < m_Score; i++)
            {
                m_ResultImages[i].color = Color.white;
            }
        }
    }
}
