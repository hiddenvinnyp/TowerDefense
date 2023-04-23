using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class TDLevelController : LevelController
    {
        private new void Start()
        {
            base.Start();
            TDPlayer.Instance.OnPlayerDead += () => //лямбда-выражение
            {
                StopLevelActivity();
                ResultPanelController.Instance.ShowResults(false);
            };
            m_EventLevelCompleted.AddListener(StopLevelActivity);
        }

        private void StopLevelActivity()
        {
            void DisableAll<T>() where T : MonoBehaviour
            {
                foreach (var obj in FindObjectsOfType<T>())
                {
                    obj.enabled = false;
                }
            }

            foreach (var obj in FindObjectsOfType<EnemySpawnManager>())
            {
                obj.gameObject.SetActive(false);
            }

            DisableAll<Spawner>();

            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.GetComponent<SpaceShip>().enabled = false;
                enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }

            DisableAll<Tower>();
            DisableAll<Projectile>();
        }

        
    }
}