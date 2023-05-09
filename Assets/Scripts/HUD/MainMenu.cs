using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefence {
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button m_ContinueButton;
        [SerializeField] private GameObject m_RequestPanel;

        private void Start()
        {
            if (m_RequestPanel != null)
                m_RequestPanel.SetActive(false);
            if (m_ContinueButton != null)
                m_ContinueButton.interactable = FileHandler.HasFile(MapComplition.filename);
        }

        public void NewGame()
        {
            if (FileHandler.HasFile(MapComplition.filename))
            {
                m_RequestPanel.SetActive(true);
                foreach (var button in GetComponentsInChildren<Button>())
                {
                    button.interactable = false;
                }
            } else
            {                
                SceneManager.LoadScene(1);
            }            
        }

        public void Continue()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    } 
}
