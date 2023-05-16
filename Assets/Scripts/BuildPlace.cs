using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class BuildPlace : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TowerAsset[] m_BuildableTowers;
        public TowerAsset[] BuildableTowers { get { return m_BuildableTowers; } }

        public void SetBuildableTowers(TowerAsset[] towers)
        {
            if (towers == null || towers.Length == 0)
            {
                Destroy(transform.parent.gameObject);
            } else
                m_BuildableTowers = towers;
        }

        public static event Action<BuildPlace> OnClickEvent;
        public static void HideControls()
        {
            OnClickEvent(null);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnClickEvent(this);
        }
    }
}
