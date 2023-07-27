using UnityEngine;

namespace SpaceDefence
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol,
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;

        [SerializeField] private AIPoints m_Point;

        [SerializeField] private float m_RandomSelectMovePointTime;
        [SerializeField] private float m_FindNewTargetTime;
        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLenght;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_Coef;

        private Destructible m_SelectedTarget;
        private SpaceShip m_SpaceShip;
        private Vector3 m_MovePosition;
        private CircleArea m_CircleArea;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();
            m_Point = FindObjectOfType<AIPoints>();
            m_CircleArea = GetComponent<CircleArea>();

            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.Null)
            {

            }

            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        public void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            //ActionFindNewAttackTarget();
            //ActionFire();
            ActionEvadeCollision();
        }

        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = MakeLead();
                }
                else
                {
                    if (m_Point != null)
                    {
                        bool isInsidePatrolZone = (m_Point.transform.position - transform.position).sqrMagnitude < m_Point.Radius * m_Point.Radius;

                        if (isInsidePatrolZone == true)
                        {
                            if (m_RandomizeDirectionTimer.IsFinished == true)
                            {
                                Vector2 newPoint = Random.onUnitSphere * m_Point.Radius + m_Point.transform.position;

                                m_MovePosition = newPoint;

                                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                            }
                        }
                        else
                        {
                            m_MovePosition = m_Point.transform.position;
                        }
                    }
                }
            }
        }

        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;

            m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }

        private const float MAX_ANGLE = 45.0f;

        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            return -angle;
        }

        //private void ActionFindNewAttackTarget()
        //{
        //    if (m_FindNewTargetTimer.IsFinished == true)
        //    {
        //        m_SelectedTarget = FindNearestDestructibleTarget();

        //        m_FindNewTargetTimer.Start(m_ShootDelay);
        //    }
        //}

        //private Destructible FindNearestDestructibleTarget()
        //{
        //    float maxDist = float.MaxValue;

        //    Destructible potentialTarget = null;

        //    foreach (var v in Destructible.AllDestructibles)
        //    {
        //        if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;
        //        if (v.TeamId == Destructible.TeamIdNeutral) continue;
        //        if (v.TeamId == m_SpaceShip.TeamId) continue;

        //        float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

        //        if (dist < maxDist)
        //        {
        //            maxDist = dist;
        //            potentialTarget = v;
        //        }
        //    }

        //    bool isInsidePatrolZone = (transform.position - potentialTarget.transform.position).sqrMagnitude < m_CircleArea.Radius * m_CircleArea.Radius;

        //    if (isInsidePatrolZone)
        //    {
        //        return potentialTarget;
        //    }

        //    return null;
        //}

        //private void ActionFire()
        //{
        //    if (m_SelectedTarget != null)
        //    {
        //        if (m_FireTimer.IsFinished == true)
        //        {
        //            m_SpaceShip.Fire(TurretMode.Primary);

        //            m_FireTimer.Start(m_ShootDelay);
        //        }
        //    }
        //}

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLenght) == true)
            {
                m_MovePosition = transform.position + transform.right * 100.0f;
            }
        }

        private Vector2 MakeLead()
        {
            Vector2 lead;

            float velocityEnemy = m_SelectedTarget.transform.GetComponent<Rigidbody2D>().velocity.magnitude;

            lead = m_SelectedTarget.transform.position + m_SelectedTarget.transform.up * (velocityEnemy * m_Coef);

            return lead;
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
    }
}