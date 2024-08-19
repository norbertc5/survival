using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IDropHandler
{
    Image icon;
    [SerializeField] bool isHandCell;
    public Item itemInCell;

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

        // swap item attached to this cell
        Item tmp = eventData.pointerDrag.GetComponent<ItemInventoryIcon>().referenceCell.itemInCell;
        eventData.pointerDrag.GetComponent<ItemInventoryIcon>().referenceCell.SetItem(itemInCell);
        SetItem(tmp);
    }

    /// <summary>
    /// Set item which is in this cell.
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(Item item)
    {
        itemInCell = item;

        if (isHandCell)
            Hand.UpdateItemInHand(itemInCell);

        // change sprite of the icon (or not if no item)
        try
        {
            icon.sprite = item.uIIcon;
            icon.enabled = true;
        }
        catch
        {
            icon.sprite = null;
        }
    }
}
