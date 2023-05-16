using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// Компонента, которую надо повесить на кнопку, проигрывающую звук
    /// </summary>
    public class SoundHook : MonoBehaviour
    {
        public Sound m_Sound;

        public void Play()
        {
            m_Sound.Play();
        }
    }
}
