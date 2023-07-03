using UnityEngine;

namespace SpaceDefence
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        [Header("Thrust")]
        [SerializeField] private float m_Thrust;

        [Header("Mobility")]
        [SerializeField] private float m_Mobility;

        [Header("Linear velocity")]
        [SerializeField] private float m_LinearVelocity;

        [Header("Angular velocity")]
        [SerializeField] private float m_MaxAngularVelocity;

        private Rigidbody2D m_Rigid;

        public float ThrustControl { get; set; }
        public float TorqueControl { get; set; }

        private void Start()
        {
            m_Rigid = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();
        }

        private void UpdateRigidbody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_LinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }
}