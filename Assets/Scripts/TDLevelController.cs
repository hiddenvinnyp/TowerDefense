using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class TDLevelController : LevelController
    {
        private int m_LevelScore = 3;

        private new void Start()
        {
            base.Start();
            TDPlayer.Instance.OnPlayerDead += () => //лямбда-выражение
            {
                StopLevelActivity();
                ResultPanelController.Instance.ShowResults(false);
            };

            m_ReferenceTime += Time.time;
            m_EventLevelCompleted.AddListener(() => 
            {
                StopLevelActivity();
                if (m_ReferenceTime <= Time.time)
                {
                    m_LevelScore--;
                }
                MapComplition.SaveEpisodeResult(m_LevelScore);
            });

            //локальная функция
            void LifeScoreChange(int _) // _ - параметр, который не используется
            {
                m_LevelScore--;
                TDPlayer.OnLifeUpdate -= LifeScoreChange;
            }
            TDPlayer.OnLifeUpdate += LifeScoreChange;
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