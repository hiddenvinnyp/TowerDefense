using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class TimeLevelCondition : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private float m_TimeLimit;

        public bool IsCompleted => Time.time > m_TimeLimit;

        public string Task => "Survive the onslaught of enemies for <color=white>" + m_TimeLimit.ToString() + "</color>";
    }
}
