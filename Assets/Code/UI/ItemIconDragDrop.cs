using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemIconDragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform rectTrans;
    Canvas canvas;
    CanvasGroup canvasGroup;
    Transform parent;
    Image image;

    Vector2 originPos;
    public static Action dragAction;
    public static Action dropAction;

    void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
        canvas = GameObject.FindWithTag("UICanvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        parent = rectTrans.parent;
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
        rectTrans.SetParent(parent);  // image goes back to origin parent 
        rectTrans.position = originPos;
        dropAction?.Invoke();
    }
}
