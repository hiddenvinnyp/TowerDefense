using TMPro;
using UnityEngine;

namespace TowerDefence
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource
        {
            Gold,
            Life,
            Mana
        }

        public UpdateSource source;
        private TextMeshProUGUI m_Text;
        private void Start() // ��� ��� Awake
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            switch (source)
            {
                case UpdateSource.Gold:
                    TDPlayer.Instance.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Life:
                    TDPlayer.Instance.LifeUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Mana:
                    TDPlayer.Instance.ManaUpdateSubscribe(UpdateText);
                    break;
            }            
        }

        private void UpdateText(int points)
        {
            m_Text.text = points.ToString();
        }
    }
}
