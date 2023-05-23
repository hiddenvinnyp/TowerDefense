using TMPro;
using TowerDefence;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ResultPanelController : SingletonBase<ResultPanelController>
    {
        [SerializeField] private Text m_KillsAmount;
        [SerializeField] private Text m_ScoreAmount;
        [SerializeField] private Text m_TimeAmount;
        [SerializeField] private Text m_TimeBonus;

        [SerializeField] private TextMeshProUGUI m_Result;
        [SerializeField] private TextMeshProUGUI m_ButtonNextText;
        [SerializeField] private Sound m_WinSound = Sound.PlayerWin;
        [SerializeField] private Sound m_LoseSound = Sound.PlayerLose;
        [SerializeField] private Image m_WinImage;
        [SerializeField] private Image m_LoseImage;

        // Определять поведение кнопки: если успешно завершили уровень, то следующий;
        // если нет, то рестарт текущего уровня.
        private bool m_Success;

        private void Start()
        {
            gameObject.SetActive(false);
            m_WinImage.enabled = false;
            m_LoseImage.enabled = false;
        }

        public void ShowResults(bool success)
        {
            gameObject.SetActive(true);
            PlaySound(success);

            m_Success = success;
            m_Result.text = success ? "Victory" : "Defeat";
            m_ButtonNextText.text = success ? "Continue" : "Continue";
            m_WinImage.enabled = success;
            m_LoseImage.enabled = !success;
        }

        private void PlaySound(bool success)
        {
            if (success)
                m_WinSound.Play();
            else m_LoseSound.Play();
        }

        public void ShowResults(PlayerStatistics levelResults, bool success)
        {
            gameObject.SetActive(true);
            PlaySound(success);

            m_Success = success;
            m_Result.text = success ? "Win" : "Lose";
            m_ButtonNextText.text = success ? "Next" : "Restart";

            m_KillsAmount.text = levelResults.NumKills.ToString();
            m_ScoreAmount.text = levelResults.Score.ToString();
            m_TimeAmount.text = levelResults.Time.ToString();
            m_TimeBonus.text = "x" + levelResults.TimeBonus.ToString();

            Time.timeScale = 0;
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            Time.timeScale = 1;

            if (m_Success)
            {
                LevelSequenceController.Instance.AdvanceLevel();
            } else
            {
                //restart

                LevelSequenceController.Instance.RestartLevel();
            }
        }
    }
}
