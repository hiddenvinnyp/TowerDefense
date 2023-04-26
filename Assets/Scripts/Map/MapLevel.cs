using SpaceShooter;
using TMPro;
using UnityEngine;

namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        private Episode m_Episode;
        [SerializeField] private TextMeshProUGUI m_Text;
        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(m_Episode);
        }

        public void SetLevelDate(Episode episode, int score)
        {
            m_Episode = episode;
            m_Text.text = $"{score}/3";
        }
    }
}
