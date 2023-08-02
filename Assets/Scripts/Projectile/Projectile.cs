using UnityEngine;

namespace SpaceDefence
{
    public class Projectile : Entity
    {
        public enum ProjectileType
        {
            Homing,
            Single
        }

        public ProjectileType type;

        [SerializeField] private float m_Velocity;

        [SerializeField] private float m_Lifetime;

        [SerializeField] private int m_Damage;

        [SerializeField] private Destructible m_ParentDest;

        [SerializeField] private Destructible m_Target;

        [SerializeField] private bool isHoming = false;

        private float m_Timer;

        protected virtual void Update()
        {
            m_Target = SearchTarget();

            float stepLenght = Time.deltaTime * m_Velocity;
            Vector2 step = GetDirection() * stepLenght;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLenght);

            if (m_ParentDest == null)
            {
                m_ParentDest = GetComponent<Destructible>();
            }

            if (hit)
            {
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();

                if (dest != null && dest != m_ParentDest)
                {
                    dest.ApplyDamage(m_Damage);
                }

                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_Lifetime)
            {
                Destroy(gameObject);
            }

            if (type == ProjectileType.Single)
            {
                transform.position += new Vector3(step.x, step.y, 0);
            }

            if (type == ProjectileType.Homing)
            {
                if (m_Target != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, m_Target.transform.position, stepLenght);
                }
                else
                {
                    return;
                }
            }
        }

        private Vector3 GetDirection()
        {
            if (isHoming && m_Target != null)
            {
                transform.up = (m_Target.transform.position - transform.position).normalized;
                return (m_Target.transform.position - transform.position).normalized;
            }
            else
            {
                return transform.up;
            }
        }

        private Destructible SearchTarget()
        {
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructibles)
            {
                float dist = Vector2.Distance(transform.position, v.transform.position);

                if (dist < maxDist && v != m_ParentDest)
                {
                    maxDist = dist;
                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }

        protected void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            Destroy(gameObject);
        }

        public void SetParentShooter(Destructible parent)
        {
            m_ParentDest = parent;
        }
    }
}