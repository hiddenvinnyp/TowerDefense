using TowerDefence;
using UnityEngine;
using UnityEngine.EventSystems;

public class NullBuildPlace : BuildPlace
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        HideControls();
    }
}
