using SpaceShooter;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefence
{
    public class PathPatrol : AIController
    {
        [SerializeField] private UnityEvent OnEndPath;
        private Path m_Path;
        private int m_Index;

        public void SetPath(Path path)
        {
            m_Path = path;
            m_Index = 0;
            SetPatrolBehavior(m_Path[m_Index]);
        }

        protected override void GetNewPoint()
        {
            if (++m_Index < m_Path.Length)
            {
                SetPatrolBehavior(m_Path[m_Index]);
            } else
            {
                OnEndPath.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
