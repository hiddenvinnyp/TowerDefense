using TMPro;
using UnityEngine;

namespace TowerDefence
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource
        {
            Gold,
            Life
        }

        public UpdateSource source;
        private TextMeshProUGUI m_Text;
        private void Start() // Это был Awake
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            switch (source)
            {
                case UpdateSource.Gold:
                    TDPlayer.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Life:
                    TDPlayer.LifeUpdateSubscribe(UpdateText);
                    break;
            }            
        }

        private void UpdateText(int points)
        {
            m_Text.text = points.ToString();
        }
    }
}
