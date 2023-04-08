using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Base class for all interactive objects.
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary>
        /// Object's name for user.
        /// </summary>
        [SerializeField] private string m_Nickname;
        public string Nickname => m_Nickname;
    }
}
