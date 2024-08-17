using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IDropHandler
{
    Image icon;

    void Start()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // when drop moving icon onto this cell

        if (eventData.pointerDrag == null)
        {
            return;
        }

        // swap icons' sprites
        icon.enabled = true;
        Sprite tmp = eventData.pointerDrag.GetComponent<Image>().sprite;
        eventData.pointerDrag.GetComponent<Image>().sprite = icon.sprite;
        icon.sprite = tmp;
    }
}
