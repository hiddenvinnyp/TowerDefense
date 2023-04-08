using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public interface ILevelCondition
    {
        bool IsCompleted { get; }
        string Task { get; }
    }

    public class LevelController : SingletonBase<LevelController>
    {  
        [SerializeField] private int m_ReferenceTime;
        public int ReferenceTime => m_ReferenceTime;

        [SerializeField] private UnityEvent m_EventLevelCompleted;

        private ILevelCondition[] m_Conditions;
        public ILevelCondition[] Tasks => m_Conditions;
        private bool m_IsLevelCompleted;
        private float m_LevelTime;
        public float LevelTime => m_LevelTime;

        private void Start()
        {
            m_Conditions = GetComponentsInChildren<ILevelCondition>();
        }

        private void Update()
        {
            if (!m_IsLevelCompleted)
            {
                m_LevelTime += Time.deltaTime;

                CheckLevelConditions();
            }
        }

        private void CheckLevelConditions()
        {
            if (m_Conditions == null || m_Conditions.Length == 0) return;

            int numTasksComplited = 0;

            foreach (var condition in m_Conditions)
            {
                if (condition.IsCompleted)
                {
                    numTasksComplited++;
                }
            }

            if (numTasksComplited == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;
                m_EventLevelCompleted?.Invoke();

                LevelSequenceController.Instance?.FinishCurrentLevel(true);
            }
        }
    }
}