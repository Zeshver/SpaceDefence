using UnityEngine;

namespace SpaceDefence
{
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol,
        }

        [SerializeField] private float m_EvadeRayLenght;

        private SpaceShip m_SpaceShip;
        private Vector3 m_MovePosition;

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();
        }

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLenght) == true)
            {
                m_MovePosition = transform.position + transform.right * 100.0f;
            }
        }
    }
}