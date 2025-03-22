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

        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        [Header("Weapon and improvements")]
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_EnergyRegenPerSecond;
        [SerializeField] private Turret[] m_Turrets;
        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;

        private Rigidbody2D m_Rigid;

        public float ThrustControl { get; set; }
        public float TorqueControl { get; set; }

        private void Start()
        {
            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();
            UpdateEnergyRegen();
        }

        private void UpdateRigidbody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_LinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        #region Weapon System
        public void AddEnergy(int e)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        public bool DrawEnergy(int count)
        {
            if (count == 0)
            {
                return true;
            }

            if (m_PrimaryEnergy >= count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }

            return false;
        }

        public bool DrawAmmo(int count)
        {
            if (count == 0)
            {
                return true;
            }

            if (m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }

            return false;
        }

        public void AssingWeapon(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }

        public void Fire(TurretMode mode)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                if (m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }
        #endregion
    }
}