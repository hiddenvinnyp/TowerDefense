using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        [SerializeField] private int m_NumLives;
        private SpaceShip m_Ship;
        //[SerializeField] private SpaceShip m_PlayerShipPrefab;
        public SpaceShip ActiveShip => m_Ship;

        //[SerializeField] private CameraController m_CameraController;
        //[SerializeField] private MovementController m_MovementController;
        //[SerializeField] private SyncTransform[] m_SyncTransforms;

        protected override void Awake()
        {
            base.Awake();

            if (m_Ship != null)
                Destroy(m_Ship.gameObject);
        }

        private void Start()
        {
                Respawn();
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
            {
                Respawn();
            } else
            {
                LevelSequenceController.Instance.FinishCurrentLevel(false); // Exit to Main menu
            }
        }

        internal void TakeDamage(int m_Damage)
        {
            m_NumLives -= m_Damage;
            if (m_NumLives <= 0)
            {
                LevelSequenceController.Instance.FinishCurrentLevel(false);
            }
        }

        private void Respawn()
        {
            if (LevelSequenceController.PlayerShip != null)
            {
                var newPlayerShip = Instantiate(LevelSequenceController.PlayerShip);

                m_Ship = newPlayerShip.GetComponent<SpaceShip>();

                /*m_CameraController.SetTarget(m_Ship.transform);
                m_MovementController.SetTargetShip(m_Ship);

                foreach (var transform in m_SyncTransforms)
                {
                    transform.SetTarget(m_Ship.transform);
                }*/
                if (m_Ship)
                    m_Ship.EventOnDeath.AddListener(OnShipDeath);
            }
        }

        #region Score on current level only
        public int Score { get; private set; }
        public int NumKill { get; private set; }

        public int PowerUpsAmount { get; set; }
        public void AddKill()
        {
            NumKill++;
        }

        public void AddScore(int amount)
        {
            Score += amount;
        }
        #endregion
    }
}
