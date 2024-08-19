using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInventoryIcon : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform rectTrans;
    Canvas canvas;
    CanvasGroup canvasGroup;
    public ItemCell referenceCell;
    Image image;

    Vector2 originPos;
    public static Action dragAction;
    public static Action dropAction;

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        canvas = GameObject.FindWithTag("UICanvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

        referenceCell = rectTrans.GetComponentInParent<ItemCell>();
        dragAction += () => { canvasGroup.blocksRaycasts = false; };
        dropAction += () => { canvasGroup.blocksRaycasts = true; };
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTrans.SetParent(rectTrans.parent.parent.parent);  // change parent to display icon above cells
        originPos = rectTrans.position;
        image.enabled = true;
        dragAction?.Invoke();  // make possibility to interact with cells by disabling blocksRaycasts in all icons
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move icon
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTrans.SetParent(referenceCell.transform);  // image goes back to origin parent 
        rectTrans.position = originPos;
        dropAction?.Invoke();
    }
}
