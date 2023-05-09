using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefence
{
    public class RequestPanel : MonoBehaviour
    {
        public void ResetSave()
        {
            FileHandler.Reset(MapComplition.filename);
            FileHandler.Reset(Upgrades.filename);
            SceneManager.LoadScene(1);
        }

        public void KeepSave()
        {
            foreach (var button in FindObjectsOfType<Button>())
            {
                button.interactable = true;
            }
            gameObject.SetActive(false);
        }
    }
}
