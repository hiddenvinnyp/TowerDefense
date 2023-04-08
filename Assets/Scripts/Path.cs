using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private AIPointPatrol[] m_Points;
        public int Length => m_Points.Length;
        public AIPointPatrol this[int index] => m_Points[index];

        private static readonly Color GizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;

            foreach (var point in m_Points)
                Gizmos.DrawSphere(point.transform.position, point.Radius);
        }
    }
}
