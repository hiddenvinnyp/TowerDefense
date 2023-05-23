using UnityEngine;
using UnityEngine.EventSystems;

public class LevelHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite m_HoverSprite;
    private Sprite m_CurrentSprite;
    private Sprite m_UnHoverSprite;

    void Start()
    {
        m_UnHoverSprite = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = m_HoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = m_UnHoverSprite;
    }
}
