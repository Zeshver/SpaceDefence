using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceDefence
{
    public class Destructible : Entity
    {
        [SerializeField] private bool m_Indestructible;
        [SerializeField] private int m_HitPoints;
        [SerializeField] private int m_MaxHitPoints;

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_HitPoints -= damage;

            if (m_HitPoints <= 0)
            {
                OnDeath();
            }
        }

        protected virtual void OnDeath()
        {
            m_AllDestructibles.Remove(this);
            Destroy(gameObject);

            m_EventOnDeath?.Invoke();
        }

        [SerializeField] private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
            {
                m_AllDestructibles = new HashSet<Destructible>();
            }

            m_AllDestructibles.Add(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;
    }
}