using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class BuildPlace : MonoBehaviour, IPointerDownHandler
    {
        public static event Action<Transform> OnClickEvent;
        public static void HideControls()
        {
            OnClickEvent(null);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"pressed {transform.root.name}");
            OnClickEvent(transform.root);
        }
    }
}
