using UnityEngine;

namespace SpaceDefence
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] private string m_Nickname;
        public string Nickname => m_Nickname;
    }
}