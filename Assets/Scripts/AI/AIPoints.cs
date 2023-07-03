using System.Collections.Generic;
using UnityEngine;

namespace SpaceDefence
{
    public class AIPoints : MonoBehaviour
    {
        [SerializeField] private static List<AIPoints> m_AllAIPoints;
        public static IReadOnlyCollection<AIPoints> AllAIPoints => m_AllAIPoints;

        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        private static readonly Color GizmoColor = new Color(1, 0, 0, 0.3f);

        private void Start()
        {
            if (m_AllAIPoints == null)
            {
                m_AllAIPoints = new List<AIPoints>
                {
                    this
                };
            }            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawSphere(transform.position, m_Radius);
        }
    }
}