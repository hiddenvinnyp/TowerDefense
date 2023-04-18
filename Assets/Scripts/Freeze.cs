using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class Freeze : MonoBehaviour
    {
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_EffectTime;
        private Destructible m_Target;

        public void SetTarget(Destructible target)
        {
            m_Target = target;
        }

        private void Start()
        {
            if (m_Target == null) return;
            GetComponent<ImpactEffect>().LifeTime = m_EffectTime;
            m_Target.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
            m_Target.GetComponent<SpaceShip>().AddSpeed(m_Speed, m_EffectTime);
        }

        private void OnDestroy()
        {
            if (m_Target != null)
                m_Target.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }
}
