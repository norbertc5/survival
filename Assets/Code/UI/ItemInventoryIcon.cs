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
    public static ItemInventoryIcon actuallyDraggingIcon;

    Vector2 originPos;

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        canvas = GameObject.FindWithTag("UICanvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

        referenceCell = rectTrans.GetComponentInParent<ItemCell>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTrans.SetParent(rectTrans.parent.parent.parent.parent);  // change parent to display icon above cells
        originPos = rectTrans.position;
        image.enabled = true;
        canvasGroup.blocksRaycasts = false;  // make possibility to interact with cells by disabling blocksRaycasts in all icons
        ItemCell.isHoldingIcon = true;
        actuallyDraggingIcon = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move icon
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // drop item on ground only if beyond inventory ui
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Inventory.Drop(referenceCell.attachedSlot, referenceCell);
        }

        rectTrans.SetParent(referenceCell.transform);  // image goes back to origin parent 
        rectTrans.position = originPos;
        canvasGroup.blocksRaycasts = true;
    }

    void Update()
    {
        // when we ate the food from cell and cell is now free, we need to disable image
        if(image.sprite == null)
        {
            image.enabled = false;
        }
    }
}
