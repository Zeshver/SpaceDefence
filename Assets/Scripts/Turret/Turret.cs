using UnityEngine;

namespace SpaceDefence
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretMode m_TurretMode;
        public TurretMode Mode => m_TurretMode;

        [SerializeField] private AudioSource m_AudioSource;
        [SerializeField] private AudioClip[] m_AudioClip;

        [SerializeField] private TurretProperties m_TurretProperties;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;

        #region Unity Event

        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
            {
                m_RefireTimer -= Time.deltaTime;
            }
        }

        #endregion

        // Public API
        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false) return;
            if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false) return;

            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.SetParentShooter(m_Ship);
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            m_RefireTimer = m_TurretProperties.RateOfFire;
        }

        public void AssignLoadout(TurretProperties props)
        {
            if (m_TurretMode != props.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = props;
        }
    }
}