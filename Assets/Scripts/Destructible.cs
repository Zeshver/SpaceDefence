using UnityEngine;

namespace SpaceDefence
{
    public class Destructible : Entity
    {
        [SerializeField] private bool m_Indestructible;
        [SerializeField] private int m_HitPoints;
        [SerializeField] private int m_MaxHitPoints;
    }
}