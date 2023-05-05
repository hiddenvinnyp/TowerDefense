using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace TowerDefence
{
    public class NextWaveGUI : MonoBehaviour
    {
        [SerializeField] private Slider m_Slider;
        [SerializeField] private TextMeshProUGUI m_BonusAmountText;
        private EnemyWaveManager m_Manager;
        private float m_TimeToNextWave;

        private void Start()
        {
            m_Manager = FindObjectOfType<EnemyWaveManager>();
            EnemyWave.OnWavePrepare += (float time) =>
            {
                m_TimeToNextWave = time;
                m_Slider.maxValue = time;

            };
            EnemyWave.OnLastWave += () =>
            {
                gameObject.GetComponentInChildren<Button>().interactable = false;
                m_BonusAmountText.text = "0";
                m_Slider.value = 0;
                enabled = false;
            };
        }

        public void CallWave()
        {
            m_Manager.ForceNextWave();
        }

        private void Update()
        {
            var bonus = (int)m_TimeToNextWave;
            if (bonus < 0) bonus = 0;
            m_BonusAmountText.text = bonus.ToString();
            m_Slider.value = m_TimeToNextWave;
            m_TimeToNextWave -= Time.deltaTime;
        }
    }
}
