using UnityEngine;

namespace TowerDefence
{
    public class StandUp : MonoBehaviour
    {
        private const float EPSILON = 0.1f;
        private Rigidbody2D m_Rigidbody;
        private SpriteRenderer m_SpriteRenderer;

        private void Start()
        {
            m_Rigidbody = transform.root.GetComponent<Rigidbody2D>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            transform.up = Vector2.up;

            var speedX = m_Rigidbody.velocity.x;
            if (speedX > EPSILON)
                m_SpriteRenderer.flipX = false;
            else if (speedX < EPSILON)
                m_SpriteRenderer.flipX = true;
        }
    }
}