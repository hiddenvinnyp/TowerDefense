using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class WaveLevelCondition : MonoBehaviour, ILevelCondition
    {
        private bool m_IsCompleted;

        private void Start()
        {
            FindObjectOfType<EnemyWaveManager>().OnAllWavesDead += () => 
            {
                if (TDPlayer.Instance.NumLives > 0)
                {
                    m_IsCompleted = true;
                }
                else
                    m_IsCompleted = false;
            };
        }

        public bool IsCompleted { get {return m_IsCompleted; } }

        public string Task => "Kill them all!";
    }
}