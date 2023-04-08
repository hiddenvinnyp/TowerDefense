using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehavior
        {
            Null,
            Patrol
        }

        [SerializeField] private AIBehavior m_AIBehavior;

        [Header("For Area Patroling")]
        [SerializeField] private AIPointPatrol m_AIPointPatrol;

        [Header("For Points Patroling")]
        [SerializeField] private AIPointPatrol[] m_AIPointsPatrol;
        [Space(10)]

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLiner;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;
        [SerializeField] private float m_RandomSelectMovePointTime;
        [SerializeField] private float m_FindNewTargetTime;
        [SerializeField] private float m_ShootDelay;
        [SerializeField] private float m_EvadeRayLength;

        private SpaceShip m_Ship;
        private Vector3 m_MovePosition = new Vector3(0,0,0);
        private Destructible m_SelectedTarget;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private AIPointPatrol m_CurrentPoint;

        private void Start()
        {
            m_Ship = GetComponent<SpaceShip>();
            //m_AIPointPatrol = FindObjectOfType<AIPointPatrol>();
            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void LateUpdate()
        {
            if (m_SelectedTarget != null)
                m_prevTargetPosition = m_SelectedTarget.transform.position;
        }

        private void UpdateAI()
        {
            if (m_AIBehavior == AIBehavior.Null)
            {

            }

            if (m_AIBehavior == AIBehavior.Patrol)
            {
                UpdateBehaviorPatrol();
            }
        }

        private void UpdateBehaviorPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
        }

        private void ActionFindNewMovePosition()
        {
            if (m_AIBehavior == AIBehavior.Patrol)
            {
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = m_SelectedTarget.transform.position;
                } 
                else
                {
                    if (m_AIPointPatrol != null)
                    {
                        bool IsInsidePatrolZone = (m_AIPointPatrol.transform.position - transform.position).sqrMagnitude < (m_AIPointPatrol.Radius * m_AIPointPatrol.Radius);
                        
                        if (IsInsidePatrolZone)
                        {
                            GetNewPoint();
                        }
                        else
                        {
                            m_MovePosition = m_AIPointPatrol.transform.position;
                        }
                    }
                }
            }            
        }

        protected virtual void GetNewPoint()
        {
            if (m_RandomizeDirectionTimer.IsFinished)
            {
                Vector2 newPoint = Random.onUnitSphere * m_AIPointPatrol.Radius + m_AIPointPatrol.transform.position;
                m_MovePosition = newPoint;
                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
            }
        }

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength))
            {
                // Можно написать свой метод. Например, корабль выбирает вправо или влево
                m_MovePosition = transform.position + transform.right * 100.0f;
            }
        }

        private void ActionControlShip()
        {
            m_Ship.ThrustControl = m_NavigationLiner;

            m_Ship.TorqueControl = ComputeAlignTorqueNormalized(m_MovePosition, m_Ship.transform) * m_NavigationAngular;
        }

        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();
                m_FindNewTargetTimer.Restart();
            }
        }

        private void ActionFire()
        {
            if (m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinished)
                {
                    m_Ship.Fire(TurretMode.Primary);
                    m_FireTimer.Restart();
                }
            }
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = m_EvadeRayLength; //float.MaxValue;
            Destructible potentialTarget = null;

            foreach (var destructible in Destructible.AllDestructibles)
            {
                if (destructible.GetComponent<SpaceShip>() == m_Ship) continue; // сам себя
                if (destructible.TeamId == Destructible.TeamIdNeutral) continue; // астероиды и пр нейтральные
                if (destructible.TeamId == m_Ship.TeamId) continue; // своя команда

                float distance = Vector2.Distance(m_Ship.transform.position, destructible.transform.position);

                if (distance < maxDist)
                {
                    maxDist = distance;
                    potentialTarget = destructible;
                }
            }
            return potentialTarget;
        }

        private Vector3 m_prevTargetPosition;
        private Vector3 m_prevPos;
        private Vector3 MakeLead(Destructible target)
        {
            Vector3 position = target.transform.position;

            if (m_prevTargetPosition == null)
                m_prevTargetPosition = position;

            Vector3 targetSpeed = (position - m_prevTargetPosition); // /Time.deltaTime;
            float time = (position - transform.position).magnitude / m_NavigationLiner;
            position += time * targetSpeed;

            if (targetSpeed != Vector3.zero)
                m_prevPos = position;

            if (position == target.transform.position)
                position = m_prevPos;


            Debug.DrawLine(transform.position, position, Color.green);

            return position;
        }

        private const float MAX_ANGLE = 45.0f;
        private static float ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);
            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);
            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;
            return -angle;
        }

        #region Timers
        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
        }
        #endregion

        public void SetPatrolBehavior(AIPointPatrol point)
        {
            m_AIBehavior = AIBehavior.Patrol;
            m_AIPointPatrol = point;
        }
    }
}
